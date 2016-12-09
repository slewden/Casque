using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CasqueReaderSimulator.Lecteur;

namespace CasqueReaderSimulator
{
  /// <summary>
  /// Feuille principale de simulation des lecteurs et encodeurs
  /// </summary>
  public partial class FSimulateur : Form
  {
    /// <summary>
    /// Travail en cours (pour éviter les ré-entrances)
    /// </summary>
    private bool processing;

    /// <summary>
    /// Le clients du hub
    /// </summary>
    private DemoClient client;

    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="FSimulateur"/>
    /// </summary>
    public FSimulateur()
    {
      this.InitializeComponent();
    }

    /// <summary>
    /// Ajoute un log à la liste
    /// </summary>
    /// <param name="msg">Le message</param>
    public void Log(string msg)
    {
      this.Invoke((Action)(() =>
      {
        this.lstLog.BeginUpdate();
        this.lstLog.Items.Add(msg);
        this.lstLog.SelectedIndex = this.lstLog.Items.Count - 1;
        this.lstLog.EndUpdate();
        this.LstLecteur_SelectedIndexChanged(null, null); // pour voir ??
        this.GereBouton();
      }));
    }

    /// <summary>
    /// Chargement de la page
    /// </summary>
    /// <param name="sender">Qui appelle</param>
    /// <param name="e">Paramètre inutile</param>
    private void FSimulateur_Load(object sender, EventArgs e)
    {
      this.Size = Properties.Settings.Default.FrmSize;
      this.checkLecteur.Checked = Properties.Settings.Default.chkLecteur;
      this.checkEncodeur.Checked = Properties.Settings.Default.chkWriter;
      this.splitContainer1.SplitterDistance = Properties.Settings.Default.Width1;
      this.splitContainer2.SplitterDistance = Properties.Settings.Default.Width2;
      this.ChkReponseAuto.Checked = Properties.Settings.Default.RepondOkStart;

      this.client = new DemoClient(this);
      this.GereBouton();
      this.btConnect.PerformClick();
    }

    /// <summary>
    /// Déchargement de la page
    /// </summary>
    /// <param name="sender">Qui appelle</param>
    /// <param name="e">Paramètre inutile</param>
    private void FSimulateur_FormClosed(object sender, FormClosedEventArgs e)
    {
      Properties.Settings.Default.FrmSize = this.Size;
      Properties.Settings.Default.chkLecteur = this.checkLecteur.Checked;
      Properties.Settings.Default.chkWriter = this.checkEncodeur.Checked;
      Properties.Settings.Default.Width1 = this.splitContainer1.SplitterDistance;
      Properties.Settings.Default.Width2 = this.splitContainer2.SplitterDistance;
      Properties.Settings.Default.Save();
    }

    /// <summary>
    /// Changement de l'état : réponse auto on sauve
    /// pas d'impluentce sur les contrôles déjà créés
    /// </summary>
    /// <param name="sender">Qui appelle</param>
    /// <param name="e">Paramètre inutile</param>
    private void ChkReponseAuto_CheckedChanged(object sender, EventArgs e)
    {
      Properties.Settings.Default.RepondOkStart = this.ChkReponseAuto.Checked;
      Properties.Settings.Default.Save();
    }

    /// <summary>
    /// Efface les logs
    /// </summary>
    /// <param name="sender">Qui appelle</param>
    /// <param name="e">Paramètre inutile</param>
    private void BtClearLog_Click(object sender, EventArgs e)
    {
      this.lstLog.Items.Clear();
    }
     
    #region Gestion du hub
    /// <summary>
    /// Connecte au hub
    /// </summary>
    /// <param name="sender">Qui appelle</param>
    /// <param name="e">Paramètre inutile</param>
    private void BtConnect_Click(object sender, EventArgs e)
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

      this.client.DriversFor = n;
      this.client.Url = System.Configuration.ConfigurationManager.AppSettings["ServerUrl"];

      this.client.Connecte();
      this.GereBouton();
    }

