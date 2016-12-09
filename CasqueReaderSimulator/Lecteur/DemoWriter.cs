using System;
using System.Collections.Generic;
using System.Drawing;
using System.Timers;
using System.Windows.Forms;
using CasqueLib.Buisness.Encode;
using CasqueLib.Common;
using CasqueLib.Matos.ClientOwin;
using CasqueLib.Matos.Encodeur;
using CasqueLib.Matos.Lecteur;
using CasqueLib.Matos.ServerOwin;

namespace CasqueReaderSimulator.Lecteur
{
  /// <summary>
  /// classe qui simule un writer de tags
  /// </summary>
  public partial class DemoWriter : UserControl, IWriter
  {
    #region Membres privés
    /// <summary>
    /// Timer pour automatisation de la simulation
    /// </summary>
    private System.Timers.Timer timer1;

    /// <summary>
    /// Le client recu dans une demande
    /// </summary>
    private List<string> recuClientIds;

    /// <summary>
    /// L'action demandée
    /// </summary>
    private EActionEncode recuAction;

    /// <summary>
    /// La clé reçue dans une demande
    /// </summary>
    private int recuCle;
    
    /// <summary>
    /// Indique que le lecteur est en cours de traitement d'une demande
    /// </summary>
    private bool busy;
    #endregion

    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="DemoWriter"/>
    /// </summary>
    public DemoWriter()
    {
      this.InitializeComponent();

      this.busy = false;
      this.Action = EActionEncode.Aucune;
      this.recuClientIds = new List<string>();
      
      this.chkReponseAuto.Checked = Properties.Settings.Default.RepondOkStart;
      this.timer1 = new System.Timers.Timer();
      this.timer1.Elapsed += this.Timer1Elapsed;
      this.timer1.Interval = 1000; // 1 seconde
      this.ClearInfos();
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
      { // on ne fait rien ==> demarre le taf
        this.recuAction = EActionEncode.TraiteCommande;
        this.recuClientIds.Clear();
        this.recuClientIds.Add(clientId);
        this.recuCle = comdId;
        if (this.chkReponseAuto.Checked)
        {
          this.BtRepondOk_Click(null, null);
        }
        else
        {
          this.GereBouton();
        }
      }
      else if (this.Cle != comdId || this.Action != EActionEncode.TraiteCommande)
      { // déjà occupé et c'est pas la commande demandée ==> rejet
        this.OnNotifie(this, new SimpleEncodeurEventArgs(clientId, EActionEncode.TraiteCommande, comdId, true, "Encodeur occupé"));
      }
      else
      { // ajout d'un client
        this.recuClientIds.Add(clientId);

        if (this.Index > 1)
        { // notifie le nouveau client d'ou on en est
          this.OnProgress(this, new SimpleEncodeurProgressArgs(clientId, this.Action, this.Cle, this.Index - 1, this.Total));
        }
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
      { // on ne fait rien ==> demarre le taf
        this.recuAction = EActionEncode.TraiteAssemblage;
        this.recuClientIds.Clear();
        this.recuClientIds.Add(clientId);
        this.recuCle = asseId;
        if (this.chkReponseAuto.Checked)
        {
          this.BtRepondOk_Click(null, null);
        }
        else if (this.recuCle != asseId)
        { // déjà occupé et c'est pas le même assemblage ==> rejet
          this.GereBouton();
        }
      }
      else if (this.Cle != asseId || this.Action != EActionEncode.TraiteAssemblage)
      { // déjà occupé et c'est pas la commande demandée ==> rejet
        this.OnNotifie(this, new SimpleEncodeurEventArgs(clientId, EActionEncode.TraiteAssemblage, asseId, true, "Encodeur occupé"));
      }
      else
      { // ajout d'un client
        this.recuClientIds.Add(clientId);

        if (this.Index > 1)
        { // notifie le nouveau client d'ou on en est
          this.OnProgress(this, new SimpleEncodeurProgressArgs(clientId, this.Action, this.Cle, this.Index - 1, this.Total));
        }
      }
    }

    /// <summary>
    /// Execute l'ordre d'annulation de l'encodage en cours
    /// La méthode n'a pas à se poser de question si l'arrêt est justifié ou pas c'est l'appellant qui le fait !
    /// </summary>
    public void Cancel()
    {
      if (this.busy)
      { // c'est pas a cet objet de décider s'il faut ou pas le faire : on fait si cela tiend la route.
        this.recuAction = EActionEncode.CancelEncodage;
        if (this.chkReponseAuto.Checked)
        {
          this.BtRepondOk_Click(null, null);
        }
        else
        {
          this.GereBouton();
        }
      }
    }
    #endregion

    #region Events de l'interface
    /// <summary>
    /// Actualise les changements dans l'interface
    /// </summary>
    /// <param name="sender">qui appelle</param>
    /// <param name="e">Paramètre inutile</param>
    private void GereBouton(object sender, EventArgs e)
    {
      this.GereBouton();
    }

    /// <summary>
    /// Simule une réponde Ok à une demande reçue
    /// </summary>
    /// <param name="sender">qui appelle</param>
    /// <param name="e">Paramètre inutile</param>
    private void BtRepondOk_Click(object sender, EventArgs e)
    {
      switch (this.recuAction)
      {
        case EActionEncode.TraiteCommande:
          if (this.recuClientIds.Count == 1)
          { // première demande
            this.busy = true;
            this.ClientId = this.recuClientIds[0];
            this.Action = EActionEncode.TraiteCommande;
            this.Cle = this.recuCle;
            this.LoadCommande();
            if (this.Total > 0)
            { // c'est bon c'est une commande dans le bon statut
              this.OnProgress(this, new SimpleEncodeurProgressArgs(this.ClientId, this.Action, this.Cle, this.Index, this.Total));
              this.Index = 1;
              this.ClearRecu();
              if (this.chkReponseAuto.Checked)
              {
                this.timer1.Start();
              }
            }
            else if (this.chkReponseAuto.Checked)
            { // Erreur commande non trouvée
              this.BtNotifieKo_Click(null, null);
            }
          }
          else
          {
            if (this.Total > 0)
            { // c'est bon c'est une commande dans le bon statut
              foreach (string cli in this.recuClientIds)
              {
                this.OnProgress(this, new SimpleEncodeurProgressArgs(cli, this.Action, this.Cle, this.Index, this.Total));
              }
            }
          }

          break;
        case EActionEncode.TraiteAssemblage:
          if (this.recuClientIds.Count == 1)
          { // première demande
            this.busy = true;
            this.ClientId = this.recuClientIds[0];
            this.Action = EActionEncode.TraiteAssemblage;
            this.Cle = this.recuCle;
            this.LoadAssemblage();
            if (this.Total > 0)
            { // c'est bon l'assemblage existe 
              this.OnProgress(this, new SimpleEncodeurProgressArgs(this.ClientId, this.Action, this.Cle, this.Index, this.Total));
              this.Index = 1;
              this.ClearRecu();
              if (this.chkReponseAuto.Checked)
              {
                this.timer1.Start();
              }
            }
            else if (this.chkReponseAuto.Checked)
            { // Erreur assemblage non trouvé
              this.BtNotifieKo_Click(null, null);
            }
          }
          else
          {
            if (this.Total > 0)
            { // c'est bon l'assemblage existe 
              foreach (string cli in this.recuClientIds)
              {
                this.OnProgress(this, new SimpleEncodeurProgressArgs(cli, this.Action, this.Cle, this.Index, this.Total));
              }
            }
          }

          break;
        case EActionEncode.CancelEncodage:
          this.busy = false;
          if (this.Action == EActionEncode.TraiteCommande)
          {
            this.CancelCommande();
          }

          this.OnNotifie(this, new SimpleEncodeurEventArgs(this.ClientId, this.Action, this.Cle, true, "Cancel demandé"));
          this.ClearInfos();
          break;
      }

      this.GereBouton();
    }

    /// <summary>
    /// simule une réponde Ko à une demande reçue
    /// </summary>
    /// <param name="sender">qui appelle</param>
    /// <param name="e">Paramètre inutile</param>
    private void BtRepondKo_Click(object sender, EventArgs e)
    {
      if (this.recuClientIds.Count > 0)
      {
        this.OnNotifie(this, new SimpleEncodeurEventArgs(this.recuClientIds[0], this.recuAction, this.recuCle, true, this.txtRepondKo.Text));
        this.ClearRecu();
        this.GereBouton();
      }
    }

    /// <summary>
    /// Simule la fin Ok d'un encodage
    /// </summary>
    /// <param name="sender">qui appelle</param>
    /// <param name="e">Paramètre inutile</param>
    private void BtNotifieOk_Click(object sender, EventArgs e)
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

      foreach (string cli in this.recuClientIds)
      { // on notifie tous les clients
        this.OnNotifie(this, new SimpleEncodeurEventArgs(cli, this.Action, this.Cle, false, "fini"));
      }

      this.ClearInfos();
      this.GereBouton();
    }

