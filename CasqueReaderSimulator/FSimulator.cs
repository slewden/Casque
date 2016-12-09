using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CasqueLib.Matos.ServerOwin;
using ServiceStack.OrmLite;

namespace CasqueReaderSimulator
{
  /// <summary>
  /// pour simuler l'activité d'un lecteur
  /// </summary>
  public partial class FSimulator : Form
  {
    /// <summary>
    /// Le connecteur au hub
    /// </summary>
    private HubConnector connector;

    /// <summary>
    /// La liste des clients avec qui on est en relation
    /// </summary>
    private List<LecteurDatas> lecteurs = new List<LecteurDatas>();

    /// <summary>
    /// Le client en cours d'affichage su lequel l'interface agit
    /// </summary>
    private LecteurDatas current = null;

    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="FSimulator"/>
    /// </summary>
    public FSimulator()
    {
      this.InitializeComponent();

      this.chkStartRepondOk.Checked = Properties.Settings.Default.RepondOkStart;
      this.chkStopRepondOk.Checked = Properties.Settings.Default.RepondOkStop;
      this.chkResetRepondOk.Checked = Properties.Settings.Default.RepondOkReset;

      this.PopulateTags();
      this.GereBouton();
    }

    #region Events
    /// <summary>
    /// Chargement de page
    /// </summary>
    /// <param name="sender">Qui appelle</param>
    /// <param name="e">Paramètre inutile</param>
    private void FSimulator_Load(object sender, EventArgs e)
    {
      this.btConnect.PerformClick();
    }

    /// <summary>
    /// Au déchargement de la page
    /// </summary>
    /// <param name="sender">Qui appelle</param>
    /// <param name="e">Paramètre inutile</param>
    private void FSimulator_FormClosed(object sender, FormClosedEventArgs e)
    {
      Properties.Settings.Default.RepondOkStart = this.chkStartRepondOk.Checked;
      Properties.Settings.Default.RepondOkStop = this.chkStopRepondOk.Checked;
      Properties.Settings.Default.RepondOkReset = this.chkResetRepondOk.Checked;
      Properties.Settings.Default.Save();

      if (this.connector != null)
      {
        this.connector.Dispose();
      }
    }

    /// <summary>
    /// Met à jour l'interface
    /// </summary>
    /// <param name="sender">Qui appelle</param>
    /// <param name="e">Paramètre inutile</param>
    private void GereBouton(object sender, EventArgs e)
    {
      this.GereBouton();
    }

    /// <summary>
    /// RAZ de la liste des logs
    /// </summary>
    /// <param name="sender">Qui appelle</param>
    /// <param name="e">Paramètre inutile</param>
    private void BtClearLog_Click(object sender, EventArgs e)
    {
      this.listBox2.Items.Clear();
    }

    /// <summary>
    /// Re remplit les listes de tags
    /// </summary>
    /// <param name="sender">Qui appelle</param>
    /// <param name="e">Paramètre inutile</param>
    private void BtRefreshTag_Click(object sender, EventArgs e)
    {
      this.PopulateTags();
    }

    /// <summary>
    /// Affiche les options lecteur ou pas
    /// </summary>
    /// <param name="sender">Qui appelle</param>
    /// <param name="e">Paramètre inutile</param>
    private void CheckLecteur_CheckedChanged(object sender, EventArgs e)
    {
      this.groupStart.Visible = this.checkLecteur.Checked;
      this.groupStop.Visible = this.checkLecteur.Checked;
      this.groupReset.Visible = this.checkLecteur.Checked;
      this.GereBouton();
    }

    /// <summary>
    /// Affiche les options encodeur ou pas
    /// </summary>
    /// <param name="sender">Qui appelle</param>
    /// <param name="e">Paramètre inutile</param>
    private void CheckEncodeur_CheckedChanged(object sender, EventArgs e)
    {
      this.groupEncodeAssemblage.Visible = this.checkEncodeur.Checked;
      this.groupEncodeCommande.Visible = this.checkEncodeur.Checked;
      this.GereBouton();
    }

    /// <summary>
    /// Connecte au hub
    /// </summary>
    /// <param name="sender">Qui appelle</param>
    /// <param name="e">Paramètre inutile</param>
    private void BtConnect_Click(object sender, EventArgs e)
    {
      this.Log("Connexion au hub...");
      this.Connecte();
      this.GereBouton();
    }

