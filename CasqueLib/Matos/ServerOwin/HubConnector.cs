using System;
using Microsoft.AspNet.SignalR.Client;

namespace CasqueLib.Matos.ServerOwin
{
  /// <summary>
  /// Class pour la gestion de la connexion au hub OWIN
  /// </summary>
  public sealed class HubConnector : IDisposable
  {
    #region Membres privés
    /// <summary>
    /// Le nom du proxy Hub à utiliser
    /// </summary>
    private const string HUBCLASSNAME = "CasqueHub";

    /// <summary>
    /// Nom de la méthode du hub pour de réception des lectures
    /// </summary>
    private const string HUBMETHODSENDTAG = "tag";

    /// <summary>
    /// Nom de la méthode du hub pour les actions à traiter par les lecteurs
    /// </summary>
    private const string HUBMETHODLECTEURACTION = "fireAction";

    /// <summary>
    /// Nom de la méthode du hub pour les actions à traiter par les encodeurs
    /// </summary>
    private const string HUBMETHODENCODEACTION = "processCommande";

    /// <summary>
    /// Nom de la méthode du hub pour les déconnexions clients
    /// </summary>
    private const string HUBMETHODENDECONNEXIONCLIENT = "deconnexionClient";

    /// <summary>
    /// Le hub proxy
    /// </summary>
    private IHubProxy hubProxy;

    /// <summary>
    /// La connexion
    /// </summary>
    private HubConnection hubConnection;

    /// <summary>
    /// Demande normale d'arret de la connexion
    /// </summary>
    private bool closing = false;

    /// <summary>
    /// Les types de lecteurs gérés par l'application
    /// Combinaison des valeurs de l'enum Buisness.Poste.EPosteType : 1 encode, 2 lecteur
    /// </summary>
    private int lesTypes;
    #endregion

    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="HubConnector"/>
    /// </summary>
    /// <param name="serverUrl">L'adresse du serveur pour le hub</param>
    /// <param name="types">Les type de lecteurs gérés</param>
    public HubConnector(string serverUrl, int types)
    {
      this.ServeurURL = serverUrl;
      this.lesTypes = types;
      this.hubConnection = new HubConnection(this.ServeurURI);
      this.hubConnection.Closed += this.HubConnection_Closed;
      this.hubConnection.ConnectionSlow += this.HubConnection_ConnectionSlow;
      this.hubConnection.Error += this.HubConnection_Error;
      this.hubConnection.Received += this.HubConnection_Received;
      this.hubConnection.Reconnected += this.HubConnection_Reconnected;
      this.hubConnection.Reconnecting += this.HubConnection_Reconnecting;
      this.hubConnection.StateChanged += this.HubConnection_StateChanged;
      this.hubProxy = this.hubConnection.CreateHubProxy(HubConnector.HUBCLASSNAME);
      ////this.hubProxy.On<string, string>(HubConnector.HUBMETHODSENDTAG, (client, tag) => this.ProcessReceiveTag(client, tag));
      this.hubProxy.On<HubConnectorEventLecteur>(HubConnector.HUBMETHODLECTEURACTION, (demande) => this.ProcessLecteurAction(demande));
      this.hubProxy.On<HubConnectorEventEncodeur>(HubConnector.HUBMETHODENCODEACTION, (demande) => this.ProcessEncodeAction(demande));
      this.hubProxy.On<string>(HubConnector.HUBMETHODENDECONNEXIONCLIENT, (demande) => this.ProcessDeconnexionClient(demande));
      this.Connected = ConnectionState.Disconnected;
    }

    /// <summary>
    /// Pour notifier les déconnexions des clients
    /// (Permet aux lecteur de voir s'il faut qu'il se stoppent tout seul)
    /// </summary>
    public event EventHandler<HubConnectorEventDeconnexionClient> OnDeconnexionClient;

    /// <summary>
    /// Pour notifier les changements dans l'état de connexion
    /// </summary>
    public event EventHandler<HubConnectorEventStatut> OnConnectedChanged;
    
    /// <summary>
    /// Pour notifier les demandes du serveur
    /// </summary>
    public event EventHandler<HubConnectorEventLecteur> OnAction;

    /// <summary>
    /// Pour notifier la demande d'encodage
    /// </summary>
    public event EventHandler<HubConnectorEventEncodeur> OnEncode;

    #region Properties
    /// <summary>
    /// Renvoie l'adresse du serveur ou trouver le hub OWIN /SignalR
    /// de la forme http://localhost:51287
    /// </summary>
    public string ServeurURL { get; private set; }

    /// <summary>
    /// Indique si l'objet est connecté au hub
    /// </summary>
    public ConnectionState Connected { get; private set; }

    /// <summary>
    /// Renvoie l'URI d'accès au hub 
    /// de la forme http://localhost:51287/signalr
    /// </summary>
    private string ServeurURI
    {
      get
      {
        if (string.IsNullOrWhiteSpace(this.ServeurURL))
        {
          return string.Empty;
        }

        if (this.ServeurURL.EndsWith("signalr"))
        {
          return this.ServeurURL;
        }

        string res = string.Empty;
        if (!this.ServeurURL.StartsWith("http://"))
        {
          res += "http://";
        }

        res += this.ServeurURL;
        if (!res.EndsWith("/"))
        {
          res += "/";
        }

        return res + "signalr";
      }
    }
    #endregion