    /// <summary>
    /// Simule la fin Ko d'un encodage
    /// </summary>
    /// <param name="sender">qui appelle</param>
    /// <param name="e">Paramètre inutile</param>
    private void BtNotifieKo_Click(object sender, EventArgs e)
    {
      this.busy = false;
      if (this.Total > 0 && this.Action == EActionEncode.TraiteCommande)
      {
        this.CancelCommande();
      }

      foreach (string cli in this.recuClientIds)
      { // on notifie tous les clients
        this.OnNotifie(this, new SimpleEncodeurEventArgs(cli, this.Action, this.Cle, true, this.txtNotifieKo.Text));
      }

      this.ClearInfos();
      this.GereBouton();
    }

    /// <summary>
    /// Simule l'nevoie d'un event de progression
    /// </summary>
    /// <param name="sender">qui appelle</param>
    /// <param name="e">Paramètre inutile</param>
    private void BtNotifieProgression_Click(object sender, EventArgs e)
    {
      if (this.Index <= this.Total)
      {
        if (this.Action == EActionEncode.TraiteCommande)
        {
          this.StepCommande();
        }

        foreach (string cli in this.recuClientIds)
        { // on notifie tous les clients
          this.OnProgress(this, new SimpleEncodeurProgressArgs(cli, this.Action, this.Cle, this.Index, this.Total));
        }

        this.Index++;
      }

      this.GereBouton();
    }