    /// <summary>
    /// Déconnecte du hub
    /// </summary>
    /// <param name="sender">Qui appelle</param>
    /// <param name="e">Paramètre inutile</param>
    private void BtDisconnect_Click(object sender, EventArgs e)
    {
      this.Log("Déconexion du hub...");
      this.Disconnecte();
      this.GereBouton();
    }

    /// <summary>
    /// Renvoie un Ok pour start
    /// Fait ce qu'il faut pour signaler que le lecteur est démarré
    /// </summary>
    /// <param name="sender">Qui appelle</param>
    /// <param name="e">Paramètre inutile</param>
    private void BtStartOk_Click(object sender, EventArgs e)
    {
      if (this.current != null)
      {
        this.current.Start(this.connector, false, "Lecteur en route");
        this.GereBouton();
      }
    }

    /// <summary>
    /// Renvoie un Ko pour start
    /// </summary>
    /// <param name="sender">Qui appelle</param>
    /// <param name="e">Paramètre inutile</param>
    private void BtStartKo_Click(object sender, EventArgs e)
    {
      if (this.current != null)
      {
        this.current.Start(this.connector, true, this.txtStartError.Text);
        this.GereBouton();
      }
    }

    /// <summary>
    /// Renvoie un Ok pour stop
    /// Fait ce qu'il faut pour signaler que le lecteur est stoppé
    /// </summary>
    /// <param name="sender">Qui appelle</param>
    /// <param name="e">Paramètre inutile</param>
    private void BtStopOk_Click(object sender, EventArgs e)
    {
      if (this.current != null)
      {
        this.current.Stop(this.connector, false, "Lecteur stoppé");
        this.GereBouton();
      }
    }

    /// <summary>
    /// Renvoie un Ko pour stop
    /// </summary>
    /// <param name="sender">Qui appelle</param>
    /// <param name="e">Paramètre inutile</param>
    private void BtStopKo_Click(object sender, EventArgs e)
    {
      if (this.current != null)
      {
        this.current.Stop(this.connector, true, this.txtStopError.Text);
        this.GereBouton();
      }
    }

    /// <summary>
    /// Renvoie un Ok pour reset
    /// </summary>
    /// <param name="sender">Qui appelle</param>
    /// <param name="e">Paramètre inutile</param>
    private void BtResetOk_Click(object sender, EventArgs e)
    {
      if (this.current != null)
      {
        this.current.Reset(this.connector, false, "Reset fait");
        this.ClearLecturesSend();
        this.GereBouton();
      }
    }

    /// <summary>
    /// Renvoie un Ko pour reset
    /// </summary>
    /// <param name="sender">Qui appelle</param>
    /// <param name="e">Paramètre inutile</param>
    private void BtResetKo_Click(object sender, EventArgs e)
    {
      if (this.current != null)
      {
        this.current.Reset(this.connector, true, this.txtResetError.Text);
        this.ClearLecturesSend();
        this.GereBouton();
      }
    }

    /// <summary>
    /// Traite un encodage de commande
    /// </summary>
    /// <param name="sender">Qui appelle</param>
    /// <param name="e">Paramètre inutile</param>
    private void BtEncodeCommande_Click(object sender, EventArgs e)
    {
      if (this.current != null && this.current.CommandeId > 0)
      {
        using (CasqueLib.Common.FConnexion cnn = new CasqueLib.Common.FConnexion())
        {
          cnn.Db.ExecuteNonQuery("EXEC [dbo].[DEBUG_cree_etiquette_pour_commande] @comdId", new { comdId = this.current.CommandeId });
          this.btEncodeCommande.Enabled = false;
          this.GereBouton();
        }
      }
    }

    /// <summary>
    /// changement de l'index : on répercute l'info dans l'objet en cours
    /// </summary>
    /// <param name="sender">Qui appelle</param>
    /// <param name="e">Paramètre inutile</param>
    private void TxtEncodeIndex_TextChanged(object sender, EventArgs e)
    {
      if (this.current != null && this.current.CommandeId > 0 && !string.IsNullOrWhiteSpace(this.txtEncodeIndex.Text))
      {
        int index;
        if (int.TryParse(this.txtEncodeIndex.Text, out index))
        {
          this.current.CommandeProgressionIndex = index;
          this.GereBouton();
        }
      }
    }

