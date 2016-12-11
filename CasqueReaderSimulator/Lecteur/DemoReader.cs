using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CasqueLib.Matos.ClientOwin;
using CasqueLib.Matos.Lecteur;

namespace CasqueReaderSimulator.Lecteur
{
  /// <summary>
  /// Classe pour le simulateur de lecteur
  /// </summary>
  public partial class DemoReader : UserControl, IReader
  {
    #region Membres
    /// <summary>
    /// La liste des tag envoyées au client
    /// </summary>
    private List<string> lectures;

    /// <summary>
    /// le client demandé
    /// </summary>
    private string recuClientId;

    /// <summary>
    /// L'action recu en attente de décision de l'utilisateur
    /// </summary>
    private EEtatLecteur recuAction;
    #endregion

    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="DemoReader"/>
    /// </summary>
    public DemoReader()
    {
      this.InitializeComponent();

      this.chkReponseAuto.Checked = Properties.Settings.Default.RepondOkStart;

      this.recuAction = EEtatLecteur.KeepAlive; // utilisé pour dire aucune !
      this.ClientId = string.Empty;
      this.Parameters = null;
      this.lectures = new List<string>();
      this.Running = false;
    }

    #region Events & Properties
    /// <summary>
    /// Evenement pour notifier que quelque chose est arrivé sur le lecteur
    /// </summary>
    public event EventHandler<SimpleReaderEventArgs> OnNotifie;

    /// <summary>
    /// Les paramètre du lecteur
    /// </summary>
    public SimpleReaderParameters Parameters { get; set; }

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

    /// <summary>
    /// Le lecteur est démarré et est en cours de lecture
    /// </summary>
    public bool Running { get; private set; }

    /// <summary>
    /// Mémorise le client a qui on doit répondre
    /// </summary>
    public string ClientId { get; set; }

    /// <summary>
    /// On peut démarrer le lecteur 
    /// ssi il ne l'est pas déjà et que la config est ok
    /// </summary>
    public bool CanStart
    {
      get
      {
        return !this.Running;
      }
    }

    /// <summary>
    /// On peut stoppper le lecteur
    /// ssi il est démarré
    /// </summary>
    public bool CanStop
    {
      get
      {
        return this.Running;
      }
    }
    #endregion

    #region Méthodes publiques
    /// <summary>
    /// Démarre le lecteur
    /// </summary>
    /// <param name="clientId">L'identifiant du client qui demande le start</param>
    public void Start(string clientId)
    {
      this.recuClientId = clientId;
      this.recuAction = EEtatLecteur.Start;
      if (this.chkReponseAuto.Checked)
      {
        this.BtRepondOk_Click(null, null);
      }
      else
      {
        this.GereBouton();
      }
    }

    /// <summary>
    /// Arrête le lecteur
    /// </summary>
    public void Stop()
    {
      this.recuAction = EEtatLecteur.Stop;
      if (this.chkReponseAuto.Checked)
      {
        this.BtRepondOk_Click(null, null);
      }
      else
      {
        this.GereBouton();
      }
    }

