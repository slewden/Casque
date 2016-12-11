using System.Linq;
using CasqueLib.Buisness;
using Microsoft.AspNet.SignalR;

namespace CasqueLib.Matos.ServerOwin
{
  /// <summary>
  /// Classe Hub pour la communication entre l'appli Web et les démons
  /// </summary>
  public class CasqueHub : Hub
  {
    /// <summary>
    /// Pour les verrouillages multi-thread
    /// </summary>
    private object thisLock = new object();

    /// <summary>
    /// réinit le hub en cas de problème avec le cache lecteur
    /// </summary>
    /// <param name="types">Les types a initialiser</param>
    /// <returns>Les types gérés sur le hub</returns>
    public int ResetHub(int types)
    {
      if ((types & (int)Poste.EPosteType.Lecture) == (int)Poste.EPosteType.Lecture)
      { // c'est un lecteur
        lock (this.thisLock)
        {
          TypeConnexionList.Lecteurs.Clear();
        }
      }

      if ((types & (int)Poste.EPosteType.EncodeTag) == (int)Poste.EPosteType.EncodeTag)
      { // c'est un encodeur
        lock (this.thisLock)
        {
          TypeConnexionList.Encodeurs.Clear();
        }
      }

      int n = CasqueHub.InternalRespondHello();
      this.Clients.All.sayHello(n);
      return n;
    }

    /// <summary>
    /// A chaque connexion on mémorise ses types
    /// </summary>
    /// <param name="types">Les types gérés</param>
    /// <returns>Les types gérés sur le hub</returns>
    public int Hello(int types)
    {
      if ((types & (int)Poste.EPosteType.Lecture) == (int)Poste.EPosteType.Lecture)
      { // c'est un lecteur
        lock (this.thisLock)
        {
          if (!TypeConnexionList.Lecteurs.ContainsKey(this.Context.ConnectionId))
          {
            TypeConnexionList.Lecteurs.Add(this.Context.ConnectionId, new HubLecteurInfo(Poste.EPosteType.Lecture, this.Context.ConnectionId));
          }
        }
      }

      if ((types & (int)Poste.EPosteType.EncodeTag) == (int)Poste.EPosteType.EncodeTag)
      { // c'est un encodeur
        lock (this.thisLock)
        {
          if (!TypeConnexionList.Encodeurs.ContainsKey(this.Context.ConnectionId))
          {
            TypeConnexionList.Encodeurs.Add(this.Context.ConnectionId, new HubLecteurInfo(Poste.EPosteType.EncodeTag, this.Context.ConnectionId));
          }
        }
      }

      int n = CasqueHub.InternalRespondHello();
      this.Clients.All.sayHello(n);
      return n;
    }

    /// <summary>
    /// A chaque déconnexion d'un client on le retire des listes
    /// </summary>
    /// <param name="types">Les types gérés</param>
    /// <returns>Les types gérés sur le hub</returns>
    public int Bye(int types)
    {
      if ((types & (int)Poste.EPosteType.Lecture) == (int)Poste.EPosteType.Lecture)
      {
        if (TypeConnexionList.Lecteurs != null)
        {
          lock (this.thisLock)
          {
            TypeConnexionList.Lecteurs.Remove(this.Context.ConnectionId);
          }
        }
      }

      if ((types & (int)Poste.EPosteType.EncodeTag) == (int)Poste.EPosteType.EncodeTag)
      {
        if (TypeConnexionList.Encodeurs != null)
        {
          lock (this.thisLock)
          {
            TypeConnexionList.Encodeurs.Remove(this.Context.ConnectionId);
          }
        }
      }

      int n = CasqueHub.InternalRespondHello();
      this.Clients.All.sayHello(n);
      return n;
    }

    /// <summary>
    /// Lorsqu'on reçoit un tag ==> le transférer aux clients connectés
    /// </summary>
    /// <param name="clientId">id du client destination de l'info</param>
    /// <param name="tag">N° de tag lu</param>
    public void NewTag(string clientId, string tag)
    {
      this.Clients.Client(clientId).tag(tag);
    }

    /// <summary>
    /// Le lecteur envoi un message au client
    /// </summary>
    /// <param name="clientId">id du client destination de l'info</param>
    /// <param name="erreur">true si erreur false sinon</param>
    /// <param name="action">L'action qui a généré ce message</param>
    /// <param name="message">Message a afficher</param>
    public void Notity(string clientId, bool erreur, int action, string message)
    {
      if (!string.IsNullOrWhiteSpace(clientId))
      {
        this.Clients.Client(clientId).message(erreur, action, message);
      }
      else
      { // au cas ou on Broad cast à tous
        this.Clients.All.message(erreur, action, message);
      }

      // on met à jour l'état des lecteurs dans le hub
      if (TypeConnexionList.Lecteurs.ContainsKey(this.Context.ConnectionId))
      { // lecteur trouvé
        TypeConnexionList.Lecteurs[this.Context.ConnectionId].IsOn = ((EActionLecteur)action == EActionLecteur.Demarre || (EActionLecteur)action == EActionLecteur.ResetLecture) && !erreur;
        this.Clients.All.sayHello(CasqueHub.InternalRespondHello());
      } 
    }