    /// <summary>
    /// changement du total : on répercute l'info dans l'objet en cours
    /// </summary>
    /// <param name="sender">Qui appelle</param>
    /// <param name="e">Paramètre inutile</param>
    private void TxtEncodeTotal_TextChanged(object sender, EventArgs e)
    {
      if (this.current != null && this.current.CommandeId > 0 && !string.IsNullOrWhiteSpace(this.txtEncodeTotal.Text))
      {
        int total;
        if (int.TryParse(this.txtEncodeTotal.Text, out total))
        {
          this.current.CommandeProgressionTotal = total;
          this.GereBouton();
        }
      }
    }

    /// <summary>
    /// Envoie un message de progression de taitement de la commande
    /// </summary>
    /// <param name="sender">Qui appelle</param>
    /// <param name="e">Paramètre inutile</param>
    private void BtProgresse_Click(object sender, EventArgs e)
    {
      if (this.current != null && this.current.CommandeId > 0 && this.current.CommandeProgressionIndex >= 0 && this.current.CommandeProgressionTotal > 0)
      {
        this.current.Progresse(this.connector, true);
        this.GereBouton();
      }
    }

    /// <summary>
    ///  Renvoie un Ok pour un encodage de commande
    /// </summary>
    /// <param name="sender">Qui appelle</param>
    /// <param name="e">Paramètre inutile</param>
    private void BtEncodeCommandeOk_Click(object sender, EventArgs e)
    {
      if (this.current != null && this.current.CommandeId > 0)
      {
        this.current.EncodeCommande(this.connector, false, "Encodage commande fini");
        this.GereBouton();
      }
    }

    /// <summary>
    /// Renvoie un Ko pour un encodage de commande
    /// </summary>
    /// <param name="sender">Qui appelle</param>
    /// <param name="e">Paramètre inutile</param>
    private void BtEncodeCommandeKo_Click(object sender, EventArgs e)
    {
      if (this.current != null && this.current.CommandeId > 0)
      {
        this.current.EncodeCommande(this.connector, true, this.txtEncodeCommandeError.Text);
        this.GereBouton();
      }
    }

    /// <summary>
    /// Traite un encodage d'un assemblage
    /// </summary>
    /// <param name="sender">Qui appelle</param>
    /// <param name="e">Paramètre inutile</param>
    private void BtEncodeAssemblage_Click(object sender, EventArgs e)
    {
      if (this.current != null && this.current.AssemblageId > 0)
      {
        using (CasqueLib.Common.FConnexion cnn = new CasqueLib.Common.FConnexion())
        {
          cnn.Db.ExecuteNonQuery("EXECUTE [dbo].[DEBUG_cree_etiquette_pour_assemblage] @asseId", new { asseId = this.current.AssemblageId });
          this.GereBouton();
        }
      }
    }

    /// <summary>
    /// Renvoie un Ok pour un encodage d'assemblage
    /// </summary>
    /// <param name="sender">Qui appelle</param>
    /// <param name="e">Paramètre inutile</param>
    private void BtEncodeAssemblageOk_Click(object sender, EventArgs e)
    {
      if (this.current != null && this.current.AssemblageId > 0)
      {
        this.current.EncodeAssemblage(this.connector, false, "Encodage assemblage fini");
        this.GereBouton();
      }
    }

    /// <summary>
    /// Renvoie un Ko pour un encodage d'assemblage
    /// </summary>
    /// <param name="sender">Qui appelle</param>
    /// <param name="e">Paramètre inutile</param>
    private void BtEncodeAssemblageKo_Click(object sender, EventArgs e)
    {
      if (this.current != null && this.current.AssemblageId > 0)
      {
        this.current.EncodeAssemblage(this.connector, true, this.txtEncodeAssemblageError.Text);
        this.GereBouton();
      }
    }

