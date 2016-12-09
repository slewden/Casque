using System;
using System.Timers;
using CasqueLib.Buisness.Encode;
using CasqueLib.Common;
using CasqueLib.Matos.ClientOwin;
using CasqueLib.Matos.Encodeur;
using CasqueLib.Matos.Lecteur;
using CasqueLib.Matos.ServerOwin;

namespace CasqueServeur.Lecteur
{
  /// <summary>
  /// Classe qui encapsule et met en oeuvre un Writer de tags !
  /// A utiliser pour de la lecture écriture d'infos dans un tag
  /// </summary>
  public class SimpleWriter : IWriter
  {
    #region Membres
    /// <summary>
    /// Pour Tests : un timer
    /// </summary>
    private Timer timer1;

    /// <summary>
    /// L'encode traite une commande ou assemblage
    /// </summary>
    private bool busy;
    #endregion

    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="SimpleWriter"/>
    /// </summary>
    public SimpleWriter()
    {
      this.busy = false;
      this.Action = EActionEncode.Aucune;

      this.timer1 = new Timer();
      this.timer1.Elapsed += this.Timer1Elapsed;
      this.timer1.Interval = 1500; // 1.5 seconde
    }

    #region Events & Properties
    /// <summary>
    /// Evenement pour notifier que quelque chose est fini d'encodé (fin ok ou ko)
    /// </summary>
    public event EventHandler<SimpleEncodeurEventArgs> OnNotifie;

    /// <summary>
    /// Evenement pour notifier la progression d'encodage
    /// </summary>
    public event EventHandler<SimpleEncodeurProgressArgs> OnProgress;

    /// <summary>
    /// Les paramètres du writer
    /// </summary>
    public SimpleReaderParameters Parameters { get; set; }

    /// <summary>
    /// Le client qui a fait la demande
    /// </summary>
    public string ClientId { get; set; }

    /// <summary>
    /// L'action en cours
    /// </summary>
    public EActionEncode Action { get; private set; }

    /// <summary>
    /// la clé associée à l'action
    /// </summary>
    public int Cle { get; private set; }

    /// <summary>
    /// Index de progression
    /// </summary>
    public int Index { get; private set; }

    /// <summary>
    /// Total d'étapes prévues
    /// </summary>
    public int Total { get; private set; }

    /// <summary>
    /// L'adresse IP du lecteur
    /// </summary>
    public string AdresseIp
    {
      get
      {
        if (this.Parameters == null)
        {
          return string.Empty;
        }
        else
        {
          return this.Parameters.AdresseIP;
        }
      }
    }
    #endregion

    #region Méthodes publiques
    /// <summary>
    /// Indique si l'encodeur est en cours de traitement d'une demande non compatible avec celle en paramètre
    /// </summary>
    /// <param name="e">les infos de la demande</param>
    /// <returns>true si l'encodeur ne peut pas traiter la demande</returns>
    public bool Busy(HubConnectorEventEncodeur e)
    {
      if (!this.busy)
      {
        return false;
      }
      else
      {
        return e.Action != this.Action || e.Cle != this.Cle;
      }
    }

    /// <summary>
    /// Traite une demande de génération de commande
    /// </summary>
    /// <param name="clientId">L'identifiant du client à notifier</param>
    /// <param name="comdId">La clé de la commande</param>
    public void ProcessCommande(string clientId, int comdId)
    {
      if (!this.busy)
      {
        this.busy = true;
        this.ClientId = clientId;
        this.Action = EActionEncode.TraiteCommande;
        this.Cle = comdId;

        this.LoadCommande();
        if (this.Total > 0)
        { // c'est bon c'est une commande dans le bon statut
          this.OnProgress(this, new SimpleEncodeurProgressArgs(this.ClientId, this.Action, this.Cle, this.Index, this.Total));
          this.Index = 1;
          this.timer1.Start();
        }
        else
        { // Erreur commande non trouvée
          this.NotifieKo("Commande non trouvée : clé invalide");
        }
      }
      else
      { // déjà occupé
        this.OnNotifie(this, new SimpleEncodeurEventArgs(clientId, EActionEncode.TraiteCommande, comdId, true, "Encodeur occupé"));
      }
    }

    /// <summary>
    /// Traite une demande de génération d'un tag d'assemblage
    /// </summary>
    /// <param name="clientId">L'identifiant du client à notifier</param>
    /// <param name="asseId">La clé de l'assemblage</param>
    public void ProcessAssemblage(string clientId, int asseId)
    {
      if (!this.busy)
      {
        this.busy = true;
        this.ClientId = clientId;
        this.Action = EActionEncode.TraiteAssemblage;
        this.Cle = asseId;
        this.LoadAssemblage();
        if (this.Total > 0)
        { // c'est bon l'assemblage existe 
          this.OnProgress(this, new SimpleEncodeurProgressArgs(this.ClientId, this.Action, this.Cle, this.Index, this.Total));
          this.Index = 1;
          this.timer1.Start();
        }
        else
        { // Erreur assemblage non trouvé
          this.NotifieKo("Assemblage non trouvé : clé invalide");
        }
      }
      else
      { // déjà occupé
        this.OnNotifie(this, new SimpleEncodeurEventArgs(clientId, EActionEncode.TraiteAssemblage, asseId, true, "Encodeur occupé"));
      }
    }

