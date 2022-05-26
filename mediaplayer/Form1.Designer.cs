namespace mediaplayer
{
    partial class frmPlayer1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnScreen = new System.Windows.Forms.Panel();
            this.lblMediaInfo = new System.Windows.Forms.Label();
            this.btnPlay = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // pnScreen
            // 
            this.pnScreen.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pnScreen.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.pnScreen.Location = new System.Drawing.Point(8, 54);
            this.pnScreen.Name = "pnScreen";
            this.pnScreen.Size = new System.Drawing.Size(922, 690);
            this.pnScreen.TabIndex = 0;
            // 
            // lblMediaInfo
            // 
            this.lblMediaInfo.BackColor = System.Drawing.Color.Black;
            this.lblMediaInfo.ForeColor = System.Drawing.Color.Transparent;
            this.lblMediaInfo.Location = new System.Drawing.Point(934, 54);
            this.lblMediaInfo.Name = "lblMediaInfo";
            this.lblMediaInfo.Size = new System.Drawing.Size(287, 687);
            this.lblMediaInfo.TabIndex = 6;
            this.lblMediaInfo.Text = "media info";
            // 
            // btnPlay
            // 
            this.btnPlay.Location = new System.Drawing.Point(455, 3);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(159, 37);
            this.btnPlay.TabIndex = 1;
            this.btnPlay.Text = "PLAY";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(620, 3);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(160, 37);
            this.btnStop.TabIndex = 2;
            this.btnStop.Text = "STOP";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // txtFilePath
            // 
            this.txtFilePath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFilePath.Location = new System.Drawing.Point(115, 12);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.Size = new System.Drawing.Size(322, 23);
            this.txtFilePath.TabIndex = 4;
            this.txtFilePath.Text = "5M_1080p29.97_1200k_lcevc.mp4";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(22, 10);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(87, 30);
            this.button1.TabIndex = 5;
            this.button1.Text = "Open";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(936, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(275, 37);
            this.button2.TabIndex = 7;
            this.button2.Text = "Media Info";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // frmPlayer1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1233, 750);
            this.Controls.Add(this.lblMediaInfo);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtFilePath);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnPlay);
            this.Controls.Add(this.pnScreen);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPlayer1";
            this.ShowIcon = false;
            this.Text = "Sample Player";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmPlayer1_FormClosed);
            this.Load += new System.EventHandler(this.frmPlayer1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Panel pnScreen;
        private Button btnPlay;
        private Button btnStop;
        private TextBox txtFilePath;
        private Button button1;
        private Label lblMediaInfo;
        private Button button2;
    }
}