    /// <summary>
    /// Choix d'un N° De tag
    /// </summary>
    /// <param name="sender">Qui appelle</param>
    /// <param name="e">Paramètre inutile</param>
    private void TagsLus_ItemCheck(object sender, ItemCheckEventArgs e)
    {
      if (this.current != null && this.current.Lectures != null && this.current.EnRoute)
      {
        if (e.NewValue == CheckState.Checked)
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

            Compteur tag = new Compteur(tagNum);
            if (!this.current.Lectures.Contains(tag))
            { // anti rebond !!
              this.current.Lectures.Add(tag);
              this.connector.SendTag(this.current.ClientId, tag.Tag);
            }
          }
        }
      }
    }

    /// <summary>
    /// Affiche un client
    /// </summary>
    /// <param name="sender">Qui appelle</param>
    /// <param name="e">Paramètre inutile</param>
    private void LstClients_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.lstClients.SelectedItem != null)
      {
        this.SetCurrent(this.lstClients.SelectedItem.ToString(), string.Empty);
      }
    }

    /// <summary>
    /// Oublie un client
    /// </summary>
    /// <param name="sender">Qui appelle</param>
    /// <param name="e">Paramètre inutile</param>
    private void BtDelClient_Click(object sender, EventArgs e)
    {
      if (this.current != null)
      {
        this.lecteurs.Remove(this.current);
        this.lstClients.Items.Clear();
        this.lstClients.Items.AddRange(this.lecteurs.Select(x => x.ClientId).ToArray());
        this.current = null;
        this.GereBouton();
      }
    }
    #endregion

    /// <summary>
    /// Défini le lecteur en cours : le crés si besoin
    /// </summary>
    /// <param name="clientId">le client</param>
    /// <param name="parametre">Les paramètres</param>
    private void SetCurrent(string clientId, string parametre)
    {
      this.current = this.lecteurs.Where(x => x.ClientId == clientId).FirstOrDefault();
      if (this.current == null)
      { // non trouvé on le créee
        this.current = new LecteurDatas(clientId, parametre);
        this.lstClients.Items.Add(this.current.ClientId);
        this.lecteurs.Add(this.current);
      }

      this.ClearLecturesSend();
      this.GereBouton();
    }

    #region Actions sur le hub
    /// <summary>
    /// Connecte le hub
    /// </summary>
    private void Connecte()
    {
      int n = 0;
      if (this.checkLecteur.Checked)
      {
        n += (int)CasqueLib.Buisness.Poste.EPosteType.Lecture;
      }

      if (this.checkEncodeur.Checked)
      {
        n += (int)CasqueLib.Buisness.Poste.EPosteType.EncodeTag;
      }

      if (this.connector == null)
      { // première fois qu'on appelle l'objet n'existe pas !
        this.connector = new HubConnector(System.Configuration.ConfigurationManager.AppSettings["ServerUrl"], n);
        this.connector.OnConnectedChanged += this.Connector_OnConnectedChanged;
        this.connector.OnAction += this.Connector_OnAction;
        this.connector.OnEncode += this.Connector_OnEncode;
        this.lblHubInfos.Text = this.connector.ServeurURL;
      }

      if (this.connector.Connected == Microsoft.AspNet.SignalR.Client.ConnectionState.Disconnected)
      { // pas connecté ni en cours de connexion
        this.connector.Connecte(n);
      }
    }

    /// <summary>
    /// Déconnecte le hub si besoin
    /// </summary>
    private void Disconnecte()
    {
      if (this.connector != null)
      {
        this.connector.Disconnecte();
      }
    }

    /// <summary>
    /// Reception d'un statut sur le hub
    /// </summary>
    /// <param name="sender">qui appelle</param>
    /// <param name="e">Les paramètres de la demande</param>
    private void Connector_OnConnectedChanged(object sender, HubConnectorEventStatut e)
    {
      this.Invoke((Action)(() =>
      {
        this.Log(e.ToString());
        this.GereBouton();
      }));
    }

    /// <summary>
    /// Réception d'une demande d'action de la part d'un client pour un lecteur
    /// </summary>
    /// <param name="sender">qui appelle</param>
    /// <param name="e">Les paramètres de la demande</param>
    private void Connector_OnAction(object sender, HubConnectorEventLecteur e)
    {
      this.Invoke((Action)(() =>
      {
        if (e != null)
        {
          this.SetCurrent(e.ClientId, e.XmlParameter);
          if (this.current == null)
          { // Problème !!
            return;
          }

          switch (e.Action)
          {
            case EActionLecteur.Demarre:
              this.Log("Reçu : demande démarrage du lecteur");
              this.current.ActionStart = true;
              if (this.chkStartRepondOk.Checked)
              {
                this.BtStartOk_Click(sender, e);
              }

              break;
            case EActionLecteur.Stoppe:
              this.Log("Reçu : demande de stop du lecteur");
              this.current.EnRoute = false;
              this.current.ActionStop = true;
              if (this.chkStopRepondOk.Checked)
              {
                this.BtStopOk_Click(sender, e);
              }

              break;
            case EActionLecteur.ResetLecture:
              this.Log("Reçu : demande de reset des lectures");
              this.current.ActionReset = true;
              if (this.chkResetRepondOk.Checked)
              {
                this.BtResetOk_Click(sender, e);
              }

              break;
            case EActionLecteur.QueryStatut:
              this.Log("Reçu : demande du statut du lecteur");
              if (this.current.EnRoute)
              { // Le lecteur est démarré
                this.current.Start(this.connector, false, "Démarré");
                this.Log("Réponse : démarré");
              }
              else
              {
                this.current.Stop(this.connector, false, "Stoppé");
                this.Log("Réponse : stopppé");
              }

              break;
          }

          this.GereBouton();
        }
      }));
    }

    /// <summary>
    /// Recu une demande d'encodage d'une commande
    /// </summary>
    /// <param name="sender">qui appelle</param>
    /// <param name="e">Les paramètres de la demande</param>
    private void Connector_OnEncode(object sender, HubConnectorEventEncodeur e)
    {
      this.Invoke((Action)(() =>
      {
        this.Log("Reçu demande : " + e.ToString());
        this.SetCurrent(e.ClientId, e.XmlParameter);
        if (this.current == null)
        { // problème
          return;
        }

        switch (e.Action)
        {
          case EActionEncode.TraiteCommande:
            if (this.current.SetCommande(this.connector, e.Cle))
            {
              this.Log(string.Format("Commande {0} en cours d'encodage...", e.Cle));
            }
            else
            {
              this.Log(string.Format("Demande d'encoder la commande {0} ignorée.", e.Cle));
            }

            break;
          case EActionEncode.TraiteAssemblage:
            if (this.current.SetAssemblage(this.connector, e.Cle))
            {
              this.Log(string.Format("Assemblage {0} en cours d'encodage...", e.Cle));
            }
            else
            {
              this.Log(string.Format("Demande d'assemblage {0} ignorée.", e.Cle));
            }

            break;
          case EActionEncode.CancelEncodage:
            if (e.Cle == this.current.EncodeCle)
            {
              this.current.EncodeCancel(this.connector, "Annulation de l'encodage en cours terminé");
              this.Log("L'annulation de l'encodage en cours terminé");
            }
            else
            {
              this.Log(string.Format("Demande d'annulation {0} ignorée car en cours sur {1} ({2})", e.Cle, this.current.EncodeAction.GetName(), this.current.EncodeCle));
            }

            break;
          case EActionEncode.QueryStatut:
            this.current.Progresse(this.connector, false);
            this.Log(string.Format("Renvoie la progression en cours (cle = {0}).", e.Cle));
            break;
        }

        this.GereBouton();
      }));
    }
    #endregion

    /// <summary>
    /// efface les lectures sélectionnées
    /// </summary>
    private void ClearLecturesSend()
    {
      this.tags0.ClearSelected();
      this.tags1.ClearSelected();
      this.tags2.ClearSelected();
      this.tags3.ClearSelected();
      this.tags4.ClearSelected();
      this.tags5.ClearSelected();
      this.tags6.ClearSelected();
      this.tags7.ClearSelected();
      this.SyncList(this.tags0);
      this.SyncList(this.tags1);
      this.SyncList(this.tags2);
      this.SyncList(this.tags3);
      this.SyncList(this.tags4);
      this.SyncList(this.tags5);
      this.SyncList(this.tags6);
      this.SyncList(this.tags7);
    }

    /// <summary>
    /// Synchronise les tag lus de la liste
    /// </summary>
    /// <param name="lst">La liste</param>
    private void SyncList(CheckedListBox lst)
    {
      bool ok;
      CasqueLib.Buisness.View.EtiquetteDebug etq;
      for (int i = 0; i < lst.Items.Count; i++)
      {
        if (this.current != null)
        {
          etq = lst.Items[i] as CasqueLib.Buisness.View.EtiquetteDebug;
          ok = etq != null && this.current.Lectures.Where(x => x.Tag == etq.Numero).Any();
          lst.SetItemChecked(i, ok);
        }
        else
        {
          lst.SetItemChecked(i, false);
        }
      }
    }

    /// <summary>
    /// Remplit la liste des tag de démos
    /// </summary>
    private void PopulateTags()
    {
      this.btRefreshTag.Enabled = false;
      this.Cursor = Cursors.WaitCursor;
      Application.DoEvents();

      try
      {
        List<CasqueLib.Buisness.View.EtiquetteDebug> etiquettes;
        using (CasqueLib.Common.FConnexion cnn = new CasqueLib.Common.FConnexion())
        {
          etiquettes = CasqueLib.Buisness.View.EtiquetteDebug.LoadAll(cnn.Db);
        }

        this.tags0.Items.Clear();
        this.tags1.Items.Clear();
        this.tags2.Items.Clear();
        this.tags3.Items.Clear();
        this.tags4.Items.Clear();
        this.tags5.Items.Clear();
        this.tags6.Items.Clear();
        this.tags7.Items.Clear();

        this.tags1.Items.AddRange(etiquettes.Where(x => x.OperationInt == 1).OrderBy(x => x.Tri).ToArray());
        this.tags2.Items.AddRange(etiquettes.Where(x => x.OperationInt == 2).OrderBy(x => x.Tri).ToArray());
        this.tags3.Items.AddRange(etiquettes.Where(x => x.OperationInt == 3).OrderBy(x => x.Tri).ToArray());
        this.tags4.Items.AddRange(etiquettes.Where(x => x.OperationInt == 4).OrderBy(x => x.Tri).ToArray());
        this.tags5.Items.AddRange(etiquettes.Where(x => x.OperationInt == 5).OrderBy(x => x.Tri).ToArray());
        this.tags6.Items.AddRange(etiquettes.Where(x => x.OperationInt == 6).OrderBy(x => x.Tri).ToArray());
        this.tags7.Items.AddRange(etiquettes.Where(x => x.OperationInt == 7).OrderBy(x => x.Tri).ToArray());
        this.tags0.Items.AddRange(etiquettes.Where(x => x.OperationInt < 1 || x.OperationInt > 7).ToArray());

        if (this.tags0.Items.Count == 0)
        {
          this.tags0.Items.Add(this.GetRandomNumber());
          this.tags0.Items.Add(this.GetRandomNumber());
          this.tags0.Items.Add(this.GetRandomNumber());
          this.tags0.Items.Add(this.GetRandomNumber());
          this.tags0.Items.Add(this.GetRandomNumber());
          this.tags0.Items.Add(this.GetRandomNumber());
          this.tags0.Items.Add(this.GetRandomNumber());
          this.tags0.Items.Add(this.GetRandomNumber());
          this.tags0.Items.Add(this.GetRandomNumber());
          this.tags0.Items.Add(this.GetRandomNumber());
        }
      }
      finally
      {
        this.btRefreshTag.Enabled = true;
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

    /// <summary>
    /// Ajoute une info à la list de log
    /// </summary>
    /// <param name="txt">le texte à afficher</param>
    private void Log(string txt)
    {
      this.listBox2.BeginUpdate();
      this.listBox2.Items.Add(txt.ToString());
      this.listBox2.SelectedIndex = this.listBox2.Items.Count - 1;
      this.listBox2.EndUpdate();
    }

    /// <summary>
    /// Met à jour l'interface
    /// </summary>
    private void GereBouton()
    {
      // fonction du current : afficher les infos
      if (this.lecteurs.Count > 0)
      {
        this.lblNombreClient.Text = this.lecteurs.Count.ToString();
      }
      else
      {
        this.lblNombreClient.Text = "Aucun";
      }

      if (this.current != null)
      {
        this.lblClient.Text = this.current.ClientId;
        this.lblXml.Text = this.current.XMLConfig;
        this.lblAsseId.Text = this.current.AssemblageId.ToString();
        this.lblComdId.Text = this.current.CommandeId.ToString();
        this.pnlLecteurOn.BackColor = this.current.EnRoute ? Color.Green : Color.Red;
        this.txtEncodeIndex.Text = this.current.CommandeProgressionIndex.ToString();
        this.txtEncodeTotal.Text = this.current.CommandeProgressionTotal.ToString();

        if (this.current.LectureAction != EActionLecteur.Acune)
        {
          this.lblEncodeTitre.Visible = true;
          this.lblEncodeTitre.Text = "Lecture en cours";
        }
        else if (this.current.EncodeAction != EActionEncode.Aucune)
        {
          this.lblEncodeTitre.Visible = true;
          this.lblEncodeTitre.Text = string.Format("Encode : {0}({1})", this.current.EncodeAction.GetName(), this.current.EncodeCle);
        }
        else
        {
          this.lblEncodeTitre.Visible = false;
        }

        this.groupStart.Enabled = this.current.ActionStart;
        this.groupStop.Enabled = this.current.ActionStop;
        this.groupReset.Enabled = this.current.ActionReset;
        this.groupEncodeAssemblage.Enabled = this.current.ActionEncodeAssemblage;
        this.groupEncodeCommande.Enabled = this.current.ActionEncodeCommande;
        this.btEncodeCommande.Enabled = this.groupEncodeCommande.Enabled;
        this.btDelClient.Visible = true;
      }
      else
      {
        this.lblClient.Text = string.Empty;
        this.lblXml.Text = string.Empty;
        this.lblAsseId.Text = string.Empty;
        this.lblComdId.Text = string.Empty;
        this.pnlLecteurOn.BackColor = SystemColors.Control;
        this.lblEncodeTitre.Visible = false;

        this.txtEncodeIndex.Text = string.Empty;
        this.txtEncodeTotal.Text = string.Empty;
        this.groupStart.Enabled = false;
        this.groupStop.Enabled = false;
        this.groupReset.Enabled = false;
        this.groupEncodeAssemblage.Enabled = false;
        this.groupEncodeCommande.Enabled = false;
        this.btDelClient.Visible = false;
      }

      if (this.connector == null)
      { // pas d'objet ==> pas de connexion
        this.btConnect.Enabled = true;
        this.btDisconnect.Enabled = false;
        this.statutHub.BackColor = SystemColors.Control;
      }
      else
      { // l'objet existe en fonction de son statut on réagit
        switch (this.connector.Connected)
        {
          case Microsoft.AspNet.SignalR.Client.ConnectionState.Connected:
            this.btConnect.Enabled = false;
            this.btDisconnect.Enabled = true;
            this.statutHub.BackColor = Color.Green;
            break;
          case Microsoft.AspNet.SignalR.Client.ConnectionState.Connecting:
          case Microsoft.AspNet.SignalR.Client.ConnectionState.Reconnecting:
            this.statutHub.BackColor = Color.Orange;
            break;
          case Microsoft.AspNet.SignalR.Client.ConnectionState.Disconnected:
            this.btConnect.Enabled = this.checkEncodeur.Checked || this.checkLecteur.Enabled;
            this.btDisconnect.Enabled = false;
            this.statutHub.BackColor = Color.Red;
            break;
        }

        this.checkEncodeur.Enabled = this.connector.Connected == Microsoft.AspNet.SignalR.Client.ConnectionState.Disconnected;
        this.checkLecteur.Enabled = this.checkEncodeur.Enabled;

        this.btStartKo.Enabled = !this.chkStartRepondOk.Checked && !string.IsNullOrWhiteSpace(this.txtStartError.Text);
        this.btStopKo.Enabled = !this.chkStopRepondOk.Checked && !string.IsNullOrWhiteSpace(this.txtStopError.Text);
        this.btResetKo.Enabled = !this.chkResetRepondOk.Checked && !string.IsNullOrWhiteSpace(this.txtResetError.Text);
        this.btStartOk.Enabled = !this.chkStartRepondOk.Checked;
        this.btStopOk.Enabled = !this.chkStopRepondOk.Checked;
        this.btResetOk.Enabled = !this.chkResetRepondOk.Checked;

        this.btEncodeCommandeKo.Enabled = !string.IsNullOrWhiteSpace(this.txtEncodeCommandeError.Text);
        this.btEncodeAssemblageKo.Enabled = !string.IsNullOrWhiteSpace(this.txtEncodeAssemblageError.Text);

        bool ok1 = !string.IsNullOrWhiteSpace(this.txtEncodeIndex.Text);
        bool ok2 = !string.IsNullOrWhiteSpace(this.txtEncodeTotal.Text);
        int a, b;
        if (ok1)
        {
          ok1 = int.TryParse(this.txtEncodeIndex.Text, out a);
        }

        if (ok2)
        {
          ok2 = int.TryParse(this.txtEncodeTotal.Text, out b);
        }

        this.btProgresse.Enabled = ok1 && ok2;
      }
    }
  }
}