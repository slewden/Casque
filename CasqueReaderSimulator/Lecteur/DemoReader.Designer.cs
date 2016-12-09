namespace CasqueReaderSimulator.Lecteur
{
  partial class DemoReader
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
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Code généré par le Concepteur de composants

    /// <summary> 
    /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
    /// le contenu de cette méthode avec l'éditeur de code.
    /// </summary>
    private void InitializeComponent()
    {
      this.panel2 = new System.Windows.Forms.Panel();
      this.chkReponseAuto = new System.Windows.Forms.CheckBox();
      this.lblNombreLecture = new System.Windows.Forms.Label();
      this.lblNombreLectureTitre = new System.Windows.Forms.Label();
      this.pnlBusy = new System.Windows.Forms.Panel();
      this.label1 = new System.Windows.Forms.Label();
      this.lblStatut = new System.Windows.Forms.Label();
      this.lblClient = new System.Windows.Forms.Label();
      this.lblClientTitre = new System.Windows.Forms.Label();
      this.panel1 = new System.Windows.Forms.Panel();
      this.lblAdresseIp = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.grpReception = new System.Windows.Forms.GroupBox();
      this.label3 = new System.Windows.Forms.Label();
      this.txtRepondKo = new System.Windows.Forms.TextBox();
      this.btRepondKo = new System.Windows.Forms.Button();
      this.btRepondOk = new System.Windows.Forms.Button();
      this.lblCommande = new System.Windows.Forms.Label();
      this.tabControl1 = new System.Windows.Forms.TabControl();
      this.tab1 = new System.Windows.Forms.TabPage();
      this.lst1 = new System.Windows.Forms.CheckedListBox();
      this.tab7 = new System.Windows.Forms.TabPage();
      this.lst7 = new System.Windows.Forms.CheckedListBox();
      this.tab3 = new System.Windows.Forms.TabPage();
      this.lst3 = new System.Windows.Forms.CheckedListBox();
      this.tab6 = new System.Windows.Forms.TabPage();
      this.lst6 = new System.Windows.Forms.CheckedListBox();
      this.tab4 = new System.Windows.Forms.TabPage();
      this.lst4 = new System.Windows.Forms.CheckedListBox();
      this.tab8 = new System.Windows.Forms.TabPage();
      this.lst5 = new System.Windows.Forms.CheckedListBox();
      this.tab0 = new System.Windows.Forms.TabPage();
      this.lst0 = new System.Windows.Forms.CheckedListBox();
      this.panel2.SuspendLayout();
      this.panel1.SuspendLayout();
      this.grpReception.SuspendLayout();
      this.tabControl1.SuspendLayout();
      this.tab1.SuspendLayout();
      this.tab7.SuspendLayout();
      this.tab3.SuspendLayout();
      this.tab6.SuspendLayout();
      this.tab4.SuspendLayout();
      this.tab8.SuspendLayout();
      this.tab0.SuspendLayout();
      this.SuspendLayout();
      // 
      // panel2
      // 
      this.panel2.Controls.Add(this.chkReponseAuto);
      this.panel2.Controls.Add(this.lblNombreLecture);
      this.panel2.Controls.Add(this.lblNombreLectureTitre);
      this.panel2.Controls.Add(this.pnlBusy);
      this.panel2.Controls.Add(this.label1);
      this.panel2.Controls.Add(this.lblStatut);
      this.panel2.Controls.Add(this.lblClient);
      this.panel2.Controls.Add(this.lblClientTitre);
      this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
      this.panel2.Location = new System.Drawing.Point(0, 25);
      this.panel2.Name = "panel2";
      this.panel2.Size = new System.Drawing.Size(610, 62);
      this.panel2.TabIndex = 11;
      // 
      // chkReponseAuto
      // 
      this.chkReponseAuto.AutoSize = true;
      this.chkReponseAuto.Location = new System.Drawing.Point(362, 44);
      this.chkReponseAuto.Name = "chkReponseAuto";
      this.chkReponseAuto.Size = new System.Drawing.Size(93, 17);
      this.chkReponseAuto.TabIndex = 7;
      this.chkReponseAuto.Text = "Reponse auto";
      this.chkReponseAuto.UseVisualStyleBackColor = true;
      // 
      // lblNombreLecture
      // 
      this.lblNombreLecture.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.lblNombreLecture.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblNombreLecture.Location = new System.Drawing.Point(550, 10);
      this.lblNombreLecture.Name = "lblNombreLecture";
      this.lblNombreLecture.Size = new System.Drawing.Size(58, 35);
      this.lblNombreLecture.TabIndex = 6;
      this.lblNombreLecture.Text = "999";
      this.lblNombreLecture.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // lblNombreLectureTitre
      // 
      this.lblNombreLectureTitre.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.lblNombreLectureTitre.AutoSize = true;
      this.lblNombreLectureTitre.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblNombreLectureTitre.Location = new System.Drawing.Point(522, 47);
      this.lblNombreLectureTitre.Name = "lblNombreLectureTitre";
      this.lblNombreLectureTitre.Size = new System.Drawing.Size(85, 12);
      this.lblNombreLectureTitre.TabIndex = 5;
      this.lblNombreLectureTitre.Text = "Nombre de lectures";
      // 
      // pnlBusy
      // 
      this.pnlBusy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
      this.pnlBusy.Location = new System.Drawing.Point(109, 12);
      this.pnlBusy.Name = "pnlBusy";
      this.pnlBusy.Size = new System.Drawing.Size(21, 20);
      this.pnlBusy.TabIndex = 1;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(7, 16);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(82, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Etat du lecteur :";
      // 
      // lblStatut
      // 
      this.lblStatut.AutoSize = true;
      this.lblStatut.Location = new System.Drawing.Point(136, 16);
      this.lblStatut.Name = "lblStatut";
      this.lblStatut.Size = new System.Drawing.Size(45, 13);
      this.lblStatut.TabIndex = 2;
      this.lblStatut.Text = "lblStatut";
      // 
      // lblClient
      // 
      this.lblClient.AutoSize = true;
      this.lblClient.Location = new System.Drawing.Point(108, 40);
      this.lblClient.Name = "lblClient";
      this.lblClient.Size = new System.Drawing.Size(22, 13);
      this.lblClient.TabIndex = 3;
      this.lblClient.Text = "xxx";
      // 
      // lblClientTitre
      // 
      this.lblClientTitre.AutoSize = true;
      this.lblClientTitre.Location = new System.Drawing.Point(7, 40);
      this.lblClientTitre.Name = "lblClientTitre";
      this.lblClientTitre.Size = new System.Drawing.Size(39, 13);
      this.lblClientTitre.TabIndex = 4;
      this.lblClientTitre.Text = "Client :";
      // 
      // panel1
      // 
      this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
      this.panel1.Controls.Add(this.lblAdresseIp);
      this.panel1.Controls.Add(this.label4);
      this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
      this.panel1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
      this.panel1.Location = new System.Drawing.Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(610, 25);
      this.panel1.TabIndex = 10;
      // 
      // lblAdresseIp
      // 
      this.lblAdresseIp.AutoSize = true;
      this.lblAdresseIp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblAdresseIp.Location = new System.Drawing.Point(66, 5);
      this.lblAdresseIp.Name = "lblAdresseIp";
      this.lblAdresseIp.Size = new System.Drawing.Size(80, 16);
      this.lblAdresseIp.TabIndex = 8;
      this.lblAdresseIp.Text = "AdresseIP";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(9, 7);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(51, 13);
      this.label4.TabIndex = 7;
      this.label4.Text = "Adresse :";
      // 
      // grpReception
      // 
      this.grpReception.Controls.Add(this.label3);
      this.grpReception.Controls.Add(this.txtRepondKo);
      this.grpReception.Controls.Add(this.btRepondKo);
      this.grpReception.Controls.Add(this.btRepondOk);
      this.grpReception.Controls.Add(this.lblCommande);
      this.grpReception.Dock = System.Windows.Forms.DockStyle.Top;
      this.grpReception.Location = new System.Drawing.Point(0, 87);
      this.grpReception.Name = "grpReception";
      this.grpReception.Size = new System.Drawing.Size(610, 75);
      this.grpReception.TabIndex = 12;
      this.grpReception.TabStop = false;
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(7, 13);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(94, 13);
      this.label3.TabIndex = 7;
      this.label3.Text = "Reçu commande :";
      // 
      // txtRepondKo
      // 
      this.txtRepondKo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtRepondKo.Location = new System.Drawing.Point(295, 42);
      this.txtRepondKo.Name = "txtRepondKo";
      this.txtRepondKo.Size = new System.Drawing.Size(302, 20);
      this.txtRepondKo.TabIndex = 6;
      this.txtRepondKo.TextChanged += new System.EventHandler(this.GereBouton);
      // 
      // btRepondKo
      // 
      this.btRepondKo.Location = new System.Drawing.Point(160, 40);
      this.btRepondKo.Name = "btRepondKo";
      this.btRepondKo.Size = new System.Drawing.Size(129, 23);
      this.btRepondKo.TabIndex = 5;
      this.btRepondKo.Text = "Repondre Ko";
      this.btRepondKo.UseVisualStyleBackColor = true;
      this.btRepondKo.Click += new System.EventHandler(this.BtRepondKo_Click);
      // 
      // btRepondOk
      // 
      this.btRepondOk.Location = new System.Drawing.Point(14, 40);
      this.btRepondOk.Name = "btRepondOk";
      this.btRepondOk.Size = new System.Drawing.Size(129, 23);
      this.btRepondOk.TabIndex = 4;
      this.btRepondOk.Text = "Repondre Ok";
      this.btRepondOk.UseVisualStyleBackColor = true;
      this.btRepondOk.Click += new System.EventHandler(this.BtRepondOk_Click);
      // 
      // lblCommande
      // 
      this.lblCommande.AutoSize = true;
      this.lblCommande.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblCommande.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
      this.lblCommande.Location = new System.Drawing.Point(107, 8);
      this.lblCommande.Name = "lblCommande";
      this.lblCommande.Size = new System.Drawing.Size(88, 20);
      this.lblCommande.TabIndex = 3;
      this.lblCommande.Text = "commande";
      // 
      // tabControl1
      // 
      this.tabControl1.Controls.Add(this.tab1);
      this.tabControl1.Controls.Add(this.tab6);
      this.tabControl1.Controls.Add(this.tab3);
      this.tabControl1.Controls.Add(this.tab8);
      this.tabControl1.Controls.Add(this.tab4);
      this.tabControl1.Controls.Add(this.tab7);
      this.tabControl1.Controls.Add(this.tab0);
      this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tabControl1.ItemSize = new System.Drawing.Size(58, 25);
      this.tabControl1.Location = new System.Drawing.Point(0, 162);
      this.tabControl1.Name = "tabControl1";
      this.tabControl1.SelectedIndex = 0;
      this.tabControl1.Size = new System.Drawing.Size(610, 386);
      this.tabControl1.TabIndex = 13;
      // 
      // tab1
      // 
      this.tab1.Controls.Add(this.lst1);
      this.tab1.Location = new System.Drawing.Point(4, 29);
      this.tab1.Name = "tab1";
      this.tab1.Padding = new System.Windows.Forms.Padding(3);
      this.tab1.Size = new System.Drawing.Size(602, 353);
      this.tab1.TabIndex = 0;
      this.tab1.Text = "Commandés";
      this.tab1.UseVisualStyleBackColor = true;
      // 
      // lst1
      // 
      this.lst1.CheckOnClick = true;
      this.lst1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lst1.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lst1.FormattingEnabled = true;
      this.lst1.IntegralHeight = false;
      this.lst1.Location = new System.Drawing.Point(3, 3);
      this.lst1.Name = "lst1";
      this.lst1.Size = new System.Drawing.Size(596, 347);
      this.lst1.TabIndex = 0;
      this.lst1.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.TagRead);
      // 
      // tab7
      // 
      this.tab7.Controls.Add(this.lst7);
      this.tab7.Location = new System.Drawing.Point(4, 29);
      this.tab7.Name = "tab7";
      this.tab7.Padding = new System.Windows.Forms.Padding(3);
      this.tab7.Size = new System.Drawing.Size(602, 353);
      this.tab7.TabIndex = 1;
      this.tab7.Text = "Casques livrés";
      this.tab7.UseVisualStyleBackColor = true;
      // 
      // lst7
      // 
      this.lst7.CheckOnClick = true;
      this.lst7.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lst7.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lst7.FormattingEnabled = true;
      this.lst7.IntegralHeight = false;
      this.lst7.Location = new System.Drawing.Point(3, 3);
      this.lst7.Name = "lst7";
      this.lst7.Size = new System.Drawing.Size(596, 347);
      this.lst7.TabIndex = 1;
      this.lst7.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.TagRead);
      // 
      // tab3
      // 
      this.tab3.Controls.Add(this.lst3);
      this.tab3.Location = new System.Drawing.Point(4, 29);
      this.tab3.Name = "tab3";
      this.tab3.Size = new System.Drawing.Size(602, 353);
      this.tab3.TabIndex = 2;
      this.tab3.Text = "Assemblés";
      this.tab3.UseVisualStyleBackColor = true;
      // 
      // lst3
      // 
      this.lst3.CheckOnClick = true;
      this.lst3.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lst3.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lst3.FormattingEnabled = true;
      this.lst3.IntegralHeight = false;
      this.lst3.Location = new System.Drawing.Point(0, 0);
      this.lst3.Name = "lst3";
      this.lst3.Size = new System.Drawing.Size(602, 353);
      this.lst3.TabIndex = 1;
      this.lst3.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.TagRead);
      // 
      // tab6
      // 
      this.tab6.Controls.Add(this.lst6);
      this.tab6.Location = new System.Drawing.Point(4, 29);
      this.tab6.Name = "tab6";
      this.tab6.Size = new System.Drawing.Size(602, 353);
      this.tab6.TabIndex = 3;
      this.tab6.Text = "Assemblages possibles";
      this.tab6.UseVisualStyleBackColor = true;
      // 
      // lst6
      // 
      this.lst6.CheckOnClick = true;
      this.lst6.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lst6.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lst6.FormattingEnabled = true;
      this.lst6.IntegralHeight = false;
      this.lst6.Location = new System.Drawing.Point(0, 0);
      this.lst6.Name = "lst6";
      this.lst6.Size = new System.Drawing.Size(602, 353);
      this.lst6.TabIndex = 1;
      this.lst6.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.TagRead);
      // 
      // tab4
      // 
      this.tab4.Controls.Add(this.lst4);
      this.tab4.Location = new System.Drawing.Point(4, 29);
      this.tab4.Name = "tab4";
      this.tab4.Size = new System.Drawing.Size(602, 353);
      this.tab4.TabIndex = 4;
      this.tab4.Text = "Livrés";
      this.tab4.UseVisualStyleBackColor = true;
      // 
      // lst4
      // 
      this.lst4.CheckOnClick = true;
      this.lst4.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lst4.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lst4.FormattingEnabled = true;
      this.lst4.IntegralHeight = false;
      this.lst4.Location = new System.Drawing.Point(0, 0);
      this.lst4.Name = "lst4";
      this.lst4.Size = new System.Drawing.Size(602, 353);
      this.lst4.TabIndex = 1;
      this.lst4.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.TagRead);
      // 
      // tab8
      // 
      this.tab8.Controls.Add(this.lst5);
      this.tab8.Location = new System.Drawing.Point(4, 29);
      this.tab8.Name = "tab8";
      this.tab8.Size = new System.Drawing.Size(602, 353);
      this.tab8.TabIndex = 5;
      this.tab8.Text = "Casques assemblés";
      this.tab8.UseVisualStyleBackColor = true;
      // 
      // lst5
      // 
      this.lst5.CheckOnClick = true;
      this.lst5.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lst5.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lst5.FormattingEnabled = true;
      this.lst5.IntegralHeight = false;
      this.lst5.Location = new System.Drawing.Point(0, 0);
      this.lst5.Name = "lst5";
      this.lst5.Size = new System.Drawing.Size(602, 353);
      this.lst5.TabIndex = 1;
      this.lst5.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.TagRead);
      // 
      // tab0
      // 
      this.tab0.Controls.Add(this.lst0);
      this.tab0.Location = new System.Drawing.Point(4, 29);
      this.tab0.Name = "tab0";
      this.tab0.Size = new System.Drawing.Size(602, 353);
      this.tab0.TabIndex = 6;
      this.tab0.Text = "Inconnus";
      this.tab0.UseVisualStyleBackColor = true;
      // 
      // lst0
      // 
      this.lst0.CheckOnClick = true;
      this.lst0.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lst0.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lst0.FormattingEnabled = true;
      this.lst0.IntegralHeight = false;
      this.lst0.Location = new System.Drawing.Point(0, 0);
      this.lst0.Name = "lst0";
      this.lst0.Size = new System.Drawing.Size(602, 353);
      this.lst0.TabIndex = 1;
      this.lst0.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.TagRead);
      // 
      // DemoReader
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.tabControl1);
      this.Controls.Add(this.grpReception);
      this.Controls.Add(this.panel2);
      this.Controls.Add(this.panel1);
      this.Name = "DemoReader";
      this.Size = new System.Drawing.Size(610, 548);
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.grpReception.ResumeLayout(false);
      this.grpReception.PerformLayout();
      this.tabControl1.ResumeLayout(false);
      this.tab1.ResumeLayout(false);
      this.tab7.ResumeLayout(false);
      this.tab3.ResumeLayout(false);
      this.tab6.ResumeLayout(false);
      this.tab4.ResumeLayout(false);
      this.tab8.ResumeLayout(false);
      this.tab0.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Panel panel2;
    private System.Windows.Forms.Panel pnlBusy;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label lblStatut;
    private System.Windows.Forms.Label lblClient;
    private System.Windows.Forms.Label lblClientTitre;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.Label lblAdresseIp;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.GroupBox grpReception;
    private System.Windows.Forms.TextBox txtRepondKo;
    private System.Windows.Forms.Button btRepondKo;
    private System.Windows.Forms.Button btRepondOk;
    private System.Windows.Forms.Label lblCommande;
    private System.Windows.Forms.TabControl tabControl1;
    private System.Windows.Forms.TabPage tab1;
    private System.Windows.Forms.TabPage tab7;
    private System.Windows.Forms.TabPage tab3;
    private System.Windows.Forms.TabPage tab6;
    private System.Windows.Forms.TabPage tab4;
    private System.Windows.Forms.TabPage tab8;
    private System.Windows.Forms.TabPage tab0;
    private System.Windows.Forms.CheckedListBox lst1;
    private System.Windows.Forms.CheckedListBox lst7;
    private System.Windows.Forms.CheckedListBox lst3;
    private System.Windows.Forms.CheckedListBox lst6;
    private System.Windows.Forms.CheckedListBox lst4;
    private System.Windows.Forms.CheckedListBox lst5;
    private System.Windows.Forms.CheckedListBox lst0;
    private System.Windows.Forms.Label lblNombreLecture;
    private System.Windows.Forms.Label lblNombreLectureTitre;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.CheckBox chkReponseAuto;
  }
}