    /// <summary>
    /// Initialise la liste des lectures
    /// </summary>
    public void ResetLectures()
    {
      this.recuAction = EEtatLecteur.Reset;
      if (this.chkReponseAuto.Checked)
      {
        this.BtRepondOk_Click(null, null);
      }
      else
      {
        this.GereBouton();
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
    /// simule une réponde Ok à une demande reçue
    /// </summary>
    /// <param name="sender">qui appelle</param>
    /// <param name="e">Paramètre inutile</param>
    private void BtRepondOk_Click(object sender, EventArgs e)
    {
      string msg = string.Empty;
      switch (this.recuAction)
      {
        case EEtatLecteur.Start:
          this.ClientId = this.recuClientId;
          this.Running = true;
          this.PopulateTags(true);
          msg = "Lecteur démarré.";
          break;
        case EEtatLecteur.Stop:
          this.Running = false;
          this.PopulateTags(false);
          msg = "Lecteur stoppé";
          break;
        case EEtatLecteur.Reset:
          if (this.lectures != null)
          {
            this.lectures.Clear();
          }

          this.PopulateTags(true);
          break;
        default: // autres valeurs on se casse
          return;
      }

      SimpleReaderEventArgs args = new SimpleReaderEventArgs(msg, ELogType.RapportOk, this.recuAction, SimpleReaderAntenneInfo.ALLPOSITION);
      if (this.Parameters != null)
      {
        args.AdresseIP = this.Parameters.AdresseIP;
      }

      if (this.OnNotifie != null)
      {
        this.OnNotifie(this, args);
      }

      this.recuAction = EEtatLecteur.KeepAlive;
      this.GereBouton();
    }

    /// <summary>
    /// simule une réponde Ko à une demande reçue
    /// </summary>
    /// <param name="sender">qui appelle</param>
    /// <param name="e">Paramètre inutile</param>
    private void BtRepondKo_Click(object sender, EventArgs e)
    {
      SimpleReaderEventArgs args = new SimpleReaderEventArgs(this.txtRepondKo.Text, ELogType.Erreur, this.recuAction, SimpleReaderAntenneInfo.ALLPOSITION);
      args.AdresseIP = this.Parameters.AdresseIP;
      this.ClientId = this.recuClientId;
      if (this.OnNotifie != null)
      {
        this.OnNotifie(this, args);
      }
    
      this.recuAction = EEtatLecteur.KeepAlive;
      this.GereBouton();
    }

    /// <summary>
    /// Clic sur un tag
    /// </summary>
    /// <param name="sender">qui appelle</param>
    /// <param name="e">Paramètre inutile</param>
    private void TagRead(object sender, ItemCheckEventArgs e)
    {
      CheckedListBox lst = sender as CheckedListBox;
      if (lst != null)
      {
        string tagNum;
        CasqueLib.Buisness.View.EtiquetteDebug et = lst.Items[e.Index] as CasqueLib.Buisness.View.EtiquetteDebug;
        if (et == null)
        {
          tagNum = lst.Items[e.Index].ToString().Trim();
        }
        else
        {
          tagNum = et.Numero;
        }

        if (!this.lectures.Contains(tagNum))
        {
          this.lectures.Add(tagNum);
          SimpleReaderEventArgs args = new SimpleReaderEventArgs(tagNum, ELogType.RapportOk, EEtatLecteur.Tag, 1);
          args.AdresseIP = this.Parameters.AdresseIP;
          if (this.OnNotifie != null)
          {
            this.OnNotifie(this, args);
          }

          this.GereBouton();
        }
      }
    }
    #endregion

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

      if (this.Running)
      {
        this.pnlBusy.BackColor = Color.Green;
        this.lblStatut.Text = "Lecture en cours...";
        this.lblNombreLecture.Text = this.lectures.Count.ToString();
        this.lblNombreLecture.Visible = true;
        this.lblNombreLectureTitre.Visible = true;
        this.lblClient.Text = this.ClientId;
        this.lblClientTitre.Visible = true;
      }
      else
      {
        this.pnlBusy.BackColor = Color.Red;
        this.lblClient.Text = string.Empty;
        this.lblClientTitre.Visible = false;
        this.lblStatut.Text = "Lecteur stoppé";
        this.lblNombreLecture.Visible = false;
        this.lblNombreLectureTitre.Visible = false;
      }

      if (this.recuAction != EEtatLecteur.KeepAlive)
      {
        switch (this.recuAction)
        {
          case EEtatLecteur.Start:
            this.txtRepondKo.Text = "Impossible de démarrer";
            break;
          case EEtatLecteur.Stop:
            this.txtRepondKo.Text = "Impossible de stopper";
            break;
          case EEtatLecteur.Reset:
            this.txtRepondKo.Text = "Impossible de réinitialiser les lectures";
            break;
        }

        this.grpReception.Visible = true;
        this.lblCommande.Text = this.recuAction == EEtatLecteur.Start ? "Démarrage" : this.recuAction == EEtatLecteur.Stop ? "Arrêt" : "Reset des lectures";
        this.btRepondKo.Enabled = !string.IsNullOrWhiteSpace(this.txtRepondKo.Text);
      }
      else
      {
        this.grpReception.Visible = false;
      }

      this.tabControl1.Visible = this.Running;
    }
    
    /// <summary>
    /// Remplit la liste des tag de démos
    /// </summary>
    /// <param name="refill">remplit les listes</param>
    private void PopulateTags(bool refill)
    {
      if (this.InvokeRequired)
      {
        this.Invoke((Action)(() => { this.ProcessPopulateTags(refill); }));
      }
      else
      {
        this.ProcessPopulateTags(refill);
      }
    }

    /// <summary>
    /// Remplit la liste des tags
    /// </summary>
    /// <param name="refill">Remplit ou simplement efface</param>
    private void ProcessPopulateTags(bool refill)
    {
      this.Cursor = Cursors.WaitCursor;
      Application.DoEvents();

      try
      {
        List<CasqueLib.Buisness.View.EtiquetteDebug> etiquettes;
        using (CasqueLib.Common.FConnexion cnn = new CasqueLib.Common.FConnexion())
        {
          etiquettes = CasqueLib.Buisness.View.EtiquetteDebug.LoadAll(cnn.Db);
        }

        this.lst1.Items.Clear();
        this.lst7.Items.Clear();
        this.lst3.Items.Clear();
        this.lst6.Items.Clear();
        this.lst4.Items.Clear();
        this.lst5.Items.Clear();
        this.lst0.Items.Clear();

        if (refill)
        {
          this.lst1.Items.AddRange(etiquettes.Where(x => x.OperationInt == 1).OrderBy(x => x.Tri).ToArray());
          this.lst7.Items.AddRange(etiquettes.Where(x => x.OperationInt == 7).OrderBy(x => x.Tri).ToArray());
          this.lst3.Items.AddRange(etiquettes.Where(x => x.OperationInt == 3).OrderBy(x => x.Tri).ToArray());
          this.lst6.Items.AddRange(etiquettes.Where(x => x.OperationInt == 6).OrderBy(x => x.Tri).ToArray());
          this.lst4.Items.AddRange(etiquettes.Where(x => x.OperationInt == 4).OrderBy(x => x.Tri).ToArray());
          this.lst5.Items.AddRange(etiquettes.Where(x => x.OperationInt == 5).OrderBy(x => x.Tri).ToArray());
          this.lst0.Items.AddRange(etiquettes.Where(x => x.OperationInt < 1 || x.OperationInt > 7).ToArray());

          if (this.lst0.Items.Count == 0)
          {
            this.lst0.Items.Add(this.GetRandomNumber());
            this.lst0.Items.Add(this.GetRandomNumber());
            this.lst0.Items.Add(this.GetRandomNumber());
            this.lst0.Items.Add(this.GetRandomNumber());
            this.lst0.Items.Add(this.GetRandomNumber());
            this.lst0.Items.Add(this.GetRandomNumber());
            this.lst0.Items.Add(this.GetRandomNumber());
            this.lst0.Items.Add(this.GetRandomNumber());
            this.lst0.Items.Add(this.GetRandomNumber());
            this.lst0.Items.Add(this.GetRandomNumber());
          }
        }
      }
      finally
      {
        ////this.btRefreshTag.Enabled = true;
        this.Cursor = Cursors.Default;
      }
    }

    /// <summary>
    /// Génère un numéro aléatoirement
    /// </summary>
    /// <returns>un numéro de tag arbitraire</returns>
    private string GetRandomNumber()
    {
      string a = Guid.NewGuid().ToString().Substring(0, 1);
      string b = Guid.NewGuid().ToString().Substring(0, 1);
      string c = Guid.NewGuid().ToString().Substring(0, 1);
      string d = Guid.NewGuid().ToString().Substring(0, 1);
      string e = Guid.NewGuid().ToString().Substring(0, 1);
      string f = Guid.NewGuid().ToString().Substring(0, 1);
      string g = Guid.NewGuid().ToString().Substring(0, 1);
      string h = Guid.NewGuid().ToString().Substring(0, 1);
      string i = Guid.NewGuid().ToString().Substring(0, 1);
      string j = Guid.NewGuid().ToString().Substring(0, 1);
      return string.Format("{0}{1} {2}{3} {4}{5} {6}{7} {8}{9}", a, b, c, d, e, f, g, h, i, j);
    }
  }
}
