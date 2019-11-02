namespace QobuzDownloaderX
{
    partial class LoginForm
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
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            this.passwordLabel = new System.Windows.Forms.Label();
            this.emailLabel = new System.Windows.Forms.Label();
            this.appIdLabel = new System.Windows.Forms.Label();
            this.emailTextbox = new System.Windows.Forms.TextBox();
            this.passwordTextbox = new System.Windows.Forms.TextBox();
            this.loginButton = new System.Windows.Forms.Button();
            this.appidTextbox = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.loginText = new System.Windows.Forms.Label();
            this.md5Button = new System.Windows.Forms.Button();
            this.loginBG = new System.ComponentModel.BackgroundWorker();
            this.getSecretBG = new System.ComponentModel.BackgroundWorker();
            this.verNumLabel2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // passwordLabel
            // 
            this.passwordLabel.AutoSize = true;
            this.passwordLabel.Location = new System.Drawing.Point(12, 143);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(59, 13);
            this.passwordLabel.TabIndex = 27;
            this.passwordLabel.Text = "Password -";
            // 
            // emailLabel
            // 
            this.emailLabel.AutoSize = true;
            this.emailLabel.Location = new System.Drawing.Point(33, 114);
            this.emailLabel.Name = "emailLabel";
            this.emailLabel.Size = new System.Drawing.Size(38, 13);
            this.emailLabel.TabIndex = 26;
            this.emailLabel.Text = "Email -";
            // 
            // appIdLabel
            // 
            this.appIdLabel.AutoSize = true;
            this.appIdLabel.Location = new System.Drawing.Point(25, 85);
            this.appIdLabel.Name = "appIdLabel";
            this.appIdLabel.Size = new System.Drawing.Size(46, 13);
            this.appIdLabel.TabIndex = 25;
            this.appIdLabel.Text = "App ID -";
            // 
            // emailTextbox
            // 
            this.emailTextbox.Location = new System.Drawing.Point(77, 111);
            this.emailTextbox.Multiline = true;
            this.emailTextbox.Name = "emailTextbox";
            this.emailTextbox.Size = new System.Drawing.Size(189, 23);
            this.emailTextbox.TabIndex = 24;
            this.emailTextbox.Text = "E-mail";
            this.emailTextbox.WordWrap = false;
            // 
            // passwordTextbox
            // 
            this.passwordTextbox.Location = new System.Drawing.Point(77, 140);
            this.passwordTextbox.Multiline = true;
            this.passwordTextbox.Name = "passwordTextbox";
            this.passwordTextbox.Size = new System.Drawing.Size(144, 23);
            this.passwordTextbox.TabIndex = 23;
            this.passwordTextbox.Text = "Password (Hashed)";
            this.passwordTextbox.WordWrap = false;
            // 
            // loginButton
            // 
            this.loginButton.Location = new System.Drawing.Point(77, 169);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(189, 23);
            this.loginButton.TabIndex = 22;
            this.loginButton.Text = "Login";
            this.loginButton.UseVisualStyleBackColor = true;
            this.loginButton.Click += new System.EventHandler(this.loginButton_Click);
            // 
            // appidTextbox
            // 
            this.appidTextbox.Location = new System.Drawing.Point(77, 82);
            this.appidTextbox.Multiline = true;
            this.appidTextbox.Name = "appidTextbox";
            this.appidTextbox.Size = new System.Drawing.Size(189, 23);
            this.appidTextbox.TabIndex = 21;
            this.appidTextbox.Text = "app_id Here";
            this.appidTextbox.WordWrap = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = global::QobuzDownloaderX.Properties.Resources.qbdlx;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(257, 64);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 28;
            this.pictureBox1.TabStop = false;
            // 
            // loginText
            // 
            this.loginText.Location = new System.Drawing.Point(12, 203);
            this.loginText.Name = "loginText";
            this.loginText.Size = new System.Drawing.Size(254, 23);
            this.loginText.TabIndex = 29;
            this.loginText.Text = "Waiting for login...";
            this.loginText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // md5Button
            // 
            this.md5Button.Location = new System.Drawing.Point(227, 140);
            this.md5Button.Name = "md5Button";
            this.md5Button.Size = new System.Drawing.Size(39, 23);
            this.md5Button.TabIndex = 30;
            this.md5Button.Text = "MD5";
            this.md5Button.UseVisualStyleBackColor = true;
            this.md5Button.Click += new System.EventHandler(this.md5Button_Click);
            // 
            // loginBG
            // 
            this.loginBG.DoWork += new System.ComponentModel.DoWorkEventHandler(this.loginBG_DoWork);
            // 
            // getSecretBG
            // 
            this.getSecretBG.DoWork += new System.ComponentModel.DoWorkEventHandler(this.getSecretBG_DoWork);
            // 
            // verNumLabel2
            // 
            this.verNumLabel2.BackColor = System.Drawing.Color.Transparent;
            this.verNumLabel2.Location = new System.Drawing.Point(181, 61);
            this.verNumLabel2.Name = "verNumLabel2";
            this.verNumLabel2.Size = new System.Drawing.Size(85, 18);
            this.verNumLabel2.TabIndex = 31;
            this.verNumLabel2.Text = "#.#.#.#";
            this.verNumLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(281, 235);
            this.Controls.Add(this.verNumLabel2);
            this.Controls.Add(this.md5Button);
            this.Controls.Add(this.loginText);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.passwordLabel);
            this.Controls.Add(this.emailLabel);
            this.Controls.Add(this.appIdLabel);
            this.Controls.Add(this.emailTextbox);
            this.Controls.Add(this.passwordTextbox);
            this.Controls.Add(this.loginButton);
            this.Controls.Add(this.appidTextbox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "LoginForm";
            this.Text = "QobuzDLX | Login";
            this.Load += new System.EventHandler(this.LoginForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label passwordLabel;
        private System.Windows.Forms.Label emailLabel;
        private System.Windows.Forms.Label appIdLabel;
        private System.Windows.Forms.TextBox emailTextbox;
        private System.Windows.Forms.TextBox passwordTextbox;
        private System.Windows.Forms.Button loginButton;
        private System.Windows.Forms.TextBox appidTextbox;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label loginText;
        private System.Windows.Forms.Button md5Button;
        private System.ComponentModel.BackgroundWorker loginBG;
        private System.ComponentModel.BackgroundWorker getSecretBG;
        private System.Windows.Forms.Label verNumLabel2;
    }
}