    /// <summary>
    /// L'encodeur envoi un message au client
    /// </summary>
    /// <param name="clientId">id du client destination de l'info</param>
    /// <param name="erreur">true si erreur false sinon</param>
    /// <param name="action">L'action qui a généré ce message</param>
    /// <param name="cle">La clé de l'objet manipulé</param>
    /// <param name="message">Message a afficher</param>
    public void Rapporte(string clientId, bool erreur, int action, int cle, string message)
    {
      ////this.Clients.Client(clientId).report(erreur, action, cle, message);
      this.Clients.All.report(erreur, action, cle, message);

      // on met à jour l'état des encodeurs dans le hub
      if (TypeConnexionList.Encodeurs.ContainsKey(this.Context.ConnectionId))
      { // Encodeur trouvé
        TypeConnexionList.Encodeurs[this.Context.ConnectionId].IsOn = ((EActionEncode)action == EActionEncode.TraiteAssemblage || (EActionEncode)action == EActionEncode.TraiteCommande) && !erreur;
        this.Clients.All.sayHello(CasqueHub.InternalRespondHello());
      }
    }

    /// <summary>
    /// L'encodeur envoie un message de progression au client
    /// </summary>
    /// <param name="clientId">id du client destination de l'info</param>
    /// <param name="action">Action en cours : EActionEncode.ToInt32</param>
    /// <param name="cle">Cle de l'element en cours d'encodage</param>
    /// <param name="index">La position actuelle</param>
    /// <param name="total">Le nombre total de positions</param>
    public void Progresse(string clientId, int action, int cle, int index, int total)
    {
      ////this.Clients.Client(clientId).progress(action, cle, index, total);
      this.Clients.All.progress(action, cle, index, total);
      
      // on met à jour l'état des encodeurs dans le hub
      if (TypeConnexionList.Encodeurs.ContainsKey(this.Context.ConnectionId))
      { // Encodeur trouvé
        TypeConnexionList.Encodeurs[this.Context.ConnectionId].IsOn = true;
        this.Clients.All.sayHello(CasqueHub.InternalRespondHello());
      }
    }

    /// <summary>
    /// Demande une action au lecteur 
    /// </summary>
    /// <param name="clientId">L'identifiant du client qui demande et a qui on doit renvoyer les réponses</param>
    /// <param name="action">Renvoie l'action à faire : Démarre ou stoppe la lecture</param>
    /// <param name="param">Le paramètre de l'appel (ex : le XML de config pour démarrer un lecteur)</param>
    public void PiloteReader(string clientId, int action, string param)
    {
      HubConnectorEventLecteur demande = new HubConnectorEventLecteur()
      {
        ClientId = clientId,
        Action = (EActionLecteur)action,
        XmlParameter = param,
      };

      if (TypeConnexionList.Lecteurs != null && TypeConnexionList.Lecteurs.Any())
      { // on répercute l'action à tous les lecteurs connus
        this.Clients.Clients(TypeConnexionList.Lecteurs.Keys.ToList()).fireAction(demande);
      }
      else
      { // au cas ou : on envoie à tous le monde moins le demandeur
        this.Clients.Others.fireAction(demande);
      }
    }

    /// <summary>
    /// Demande au writer de démarrer le traitement d'une commande (si commandeCle > 0) ou d'un assemblage (si assemblageCle > 0)
    /// </summary>
    /// <param name="clientId">>L'identifiant du client qui demande et a qui on doit renvoyer les réponses</param>
    /// <param name="action">L'action demandée : 1 commande; 2 assemblage </param>
    /// <param name="cle">Clé de la commande ou de l'assemblage à traiter</param>
    /// <param name="param">Le paramètre de l'appel (ex : le XML de config pour démarrer un lecteur)</param>
    public void PiloteWriter(string clientId, int action, int cle, string param)
    {
      HubConnectorEventEncodeur demande = new HubConnectorEventEncodeur()
      {
        ClientId = clientId,
        Action = (EActionEncode)action,
        Cle = cle,
        XmlParameter = param,
      };

      if (demande.Action != EActionEncode.Aucune)
      { // faut envoyer aux encodeurs
        if (TypeConnexionList.Encodeurs != null && TypeConnexionList.Encodeurs.Any())
        { // on répercute l'action à tous les encodeurs connus
          this.Clients.Clients(TypeConnexionList.Encodeurs.Keys.ToList()).processCommande(demande);
        }
        else
        { // au cas ou : on envoie à tous le monde moins le demandeur
          this.Clients.Others.processCommande(demande);
        }
      }
    }

    /// <summary>
    /// Renvoie les infos des matériels présents
    /// </summary>
    /// <returns>0 si rien dispo, 1 s'il y a au moins une imprimante, 2 s'il y a au moins un lecteur, 3 pour les 2 dispo</returns>
    private static int InternalRespondHello()
    {
      // Le retour les types de lecteurs géré
      int typ = 0;
      if (TypeConnexionList.Lecteurs != null && TypeConnexionList.Lecteurs.Any())
      {
        typ += (int)Poste.EPosteType.Lecture;

        if (TypeConnexionList.Lecteurs.Values.Any(x => x.IsOn))
        {
          typ += (int)Poste.EPosteIsOn.LecteurIsON;
        }
      }

      if (TypeConnexionList.Encodeurs != null && TypeConnexionList.Encodeurs.Any())
      {
        typ += (int)Poste.EPosteType.EncodeTag;

        if (TypeConnexionList.Encodeurs.Values.Any(x => x.IsOn))
        {
          typ += (int)Poste.EPosteIsOn.WriterIsON;
        }
      }

      return typ;
    }
  }
}