    /// <summary>
    /// Déconnecte du hub
    /// </summary>
    /// <param name="sender">Qui appelle</param>
    /// <param name="e">Paramètre inutile</param>
    private void BtDisconnect_Click(object sender, EventArgs e)
    {
      this.client.Deconnecte();
      this.client.Dispose();
      this.GereBouton();
    }

    /// <summary>
    /// Changement dans les types de matériel gérés
    /// </summary>
    /// <param name="sender">Qui appelle</param>
    /// <param name="e">Paramètre inutile</param>
    private void CheckMatosChanged(object sender, EventArgs e)
    {
      this.GereBouton();
    }
    #endregion

    /// <summary>
    /// Changement de lecteur
    /// </summary>
    /// <param name="sender">Qui appelle</param>
    /// <param name="e">Paramètre inutile</param>
    private void LstLecteur_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.processing)
      {
        return;
      }

      if (this.lstLecteur.SelectedItem != null && !string.IsNullOrWhiteSpace(this.lstLecteur.SelectedItem.ToString())) 
      {
        Control ctrl = this.client.GetLecteur(this.lstLecteur.SelectedItem.ToString());
        if (ctrl != null)
        {
          this.splitContainer2.Panel1.Controls.Clear();
          this.splitContainer2.Panel1.Controls.Add(ctrl);
          ctrl.Dock = DockStyle.Fill;
        }
      }
    }

    /// <summary>
    /// Changement d'encodeur
    /// </summary>
    /// <param name="sender">Qui appelle</param>
    /// <param name="e">Paramètre inutile</param>
    private void LstEncodeur_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.processing)
      {
        return;
      }

      if (this.lstEncodeur.SelectedItem != null && !string.IsNullOrWhiteSpace(this.lstEncodeur.SelectedItem.ToString())) 
      {
        Control ctrl = this.client.GetEncodeur(this.lstEncodeur.SelectedItem.ToString());
        if (ctrl != null)
        {
          this.splitContainer2.Panel1.Controls.Clear();
          this.splitContainer2.Panel1.Controls.Add(ctrl);
          ctrl.Dock = DockStyle.Fill;
        }
      }
    }

    /// <summary>
    /// Met à jour l'interface
    /// </summary>
    private void GereBouton()
    {
      if (this.client != null)
      {
        bool connected = this.client.Connected;

        this.lblTitreHub.Text = connected ? this.client.Url : "Hub";
        this.statutHub.BackColor = connected ? Color.Green : Color.Red;
        this.btConnect.Visible = !connected;
        this.btConnect.Enabled = this.checkEncodeur.Checked || this.checkLecteur.Checked;
        this.btDisconnect.Visible = connected;

        this.checkEncodeur.Enabled = !connected;
        this.checkLecteur.Enabled = !connected;

        // Faire mieux que cela !
        this.Difference(this.client.LecteursKeys, this.lstLecteur);
        this.lblNombreLecteur.Text = this.lstLecteur.Items.Count.ToString();
        this.Difference(this.client.EncodeursKeys, this.lstEncodeur);
        this.lblNombreEncodeur.Text = this.lstEncodeur.Items.Count.ToString();
      }
    }

    /// <summary>
    /// Remplit une liste avec les adresse des machines pilotées
    /// </summary>
    /// <param name="keys">les adresses à avoir</param>
    /// <param name="lst">la liste à remplir</param>
    private void Difference(List<string> keys, ListBox lst)
    {
      bool oneAdd = false;
      this.processing = true;
      try
      {
        object sel = lst.SelectedItem;
        List<string> l2 = new List<string>();
        foreach (string key in lst.Items)
        {
          if (!keys.Contains(key))
          {
            lst.Items.Remove(key);
          }
          else
          {
            l2.Add(key);
          }
        }

        foreach (string newKey in keys.Except(l2))
        {
          oneAdd = true;
          lst.Items.Add(newKey);
        }

        if (sel != null && lst.Items.Contains(sel))
        {
          lst.SelectedItem = sel;
        }
      }
      finally
      {
        this.processing = false;
      }

      if (lst.SelectedItem == null && oneAdd)
      {
        lst.SelectedItem = lst.Items[lst.Items.Count - 1];
      }
    }
  }
}