    /// <summary>
    /// Le timer se déclenche
    /// </summary>
    /// <param name="sender">qui apelle</param>
    /// <param name="e">les infos</param>
    private void Timer1Elapsed(object sender, ElapsedEventArgs e)
    {
      this.timer1.Stop();
      if (this.busy && this.Cle > 0)
      {
        if (this.Index <= this.Total)
        {
          this.BtNotifieProgression_Click(null, null);
          this.timer1.Start();
        }
        else
        { // fini
          this.BtNotifieOk_Click(null, null);
        }
      }
    }
    #endregion

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
        // TODO here : manipuler les infos de l'assemblage pour ecrire dans le tag
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

      this.ClearRecu();
    }

    /// <summary>
    /// Efface les infos reçues
    /// </summary>
    private void ClearRecu()
    {
      this.recuAction = EActionEncode.Aucune;
      this.recuCle = 0;
    }

    /// <summary>
    /// Gère le look du controle
    /// </summary>
    private void GereBouton()
    {
      if (this.InvokeRequired)
      {
        this.Invoke((Action)(() => { this.ProcessGererBouton(); }));
      }
      else
      {
        this.ProcessGererBouton();
      }
    }

    /// <summary>
    /// Gère le look du controle
    /// </summary>
    private void ProcessGererBouton()
    {
      this.lblAdresseIp.Text = this.AdresseIp;

      if (this.busy)
      {
        this.pnlBusy.BackColor = Color.Green;
        this.lblClientTitre.Visible = true;
        this.lblClient.Text = this.ClientId;
        if (this.Action != EActionEncode.Aucune)
        {
          this.lblStatut.Text = string.Format("Action en cours : {0} Cle = {1}", this.Action.GetName(), this.Cle);
        }
        else
        {
          this.lblStatut.Text = "Attend une commande ??";
        }
      }
      else
      {
        this.pnlBusy.BackColor = Color.Red;
        this.lblClientTitre.Visible = false;
        this.lblClient.Text = string.Empty;
        this.lblStatut.Text = "Attend une commande";
      }

      if (this.recuAction != EActionEncode.Aucune)
      { // une demande est en cours
        if (this.txtRepondKo.InvokeRequired)
        {
          this.txtRepondKo.Invoke((Action)(() => { this.UpdateTxt(); }));
        }
        else
        {
          this.UpdateTxt();
        }

        this.grpReception.Visible = true;
        this.lblCommande.Text = string.Format("Reçu demande : {0} Cle = {1}", this.recuAction.GetName(), this.recuCle);
        this.btRepondKo.Enabled = !string.IsNullOrEmpty(this.txtRepondKo.Text);
      }
      else
      { // pas de demande
        this.grpReception.Visible = false;
      }

      if (this.busy)
      { // le writer est en cours d'encodage
        this.grpOn.Visible = true;
        this.txtIndex.Text = this.Index.ToString();
        this.txtTotal.Text = this.Total.ToString();
        this.btNotifieKo.Enabled = !string.IsNullOrEmpty(this.txtNotifieKo.Text);
        this.btNotifieProgression.Enabled = this.Index >= 0 && this.Total > 0 && this.Index <= this.Total;
      }
      else
      {
        this.grpOn.Visible = false;
      }
    }

    /// <summary>
    /// Met à jour le texte
    /// </summary>
    private void UpdateTxt()
    {
      switch (this.recuAction)
      {
        case EActionEncode.TraiteCommande:
          this.txtRepondKo.Text = "Erreur de traitement commande";
          break;
        case EActionEncode.TraiteAssemblage:
          this.txtRepondKo.Text = "Erreur de traitement assemblage";
          break;
        case EActionEncode.CancelEncodage:
          this.txtRepondKo.Text = "Impossible d'annuler l'oparion en cours";
          break;
      }
    }
  }
}