    /// <summary>
    /// Execute l'ordre d'annulation de l'encodage en cours
    /// La méthode n'a pas à se poser de question si l'arrêt est justifié ou pas c'est l'appellant qui le fait !
    /// </summary>
    public void Cancel()
    {
      if (this.busy)
      { // c'est pas a cet objet de décider s'il 
        this.busy = false;
        if (this.Action == EActionEncode.TraiteCommande)
        {
          this.CancelCommande();
        }

        this.OnNotifie(this, new SimpleEncodeurEventArgs(this.ClientId, this.Action, this.Cle, true, "Cancel demandé"));
        this.ClearInfos();
      }
    }

    /// <summary>
    /// Nettoie l'objet
    /// </summary>
    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize(this);
    }
    #endregion

    /// <summary>
    /// Nettoie l'objet
    /// </summary>
    /// <param name="p">Argument magique</param>
    protected virtual void Dispose(bool p)
    {
      if (this.timer1 != null)
      {
        this.timer1.Dispose();
      }

      // TODO : Dispose des ressoucres du Writer de tags
    }

    /// <summary>
    /// L'encodage est fini : initialise un minimum l'objet
    /// </summary>
    private void NotifieFiniOk()
    {
      this.busy = false;
      if (this.Action == EActionEncode.TraiteCommande)
      {
        this.EndCommande();
      }
      else if (this.Action == EActionEncode.TraiteAssemblage)
      {
        this.EndAssemblage();
      }

      this.OnNotifie(this, new SimpleEncodeurEventArgs(this.ClientId, this.Action, this.Cle, false, "fini"));
      this.ClearInfos();
    }

    /// <summary>
    /// Signale que le traitement est KO
    /// </summary>
    /// <param name="message">Le message à ntotifier</param>
    private void NotifieKo(string message)
    {
      this.busy = false;
      if (this.Total > 0 && this.Action == EActionEncode.TraiteCommande)
      {
        this.CancelCommande();
      }

      this.OnNotifie(this, new SimpleEncodeurEventArgs(this.ClientId, this.Action, this.Cle, true, message));
      this.ClearInfos();
    }

    /// <summary>
    /// Notifie une progression
    /// </summary>
    private void NotifieStep()
    {
      if (this.Index <= this.Total)
      {
        if (this.Action == EActionEncode.TraiteCommande)
        {
          this.StepCommande();
        }

        this.OnProgress(this, new SimpleEncodeurProgressArgs(this.ClientId, this.Action, this.Cle, this.Index, this.Total));
        this.Index++;
      }
    }

    /// <summary>
    /// RAZ des infos de l'encodeur
    /// </summary>
    private void ClearInfos()
    {
      this.busy = false;
      this.Action = EActionEncode.Aucune;
      this.ClientId = string.Empty;
      this.Cle = 0;
      this.Index = 0;
      this.Total = 0;
    }

    /// <summary>
    /// Pour Tests : Simule la fin d'un encodage de tag
    /// </summary>
    /// <param name="sender">Le timer</param>
    /// <param name="e">les infos de temps passé</param>
    private void Timer1Elapsed(object sender, ElapsedEventArgs e)
    {
      this.timer1.Stop();
      if (this.busy && this.Cle > 0)
      {
        if (this.Index <= this.Total)
        {
          this.NotifieStep();
          this.timer1.Start();
        }
        else
        { // fini
          this.NotifieFiniOk();
        }
      }
    }

    #region BDD
    /// <summary>
    /// Démarre et charge les infos de la commande en cours
    /// </summary>
    private void LoadCommande()
    {
      EncodeCommande cmd = null;
      using (FConnexion cnn = new FConnexion())
      {
        cmd = EncodeCommande.Start(cnn.Db, this.Cle);
      }

      if (cmd != null)
      {
        this.Index = 0;
        this.Total = cmd.NombreTag;
      }
      else
      { // on s'assure que index soit supérieur à total (qui doit être à 0 !!)
        this.Index = 1;
        this.Total = 0;
      }
    }

    /// <summary>
    /// Ajoute une étape à un traitement de commande
    /// </summary>
    private void StepCommande()
    {
      using (FConnexion cnn = new FConnexion())
      {
        // TODO : Loader les infos à encoder et lancer l'encodage de celles-ci
        EncodeCommande.Step(cnn.Db, this.Cle, this.Index);
      }
    }

    /// <summary>
    /// Termine une commande en cours
    /// </summary>
    private void EndCommande()
    {
      using (FConnexion cnn = new FConnexion())
      {
        EncodeCommande.Stop(cnn.Db, this.Cle);
      }
    }

    /// <summary>
    /// Annule tout ce qui a été fait en base sur la commande fournie
    /// </summary>
    private void CancelCommande()
    {
      using (FConnexion cnn = new FConnexion())
      {
        EncodeCommande.Cancel(cnn.Db, this.Cle);
      }
    }

    /// <summary>
    /// Charge les infos de l'assemblage en cours
    /// </summary>
    private void LoadAssemblage()
    {
      EncodeAssemblage ass = null;
      using (FConnexion cnn = new FConnexion())
      {
        ass = EncodeAssemblage.Start(cnn.Db, this.Cle);
      }

      if (ass != null)
      {
        // TODO : manipuler ici les infos de l'assemblage pour ecrire dans le tag
        this.Index = 0;
        this.Total = 1;
      }
      else
      { // on s'assure que index soit supérieur à total (qui doit être à 0 !!)
        this.Index = 1;
        this.Total = 0;
      }
    }

    /// <summary>
    /// Termine un assemblage en cours
    /// </summary>
    private void EndAssemblage()
    {
      using (FConnexion cnn = new FConnexion())
      {
        EncodeAssemblage.Stop(cnn.Db, this.Cle);
      }
    }
    #endregion
  }
}