    /// <summary>
    /// Néttoyage de l'objet en fin de vie
    /// </summary>
    public void Dispose()
    {
      this.Disconnecte();

      if (this.hubConnection != null)
      {
        this.hubConnection.Closed -= this.HubConnection_Closed;
        this.hubConnection.ConnectionSlow -= this.HubConnection_ConnectionSlow;
        this.hubConnection.Error -= this.HubConnection_Error;
        this.hubConnection.Received -= this.HubConnection_Received;
        this.hubConnection.Reconnected -= this.HubConnection_Reconnected;
        this.hubConnection.Reconnecting -= this.HubConnection_Reconnecting;
        this.hubConnection.StateChanged -= this.HubConnection_StateChanged;
        this.hubConnection.Dispose();
      }
    }

    /// <summary>
    /// Configure le hub et s'y connecter
    /// </summary>
    /// <param name="types">Les type de lecteurs gérés</param>
    public async void Connecte(int types)
    {
      if (this.Connected == ConnectionState.Disconnected)
      {
        this.lesTypes = types;
        ConnectionState st = this.Connected;
        try
        {
          this.Connected = ConnectionState.Connecting;
          this.closing = false;
          await this.hubConnection.Start();
        }
        catch (Exception ex)
        {
          this.Connected = ConnectionState.Disconnected;
          this.FireConnectedChanged(st, true, "Fin de Connexion", ex);
        }
      }
    }

    /// <summary>
    /// Se déconnecter du hub
    /// </summary>
    public void Disconnecte()
    {
      // Fermeture du hub de communication
      if (this.Connected != ConnectionState.Disconnected)
      {
        this.Bye();
        System.Threading.Thread.Sleep(200);
        ConnectionState st = this.Connected; 
        this.hubConnection.Stop();
      }
    }

    /// <summary>
    /// Notifie une lecture de tag
    /// </summary>
    /// <param name="client">L'identifiant du client</param>
    /// <param name="tag">le numéro lu</param>
    public void SendTag(string client, string tag)
    {
      if (this.hubProxy != null && !string.IsNullOrWhiteSpace(tag))
      { // le hub est activé : diffusion du nouveau N°
        this.hubProxy.Invoke("NewTag", client ?? string.Empty, tag);
      }
    }

    /// <summary>
    /// Notifie un message au client
    /// </summary>
    /// <param name="client">L'identifiant du client</param>
    /// <param name="error">est-ce une erreur</param>
    /// <param name="action">Action à l'origine de ce message</param>
    /// <param name="message">le message</param>
    public void Notify(string client, bool error, EActionLecteur action, string message)
    {
      if (this.hubProxy != null && this.Connected != ConnectionState.Disconnected)
      { // le hub est activé : envoie du message
        try
        {
          this.hubProxy.Invoke("Notity", client ?? string.Empty, error, (int)action, message);
        }
        catch
        {
          // Nothing to do !!
        }
      }
    }

    /// <summary>
    /// Rapporte une fin de traitement d'encodage
    /// </summary>
    /// <param name="client">L'identifiant du client</param>
    /// <param name="error">est-ce une erreur</param>
    /// <param name="action">Action à l'origine de ce message</param>
    /// <param name="cle">La clé de l'objet traité</param>
    /// <param name="message">le message</param>
    public void Rapporte(string client, bool error, EActionEncode action, int cle, string message)
    {
      if (this.hubProxy != null && this.Connected != ConnectionState.Disconnected)
      { // le hub est activé : envoie du rapport
        try
        {
          this.hubProxy.Invoke("Rapporte", client ?? string.Empty, error, (int)action, cle, message);
        }
        catch
        {
          // Nothing to do !!
        }
      }
    }

    /// <summary>
    /// L'encodeur envoie un message de progression au client
    /// </summary>
    /// <param name="clientId">id du client destination de l'info</param>
    /// <param name="action">Le type d'action en cours</param>
    /// <param name="cle">Clé de la commande ou de l'assemblage en cours de traitement</param>
    /// <param name="index">La position actuelle</param>
    /// <param name="total">Le nombre total de positions</param>
    public void Progresse(string clientId, EActionEncode action, int cle, int index, int total)
    {
      if (this.hubProxy != null && this.Connected != ConnectionState.Disconnected)
      { // le hub est activé : envoie du rapport
        try
        {
          this.hubProxy.Invoke("Progresse", clientId ?? string.Empty, Convert.ToInt32(action), cle, index, total);
        }
        catch
        {
          // Nothing to do !!
        }
      }
    }

    /// <summary>
    /// Envoie au hub un message Hello
    /// </summary>
    public void Hello()
    {
      if (this.hubProxy != null && this.Connected != ConnectionState.Disconnected)
      { // le hub est activé : diffusion du nouveau N°
        try
        {
          this.hubProxy.Invoke("Hello", this.lesTypes);
        }
        catch
        {
          // Nothing to do !!
        }
      }
    }

