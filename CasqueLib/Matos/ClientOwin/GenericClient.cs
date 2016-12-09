using System;
using System.Collections.Generic;
using System.Linq;
using CasqueLib.Matos.Encodeur;
using CasqueLib.Matos.Lecteur;
using CasqueLib.Matos.ServerOwin;

namespace CasqueLib.Matos.ClientOwin
{
  /// <summary>
  /// Classe de base pour les applications qui pilotent des lecteurs et/ou des encodeurs
  /// </summary>
  /// <typeparam name="Lecteur">Le type des lecteur</typeparam>
  /// <typeparam name="Writeur">Le type des encodeurs</typeparam>
  public abstract class GenericClient<Lecteur, Writeur> : IDisposable 
    where Lecteur : class, IReader, new()
    where Writeur : class, IWriter, new()
  {
    #region Membre privés
    /// <summary>
    /// Le connecteur au hub
    /// </summary>
    private HubConnector connector;
    #endregion

    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="GenericClient{Lecteur,Writeur}"/>
    /// </summary>
    public GenericClient()
    {
      this.Readers = new Dictionary<string, Lecteur>();
      this.Writers = new Dictionary<string, Writeur>();
    }

    #region Properties
    /// <summary>
    /// L'adresse du hub SignalR
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// 1 pour lecteur, 2 pour encodeur, 3 pour les 2
    /// </summary>
    public int DriversFor { get; set; }

    #region For Debugg
    /// <summary>
    /// Est on connecté
    /// </summary>
    public bool Connected
    {
      get
      {
        return this.connector != null && this.connector.Connected == Microsoft.AspNet.SignalR.Client.ConnectionState.Connected;
      }
    }

    /// <summary>
    /// La liste des clé des Lecteurs
    /// </summary>
    public List<string> LecteursKeys
    {
      get
      {
        if (this.Readers != null)
        {
          return this.Readers.Keys.Select(x => x).ToList();
        }
        else
        {
          return new List<string>();
        }
      }
    }

    /// <summary>
    /// La liste des clé des encodeurs
    /// </summary>
    public List<string> EncodeursKeys
    {
      get
      {
        if (this.Writers != null)
        {
          return this.Writers.Keys.Select(x => x).ToList();
        }
        else
        {
          return new List<string>();
        }
      }
    }
    #endregion

    /// <summary>
    /// Les lecteurs par adresseIP
    /// </summary>
    protected System.Collections.Generic.Dictionary<string, Lecteur> Readers { get; set; }

    /// <summary>
    /// Les encodeurs de tag par adresseIP
    /// </summary>
    protected System.Collections.Generic.Dictionary<string, Writeur> Writers { get; set; }
    #endregion

    #region for debug
    /// <summary>
    /// Renvoie l'encodeur demandé
    /// </summary>
    /// <param name="key">La clé</param>
    /// <returns>L'encodeur demandé</returns>
    public Writeur GetEncodeur(string key)
    {
      if (this.Writers.ContainsKey(key))
      {
        return this.Writers[key];
      }
      else
      {
        return null;
      }
    }

    /// <summary>
    /// Renvoie le lecteur demandé
    /// </summary>
    /// <param name="key">La clé</param>
    /// <returns>La lecteur</returns>
    public Lecteur GetLecteur(string key)
    {
      if (this.Readers.ContainsKey(key))
      {
        return this.Readers[key];
      }
      else
      {
        return null;
      }
    }
    #endregion

    /// <summary>
    /// Nettoye l'objet
    /// </summary>
    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Démarre la connexion au hub
    /// </summary>
    /// <returns>True si on a démarré</returns>
    public virtual bool Connecte()
    {
      if (!this.CheckConfig())
      { // config invalide ==> on sort
        return false;
      }

      if (this.connector == null)
      { // première fois qu'on appelle l'objet n'existe pas !
        this.connector = new HubConnector(this.Url, this.DriversFor);
        this.connector.OnConnectedChanged += this.Connector_OnConnectedChanged;
        this.connector.OnAction += this.Connector_OnAction;
        this.connector.OnEncode += this.Connector_OnEncode;
        this.LogInfo(
          "Connexion au hub {0} pour {1}", 
          this.Url, 
          (this.DriversFor & 1) == 1 ? " piloter des lecteurs" : string.Empty,
          (this.DriversFor & 2) == 2 ? " piloter des encodeurs" : string.Empty);
      }

      if (this.connector.Connected == Microsoft.AspNet.SignalR.Client.ConnectionState.Disconnected)
      { // pas connecté ni en cours de connexion
        this.connector.Connecte(this.DriversFor);
      }

      return true;
    }

    /// <summary>
    /// Stoppe la connexion au hub
    /// </summary>
    public virtual void Deconnecte()
    {
      this.LogInfo("Déconnexion du hub {0}", this.Url);
     if (this.connector != null)
      {
        this.connector.Disconnecte();
      }
    }

    /// <summary>
    /// Nettoyage de l'objet
    /// </summary>
    /// <param name="p">paramètre magique</param>
    protected virtual void Dispose(bool p)
    {
      this.LogInfo("Dispose du client driver");
      if (this.connector != null)
      { // déconnexion du hub
        this.connector.Disconnecte();
      }

      this.CloseAndDisposeAllReadersAndWriters();
    }

    /// <summary>
    /// Vérifie que toute la config est correcte
    /// </summary>
    /// <returns>true si correcte false sinon</returns>
    protected abstract bool CheckConfig();

    /// <summary>
    /// Log une information
    /// </summary>
    /// <param name="template">Le texte formatté</param>
    /// <param name="args">Les infos</param>
    protected abstract void LogInfo(string template, params object[] args);

    /// <summary>
    /// Log une erreur
    /// </summary>
    /// <param name="template">Le texte formatté</param>
    /// <param name="args">Les infos</param>
    protected abstract void LogErreur(string template, params object[] args);
    
    /// <summary>
    /// Ajoute un lecteur (peut être surchargé)
    /// </summary>
    /// <param name="param">les paramètres d'appel XML</param>
    /// <returns>Le lecteur créé</returns>
    protected virtual Lecteur AddReader(SimpleReaderParameters param)
    {
      Lecteur reader = new Lecteur();
      reader.Parameters = param;
      reader.OnNotifie += this.Reader_OnNotifie;
      return reader;
    }

    /// <summary>
    /// Ajoute un encodeur
    /// </summary>
    /// <param name="param">Les paramètres d'appel XML</param>
    /// <returns>L'encodeur créé</returns>
    protected virtual Writeur AddWriter(SimpleReaderParameters param)
    {
      Writeur writer = new Writeur();
      writer.Parameters = param;
      writer.OnNotifie += this.Writer_OnNotifie;
      writer.OnProgress += this.Writer_OnProgress;
      return writer;
    }

    /// <summary>
    /// ferme et nettoie un lecteur
    /// (utilisé avant de démarrer un lecteur ou en quittnt l'application)
    /// </summary>
    /// <param name="reader">Le lecteur concerné</param>
    protected virtual void CloseAndDisposeReader(Lecteur reader)
    {
      if (reader != null)
      {
        if (reader.CanStop)
        { // fermeture de la com avec le lecteur
          reader.Stop();
        }

        reader.OnNotifie -= this.Reader_OnNotifie;
        ////reader.Dispose();
      }
    }

    #region Events en provenance du hub
    /// <summary>
    /// Réception d'un changement de statut de connexion sur le hub
    /// </summary>
    /// <param name="sender">qui appelle</param>
    /// <param name="e">Les paramètres de la demande</param>
    private void Connector_OnConnectedChanged(object sender, HubConnectorEventStatut e)
    {
      this.LogInfo(e.ToString());
      if (this.connector.Connected == Microsoft.AspNet.SignalR.Client.ConnectionState.Disconnected)
      {
        this.CloseAndDisposeAllReadersAndWriters();
      }
    }

    /// <summary>
    /// Réception d'une demande d'action d'un client
    /// </summary>
    /// <param name="sender">qui appelle</param>
    /// <param name="e">Les paramètres de la demande</param>
    private void Connector_OnAction(object sender, HubConnectorEventLecteur e)
    {
      if (e != null)
      {
        this.LogInfo("Reçu demande pour lecteur : {0}", e.ToString());
        Lecteur lect = this.GetReader(e.AdresseIp);
        switch (e.Action)
        {
          case EActionLecteur.Demarre:
            this.ProcessReaderDemarre(e, lect);
            break;
          case EActionLecteur.Stoppe:
            this.ProcessReaderStoppe(e, lect);
            break;
          case EActionLecteur.ResetLecture:
            this.ProcessReaderResetLectures(e, lect);
            break;
          case EActionLecteur.QueryStatut:
            this.ProcessQueryStatut(e, lect);
            break;
        }
      }
    }

    /// <summary>
    /// Recu une demande d'encodage 
    /// </summary>
    /// <param name="sender">qui appelle</param>
    /// <param name="e">Les paramètres de la demande</param>
    private void Connector_OnEncode(object sender, HubConnectorEventEncodeur e)
    {
      this.LogInfo("Reçu demande pour encodeur {0}", e.ToString());
      Writeur writer = this.GetWriter(e.AdresseIp);
      
      if (writer == null)
      { // faut le créer
        this.StartWriter(e, writer);
      }
      else
      { // c'est Ok on lance
        switch (e.Action)
        {
          case EActionEncode.TraiteCommande:
            this.ProcessWriterCommande(e, writer);
            break;
          case EActionEncode.TraiteAssemblage:
            this.ProcessWriterAssemblage(e, writer);
            break;
          case EActionEncode.CancelEncodage:
            this.ProcessWriterCancel(e, writer);
            break;
          case EActionEncode.QueryStatut:
            this.ProcessWriterQueryStatut(e, writer);
            break;
        }
      }
    }
    #endregion

    /// <summary>
    /// Nettoye tous les lecteurs
    /// </summary>
    private void CloseAndDisposeAllReadersAndWriters()
    {
      foreach (string key in this.Readers.Keys)
      {
        this.CloseAndDisposeReader(this.Readers[key]);
      }

      foreach (string key in this.Writers.Keys)
      {
        this.CloseAndDisposeWriter(this.Writers[key]);
      }
    }

    #region Méthodes pour les lecteurs
    /// <summary>
    /// Renvoie le lecteur existant à l'adresse IP
    /// </summary>
    /// <param name="adresseIp">l'adresse du lecteur cherché</param>
    /// <returns>le lecteur ou null s'il n'existe pas encore</returns>
    private Lecteur GetReader(string adresseIp)
    {
      Lecteur reader = default(Lecteur);
      if (this.Readers.ContainsKey(adresseIp))
      {
        reader = this.Readers[adresseIp];
      }

      return reader;
    }

    /// <summary>
    /// Traite un démarrage lecteur
    /// </summary>
    /// <param name="e">les infos sur l'action a mener</param>
    /// <param name="reader">Le lecteur s'il existe ou null sinon</param>
    private void ProcessReaderDemarre(HubConnectorEventLecteur e, Lecteur reader)
    {
      string msg;
      if (reader != null && !reader.CanStart)
      { // lecteur déjà en route !
        if (string.IsNullOrWhiteSpace(reader.ClientId) && !string.IsNullOrWhiteSpace(e.ClientId))
        { // il a démaré mais mal ==> on rattrape le coup par une réaffectation du lecteur au client
          msg = "Réattribution d'un lecteur démarré";
          this.LogInfo(msg);
          this.connector.Notify(e.ClientId, false, EActionLecteur.Demarre, msg);
          this.StartLecteur(e, reader);
        }
        else
        {
          msg = "Erreur lecteur déjà démarré";
          this.LogErreur(msg);
          this.connector.Notify(e.ClientId, true, EActionLecteur.Demarre, msg);
        }
      }
      else
      { // pas de lecteur ou il est stoppé : on le crée et on démarre
        if (string.IsNullOrWhiteSpace(e.ClientId))
        { // pas d'identifiant client
          msg = "Erreur démarrage lecteur impossible car identifiant client vide";
          this.LogErreur(msg);
          this.connector.Notify(e.ClientId, true, EActionLecteur.Demarre, msg);
        }
        else
        { // pas de soucis on crée et démarre le lecteur
          this.StartLecteur(e, reader);
        }
      }
    }

    /// <summary>
    /// Traite la demande d'arret d'un lecteur
    /// </summary>
    /// <param name="e">Les infos de la demande</param>
    /// <param name="reader">Le lecteur</param>
    private void ProcessReaderStoppe(HubConnectorEventLecteur e, Lecteur reader)
    {
      string msg;
      if (reader == null)
      { // demande d'arret alors qu'on a pas de lecteur en route ==> on ne fait rien
        this.LogInfo("Le lecteur n'est pas connu (commande ignorée)");
        return;
      }

      if (!reader.Running)
      { // lecteur déjà arrété
        this.LogInfo("Le lecteur n'est pas en route (commande ignorée)");
        return;
      }

      if (!reader.CanStop)
      {
        msg = "Arrêt impossible : Raison inconnue";
        this.LogErreur(msg);
        this.connector.Notify(e.ClientId, true, EActionLecteur.Stoppe, msg);
      }
      else
      { // arrêt demandé et lecteur en route
        if (string.IsNullOrWhiteSpace(reader.ClientId))
        { // uniquement si on a raté quelque chose : sinon on garde celui en mémoire
          reader.ClientId = e.ClientId;
        }

        if (reader.ClientId != e.ClientId)
        { // quelqu'un d'autre que le propiétaire essaie de stopper le lecteur ==> refuser
          msg = "Arrêt impossible : Le lecteur est utilisé par quelqu'un d'autre";
          this.LogErreur(msg);
          this.connector.Notify(e.ClientId, true, EActionLecteur.Stoppe, msg);
          return; // on n'oublie pas de sortir !!!
        }
        else
        {
          this.LogInfo("Arrêt en cours...");
          this.StoppeLecteur(reader);
        }
      }
    }

    /// <summary>
    /// Traite la demande de reset des lectures d'un lecteur
    /// </summary>
    /// <param name="e">Les infos de la demande</param>
    /// <param name="reader">Le lecteur</param>
    private void ProcessReaderResetLectures(HubConnectorEventLecteur e, Lecteur reader)
    {
      string msg = string.Format("Reset des lectures du lecteur {0} : ", e.AdresseIp);
      if (reader == null)
      {
        this.LogInfo("Lecteur pas connu (commande ignorée)");
        return;
      }

      if (!string.IsNullOrWhiteSpace(reader.ClientId) && reader.ClientId != e.ClientId)
      { // quelqu'un d'autre que le propiétaire essaie de stop le lecteur ==> refuser
        msg = "Reset impossible car le lecteur est utilisé par quelqu'un d'autre";
        this.LogErreur(msg);
        this.connector.Notify(e.ClientId, true, EActionLecteur.ResetLecture, msg);
      }
      else
      { // on transmet au fils
        reader.ResetLectures();
      }
    }

    /// <summary>
    /// Traite la demande d'état d'un lecteur
    /// </summary>
    /// <param name="e">Les infos de la demande</param>
    /// <param name="reader">Le lecteur</param>
    private void ProcessQueryStatut(HubConnectorEventLecteur e, Lecteur reader)
    {
      string msg;
      if (reader == null || !reader.Running)
      { // pas trouvé ou stoppé
        msg = "Lecteur stoppé";
        this.LogInfo(msg);
        this.connector.Notify(e.ClientId, false, EActionLecteur.Stoppe, msg);
      }
      else
      { // trouvé et donc en route
        msg = "Lecteur en route";
        this.LogInfo(msg);
        this.connector.Notify(e.ClientId, false, EActionLecteur.Demarre, msg);
      }
    }

    /// <summary>
    /// Démarre un lecteur
    /// </summary>
    /// <param name="e">infos de démarrage</param>
    /// <param name="reader">le lecteur</param>
    private void StartLecteur(HubConnectorEventLecteur e, Lecteur reader)
    {
      SimpleReaderParameters param = new SimpleReaderParameters(e.XmlParameter);
      SimpleReaderParameters.ValidInfo val = param.IsValid;
      if (val.Valid)
      {
        this.CloseAndDisposeReader(reader);

        reader = this.AddReader(param);
        if (this.Readers.ContainsKey(reader.AdresseIp))
        { // mise à jour
          this.Readers[reader.AdresseIp] = reader;
        }
        else
        { // ajout
          this.Readers.Add(reader.AdresseIp, reader);
        }

        // Démarrage des lectures
        this.LogInfo("Démarrage du lecteur {0}...", reader.AdresseIp);
        reader.Start(e.ClientId);
      }
      else
      { // Paramètres du lecteur non valides
        string id = e.ClientId;
        if (reader != null)
        {
          id = reader.ClientId;
        }

        // Notifie l'erreur sur le hub
        string msg = string.Format("Lecteur '{0}' : configuration non valide : {1}", param.AdresseIP, val.Message);
        this.LogErreur(msg);
        this.connector.Notify(id, true, EActionLecteur.Demarre, msg);
      }
    }

    /// <summary>
    /// Traite l'arrêt du lecteur
    /// </summary>
    /// <param name="reader">le lecteur</param>
    private void StoppeLecteur(Lecteur reader)
    {
      if (reader != null)
      {
        this.LogInfo("Arrêt du lecteur {0}...", reader.AdresseIp);
        reader.Stop();
      }
    }

    /// <summary>
    /// Lecteur notifie quelque chose d'important
    /// </summary>
    /// <param name="sender">Qui appelle</param>
    /// <param name="e">Les paramètres</param>
    private void Reader_OnNotifie(object sender, SimpleReaderEventArgs e)
    {
      Lecteur reader = sender as Lecteur;
      if (reader == null)
      { // on ne sait pas d'ou vient l'info ==> on sort
        return;
      }

      string msg = "Lecteur {0} notifie : {1}";
      switch (e.Action)
      {
        case EEtatLecteur.Tag:
          // lecture d'un nouveau tag
          this.LogInfo(msg, reader.AdresseIp, e);
          this.connector.SendTag(reader.ClientId, e.Contenu);
          break;
        case EEtatLecteur.Start:
          // Gère l'état du lecteur au démarrage
          switch (e.ActionResult)
          {
            case ELogType.RapportOk:
              this.LogInfo(msg, reader.AdresseIp, e);
              this.connector.Notify(reader.ClientId, false, EActionLecteur.Demarre, "Lecteur en route");
              break;
            case ELogType.Erreur:
              this.LogErreur(msg, reader.AdresseIp, e);
              this.connector.Notify(reader.ClientId, true, EActionLecteur.Demarre, e.Contenu);
              break;
          }

          break;
        case EEtatLecteur.Stop:
          // gère l'état du lecteur au stop
          switch (e.ActionResult)
          {
            case ELogType.RapportOk:
              this.LogInfo(msg, reader.AdresseIp, e);
              this.connector.Notify(reader.ClientId, false, EActionLecteur.Stoppe, "Lecteur stoppé");
              reader.ClientId = string.Empty;
              break;
            default:
              this.LogErreur(msg, reader.AdresseIp, e);
              this.connector.Notify(reader.ClientId, true, EActionLecteur.Stoppe, e.Contenu);
              break;
          }

          break;
        case EEtatLecteur.Reset:
          switch (e.ActionResult)
          {
            case ELogType.RapportOk:
              this.LogInfo(msg, reader.AdresseIp, e);
              this.connector.Notify(reader.ClientId, false, EActionLecteur.ResetLecture, "Lectures Initialisées");
              break;
            default:
              this.LogErreur(msg, reader.AdresseIp, e);
              this.connector.Notify(reader.ClientId, true, EActionLecteur.ResetLecture, e.Contenu);
              break;
          }

          break;
      }
    }
    #endregion

    #region Méthodes pour les encodeurs
    /// <summary>
    /// Renvoie l'encodeur existant à l'adresse IP
    /// </summary>
    /// <param name="adresseIp">l'adresse de l'encodeur cherché</param>
    /// <returns>l'necodeur ou null s'il n'existe pas encore</returns>
    private Writeur GetWriter(string adresseIp)
    {
      Writeur writer = default(Writeur);
      if (this.Writers.ContainsKey(adresseIp))
      {
        writer = this.Writers[adresseIp];
      }

      return writer;
    }

    /// <summary>
    /// ferme et nettoie une imprimante
    /// (utilisé avant de démarrer ou en quittnt l'application)
    /// </summary>
    /// <param name="writer">L'imprimante concernée</param>
    private void CloseAndDisposeWriter(Writeur writer)
    {
      if (writer != null)
      {
        writer.Cancel();
        writer.OnNotifie -= this.Writer_OnNotifie;
        writer.OnProgress -= this.Writer_OnProgress;
        ////writer.Dispose();
      }
    }

    /// <summary>
    /// Démarre un writer
    /// </summary>
    /// <param name="e">les infos de la demande</param>
    /// <param name="writer">l'encodeur concerné</param>
    private void StartWriter(HubConnectorEventEncodeur e, Writeur writer)
    {
      SimpleReaderParameters param = new SimpleReaderParameters(e.XmlParameter);
      SimpleReaderParameters.ValidInfo val = param.IsValid;
      if (val.Valid)
      { // c'est OK on crée le write et on lance l'impression
        this.CloseAndDisposeWriter(writer);

        writer = this.AddWriter(param);
        this.Writers.Add(writer.AdresseIp, writer);
        if (this.Writers.ContainsKey(writer.AdresseIp))
        { // mise à jour
          this.Writers[writer.AdresseIp] = writer;
        }
        else
        { // ajout
          this.Writers.Add(writer.AdresseIp, writer);
        }

        switch (e.Action)
        {
          case EActionEncode.TraiteCommande:
            this.ProcessWriterCommande(e, writer);
            break;
          case EActionEncode.TraiteAssemblage:
            this.ProcessWriterAssemblage(e, writer);
            break;
          case EActionEncode.CancelEncodage:
            this.ProcessWriterCancel(e, writer);
            break;
          case EActionEncode.QueryStatut:
            this.ProcessWriterQueryStatut(e, writer);
            break;
        }
      }
      else
      {
        string id = e.ClientId;
        if (writer != null)
        {
          id = writer.ClientId;
        }

        string msg = string.Format("Démarrage encodeur avec des paramètres non valide {0}", param.ToString());
        this.LogErreur(msg);
        this.connector.Rapporte(id, true, e.Action, e.Cle, msg);
      }
    }

    /// <summary>
    /// Traite une demande de commande
    /// </summary>
    /// <param name="e">les infos de la demande</param>
    /// <param name="writer">l'encodeur concerné</param>
    private void ProcessWriterCommande(HubConnectorEventEncodeur e, Writeur writer)
    {
      if (writer.Busy(e))
      { // Erreur une autre impression est déjà en cours
        string msg = "Encodeur occupé par une autre tâche";
        this.LogErreur(msg);
        this.connector.Rapporte(e.ClientId, true, e.Action, e.Cle, msg);
      }
      else
      {
        this.LogInfo("Démarrage de l'encodage de la commande {0}...", e.Cle);
        writer.ProcessCommande(e.ClientId, e.Cle);
      }
    }

    /// <summary>
    /// Traite une demande d'assemblage
    /// </summary>
    /// <param name="e">les infos de la demande</param>
    /// <param name="writer">l'encodeur concerné</param>
    private void ProcessWriterAssemblage(HubConnectorEventEncodeur e, Writeur writer)
    {
      if (writer.Busy(e))
      { // Erreur une autre impression est déjà en cours
        string msg = "Encodeur occupé par une autre tâche";
        this.LogErreur(msg);
        this.connector.Rapporte(e.ClientId, true, e.Action, e.Cle, msg);
      }
      else
      {
        this.LogInfo("Démarrage de l'encodage de l'assemblage {0}...", e.Cle);
        writer.ProcessAssemblage(e.ClientId, e.Cle);
      }
    }

    /// <summary>
    /// Traite une demande d'annulation de l'opération en cours
    /// </summary>
    /// <param name="e">les infos de la demande</param>
    /// <param name="writer">l'encodeur concerné</param>
    private void ProcessWriterCancel(HubConnectorEventEncodeur e, Writeur writer)
    {
      string msg;
      if (writer.Cle > 0 && writer.Cle == e.Cle)
      { // Le lecteur est en cours de traitement sur la bonne clé : Lance l'annulation
        writer.Cancel();
      }
      else
      { // y a problème
        if (writer.Cle == 0)
        { // le lecteur ne fait rien en ce moment
          msg = "Demande ignorée car l'encodeur est stoppé";
        }
        else if (e.Cle == 0)
        { // la clé n'est pas valide
          msg = "Annulation impossible pas de clé fournie";
        }
        else
        {
          msg = string.Format("Demande ignorée car l'encodeur traite une autre clé {0}", writer.Cle);
        }

        this.LogErreur(msg);
        this.connector.Rapporte(e.ClientId, true, EActionEncode.CancelEncodage, e.Cle, msg);
      }
    }

    /// <summary>
    /// Demande l'état du lecteur
    /// </summary>
    /// <param name="e">les infos de la demande</param>
    /// <param name="writer">l'encodeur concerné</param>
    private void ProcessWriterQueryStatut(HubConnectorEventEncodeur e, Writeur writer)
    {
      int cle = writer.Cle;
      EActionEncode act = writer.Action;
      if (act == EActionEncode.QueryStatut)
      {
        act = EActionEncode.Aucune;
        cle = 0;
      }
      else if (act == EActionEncode.Aucune)
      {
        cle = 0;
      }

      this.connector.Progresse(e.ClientId, act, cle, writer.Index, writer.Total);
    }
   
    /// <summary>
    /// L'encodeur notifie un succès ou une erreur
    /// </summary>
    /// <param name="sender">l'encodeur qui notifie</param>
    /// <param name="e">les infos</param>
    private void Writer_OnNotifie(object sender, SimpleEncodeurEventArgs e)
    {
      Writeur writer = sender as Writeur;
      if (writer == null || e == null)
      { // c'est pas un writer qui nous parle, ou y a pas les infos nécessaires : on se casse !
        return;
      }

      this.LogInfo("L'encodeur {0} notifie : {1}", writer.AdresseIp, e);
      this.connector.Rapporte(e.ClientId, e.Erreur, e.Action, e.Cle, e.Message);
    }

    /// <summary>
    /// L'encodeur notifie une progression dans son travail
    /// </summary>
    /// <param name="sender">l'encodeur qui appelle</param>
    /// <param name="e">les infos de pregression</param>
    private void Writer_OnProgress(object sender, SimpleEncodeurProgressArgs e)
    {
      Writeur writer = sender as Writeur;
      if (writer == null || e == null)
      { // c'est pas un writer qui nous parle, ou y a pas les infos nécessaires : on se casse !
        return;
      }

      this.LogInfo("L'encodeur {0} progresse : {1}", writer.AdresseIp, e);
      this.connector.Progresse(e.ClientId, e.Action, e.Cle, e.Index, e.Total);
    }
    #endregion
  }
}
