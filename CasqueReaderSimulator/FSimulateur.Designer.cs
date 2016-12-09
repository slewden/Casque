namespace CasqueReaderSimulator
{
  partial class FSimulateur
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }

      this.client.Dispose();
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FSimulateur));
      this.splitContainer1 = new System.Windows.Forms.SplitContainer();
      this.splitContainer3 = new System.Windows.Forms.SplitContainer();
      this.lblNombreLecteur = new System.Windows.Forms.Label();
      this.lstLecteur = new System.Windows.Forms.ListBox();
      this.pictureBox1 = new System.Windows.Forms.PictureBox();
      this.label4 = new System.Windows.Forms.Label();
      this.lblNombreEncodeur = new System.Windows.Forms.Label();
      this.lstEncodeur = new System.Windows.Forms.ListBox();
      this.pictureBox2 = new System.Windows.Forms.PictureBox();
      this.label2 = new System.Windows.Forms.Label();
      this.panel4 = new System.Windows.Forms.Panel();
      this.checkEncodeur = new System.Windows.Forms.CheckBox();
      this.label5 = new System.Windows.Forms.Label();
      this.checkLecteur = new System.Windows.Forms.CheckBox();
      this.statutHub = new System.Windows.Forms.Panel();
      this.btDisconnect = new System.Windows.Forms.Button();
      this.btConnect = new System.Windows.Forms.Button();
      this.panel3 = new System.Windows.Forms.Panel();
      this.pictureBox3 = new System.Windows.Forms.PictureBox();
      this.lblTitreHub = new System.Windows.Forms.Label();
      this.splitContainer2 = new System.Windows.Forms.SplitContainer();
      this.lstLog = new System.Windows.Forms.ListBox();
      this.panel1 = new System.Windows.Forms.Panel();
      this.btClearLog = new System.Windows.Forms.Button();
      this.label1 = new System.Windows.Forms.Label();
      this.ChkReponseAuto = new System.Windows.Forms.CheckBox();
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
      this.splitContainer3.Panel1.SuspendLayout();
      this.splitContainer3.Panel2.SuspendLayout();
      this.splitContainer3.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
      this.panel4.SuspendLayout();
      this.panel3.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
      this.splitContainer2.Panel2.SuspendLayout();
      this.splitContainer2.SuspendLayout();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      // 
      // splitContainer1
      // 
      this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
      this.splitContainer1.Location = new System.Drawing.Point(0, 0);
      this.splitContainer1.Name = "splitContainer1";
      // 
      // splitContainer1.Panel1
      // 
      this.splitContainer1.Panel1.Controls.Add(this.splitContainer3);
      this.splitContainer1.Panel1.Controls.Add(this.panel4);
      this.splitContainer1.Panel1.Controls.Add(this.panel3);
      // 
      // splitContainer1.Panel2
      // 
      this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
      this.splitContainer1.Size = new System.Drawing.Size(969, 521);
      this.splitContainer1.SplitterDistance = 154;
      this.splitContainer1.TabIndex = 0;
      // 
      // splitContainer3
      // 
      this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitContainer3.Location = new System.Drawing.Point(0, 194);
      this.splitContainer3.Name = "splitContainer3";
      this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
      // 
      // splitContainer3.Panel1
      // 
      this.splitContainer3.Panel1.Controls.Add(this.lblNombreLecteur);
      this.splitContainer3.Panel1.Controls.Add(this.lstLecteur);
      this.splitContainer3.Panel1.Controls.Add(this.pictureBox1);
      this.splitContainer3.Panel1.Controls.Add(this.label4);
      // 
      // splitContainer3.Panel2
      // 
      this.splitContainer3.Panel2.Controls.Add(this.lblNombreEncodeur);
      this.splitContainer3.Panel2.Controls.Add(this.lstEncodeur);
      this.splitContainer3.Panel2.Controls.Add(this.pictureBox2);
      this.splitContainer3.Panel2.Controls.Add(this.label2);
      this.splitContainer3.Size = new System.Drawing.Size(154, 327);
      this.splitContainer3.SplitterDistance = 172;
      this.splitContainer3.TabIndex = 53;
      // 
      // lblNombreLecteur
      // 
      this.lblNombreLecteur.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.lblNombreLecteur.BackColor = System.Drawing.SystemColors.ActiveCaption;
      this.lblNombreLecteur.Location = new System.Drawing.Point(122, 8);
      this.lblNombreLecteur.Name = "lblNombreLecteur";
      this.lblNombreLecteur.Size = new System.Drawing.Size(29, 19);
      this.lblNombreLecteur.TabIndex = 55;
      this.lblNombreLecteur.Text = "999";
      this.lblNombreLecteur.TextAlign = System.Drawing.ContentAlignment.BottomRight;
      // 
      // lstLecteur
      // 
      this.lstLecteur.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lstLecteur.FormattingEnabled = true;
      this.lstLecteur.IntegralHeight = false;
      this.lstLecteur.Location = new System.Drawing.Point(0, 29);
      this.lstLecteur.Name = "lstLecteur";
      this.lstLecteur.Size = new System.Drawing.Size(154, 143);
      this.lstLecteur.TabIndex = 54;
      this.lstLecteur.SelectedIndexChanged += new System.EventHandler(this.LstLecteur_SelectedIndexChanged);
      // 
      // pictureBox1
      // 
      this.pictureBox1.Image = global::CasqueReaderSimulator.Properties.Resources.Lecteur;
      this.pictureBox1.Location = new System.Drawing.Point(6, 2);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new System.Drawing.Size(24, 25);
      this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBox1.TabIndex = 53;
      this.pictureBox1.TabStop = false;
      // 
      // label4
      // 
      this.label4.BackColor = System.Drawing.SystemColors.ActiveCaption;
      this.label4.Dock = System.Windows.Forms.DockStyle.Top;
      this.label4.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
      this.label4.Location = new System.Drawing.Point(0, 0);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(154, 29);
      this.label4.TabIndex = 52;
      this.label4.Text = "Lecteurs";
      this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // lblNombreEncodeur
      // 
      this.lblNombreEncodeur.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.lblNombreEncodeur.BackColor = System.Drawing.SystemColors.ActiveCaption;
      this.lblNombreEncodeur.Location = new System.Drawing.Point(125, 10);
      this.lblNombreEncodeur.Name = "lblNombreEncodeur";
      this.lblNombreEncodeur.Size = new System.Drawing.Size(29, 19);
      this.lblNombreEncodeur.TabIndex = 56;
      this.lblNombreEncodeur.Text = "999";
      this.lblNombreEncodeur.TextAlign = System.Drawing.ContentAlignment.BottomRight;
      // 
      // lstEncodeur
      // 
      this.lstEncodeur.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lstEncodeur.FormattingEnabled = true;
      this.lstEncodeur.IntegralHeight = false;
      this.lstEncodeur.Location = new System.Drawing.Point(0, 29);
      this.lstEncodeur.Name = "lstEncodeur";
      this.lstEncodeur.Size = new System.Drawing.Size(154, 122);
      this.lstEncodeur.TabIndex = 55;
      this.lstEncodeur.SelectedIndexChanged += new System.EventHandler(this.LstEncodeur_SelectedIndexChanged);
      // 
      // pictureBox2
      // 
      this.pictureBox2.Image = global::CasqueReaderSimulator.Properties.Resources.Imprimante;
      this.pictureBox2.Location = new System.Drawing.Point(6, 2);
      this.pictureBox2.Name = "pictureBox2";
      this.pictureBox2.Size = new System.Drawing.Size(30, 25);
      this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBox2.TabIndex = 54;
      this.pictureBox2.TabStop = false;
      // 
      // label2
      // 
      this.label2.BackColor = System.Drawing.SystemColors.ActiveCaption;
      this.label2.Dock = System.Windows.Forms.DockStyle.Top;
      this.label2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
      this.label2.Location = new System.Drawing.Point(0, 0);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(154, 29);
      this.label2.TabIndex = 52;
      this.label2.Text = "Encodeurs";
      this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // panel4
      // 
      this.panel4.BackColor = System.Drawing.SystemColors.Window;
      this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.panel4.Controls.Add(this.ChkReponseAuto);
      this.panel4.Controls.Add(this.checkEncodeur);
      this.panel4.Controls.Add(this.label5);
      this.panel4.Controls.Add(this.checkLecteur);
      this.panel4.Controls.Add(this.statutHub);
      this.panel4.Controls.Add(this.btDisconnect);
      this.panel4.Controls.Add(this.btConnect);
      this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
      this.panel4.Location = new System.Drawing.Point(0, 32);
      this.panel4.Name = "panel4";
      this.panel4.Size = new System.Drawing.Size(154, 162);
      this.panel4.TabIndex = 52;
      // 
      // checkEncodeur
      // 
      this.checkEncodeur.AutoSize = true;
      this.checkEncodeur.Checked = true;
      this.checkEncodeur.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkEncodeur.Location = new System.Drawing.Point(11, 90);
      this.checkEncodeur.Name = "checkEncodeur";
      this.checkEncodeur.Size = new System.Drawing.Size(120, 17);
      this.checkEncodeur.TabIndex = 50;
      this.checkEncodeur.Text = "Simule un encodeur";
      this.checkEncodeur.UseVisualStyleBackColor = true;
      this.checkEncodeur.CheckedChanged += new System.EventHandler(this.CheckMatosChanged);
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(11, 12);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(95, 13);
      this.label5.TabIndex = 46;
      this.label5.Text = "Connexion au Hub";
      // 
      // checkLecteur
      // 
      this.checkLecteur.AutoSize = true;
      this.checkLecteur.Checked = true;
      this.checkLecteur.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkLecteur.Location = new System.Drawing.Point(11, 67);
      this.checkLecteur.Name = "checkLecteur";
      this.checkLecteur.Size = new System.Drawing.Size(107, 17);
      this.checkLecteur.TabIndex = 49;
      this.checkLecteur.Text = "Simule un lecteur";
      this.checkLecteur.UseVisualStyleBackColor = true;
      this.checkLecteur.CheckedChanged += new System.EventHandler(this.CheckMatosChanged);
      // 
      // statutHub
      // 
      this.statutHub.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.statutHub.Location = new System.Drawing.Point(116, 6);
      this.statutHub.Name = "statutHub";
      this.statutHub.Size = new System.Drawing.Size(25, 21);
      this.statutHub.TabIndex = 45;
      // 
      // btDisconnect
      // 
      this.btDisconnect.Location = new System.Drawing.Point(11, 38);
      this.btDisconnect.Name = "btDisconnect";
      this.btDisconnect.Size = new System.Drawing.Size(129, 23);
      this.btDisconnect.TabIndex = 48;
      this.btDisconnect.Text = "Deconnecte";
      this.btDisconnect.UseVisualStyleBackColor = true;
      this.btDisconnect.Click += new System.EventHandler(this.BtDisconnect_Click);
      // 
      // btConnect
      // 
      this.btConnect.Location = new System.Drawing.Point(11, 38);
      this.btConnect.Name = "btConnect";
      this.btConnect.Size = new System.Drawing.Size(130, 23);
      this.btConnect.TabIndex = 47;
      this.btConnect.Text = "Connect";
      this.btConnect.UseVisualStyleBackColor = true;
      this.btConnect.Click += new System.EventHandler(this.BtConnect_Click);
      // 
      // panel3
      // 
      this.panel3.BackColor = System.Drawing.SystemColors.ActiveCaption;
      this.panel3.Controls.Add(this.pictureBox3);
      this.panel3.Controls.Add(this.lblTitreHub);
      this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
      this.panel3.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
      this.panel3.Location = new System.Drawing.Point(0, 0);
      this.panel3.Name = "panel3";
      this.panel3.Size = new System.Drawing.Size(154, 32);
      this.panel3.TabIndex = 2;
      // 
      // pictureBox3
      // 
      this.pictureBox3.Image = global::CasqueReaderSimulator.Properties.Resources.SignalRs;
      this.pictureBox3.Location = new System.Drawing.Point(6, 5);
      this.pictureBox3.Name = "pictureBox3";
      this.pictureBox3.Size = new System.Drawing.Size(24, 25);
      this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBox3.TabIndex = 54;
      this.pictureBox3.TabStop = false;
      // 
      // lblTitreHub
      // 
      this.lblTitreHub.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lblTitreHub.Location = new System.Drawing.Point(0, 0);
      this.lblTitreHub.Name = "lblTitreHub";
      this.lblTitreHub.Size = new System.Drawing.Size(154, 32);
      this.lblTitreHub.TabIndex = 0;
      this.lblTitreHub.Text = "Hub";
      this.lblTitreHub.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // splitContainer2
      // 
      this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
      this.splitContainer2.Location = new System.Drawing.Point(0, 0);
      this.splitContainer2.Name = "splitContainer2";
      // 
      // splitContainer2.Panel2
      // 
      this.splitContainer2.Panel2.Controls.Add(this.lstLog);
      this.splitContainer2.Panel2.Controls.Add(this.panel1);
      this.splitContainer2.Size = new System.Drawing.Size(811, 521);
      this.splitContainer2.SplitterDistance = 417;
      this.splitContainer2.TabIndex = 0;
      // 
      // lstLog
      // 
      this.lstLog.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lstLog.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lstLog.FormattingEnabled = true;
      this.lstLog.IntegralHeight = false;
      this.lstLog.ItemHeight = 14;
      this.lstLog.Location = new System.Drawing.Point(0, 32);
      this.lstLog.Name = "lstLog";
      this.lstLog.Size = new System.Drawing.Size(390, 489);
      this.lstLog.TabIndex = 0;
      // 
      // panel1
      // 
      this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
      this.panel1.Controls.Add(this.btClearLog);
      this.panel1.Controls.Add(this.label1);
      this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
      this.panel1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
      this.panel1.Location = new System.Drawing.Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(390, 32);
      this.panel1.TabIndex = 1;
      // 
      // btClearLog
      // 
      this.btClearLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btClearLog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btClearLog.Location = new System.Drawing.Point(362, 5);
      this.btClearLog.Name = "btClearLog";
      this.btClearLog.Size = new System.Drawing.Size(22, 23);
      this.btClearLog.TabIndex = 1;
      this.btClearLog.Text = "X";
      this.btClearLog.UseVisualStyleBackColor = true;
      this.btClearLog.Click += new System.EventHandler(this.BtClearLog_Click);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(12, 9);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(30, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Logs";
      // 
      // ChkReponseAuto
      // 
      this.ChkReponseAuto.AutoSize = true;
      this.ChkReponseAuto.Checked = true;
      this.ChkReponseAuto.CheckState = System.Windows.Forms.CheckState.Checked;
      this.ChkReponseAuto.Location = new System.Drawing.Point(11, 138);
      this.ChkReponseAuto.Name = "ChkReponseAuto";
      this.ChkReponseAuto.Size = new System.Drawing.Size(94, 17);
      this.ChkReponseAuto.TabIndex = 51;
      this.ChkReponseAuto.Text = "Réponse Auto";
      this.ChkReponseAuto.UseVisualStyleBackColor = true;
      this.ChkReponseAuto.CheckedChanged += new System.EventHandler(this.ChkReponseAuto_CheckedChanged);
      // 
      // FSimulateur
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(969, 521);
      this.Controls.Add(this.splitContainer1);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "FSimulateur";
      this.Text = "Simulation des lecteurs et encodeurs";
      this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FSimulateur_FormClosed);
      this.Load += new System.EventHandler(this.FSimulateur_Load);
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
      this.splitContainer1.ResumeLayout(false);
      this.splitContainer3.Panel1.ResumeLayout(false);
      this.splitContainer3.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
      this.splitContainer3.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
      this.panel4.ResumeLayout(false);
      this.panel4.PerformLayout();
      this.panel3.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
      this.splitContainer2.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
      this.splitContainer2.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.SplitContainer splitContainer1;
    private System.Windows.Forms.Panel panel3;
    private System.Windows.Forms.Label lblTitreHub;
    private System.Windows.Forms.SplitContainer splitContainer2;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.ListBox lstLog;
    private System.Windows.Forms.Panel panel4;
    private System.Windows.Forms.Button btConnect;
    private System.Windows.Forms.CheckBox checkEncodeur;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.CheckBox checkLecteur;
    private System.Windows.Forms.Panel statutHub;
    private System.Windows.Forms.Button btDisconnect;
    private System.Windows.Forms.Button btClearLog;
    private System.Windows.Forms.SplitContainer splitContainer3;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.PictureBox pictureBox1;
    private System.Windows.Forms.PictureBox pictureBox2;
    private System.Windows.Forms.ListBox lstLecteur;
    private System.Windows.Forms.ListBox lstEncodeur;
    private System.Windows.Forms.PictureBox pictureBox3;
    private System.Windows.Forms.Label lblNombreLecteur;
    private System.Windows.Forms.Label lblNombreEncodeur;
    private System.Windows.Forms.CheckBox ChkReponseAuto;
  }
}