    /// <summary>
    /// Envoie au hub un message Bye
    /// </summary>
    public void Bye()
    {
      if (this.hubProxy != null && this.Connected != ConnectionState.Disconnected)
      { // le hub est activé : diffusion du nouveau N°
        try
        {
          this.hubProxy.Invoke("Bye", this.lesTypes);
        }
        catch
        {
          // Nothing to do !!
        }
      }
    }

    #region Events au niveau du hub
    /// <summary>
    /// Le hub signale une fermeture de la connexion
    /// </summary>
    private void HubConnection_Closed()
    {
      if (this.closing)
      {
        ConnectionState st = this.Connected;
        this.Connected = ConnectionState.Disconnected;
        this.FireConnectedChanged(st, false, "Déconnecté");
        this.closing = false;
      }
      else
      {
        ConnectionState st = this.Connected;
        if (st != ConnectionState.Disconnected)
        {
          this.Connected = ConnectionState.Disconnected;
          this.FireConnectedChanged(st, true, "Déconnexion inprévue");
        }
      }
    }

    /// <summary>
    /// Le hub signale une connexion slow
    /// </summary>
    private void HubConnection_ConnectionSlow()
    {
      this.FireConnectedChanged(this.Connected, false, "Connexion lente !!!");
    }

    /// <summary>
    /// Le hub signale une erreur
    /// </summary>
    /// <param name="obj">Les infos de l'erreur</param>
    private void HubConnection_Error(Exception obj)
    {
      this.FireConnectedChanged(this.Connected, true, "Erreur sur le hub", obj);
    }

    /// <summary>
    /// Le hub signale une reception d'infos
    /// </summary>
    /// <param name="obj">l'info reçue</param>
    private void HubConnection_Received(string obj)
    {
      //// Pas utile
      ////this.FireConnectedChanged(this.Connected, false, "Données reçues", obj);
    }

    /// <summary>
    /// Le hub signale une reconnexion finie
    /// </summary>
    private void HubConnection_Reconnected()
    {
      ConnectionState st = this.Connected;
      this.Connected = ConnectionState.Connected;
      this.FireConnectedChanged(st, false, "Reconnecté");
    }

    /// <summary>
    /// Le hub signale qu'il va se reconnecter
    /// </summary>
    private void HubConnection_Reconnecting()
    {
      ConnectionState st = this.Connected;
      this.Connected = ConnectionState.Connecting;
      this.FireConnectedChanged(st, false, "Reconnection en cours");
    }

    /// <summary>
    /// Le hub signale un changement d'état
    /// </summary>
    /// <param name="obj">L'information sur le changement d'état</param>
    private void HubConnection_StateChanged(StateChange obj)
    {
      ConnectionState st = this.Connected;
      if (obj.NewState == ConnectionState.Reconnecting)
      {
        this.Connected = ConnectionState.Connecting;
      }
      else
      {
        this.Connected = obj.NewState;
      }

      if (obj.NewState == ConnectionState.Connected)
      { // à La connexion on indique au hub quels lecteurs on gère
        this.Hello();
      }

      if (st != this.Connected)
      {
        this.FireConnectedChanged(st, false, string.Empty);
      }
    }
    #endregion

    #region Traitement des infos reçues à travers le hub
    /// <summary>
    /// Propage l'information
    /// </summary>
    /// <param name="clientId">La clé du client</param>
    private void ProcessDeconnexionClient(string clientId)
    {
      if (this.OnDeconnexionClient != null)
      {
        this.OnDeconnexionClient(this, new HubConnectorEventDeconnexionClient() { ClientId = clientId });
      }
    }

    /// <summary>
    /// Envoie simplement la demande au parent
    /// </summary>
    /// <param name="demande">La demande d'action</param>
    private void ProcessLecteurAction(HubConnectorEventLecteur demande)
    { 
      if (this.OnAction != null)
      {
        this.OnAction(this, demande);
      }
    }

    /// <summary>
    /// Charge les deonnées et envoie la demande au parent
    /// </summary>
    /// <param name="demande">La demande d'encodage</param>
    private void ProcessEncodeAction(HubConnectorEventEncodeur demande)
    {
      if (demande != null && demande.Action != EActionEncode.Aucune && (demande.Cle > 0 || (demande.Cle == 0 && demande.Action == EActionEncode.QueryStatut)))
      { // la demande est valide
        if (this.OnEncode != null)
        {
          this.OnEncode(this, demande);
        }
      }
    }
    #endregion

    /// <summary>
    /// Notifie au parent un changement dans l'état de connexion
    /// </summary>
    /// <param name="oldSt">L'ancien statut</param>
    /// <param name="fromError">Indique si le changement est due à une erreur ou pas</param>
    /// <param name="message">Le message</param>
    /// <param name="param">Paramètre supplémentaire optionnel</param>
    private void FireConnectedChanged(ConnectionState oldSt, bool fromError, string message, object param = null)
    {
      if (this.OnConnectedChanged != null)
      {
        this.OnConnectedChanged(this, new HubConnectorEventStatut(oldSt, this.Connected, fromError, message, param));
      }
    }
  }
}
