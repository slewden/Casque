namespace CasqueReaderSimulator.Lecteur
{
  partial class DemoWriter
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

      if (this.timer1 != null)
      {
        this.timer1.Dispose();
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
      this.label1 = new System.Windows.Forms.Label();
      this.pnlBusy = new System.Windows.Forms.Panel();
      this.lblStatut = new System.Windows.Forms.Label();
      this.lblClient = new System.Windows.Forms.Label();
      this.lblClientTitre = new System.Windows.Forms.Label();
      this.grpReception = new System.Windows.Forms.GroupBox();
      this.label2 = new System.Windows.Forms.Label();
      this.txtRepondKo = new System.Windows.Forms.TextBox();
      this.btRepondKo = new System.Windows.Forms.Button();
      this.btRepondOk = new System.Windows.Forms.Button();
      this.lblCommande = new System.Windows.Forms.Label();
      this.grpOn = new System.Windows.Forms.GroupBox();
      this.txtNotifieKo = new System.Windows.Forms.TextBox();
      this.btNotifieKo = new System.Windows.Forms.Button();
      this.btNotifieOk = new System.Windows.Forms.Button();
      this.label3 = new System.Windows.Forms.Label();
      this.txtTotal = new System.Windows.Forms.TextBox();
      this.txtIndex = new System.Windows.Forms.TextBox();
      this.btNotifieProgression = new System.Windows.Forms.Button();
      this.label4 = new System.Windows.Forms.Label();
      this.panel1 = new System.Windows.Forms.Panel();
      this.lblAdresseIp = new System.Windows.Forms.Label();
      this.panel2 = new System.Windows.Forms.Panel();
      this.chkReponseAuto = new System.Windows.Forms.CheckBox();
      this.grpReception.SuspendLayout();
      this.grpOn.SuspendLayout();
      this.panel1.SuspendLayout();
      this.panel2.SuspendLayout();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(7, 16);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(99, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Etat de l\'encodeur :";
      // 
      // pnlBusy
      // 
      this.pnlBusy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
      this.pnlBusy.Location = new System.Drawing.Point(109, 12);
      this.pnlBusy.Name = "pnlBusy";
      this.pnlBusy.Size = new System.Drawing.Size(21, 20);
      this.pnlBusy.TabIndex = 1;
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
      // grpReception
      // 
      this.grpReception.Controls.Add(this.label2);
      this.grpReception.Controls.Add(this.txtRepondKo);
      this.grpReception.Controls.Add(this.btRepondKo);
      this.grpReception.Controls.Add(this.btRepondOk);
      this.grpReception.Controls.Add(this.lblCommande);
      this.grpReception.Dock = System.Windows.Forms.DockStyle.Top;
      this.grpReception.Location = new System.Drawing.Point(0, 87);
      this.grpReception.Name = "grpReception";
      this.grpReception.Size = new System.Drawing.Size(426, 76);
      this.grpReception.TabIndex = 5;
      this.grpReception.TabStop = false;
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(7, 14);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(94, 13);
      this.label2.TabIndex = 7;
      this.label2.Text = "Reçu commande :";
      // 
      // txtRepondKo
      // 
      this.txtRepondKo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtRepondKo.Location = new System.Drawing.Point(280, 46);
      this.txtRepondKo.Name = "txtRepondKo";
      this.txtRepondKo.Size = new System.Drawing.Size(140, 20);
      this.txtRepondKo.TabIndex = 6;
      this.txtRepondKo.TextChanged += new System.EventHandler(this.GereBouton);
      // 
      // btRepondKo
      // 
      this.btRepondKo.Location = new System.Drawing.Point(145, 43);
      this.btRepondKo.Name = "btRepondKo";
      this.btRepondKo.Size = new System.Drawing.Size(129, 23);
      this.btRepondKo.TabIndex = 5;
      this.btRepondKo.Text = "Repondre Ko";
      this.btRepondKo.UseVisualStyleBackColor = true;
      this.btRepondKo.Click += new System.EventHandler(this.BtRepondKo_Click);
      // 
      // btRepondOk
      // 
      this.btRepondOk.Location = new System.Drawing.Point(10, 43);
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
      this.lblCommande.Location = new System.Drawing.Point(107, 9);
      this.lblCommande.Name = "lblCommande";
      this.lblCommande.Size = new System.Drawing.Size(88, 20);
      this.lblCommande.TabIndex = 3;
      this.lblCommande.Text = "commande";
      // 
      // grpOn
      // 
      this.grpOn.Controls.Add(this.txtNotifieKo);
      this.grpOn.Controls.Add(this.btNotifieKo);
      this.grpOn.Controls.Add(this.btNotifieOk);
      this.grpOn.Controls.Add(this.label3);
      this.grpOn.Controls.Add(this.txtTotal);
      this.grpOn.Controls.Add(this.txtIndex);
      this.grpOn.Controls.Add(this.btNotifieProgression);
      this.grpOn.Dock = System.Windows.Forms.DockStyle.Top;
      this.grpOn.Location = new System.Drawing.Point(0, 163);
      this.grpOn.Name = "grpOn";
      this.grpOn.Size = new System.Drawing.Size(426, 88);
      this.grpOn.TabIndex = 6;
      this.grpOn.TabStop = false;
      this.grpOn.Text = "Encoreur en fonction";
      // 
      // txtNotifieKo
      // 
      this.txtNotifieKo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtNotifieKo.Location = new System.Drawing.Point(160, 56);
      this.txtNotifieKo.Name = "txtNotifieKo";
      this.txtNotifieKo.Size = new System.Drawing.Size(260, 20);
      this.txtNotifieKo.TabIndex = 9;
      this.txtNotifieKo.Text = "Erreur dans l\'encodage";
      this.txtNotifieKo.TextChanged += new System.EventHandler(this.GereBouton);
      // 
      // btNotifieKo
      // 
      this.btNotifieKo.Location = new System.Drawing.Point(14, 53);
      this.btNotifieKo.Name = "btNotifieKo";
      this.btNotifieKo.Size = new System.Drawing.Size(129, 23);
      this.btNotifieKo.TabIndex = 8;
      this.btNotifieKo.Text = "Fin Ko";
      this.btNotifieKo.UseVisualStyleBackColor = true;
      this.btNotifieKo.Click += new System.EventHandler(this.BtNotifieKo_Click);
      // 
      // btNotifieOk
      // 
      this.btNotifieOk.Location = new System.Drawing.Point(14, 24);
      this.btNotifieOk.Name = "btNotifieOk";
      this.btNotifieOk.Size = new System.Drawing.Size(129, 23);
      this.btNotifieOk.TabIndex = 7;
      this.btNotifieOk.Text = "Fin Ok";
      this.btNotifieOk.UseVisualStyleBackColor = true;
      this.btNotifieOk.Click += new System.EventHandler(this.BtNotifieOk_Click);
      // 
      // label3
      // 
      this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(203, 29);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(21, 13);
      this.label3.TabIndex = 3;
      this.label3.Text = "sur";
      // 
      // txtTotal
      // 
      this.txtTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.txtTotal.Location = new System.Drawing.Point(230, 26);
      this.txtTotal.Name = "txtTotal";
      this.txtTotal.ReadOnly = true;
      this.txtTotal.Size = new System.Drawing.Size(37, 20);
      this.txtTotal.TabIndex = 2;
      this.txtTotal.TextChanged += new System.EventHandler(this.GereBouton);
      // 
      // txtIndex
      // 
      this.txtIndex.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.txtIndex.Location = new System.Drawing.Point(160, 26);
      this.txtIndex.Name = "txtIndex";
      this.txtIndex.ReadOnly = true;
      this.txtIndex.Size = new System.Drawing.Size(37, 20);
      this.txtIndex.TabIndex = 1;
      this.txtIndex.TextChanged += new System.EventHandler(this.GereBouton);
      // 
      // btNotifieProgression
      // 
      this.btNotifieProgression.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btNotifieProgression.Location = new System.Drawing.Point(273, 24);
      this.btNotifieProgression.Name = "btNotifieProgression";
      this.btNotifieProgression.Size = new System.Drawing.Size(147, 23);
      this.btNotifieProgression.TabIndex = 0;
      this.btNotifieProgression.Text = "Notifier la progression";
      this.btNotifieProgression.UseVisualStyleBackColor = true;
      this.btNotifieProgression.Click += new System.EventHandler(this.BtNotifieProgression_Click);
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
      // panel1
      // 
      this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
      this.panel1.Controls.Add(this.lblAdresseIp);
      this.panel1.Controls.Add(this.label4);
      this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
      this.panel1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
      this.panel1.Location = new System.Drawing.Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(426, 25);
      this.panel1.TabIndex = 8;
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
      // panel2
      // 
      this.panel2.Controls.Add(this.chkReponseAuto);
      this.panel2.Controls.Add(this.pnlBusy);
      this.panel2.Controls.Add(this.label1);
      this.panel2.Controls.Add(this.lblStatut);
      this.panel2.Controls.Add(this.lblClient);
      this.panel2.Controls.Add(this.lblClientTitre);
      this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
      this.panel2.Location = new System.Drawing.Point(0, 25);
      this.panel2.Name = "panel2";
      this.panel2.Size = new System.Drawing.Size(426, 62);
      this.panel2.TabIndex = 9;
      // 
      // chkReponseAuto
      // 
      this.chkReponseAuto.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.chkReponseAuto.AutoSize = true;
      this.chkReponseAuto.Location = new System.Drawing.Point(333, 45);
      this.chkReponseAuto.Name = "chkReponseAuto";
      this.chkReponseAuto.Size = new System.Drawing.Size(93, 17);
      this.chkReponseAuto.TabIndex = 8;
      this.chkReponseAuto.Text = "Reponse auto";
      this.chkReponseAuto.UseVisualStyleBackColor = true;
      // 
      // DemoWriter
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.grpOn);
      this.Controls.Add(this.grpReception);
      this.Controls.Add(this.panel2);
      this.Controls.Add(this.panel1);
      this.MinimumSize = new System.Drawing.Size(426, 296);
      this.Name = "DemoWriter";
      this.Size = new System.Drawing.Size(426, 296);
      this.grpReception.ResumeLayout(false);
      this.grpReception.PerformLayout();
      this.grpOn.ResumeLayout(false);
      this.grpOn.PerformLayout();
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Panel pnlBusy;
    private System.Windows.Forms.Label lblStatut;
    private System.Windows.Forms.Label lblClient;
    private System.Windows.Forms.Label lblClientTitre;
    private System.Windows.Forms.GroupBox grpReception;
    private System.Windows.Forms.Label lblCommande;
    private System.Windows.Forms.TextBox txtRepondKo;
    private System.Windows.Forms.Button btRepondKo;
    private System.Windows.Forms.Button btRepondOk;
    private System.Windows.Forms.GroupBox grpOn;
    private System.Windows.Forms.Button btNotifieProgression;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox txtTotal;
    private System.Windows.Forms.TextBox txtIndex;
    private System.Windows.Forms.TextBox txtNotifieKo;
    private System.Windows.Forms.Button btNotifieKo;
    private System.Windows.Forms.Button btNotifieOk;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.Label lblAdresseIp;
    private System.Windows.Forms.Panel panel2;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.CheckBox chkReponseAuto;
  }
}
