namespace CasqueReaderSimulator
{
  partial class FSimulator
  {
    /// <summary>
    /// Variable nécessaire au concepteur.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Nettoyage des ressources utilisées.
    /// </summary>
    /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (this.components != null))
      {
        this.components.Dispose();
      }

      this.connector.Dispose();
      base.Dispose(disposing);
    }

    #region Code généré par le Concepteur Windows Form

    /// <summary>
    /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
    /// le contenu de cette méthode avec l'éditeur de code.
    /// </summary>
    private void InitializeComponent()
    {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FSimulator));
      this.listBox2 = new System.Windows.Forms.ListBox();
      this.groupStart = new System.Windows.Forms.GroupBox();
      this.btStartKo = new System.Windows.Forms.Button();
      this.txtStartError = new System.Windows.Forms.TextBox();
      this.btStartOk = new System.Windows.Forms.Button();
      this.label1 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.panel1 = new System.Windows.Forms.Panel();
      this.lblEncodeTitre = new System.Windows.Forms.Label();
      this.btDelClient = new System.Windows.Forms.Button();
      this.label3 = new System.Windows.Forms.Label();
      this.lblXml = new System.Windows.Forms.Label();
      this.lblClient = new System.Windows.Forms.Label();
      this.pnlLecteurOn = new System.Windows.Forms.Panel();
      this.groupStop = new System.Windows.Forms.GroupBox();
      this.btStopKo = new System.Windows.Forms.Button();
      this.txtStopError = new System.Windows.Forms.TextBox();
      this.btStopOk = new System.Windows.Forms.Button();
      this.groupReset = new System.Windows.Forms.GroupBox();
      this.btResetKo = new System.Windows.Forms.Button();
      this.txtResetError = new System.Windows.Forms.TextBox();
      this.btResetOk = new System.Windows.Forms.Button();
      this.tags1 = new System.Windows.Forms.CheckedListBox();
      this.groupEncodeCommande = new System.Windows.Forms.GroupBox();
      this.lblComdId = new System.Windows.Forms.Label();
      this.btProgresse = new System.Windows.Forms.Button();
      this.txtEncodeTotal = new System.Windows.Forms.TextBox();
      this.txtEncodeIndex = new System.Windows.Forms.TextBox();
      this.btEncodeCommande = new System.Windows.Forms.Button();
      this.label2 = new System.Windows.Forms.Label();
      this.btEncodeCommandeKo = new System.Windows.Forms.Button();
      this.txtEncodeCommandeError = new System.Windows.Forms.TextBox();
      this.btEncodeCommandeOk = new System.Windows.Forms.Button();
      this.groupEncodeAssemblage = new System.Windows.Forms.GroupBox();
      this.lblAsseId = new System.Windows.Forms.Label();
      this.btEncodeAssemblage = new System.Windows.Forms.Button();
      this.label6 = new System.Windows.Forms.Label();
      this.btEncodeAssemblageKo = new System.Windows.Forms.Button();
      this.txtEncodeAssemblageError = new System.Windows.Forms.TextBox();
      this.btEncodeAssemblageOk = new System.Windows.Forms.Button();
      this.tabs = new System.Windows.Forms.TabControl();
      this.tabPage9 = new System.Windows.Forms.TabPage();
      this.chkStopRepondOk = new System.Windows.Forms.CheckBox();
      this.chkResetRepondOk = new System.Windows.Forms.CheckBox();
      this.chkStartRepondOk = new System.Windows.Forms.CheckBox();
      this.lblNombreClient = new System.Windows.Forms.Label();
      this.label15 = new System.Windows.Forms.Label();
      this.lstClients = new System.Windows.Forms.ListBox();
      this.tabPage1 = new System.Windows.Forms.TabPage();
      this.label7 = new System.Windows.Forms.Label();
      this.tabPage2 = new System.Windows.Forms.TabPage();
      this.tags2 = new System.Windows.Forms.CheckedListBox();
      this.label8 = new System.Windows.Forms.Label();
      this.tabPage7 = new System.Windows.Forms.TabPage();
      this.tags6 = new System.Windows.Forms.CheckedListBox();
      this.label9 = new System.Windows.Forms.Label();
      this.tabPage3 = new System.Windows.Forms.TabPage();
      this.tags3 = new System.Windows.Forms.CheckedListBox();
      this.label10 = new System.Windows.Forms.Label();
      this.tabPage6 = new System.Windows.Forms.TabPage();
      this.tags5 = new System.Windows.Forms.CheckedListBox();
      this.label11 = new System.Windows.Forms.Label();
      this.tabPage4 = new System.Windows.Forms.TabPage();
      this.tags4 = new System.Windows.Forms.CheckedListBox();
      this.label12 = new System.Windows.Forms.Label();
      this.tabPage8 = new System.Windows.Forms.TabPage();
      this.tags7 = new System.Windows.Forms.CheckedListBox();
      this.label13 = new System.Windows.Forms.Label();
      this.tabPage5 = new System.Windows.Forms.TabPage();
      this.tags0 = new System.Windows.Forms.CheckedListBox();
      this.label14 = new System.Windows.Forms.Label();
      this.tabPage10 = new System.Windows.Forms.TabPage();
      this.btClearLog = new System.Windows.Forms.Button();
      this.btRefreshTag = new System.Windows.Forms.Button();
      this.checkEncodeur = new System.Windows.Forms.CheckBox();
      this.checkLecteur = new System.Windows.Forms.CheckBox();
      this.lblHubInfos = new System.Windows.Forms.Label();
      this.btDisconnect = new System.Windows.Forms.Button();
      this.btConnect = new System.Windows.Forms.Button();
      this.label5 = new System.Windows.Forms.Label();
      this.statutHub = new System.Windows.Forms.Panel();
      this.groupStart.SuspendLayout();
      this.panel1.SuspendLayout();
      this.groupStop.SuspendLayout();
      this.groupReset.SuspendLayout();
      this.groupEncodeCommande.SuspendLayout();
      this.groupEncodeAssemblage.SuspendLayout();
      this.tabs.SuspendLayout();
      this.tabPage9.SuspendLayout();
      this.tabPage1.SuspendLayout();
      this.tabPage2.SuspendLayout();
      this.tabPage7.SuspendLayout();
      this.tabPage3.SuspendLayout();
      this.tabPage6.SuspendLayout();
      this.tabPage4.SuspendLayout();
      this.tabPage8.SuspendLayout();
      this.tabPage5.SuspendLayout();
      this.tabPage10.SuspendLayout();
      this.SuspendLayout();
      // 
      // listBox2
      // 
      this.listBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.listBox2.FormattingEnabled = true;
      this.listBox2.IntegralHeight = false;
      this.listBox2.Location = new System.Drawing.Point(8, 45);
      this.listBox2.Name = "listBox2";
      this.listBox2.Size = new System.Drawing.Size(811, 441);
      this.listBox2.TabIndex = 25;
      // 
      // groupStart
      // 
      this.groupStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.groupStart.Controls.Add(this.btStartKo);
      this.groupStart.Controls.Add(this.txtStartError);
      this.groupStart.Controls.Add(this.btStartOk);
      this.groupStart.Enabled = false;
      this.groupStart.Location = new System.Drawing.Point(22, 231);
      this.groupStart.Name = "groupStart";
      this.groupStart.Size = new System.Drawing.Size(269, 76);
      this.groupStart.TabIndex = 26;
      this.groupStart.TabStop = false;
      this.groupStart.Text = "Reçu demande démarrage";
      // 
      // btStartKo
      // 
      this.btStartKo.Location = new System.Drawing.Point(14, 48);
      this.btStartKo.Name = "btStartKo";
      this.btStartKo.Size = new System.Drawing.Size(114, 23);
      this.btStartKo.TabIndex = 6;
      this.btStartKo.Text = "Répondre Ko";
      this.btStartKo.UseVisualStyleBackColor = true;
      this.btStartKo.Click += new System.EventHandler(this.BtStartKo_Click);
      // 
      // txtStartError
      // 
      this.txtStartError.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtStartError.Location = new System.Drawing.Point(140, 48);
      this.txtStartError.Name = "txtStartError";
      this.txtStartError.Size = new System.Drawing.Size(122, 20);
      this.txtStartError.TabIndex = 5;
      this.txtStartError.Text = "Erreur de démarrage";
      this.txtStartError.TextChanged += new System.EventHandler(this.GereBouton);
      // 
      // btStartOk
      // 
      this.btStartOk.Location = new System.Drawing.Point(14, 19);
      this.btStartOk.Name = "btStartOk";
      this.btStartOk.Size = new System.Drawing.Size(114, 23);
      this.btStartOk.TabIndex = 4;
      this.btStartOk.Text = "Répondre Ok";
      this.btStartOk.UseVisualStyleBackColor = true;
      this.btStartOk.Click += new System.EventHandler(this.BtStartOk_Click);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label1.Location = new System.Drawing.Point(10, 7);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(57, 20);
      this.label1.TabIndex = 0;
      this.label1.Text = "Client :";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(14, 65);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(35, 13);
      this.label4.TabIndex = 28;
      this.label4.Text = "XML :";
      // 
      // panel1
      // 
      this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.panel1.Controls.Add(this.lblEncodeTitre);
      this.panel1.Controls.Add(this.btDelClient);
      this.panel1.Controls.Add(this.label3);
      this.panel1.Controls.Add(this.lblXml);
      this.panel1.Controls.Add(this.lblClient);
      this.panel1.Controls.Add(this.label4);
      this.panel1.Controls.Add(this.pnlLecteurOn);
      this.panel1.Controls.Add(this.label1);
      this.panel1.Location = new System.Drawing.Point(317, 10);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(505, 202);
      this.panel1.TabIndex = 29;
      // 
      // lblEncodeTitre
      // 
      this.lblEncodeTitre.AutoSize = true;
      this.lblEncodeTitre.Location = new System.Drawing.Point(126, 37);
      this.lblEncodeTitre.Name = "lblEncodeTitre";
      this.lblEncodeTitre.Size = new System.Drawing.Size(50, 13);
      this.lblEncodeTitre.TabIndex = 35;
      this.lblEncodeTitre.Text = "Encode :";
      // 
      // btDelClient
      // 
      this.btDelClient.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btDelClient.Location = new System.Drawing.Point(405, 32);
      this.btDelClient.Name = "btDelClient";
      this.btDelClient.Size = new System.Drawing.Size(94, 23);
      this.btDelClient.TabIndex = 34;
      this.btDelClient.Text = "Oublier ce client";
      this.btDelClient.UseVisualStyleBackColor = true;
      this.btDelClient.Click += new System.EventHandler(this.BtDelClient_Click);
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(10, 37);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(53, 13);
      this.label3.TabIndex = 33;
      this.label3.Text = "En route :";
      // 
      // lblXml
      // 
      this.lblXml.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.lblXml.Location = new System.Drawing.Point(66, 65);
      this.lblXml.Name = "lblXml";
      this.lblXml.Size = new System.Drawing.Size(433, 136);
      this.lblXml.TabIndex = 30;
      this.lblXml.Text = "xxxx";
      // 
      // lblClient
      // 
      this.lblClient.AutoSize = true;
      this.lblClient.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblClient.Location = new System.Drawing.Point(66, 7);
      this.lblClient.Name = "lblClient";
      this.lblClient.Size = new System.Drawing.Size(37, 20);
      this.lblClient.TabIndex = 29;
      this.lblClient.Text = "xxxx";
      // 
      // pnlLecteurOn
      // 
      this.pnlLecteurOn.BackColor = System.Drawing.Color.Green;
      this.pnlLecteurOn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.pnlLecteurOn.Location = new System.Drawing.Point(69, 36);
      this.pnlLecteurOn.Name = "pnlLecteurOn";
      this.pnlLecteurOn.Size = new System.Drawing.Size(42, 18);
      this.pnlLecteurOn.TabIndex = 32;
      // 
      // groupStop
      // 
      this.groupStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.groupStop.Controls.Add(this.btStopKo);
      this.groupStop.Controls.Add(this.txtStopError);
      this.groupStop.Controls.Add(this.btStopOk);
      this.groupStop.Enabled = false;
      this.groupStop.Location = new System.Drawing.Point(22, 397);
      this.groupStop.Name = "groupStop";
      this.groupStop.Size = new System.Drawing.Size(269, 74);
      this.groupStop.TabIndex = 30;
      this.groupStop.TabStop = false;
      this.groupStop.Text = "Reçu demande arrêt";
      // 
      // btStopKo
      // 
      this.btStopKo.Location = new System.Drawing.Point(14, 48);
      this.btStopKo.Name = "btStopKo";
      this.btStopKo.Size = new System.Drawing.Size(114, 23);
      this.btStopKo.TabIndex = 6;
      this.btStopKo.Text = "Répondre Ko";
      this.btStopKo.UseVisualStyleBackColor = true;
      this.btStopKo.Click += new System.EventHandler(this.BtStopKo_Click);
      // 
      // txtStopError
      // 
      this.txtStopError.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtStopError.Location = new System.Drawing.Point(140, 48);
      this.txtStopError.Name = "txtStopError";
      this.txtStopError.Size = new System.Drawing.Size(122, 20);
      this.txtStopError.TabIndex = 5;
      this.txtStopError.Text = "Erreur lors de l\'arrêt";
      this.txtStopError.TextChanged += new System.EventHandler(this.GereBouton);
      // 
      // btStopOk
      // 
      this.btStopOk.Location = new System.Drawing.Point(14, 19);
      this.btStopOk.Name = "btStopOk";
      this.btStopOk.Size = new System.Drawing.Size(114, 23);
      this.btStopOk.TabIndex = 4;
      this.btStopOk.Text = "Répondre Ok";
      this.btStopOk.UseVisualStyleBackColor = true;
      this.btStopOk.Click += new System.EventHandler(this.BtStopOk_Click);
      // 
      // groupReset
      // 
      this.groupReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.groupReset.Controls.Add(this.btResetKo);
      this.groupReset.Controls.Add(this.txtResetError);
      this.groupReset.Controls.Add(this.btResetOk);
      this.groupReset.Enabled = false;
      this.groupReset.Location = new System.Drawing.Point(22, 313);
      this.groupReset.Name = "groupReset";
      this.groupReset.Size = new System.Drawing.Size(269, 78);
      this.groupReset.TabIndex = 31;
      this.groupReset.TabStop = false;
      this.groupReset.Text = "Reçu demande reset";
      // 
      // btResetKo
      // 
      this.btResetKo.Location = new System.Drawing.Point(14, 48);
      this.btResetKo.Name = "btResetKo";
      this.btResetKo.Size = new System.Drawing.Size(114, 23);
      this.btResetKo.TabIndex = 6;
      this.btResetKo.Text = "Répondre Ko";
      this.btResetKo.UseVisualStyleBackColor = true;
      this.btResetKo.Click += new System.EventHandler(this.BtResetKo_Click);
      // 
      // txtResetError
      // 
      this.txtResetError.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtResetError.Location = new System.Drawing.Point(140, 48);
      this.txtResetError.Name = "txtResetError";
      this.txtResetError.Size = new System.Drawing.Size(122, 20);
      this.txtResetError.TabIndex = 5;
      this.txtResetError.Text = "Erreur lors du reset";
      this.txtResetError.TextChanged += new System.EventHandler(this.GereBouton);
      // 
      // btResetOk
      // 
      this.btResetOk.Location = new System.Drawing.Point(14, 19);
      this.btResetOk.Name = "btResetOk";
      this.btResetOk.Size = new System.Drawing.Size(114, 23);
      this.btResetOk.TabIndex = 4;
      this.btResetOk.Text = "Répondre Ok";
      this.btResetOk.UseVisualStyleBackColor = true;
      this.btResetOk.Click += new System.EventHandler(this.BtResetOk_Click);
      // 
      // tags1
      // 
      this.tags1.CheckOnClick = true;
      this.tags1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tags1.FormattingEnabled = true;
      this.tags1.IntegralHeight = false;
      this.tags1.Location = new System.Drawing.Point(3, 37);
      this.tags1.Name = "tags1";
      this.tags1.Size = new System.Drawing.Size(830, 449);
      this.tags1.TabIndex = 33;
      this.tags1.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.TagsLus_ItemCheck);
      // 
      // groupEncodeCommande
      // 
      this.groupEncodeCommande.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.groupEncodeCommande.Controls.Add(this.lblComdId);
      this.groupEncodeCommande.Controls.Add(this.btProgresse);
      this.groupEncodeCommande.Controls.Add(this.txtEncodeTotal);
      this.groupEncodeCommande.Controls.Add(this.txtEncodeIndex);
      this.groupEncodeCommande.Controls.Add(this.btEncodeCommande);
      this.groupEncodeCommande.Controls.Add(this.label2);
      this.groupEncodeCommande.Controls.Add(this.btEncodeCommandeKo);
      this.groupEncodeCommande.Controls.Add(this.txtEncodeCommandeError);
      this.groupEncodeCommande.Controls.Add(this.btEncodeCommandeOk);
      this.groupEncodeCommande.Enabled = false;
      this.groupEncodeCommande.Location = new System.Drawing.Point(420, 231);
      this.groupEncodeCommande.Name = "groupEncodeCommande";
      this.groupEncodeCommande.Size = new System.Drawing.Size(402, 110);
      this.groupEncodeCommande.TabIndex = 34;
      this.groupEncodeCommande.TabStop = false;
      this.groupEncodeCommande.Text = "Reçu demande d\'encoder une commande";
      // 
      // lblComdId
      // 
      this.lblComdId.AutoSize = true;
      this.lblComdId.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblComdId.Location = new System.Drawing.Point(127, 24);
      this.lblComdId.Name = "lblComdId";
      this.lblComdId.Size = new System.Drawing.Size(31, 13);
      this.lblComdId.TabIndex = 26;
      this.lblComdId.Text = "XXX";
      // 
      // btProgresse
      // 
      this.btProgresse.Location = new System.Drawing.Point(317, 46);
      this.btProgresse.Name = "btProgresse";
      this.btProgresse.Size = new System.Drawing.Size(80, 23);
      this.btProgresse.TabIndex = 25;
      this.btProgresse.Text = "Progression";
      this.btProgresse.UseVisualStyleBackColor = true;
      this.btProgresse.Click += new System.EventHandler(this.BtProgresse_Click);
      // 
      // txtEncodeTotal
      // 
      this.txtEncodeTotal.Location = new System.Drawing.Point(360, 21);
      this.txtEncodeTotal.Name = "txtEncodeTotal";
      this.txtEncodeTotal.Size = new System.Drawing.Size(37, 20);
      this.txtEncodeTotal.TabIndex = 24;
      this.txtEncodeTotal.Text = "10";
      this.txtEncodeTotal.TextChanged += new System.EventHandler(this.TxtEncodeTotal_TextChanged);
      // 
      // txtEncodeIndex
      // 
      this.txtEncodeIndex.Location = new System.Drawing.Point(317, 21);
      this.txtEncodeIndex.Name = "txtEncodeIndex";
      this.txtEncodeIndex.Size = new System.Drawing.Size(37, 20);
      this.txtEncodeIndex.TabIndex = 23;
      this.txtEncodeIndex.Text = "4";
      this.txtEncodeIndex.TextChanged += new System.EventHandler(this.TxtEncodeIndex_TextChanged);
      // 
      // btEncodeCommande
      // 
      this.btEncodeCommande.Location = new System.Drawing.Point(170, 19);
      this.btEncodeCommande.Name = "btEncodeCommande";
      this.btEncodeCommande.Size = new System.Drawing.Size(114, 23);
      this.btEncodeCommande.TabIndex = 22;
      this.btEncodeCommande.Text = "Traiter la demande";
      this.btEncodeCommande.UseVisualStyleBackColor = true;
      this.btEncodeCommande.Click += new System.EventHandler(this.BtEncodeCommande_Click);
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(12, 23);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(109, 13);
      this.label2.TabIndex = 21;
      this.label2.Text = "Clé de la commande :";
      // 
      // btEncodeCommandeKo
      // 
      this.btEncodeCommandeKo.Location = new System.Drawing.Point(14, 75);
      this.btEncodeCommandeKo.Name = "btEncodeCommandeKo";
      this.btEncodeCommandeKo.Size = new System.Drawing.Size(114, 23);
      this.btEncodeCommandeKo.TabIndex = 6;
      this.btEncodeCommandeKo.Text = "Répondre Ko";
      this.btEncodeCommandeKo.UseVisualStyleBackColor = true;
      this.btEncodeCommandeKo.Click += new System.EventHandler(this.BtEncodeCommandeKo_Click);
      // 
      // txtEncodeCommandeError
      // 
      this.txtEncodeCommandeError.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtEncodeCommandeError.Location = new System.Drawing.Point(140, 75);
      this.txtEncodeCommandeError.Name = "txtEncodeCommandeError";
      this.txtEncodeCommandeError.Size = new System.Drawing.Size(256, 20);
      this.txtEncodeCommandeError.TabIndex = 5;
      this.txtEncodeCommandeError.Text = "Erreur lors de l\'encodage d\'une commande";
      this.txtEncodeCommandeError.TextChanged += new System.EventHandler(this.GereBouton);
      // 
      // btEncodeCommandeOk
      // 
      this.btEncodeCommandeOk.Location = new System.Drawing.Point(14, 46);
      this.btEncodeCommandeOk.Name = "btEncodeCommandeOk";
      this.btEncodeCommandeOk.Size = new System.Drawing.Size(114, 23);
      this.btEncodeCommandeOk.TabIndex = 4;
      this.btEncodeCommandeOk.Text = "Répondre Ok";
      this.btEncodeCommandeOk.UseVisualStyleBackColor = true;
      this.btEncodeCommandeOk.Click += new System.EventHandler(this.BtEncodeCommandeOk_Click);
      // 
      // groupEncodeAssemblage
      // 
      this.groupEncodeAssemblage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.groupEncodeAssemblage.Controls.Add(this.lblAsseId);
      this.groupEncodeAssemblage.Controls.Add(this.btEncodeAssemblage);
      this.groupEncodeAssemblage.Controls.Add(this.label6);
      this.groupEncodeAssemblage.Controls.Add(this.btEncodeAssemblageKo);
      this.groupEncodeAssemblage.Controls.Add(this.txtEncodeAssemblageError);
      this.groupEncodeAssemblage.Controls.Add(this.btEncodeAssemblageOk);
      this.groupEncodeAssemblage.Enabled = false;
      this.groupEncodeAssemblage.Location = new System.Drawing.Point(420, 361);
      this.groupEncodeAssemblage.Name = "groupEncodeAssemblage";
      this.groupEncodeAssemblage.Size = new System.Drawing.Size(402, 110);
      this.groupEncodeAssemblage.TabIndex = 35;
      this.groupEncodeAssemblage.TabStop = false;
      this.groupEncodeAssemblage.Text = "Reçu demande d\'encoder un assemblage";
      // 
      // lblAsseId
      // 
      this.lblAsseId.AutoSize = true;
      this.lblAsseId.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblAsseId.Location = new System.Drawing.Point(124, 23);
      this.lblAsseId.Name = "lblAsseId";
      this.lblAsseId.Size = new System.Drawing.Size(31, 13);
      this.lblAsseId.TabIndex = 23;
      this.lblAsseId.Text = "XXX";
      // 
      // btEncodeAssemblage
      // 
      this.btEncodeAssemblage.Location = new System.Drawing.Point(174, 18);
      this.btEncodeAssemblage.Name = "btEncodeAssemblage";
      this.btEncodeAssemblage.Size = new System.Drawing.Size(114, 23);
      this.btEncodeAssemblage.TabIndex = 22;
      this.btEncodeAssemblage.Text = "Traiter la demande";
      this.btEncodeAssemblage.UseVisualStyleBackColor = true;
      this.btEncodeAssemblage.Click += new System.EventHandler(this.BtEncodeAssemblage_Click);
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(12, 23);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(106, 13);
      this.label6.TabIndex = 21;
      this.label6.Text = "Clé de l\'assemblage :";
      // 
      // btEncodeAssemblageKo
      // 
      this.btEncodeAssemblageKo.Location = new System.Drawing.Point(14, 75);
      this.btEncodeAssemblageKo.Name = "btEncodeAssemblageKo";
      this.btEncodeAssemblageKo.Size = new System.Drawing.Size(114, 23);
      this.btEncodeAssemblageKo.TabIndex = 6;
      this.btEncodeAssemblageKo.Text = "Répondre Ko";
      this.btEncodeAssemblageKo.UseVisualStyleBackColor = true;
      this.btEncodeAssemblageKo.Click += new System.EventHandler(this.BtEncodeAssemblageKo_Click);
      // 
      // txtEncodeAssemblageError
      // 
      this.txtEncodeAssemblageError.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtEncodeAssemblageError.Location = new System.Drawing.Point(140, 75);
      this.txtEncodeAssemblageError.Name = "txtEncodeAssemblageError";
      this.txtEncodeAssemblageError.Size = new System.Drawing.Size(256, 20);
      this.txtEncodeAssemblageError.TabIndex = 5;
      this.txtEncodeAssemblageError.Text = "Erreur lors de l\'encodage d\'une étiquette";
      this.txtEncodeAssemblageError.TextChanged += new System.EventHandler(this.GereBouton);
      // 
      // btEncodeAssemblageOk
      // 
      this.btEncodeAssemblageOk.Location = new System.Drawing.Point(14, 46);
      this.btEncodeAssemblageOk.Name = "btEncodeAssemblageOk";
      this.btEncodeAssemblageOk.Size = new System.Drawing.Size(114, 23);
      this.btEncodeAssemblageOk.TabIndex = 4;
      this.btEncodeAssemblageOk.Text = "Répondre Ok";
      this.btEncodeAssemblageOk.UseVisualStyleBackColor = true;
      this.btEncodeAssemblageOk.Click += new System.EventHandler(this.BtEncodeAssemblageOk_Click);
      // 
      // tabs
      // 
      this.tabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tabs.Controls.Add(this.tabPage9);
      this.tabs.Controls.Add(this.tabPage1);
      this.tabs.Controls.Add(this.tabPage2);
      this.tabs.Controls.Add(this.tabPage7);
      this.tabs.Controls.Add(this.tabPage3);
      this.tabs.Controls.Add(this.tabPage6);
      this.tabs.Controls.Add(this.tabPage4);
      this.tabs.Controls.Add(this.tabPage8);
      this.tabs.Controls.Add(this.tabPage5);
      this.tabs.Controls.Add(this.tabPage10);
      this.tabs.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.tabs.Location = new System.Drawing.Point(12, 32);
      this.tabs.Name = "tabs";
      this.tabs.SelectedIndex = 0;
      this.tabs.Size = new System.Drawing.Size(844, 516);
      this.tabs.TabIndex = 39;
      // 
      // tabPage9
      // 
      this.tabPage9.Controls.Add(this.chkStopRepondOk);
      this.tabPage9.Controls.Add(this.chkResetRepondOk);
      this.tabPage9.Controls.Add(this.chkStartRepondOk);
      this.tabPage9.Controls.Add(this.lblNombreClient);
      this.tabPage9.Controls.Add(this.label15);
      this.tabPage9.Controls.Add(this.lstClients);
      this.tabPage9.Controls.Add(this.groupEncodeCommande);
      this.tabPage9.Controls.Add(this.groupEncodeAssemblage);
      this.tabPage9.Controls.Add(this.groupStop);
      this.tabPage9.Controls.Add(this.groupReset);
      this.tabPage9.Controls.Add(this.panel1);
      this.tabPage9.Controls.Add(this.groupStart);
      this.tabPage9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.tabPage9.Location = new System.Drawing.Point(4, 23);
      this.tabPage9.Name = "tabPage9";
      this.tabPage9.Size = new System.Drawing.Size(836, 489);
      this.tabPage9.TabIndex = 8;
      this.tabPage9.Text = "Lecteurs";
      this.tabPage9.UseVisualStyleBackColor = true;
      // 
      // chkStopRepondOk
      // 
      this.chkStopRepondOk.AutoSize = true;
      this.chkStopRepondOk.Checked = true;
      this.chkStopRepondOk.CheckState = System.Windows.Forms.CheckState.Checked;
      this.chkStopRepondOk.Location = new System.Drawing.Point(164, 420);
      this.chkStopRepondOk.Name = "chkStopRepondOk";
      this.chkStopRepondOk.Size = new System.Drawing.Size(105, 17);
      this.chkStopRepondOk.TabIndex = 8;
      this.chkStopRepondOk.Text = "Ok tous le temps";
      this.chkStopRepondOk.UseVisualStyleBackColor = true;
      this.chkStopRepondOk.CheckedChanged += new System.EventHandler(this.GereBouton);
      // 
      // chkResetRepondOk
      // 
      this.chkResetRepondOk.AutoSize = true;
      this.chkResetRepondOk.Checked = true;
      this.chkResetRepondOk.CheckState = System.Windows.Forms.CheckState.Checked;
      this.chkResetRepondOk.Location = new System.Drawing.Point(164, 336);
      this.chkResetRepondOk.Name = "chkResetRepondOk";
      this.chkResetRepondOk.Size = new System.Drawing.Size(105, 17);
      this.chkResetRepondOk.TabIndex = 8;
      this.chkResetRepondOk.Text = "Ok tous le temps";
      this.chkResetRepondOk.UseVisualStyleBackColor = true;
      this.chkResetRepondOk.CheckedChanged += new System.EventHandler(this.GereBouton);
      // 
      // chkStartRepondOk
      // 
      this.chkStartRepondOk.AutoSize = true;
      this.chkStartRepondOk.Checked = true;
      this.chkStartRepondOk.CheckState = System.Windows.Forms.CheckState.Checked;
      this.chkStartRepondOk.Location = new System.Drawing.Point(164, 253);
      this.chkStartRepondOk.Name = "chkStartRepondOk";
      this.chkStartRepondOk.Size = new System.Drawing.Size(105, 17);
      this.chkStartRepondOk.TabIndex = 7;
      this.chkStartRepondOk.Text = "Ok tous le temps";
      this.chkStartRepondOk.UseVisualStyleBackColor = true;
      this.chkStartRepondOk.CheckedChanged += new System.EventHandler(this.GereBouton);
      // 
      // lblNombreClient
      // 
      this.lblNombreClient.Location = new System.Drawing.Point(194, 10);
      this.lblNombreClient.Name = "lblNombreClient";
      this.lblNombreClient.Size = new System.Drawing.Size(97, 13);
      this.lblNombreClient.TabIndex = 37;
      this.lblNombreClient.Text = "xxx";
      this.lblNombreClient.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // label15
      // 
      this.label15.AutoSize = true;
      this.label15.Location = new System.Drawing.Point(19, 10);
      this.label15.Name = "label15";
      this.label15.Size = new System.Drawing.Size(97, 13);
      this.label15.TabIndex = 36;
      this.label15.Text = "Clients connectés :";
      // 
      // lstClients
      // 
      this.lstClients.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
      this.lstClients.FormattingEnabled = true;
      this.lstClients.Location = new System.Drawing.Point(22, 26);
      this.lstClients.Name = "lstClients";
      this.lstClients.Size = new System.Drawing.Size(269, 186);
      this.lstClients.TabIndex = 29;
      this.lstClients.SelectedIndexChanged += new System.EventHandler(this.LstClients_SelectedIndexChanged);
      // 
      // tabPage1
      // 
      this.tabPage1.Controls.Add(this.tags1);
      this.tabPage1.Controls.Add(this.label7);
      this.tabPage1.Location = new System.Drawing.Point(4, 23);
      this.tabPage1.Name = "tabPage1";
      this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
      this.tabPage1.Size = new System.Drawing.Size(836, 489);
      this.tabPage1.TabIndex = 0;
      this.tabPage1.Text = "Commandé";
      this.tabPage1.UseVisualStyleBackColor = true;
      // 
      // label7
      // 
      this.label7.Dock = System.Windows.Forms.DockStyle.Top;
      this.label7.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label7.Location = new System.Drawing.Point(3, 3);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(830, 34);
      this.label7.TabIndex = 34;
      this.label7.Text = "N° TAG     :  Clé ligne commande   -    Type de pièce";
      this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // tabPage2
      // 
      this.tabPage2.Controls.Add(this.tags2);
      this.tabPage2.Controls.Add(this.label8);
      this.tabPage2.Location = new System.Drawing.Point(4, 23);
      this.tabPage2.Name = "tabPage2";
      this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
      this.tabPage2.Size = new System.Drawing.Size(836, 489);
      this.tabPage2.TabIndex = 1;
      this.tabPage2.Text = "Livrés";
      this.tabPage2.UseVisualStyleBackColor = true;
      // 
      // tags2
      // 
      this.tags2.CheckOnClick = true;
      this.tags2.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tags2.FormattingEnabled = true;
      this.tags2.IntegralHeight = false;
      this.tags2.Location = new System.Drawing.Point(3, 37);
      this.tags2.Name = "tags2";
      this.tags2.Size = new System.Drawing.Size(830, 449);
      this.tags2.TabIndex = 34;
      this.tags2.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.TagsLus_ItemCheck);
      // 
      // label8
      // 
      this.label8.Dock = System.Windows.Forms.DockStyle.Top;
      this.label8.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label8.Location = new System.Drawing.Point(3, 3);
      this.label8.Name = "label8";
      this.label8.Size = new System.Drawing.Size(830, 34);
      this.label8.TabIndex = 35;
      this.label8.Text = "N° TAG     :  Clé ligne commande   -    Type de pièce";
      this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // tabPage7
      // 
      this.tabPage7.Controls.Add(this.tags6);
      this.tabPage7.Controls.Add(this.label9);
      this.tabPage7.Location = new System.Drawing.Point(4, 23);
      this.tabPage7.Name = "tabPage7";
      this.tabPage7.Size = new System.Drawing.Size(836, 489);
      this.tabPage7.TabIndex = 6;
      this.tabPage7.Text = "Livré par Casque";
      this.tabPage7.UseVisualStyleBackColor = true;
      // 
      // tags6
      // 
      this.tags6.CheckOnClick = true;
      this.tags6.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tags6.FormattingEnabled = true;
      this.tags6.IntegralHeight = false;
      this.tags6.Location = new System.Drawing.Point(0, 34);
      this.tags6.Name = "tags6";
      this.tags6.Size = new System.Drawing.Size(836, 455);
      this.tags6.TabIndex = 35;
      this.tags6.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.TagsLus_ItemCheck);
      // 
      // label9
      // 
      this.label9.Dock = System.Windows.Forms.DockStyle.Top;
      this.label9.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label9.Location = new System.Drawing.Point(0, 0);
      this.label9.Name = "label9";
      this.label9.Size = new System.Drawing.Size(836, 34);
      this.label9.TabIndex = 36;
      this.label9.Text = "N° TAG : Idx de la compo  - Nom casque (Nb pièces ds compo)";
      this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // tabPage3
      // 
      this.tabPage3.Controls.Add(this.tags3);
      this.tabPage3.Controls.Add(this.label10);
      this.tabPage3.Location = new System.Drawing.Point(4, 23);
      this.tabPage3.Name = "tabPage3";
      this.tabPage3.Size = new System.Drawing.Size(836, 489);
      this.tabPage3.TabIndex = 2;
      this.tabPage3.Text = "Assemblés";
      this.tabPage3.UseVisualStyleBackColor = true;
      // 
      // tags3
      // 
      this.tags3.CheckOnClick = true;
      this.tags3.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tags3.FormattingEnabled = true;
      this.tags3.IntegralHeight = false;
      this.tags3.Location = new System.Drawing.Point(0, 34);
      this.tags3.Name = "tags3";
      this.tags3.Size = new System.Drawing.Size(836, 455);
      this.tags3.TabIndex = 34;
      this.tags3.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.TagsLus_ItemCheck);
      // 
      // label10
      // 
      this.label10.Dock = System.Windows.Forms.DockStyle.Top;
      this.label10.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label10.Location = new System.Drawing.Point(0, 0);
      this.label10.Name = "label10";
      this.label10.Size = new System.Drawing.Size(836, 34);
      this.label10.TabIndex = 36;
      this.label10.Text = "N° TAG     :  Clé de l\'assemblage   -    Type de pièce";
      this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // tabPage6
      // 
      this.tabPage6.Controls.Add(this.tags5);
      this.tabPage6.Controls.Add(this.label11);
      this.tabPage6.Location = new System.Drawing.Point(4, 23);
      this.tabPage6.Name = "tabPage6";
      this.tabPage6.Size = new System.Drawing.Size(836, 489);
      this.tabPage6.TabIndex = 5;
      this.tabPage6.Text = "Assemblage";
      this.tabPage6.UseVisualStyleBackColor = true;
      // 
      // tags5
      // 
      this.tags5.CheckOnClick = true;
      this.tags5.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tags5.FormattingEnabled = true;
      this.tags5.IntegralHeight = false;
      this.tags5.Location = new System.Drawing.Point(0, 34);
      this.tags5.Name = "tags5";
      this.tags5.Size = new System.Drawing.Size(836, 455);
      this.tags5.TabIndex = 35;
      this.tags5.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.TagsLus_ItemCheck);
      // 
      // label11
      // 
      this.label11.Dock = System.Windows.Forms.DockStyle.Top;
      this.label11.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label11.Location = new System.Drawing.Point(0, 0);
      this.label11.Name = "label11";
      this.label11.Size = new System.Drawing.Size(836, 34);
      this.label11.TabIndex = 36;
      this.label11.Text = "N° TAG (Assemblage)    :  Clé de l\'assemblage  -  Casque Nom";
      this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // tabPage4
      // 
      this.tabPage4.Controls.Add(this.tags4);
      this.tabPage4.Controls.Add(this.label12);
      this.tabPage4.Location = new System.Drawing.Point(4, 23);
      this.tabPage4.Name = "tabPage4";
      this.tabPage4.Size = new System.Drawing.Size(836, 489);
      this.tabPage4.TabIndex = 3;
      this.tabPage4.Text = "Livrés";
      this.tabPage4.UseVisualStyleBackColor = true;
      // 
      // tags4
      // 
      this.tags4.CheckOnClick = true;
      this.tags4.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tags4.FormattingEnabled = true;
      this.tags4.IntegralHeight = false;
      this.tags4.Location = new System.Drawing.Point(0, 34);
      this.tags4.Name = "tags4";
      this.tags4.Size = new System.Drawing.Size(836, 455);
      this.tags4.TabIndex = 34;
      this.tags4.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.TagsLus_ItemCheck);
      // 
      // label12
      // 
      this.label12.Dock = System.Windows.Forms.DockStyle.Top;
      this.label12.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label12.Location = new System.Drawing.Point(0, 0);
      this.label12.Name = "label12";
      this.label12.Size = new System.Drawing.Size(836, 34);
      this.label12.TabIndex = 36;
      this.label12.Text = "N° TAG     :  Clé livraison   -    Type de pièce";
      this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // tabPage8
      // 
      this.tabPage8.Controls.Add(this.tags7);
      this.tabPage8.Controls.Add(this.label13);
      this.tabPage8.Location = new System.Drawing.Point(4, 23);
      this.tabPage8.Name = "tabPage8";
      this.tabPage8.Size = new System.Drawing.Size(836, 489);
      this.tabPage8.TabIndex = 7;
      this.tabPage8.Text = "Ass. Livrés";
      this.tabPage8.UseVisualStyleBackColor = true;
      // 
      // tags7
      // 
      this.tags7.CheckOnClick = true;
      this.tags7.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tags7.FormattingEnabled = true;
      this.tags7.IntegralHeight = false;
      this.tags7.Location = new System.Drawing.Point(0, 34);
      this.tags7.Name = "tags7";
      this.tags7.Size = new System.Drawing.Size(836, 455);
      this.tags7.TabIndex = 36;
      this.tags7.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.TagsLus_ItemCheck);
      // 
      // label13
      // 
      this.label13.Dock = System.Windows.Forms.DockStyle.Top;
      this.label13.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label13.Location = new System.Drawing.Point(0, 0);
      this.label13.Name = "label13";
      this.label13.Size = new System.Drawing.Size(836, 34);
      this.label13.TabIndex = 37;
      this.label13.Text = "N° TAG (Assemblage)     :  Clé livraison   -    Casque Nom";
      this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // tabPage5
      // 
      this.tabPage5.Controls.Add(this.tags0);
      this.tabPage5.Controls.Add(this.label14);
      this.tabPage5.Location = new System.Drawing.Point(4, 23);
      this.tabPage5.Name = "tabPage5";
      this.tabPage5.Size = new System.Drawing.Size(836, 489);
      this.tabPage5.TabIndex = 4;
      this.tabPage5.Text = "Inconnus";
      this.tabPage5.UseVisualStyleBackColor = true;
      // 
      // tags0
      // 
      this.tags0.CheckOnClick = true;
      this.tags0.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tags0.FormattingEnabled = true;
      this.tags0.IntegralHeight = false;
      this.tags0.Location = new System.Drawing.Point(0, 34);
      this.tags0.Name = "tags0";
      this.tags0.Size = new System.Drawing.Size(836, 455);
      this.tags0.TabIndex = 35;
      this.tags0.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.TagsLus_ItemCheck);
      // 
      // label14
      // 
      this.label14.Dock = System.Windows.Forms.DockStyle.Top;
      this.label14.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label14.Location = new System.Drawing.Point(0, 0);
      this.label14.Name = "label14";
      this.label14.Size = new System.Drawing.Size(836, 34);
      this.label14.TabIndex = 36;
      this.label14.Text = "N° aléatoire";
      this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // tabPage10
      // 
      this.tabPage10.Controls.Add(this.btClearLog);
      this.tabPage10.Controls.Add(this.listBox2);
      this.tabPage10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.tabPage10.Location = new System.Drawing.Point(4, 23);
      this.tabPage10.Name = "tabPage10";
      this.tabPage10.Size = new System.Drawing.Size(836, 489);
      this.tabPage10.TabIndex = 9;
      this.tabPage10.Text = "Logs";
      this.tabPage10.UseVisualStyleBackColor = true;
      // 
      // btClearLog
      // 
      this.btClearLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btClearLog.Location = new System.Drawing.Point(744, 16);
      this.btClearLog.Name = "btClearLog";
      this.btClearLog.Size = new System.Drawing.Size(75, 23);
      this.btClearLog.TabIndex = 40;
      this.btClearLog.Text = "RAZ";
      this.btClearLog.UseVisualStyleBackColor = true;
      this.btClearLog.Click += new System.EventHandler(this.BtClearLog_Click);
      // 
      // btRefreshTag
      // 
      this.btRefreshTag.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btRefreshTag.Location = new System.Drawing.Point(777, 4);
      this.btRefreshTag.Name = "btRefreshTag";
      this.btRefreshTag.Size = new System.Drawing.Size(75, 23);
      this.btRefreshTag.TabIndex = 41;
      this.btRefreshTag.Text = "Refill Tags";
      this.btRefreshTag.UseVisualStyleBackColor = true;
      this.btRefreshTag.Click += new System.EventHandler(this.BtRefreshTag_Click);
      // 
      // checkEncodeur
      // 
      this.checkEncodeur.AutoSize = true;
      this.checkEncodeur.Checked = true;
      this.checkEncodeur.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkEncodeur.Location = new System.Drawing.Point(429, 8);
      this.checkEncodeur.Name = "checkEncodeur";
      this.checkEncodeur.Size = new System.Drawing.Size(120, 17);
      this.checkEncodeur.TabIndex = 44;
      this.checkEncodeur.Text = "Simule un encodeur";
      this.checkEncodeur.UseVisualStyleBackColor = true;
      this.checkEncodeur.CheckedChanged += new System.EventHandler(this.CheckEncodeur_CheckedChanged);
      // 
      // checkLecteur
      // 
      this.checkLecteur.AutoSize = true;
      this.checkLecteur.Checked = true;
      this.checkLecteur.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkLecteur.Location = new System.Drawing.Point(316, 8);
      this.checkLecteur.Name = "checkLecteur";
      this.checkLecteur.Size = new System.Drawing.Size(107, 17);
      this.checkLecteur.TabIndex = 43;
      this.checkLecteur.Text = "Simule un lecteur";
      this.checkLecteur.UseVisualStyleBackColor = true;
      this.checkLecteur.CheckedChanged += new System.EventHandler(this.CheckLecteur_CheckedChanged);
      // 
      // lblHubInfos
      // 
      this.lblHubInfos.AutoSize = true;
      this.lblHubInfos.Location = new System.Drawing.Point(555, 9);
      this.lblHubInfos.Name = "lblHubInfos";
      this.lblHubInfos.Size = new System.Drawing.Size(16, 13);
      this.lblHubInfos.TabIndex = 42;
      this.lblHubInfos.Text = "...";
      // 
      // btDisconnect
      // 
      this.btDisconnect.Location = new System.Drawing.Point(226, 4);
      this.btDisconnect.Name = "btDisconnect";
      this.btDisconnect.Size = new System.Drawing.Size(75, 23);
      this.btDisconnect.TabIndex = 41;
      this.btDisconnect.Text = "DisConnected";
      this.btDisconnect.UseVisualStyleBackColor = true;
      this.btDisconnect.Click += new System.EventHandler(this.BtDisconnect_Click);
      // 
      // btConnect
      // 
      this.btConnect.Location = new System.Drawing.Point(145, 4);
      this.btConnect.Name = "btConnect";
      this.btConnect.Size = new System.Drawing.Size(75, 23);
      this.btConnect.TabIndex = 40;
      this.btConnect.Text = "Connect";
      this.btConnect.UseVisualStyleBackColor = true;
      this.btConnect.Click += new System.EventHandler(this.BtConnect_Click);
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(13, 9);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(95, 13);
      this.label5.TabIndex = 39;
      this.label5.Text = "Connexion au Hub";
      // 
      // statutHub
      // 
      this.statutHub.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.statutHub.Location = new System.Drawing.Point(114, 4);
      this.statutHub.Name = "statutHub";
      this.statutHub.Size = new System.Drawing.Size(25, 21);
      this.statutHub.TabIndex = 38;
      // 
      // FSimulator
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(860, 547);
      this.Controls.Add(this.btRefreshTag);
      this.Controls.Add(this.checkEncodeur);
      this.Controls.Add(this.tabs);
      this.Controls.Add(this.checkLecteur);
      this.Controls.Add(this.btDisconnect);
      this.Controls.Add(this.lblHubInfos);
      this.Controls.Add(this.statutHub);
      this.Controls.Add(this.label5);
      this.Controls.Add(this.btConnect);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MinimumSize = new System.Drawing.Size(773, 474);
      this.Name = "FSimulator";
      this.Text = "Simulation de lecteur";
      this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FSimulator_FormClosed);
      this.Load += new System.EventHandler(this.FSimulator_Load);
      this.groupStart.ResumeLayout(false);
      this.groupStart.PerformLayout();
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.groupStop.ResumeLayout(false);
      this.groupStop.PerformLayout();
      this.groupReset.ResumeLayout(false);
      this.groupReset.PerformLayout();
      this.groupEncodeCommande.ResumeLayout(false);
      this.groupEncodeCommande.PerformLayout();
      this.groupEncodeAssemblage.ResumeLayout(false);
      this.groupEncodeAssemblage.PerformLayout();
      this.tabs.ResumeLayout(false);
      this.tabPage9.ResumeLayout(false);
      this.tabPage9.PerformLayout();
      this.tabPage1.ResumeLayout(false);
      this.tabPage2.ResumeLayout(false);
      this.tabPage7.ResumeLayout(false);
      this.tabPage3.ResumeLayout(false);
      this.tabPage6.ResumeLayout(false);
      this.tabPage4.ResumeLayout(false);
      this.tabPage8.ResumeLayout(false);
      this.tabPage5.ResumeLayout(false);
      this.tabPage10.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ListBox listBox2;
    private System.Windows.Forms.GroupBox groupStart;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Button btStartOk;
    private System.Windows.Forms.Button btStartKo;
    private System.Windows.Forms.TextBox txtStartError;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.GroupBox groupStop;
    private System.Windows.Forms.Button btStopKo;
    private System.Windows.Forms.TextBox txtStopError;
    private System.Windows.Forms.Button btStopOk;
    private System.Windows.Forms.GroupBox groupReset;
    private System.Windows.Forms.Button btResetKo;
    private System.Windows.Forms.TextBox txtResetError;
    private System.Windows.Forms.Button btResetOk;
    private System.Windows.Forms.Panel pnlLecteurOn;
    private System.Windows.Forms.CheckedListBox tags1;
    private System.Windows.Forms.GroupBox groupEncodeCommande;
    private System.Windows.Forms.Button btEncodeCommande;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Button btEncodeCommandeKo;
    private System.Windows.Forms.TextBox txtEncodeCommandeError;
    private System.Windows.Forms.Button btEncodeCommandeOk;
    private System.Windows.Forms.GroupBox groupEncodeAssemblage;
    private System.Windows.Forms.Button btEncodeAssemblage;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.Button btEncodeAssemblageKo;
    private System.Windows.Forms.TextBox txtEncodeAssemblageError;
    private System.Windows.Forms.Button btEncodeAssemblageOk;
    private System.Windows.Forms.TabControl tabs;
    private System.Windows.Forms.TabPage tabPage1;
    private System.Windows.Forms.TabPage tabPage2;
    private System.Windows.Forms.CheckedListBox tags2;
    private System.Windows.Forms.TabPage tabPage3;
    private System.Windows.Forms.CheckedListBox tags3;
    private System.Windows.Forms.TabPage tabPage4;
    private System.Windows.Forms.CheckedListBox tags4;
    private System.Windows.Forms.TabPage tabPage5;
    private System.Windows.Forms.CheckedListBox tags0;
    private System.Windows.Forms.TabPage tabPage6;
    private System.Windows.Forms.CheckedListBox tags5;
    private System.Windows.Forms.Button btClearLog;
    private System.Windows.Forms.Button btRefreshTag;
    private System.Windows.Forms.TabPage tabPage7;
    private System.Windows.Forms.CheckedListBox tags6;
    private System.Windows.Forms.TabPage tabPage8;
    private System.Windows.Forms.CheckedListBox tags7;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.Label label9;
    private System.Windows.Forms.Label label10;
    private System.Windows.Forms.Label label11;
    private System.Windows.Forms.Label label12;
    private System.Windows.Forms.Label label13;
    private System.Windows.Forms.Label label14;
    private System.Windows.Forms.Button btProgresse;
    private System.Windows.Forms.TextBox txtEncodeTotal;
    private System.Windows.Forms.TextBox txtEncodeIndex;
    private System.Windows.Forms.TabPage tabPage9;
    private System.Windows.Forms.CheckBox checkEncodeur;
    private System.Windows.Forms.CheckBox checkLecteur;
    private System.Windows.Forms.Label lblHubInfos;
    private System.Windows.Forms.Button btDisconnect;
    private System.Windows.Forms.Button btConnect;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Panel statutHub;
    private System.Windows.Forms.TabPage tabPage10;
    private System.Windows.Forms.ListBox lstClients;
    private System.Windows.Forms.Label lblClient;
    private System.Windows.Forms.Label lblXml;
    private System.Windows.Forms.Label lblAsseId;
    private System.Windows.Forms.Label lblComdId;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label15;
    private System.Windows.Forms.Label lblNombreClient;
    private System.Windows.Forms.Button btDelClient;
    private System.Windows.Forms.CheckBox chkStartRepondOk;
    private System.Windows.Forms.CheckBox chkStopRepondOk;
    private System.Windows.Forms.CheckBox chkResetRepondOk;
    private System.Windows.Forms.Label lblEncodeTitre;
  }
}

