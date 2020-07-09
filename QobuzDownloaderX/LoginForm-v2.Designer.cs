namespace QobuzDownloaderX
{
    partial class LoginFrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginFrm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.disableLogin = new System.Windows.Forms.CheckBox();
            this.verNumLabel2 = new System.Windows.Forms.Label();
            this.md5Button = new System.Windows.Forms.Button();
            this.exitLabel = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.userAuthTokenTextbox = new System.Windows.Forms.TextBox();
            this.loginButton = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.emailTextbox = new System.Windows.Forms.TextBox();
            this.passwordTextbox = new System.Windows.Forms.TextBox();
            this.loginText = new System.Windows.Forms.Label();
            this.getSecretBG = new System.ComponentModel.BackgroundWorker();
            this.loginBG = new System.ComponentModel.BackgroundWorker();
            this.visableCheckbox = new System.Windows.Forms.CheckBox();
            this.altLoginLabel = new System.Windows.Forms.Label();
            this.altLoginTutLabel = new System.Windows.Forms.Label();
            this.userIdTextbox = new System.Windows.Forms.TextBox();
            this.altLoginBG = new System.ComponentModel.BackgroundWorker();
            this.aboutLabel = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = global::QobuzDownloaderX.Properties.Resources.login_frame;
            this.panel1.Controls.Add(this.aboutLabel);
            this.panel1.Controls.Add(this.disableLogin);
            this.panel1.Controls.Add(this.verNumLabel2);
            this.panel1.Controls.Add(this.md5Button);
            this.panel1.Controls.Add(this.exitLabel);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(282, 175);
            this.panel1.TabIndex = 0;
            this.panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseMove);
            // 
            // disableLogin
            // 
            this.disableLogin.AutoSize = true;
            this.disableLogin.ForeColor = System.Drawing.Color.Transparent;
            this.disableLogin.Location = new System.Drawing.Point(12, 155);
            this.disableLogin.Name = "disableLogin";
            this.disableLogin.Size = new System.Drawing.Size(90, 17);
            this.disableLogin.TabIndex = 34;
            this.disableLogin.Text = "Disable Login";
            this.disableLogin.UseVisualStyleBackColor = true;
            this.disableLogin.Visible = false;
            // 
            // verNumLabel2
            // 
            this.verNumLabel2.BackColor = System.Drawing.Color.Transparent;
            this.verNumLabel2.Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.verNumLabel2.ForeColor = System.Drawing.Color.White;
            this.verNumLabel2.Location = new System.Drawing.Point(194, 157);
            this.verNumLabel2.Name = "verNumLabel2";
            this.verNumLabel2.Size = new System.Drawing.Size(85, 18);
            this.verNumLabel2.TabIndex = 32;
            this.verNumLabel2.Text = "#.#.#.#";
            this.verNumLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.verNumLabel2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.verNumLabel2_MouseMove);
            // 
            // md5Button
            // 
            this.md5Button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(112)))), ((int)(((byte)(239)))));
            this.md5Button.Enabled = false;
            this.md5Button.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.md5Button.FlatAppearance.BorderSize = 2;
            this.md5Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.md5Button.ForeColor = System.Drawing.Color.White;
            this.md5Button.Location = new System.Drawing.Point(227, 122);
            this.md5Button.Name = "md5Button";
            this.md5Button.Size = new System.Drawing.Size(43, 27);
            this.md5Button.TabIndex = 9;
            this.md5Button.Text = "MD5";
            this.md5Button.UseVisualStyleBackColor = false;
            this.md5Button.Visible = false;
            this.md5Button.Click += new System.EventHandler(this.md5Button_Click);
            // 
            // exitLabel
            // 
            this.exitLabel.AutoSize = true;
            this.exitLabel.BackColor = System.Drawing.Color.Transparent;
            this.exitLabel.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exitLabel.ForeColor = System.Drawing.Color.Black;
            this.exitLabel.Location = new System.Drawing.Point(262, 0);
            this.exitLabel.Name = "exitLabel";
            this.exitLabel.Size = new System.Drawing.Size(20, 23);
            this.exitLabel.TabIndex = 9;
            this.exitLabel.Text = "X";
            this.exitLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.exitLabel.Click += new System.EventHandler(this.exitLabel_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = global::QobuzDownloaderX.Properties.Resources.qbdlx_white;
            this.pictureBox1.Location = new System.Drawing.Point(12, 52);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(258, 64);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 29;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            // 
            // userAuthTokenTextbox
            // 
            this.userAuthTokenTextbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.userAuthTokenTextbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.userAuthTokenTextbox.Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.userAuthTokenTextbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(92)))), ((int)(((byte)(102)))));
            this.userAuthTokenTextbox.Location = new System.Drawing.Point(12, 255);
            this.userAuthTokenTextbox.Multiline = true;
            this.userAuthTokenTextbox.Name = "userAuthTokenTextbox";
            this.userAuthTokenTextbox.Size = new System.Drawing.Size(237, 23);
            this.userAuthTokenTextbox.TabIndex = 35;
            this.userAuthTokenTextbox.Text = "user_auth_token";
            this.userAuthTokenTextbox.Visible = false;
            this.userAuthTokenTextbox.Click += new System.EventHandler(this.userAuthTokenTextbox_Click);
            this.userAuthTokenTextbox.Leave += new System.EventHandler(this.userAuthTokenTextbox_Leave);
            // 
            // loginButton
            // 
            this.loginButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(112)))), ((int)(((byte)(239)))));
            this.loginButton.FlatAppearance.BorderSize = 0;
            this.loginButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.loginButton.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loginButton.ForeColor = System.Drawing.Color.White;
            this.loginButton.Location = new System.Drawing.Point(12, 293);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(258, 30);
            this.loginButton.TabIndex = 2;
            this.loginButton.Text = "LOGIN";
            this.loginButton.UseVisualStyleBackColor = false;
            this.loginButton.Click += new System.EventHandler(this.loginButton_Click);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(92)))), ((int)(((byte)(102)))));
            this.panel3.Location = new System.Drawing.Point(12, 221);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(258, 1);
            this.panel3.TabIndex = 2;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(92)))), ((int)(((byte)(102)))));
            this.panel4.Location = new System.Drawing.Point(12, 277);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(258, 1);
            this.panel4.TabIndex = 2;
            // 
            // emailTextbox
            // 
            this.emailTextbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.emailTextbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.emailTextbox.Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.emailTextbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(92)))), ((int)(((byte)(102)))));
            this.emailTextbox.Location = new System.Drawing.Point(12, 199);
            this.emailTextbox.Multiline = true;
            this.emailTextbox.Name = "emailTextbox";
            this.emailTextbox.Size = new System.Drawing.Size(258, 23);
            this.emailTextbox.TabIndex = 7;
            this.emailTextbox.Text = "Email";
            this.emailTextbox.Click += new System.EventHandler(this.emailTextbox_Click);
            this.emailTextbox.Leave += new System.EventHandler(this.emailTextbox_Leave);
            // 
            // passwordTextbox
            // 
            this.passwordTextbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.passwordTextbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.passwordTextbox.Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.passwordTextbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(92)))), ((int)(((byte)(102)))));
            this.passwordTextbox.Location = new System.Drawing.Point(12, 255);
            this.passwordTextbox.Multiline = true;
            this.passwordTextbox.Name = "passwordTextbox";
            this.passwordTextbox.Size = new System.Drawing.Size(237, 23);
            this.passwordTextbox.TabIndex = 8;
            this.passwordTextbox.Text = "Password";
            this.passwordTextbox.Click += new System.EventHandler(this.passwordTextbox_Click);
            this.passwordTextbox.Leave += new System.EventHandler(this.passwordTextbox_Leave);
            // 
            // loginText
            // 
            this.loginText.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loginText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(92)))), ((int)(((byte)(102)))));
            this.loginText.Location = new System.Drawing.Point(12, 349);
            this.loginText.Name = "loginText";
            this.loginText.Size = new System.Drawing.Size(258, 23);
            this.loginText.TabIndex = 30;
            this.loginText.Text = "Waiting for login...";
            this.loginText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // getSecretBG
            // 
            this.getSecretBG.DoWork += new System.ComponentModel.DoWorkEventHandler(this.getSecretBG_DoWork);
            // 
            // loginBG
            // 
            this.loginBG.DoWork += new System.ComponentModel.DoWorkEventHandler(this.loginBG_DoWork);
            // 
            // visableCheckbox
            // 
            this.visableCheckbox.AutoSize = true;
            this.visableCheckbox.Location = new System.Drawing.Point(255, 256);
            this.visableCheckbox.Name = "visableCheckbox";
            this.visableCheckbox.Size = new System.Drawing.Size(15, 14);
            this.visableCheckbox.TabIndex = 31;
            this.visableCheckbox.UseVisualStyleBackColor = true;
            this.visableCheckbox.CheckedChanged += new System.EventHandler(this.visableCheckbox_CheckedChanged);
            // 
            // altLoginLabel
            // 
            this.altLoginLabel.Font = new System.Drawing.Font("Trebuchet MS", 8.25F);
            this.altLoginLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(92)))), ((int)(((byte)(102)))));
            this.altLoginLabel.Location = new System.Drawing.Point(12, 326);
            this.altLoginLabel.Name = "altLoginLabel";
            this.altLoginLabel.Size = new System.Drawing.Size(258, 20);
            this.altLoginLabel.TabIndex = 32;
            this.altLoginLabel.Text = "Can\'t login? Click here";
            this.altLoginLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.altLoginLabel.Click += new System.EventHandler(this.altLoginLabel_Click);
            // 
            // altLoginTutLabel
            // 
            this.altLoginTutLabel.AutoSize = true;
            this.altLoginTutLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(92)))), ((int)(((byte)(102)))));
            this.altLoginTutLabel.Location = new System.Drawing.Point(77, 180);
            this.altLoginTutLabel.Name = "altLoginTutLabel";
            this.altLoginTutLabel.Size = new System.Drawing.Size(128, 13);
            this.altLoginTutLabel.TabIndex = 33;
            this.altLoginTutLabel.Text = "Click Here for Instructions";
            this.altLoginTutLabel.Visible = false;
            this.altLoginTutLabel.Click += new System.EventHandler(this.altLoginTutLabel_Click);
            // 
            // userIdTextbox
            // 
            this.userIdTextbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.userIdTextbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.userIdTextbox.Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.userIdTextbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(92)))), ((int)(((byte)(102)))));
            this.userIdTextbox.Location = new System.Drawing.Point(12, 199);
            this.userIdTextbox.Multiline = true;
            this.userIdTextbox.Name = "userIdTextbox";
            this.userIdTextbox.Size = new System.Drawing.Size(258, 23);
            this.userIdTextbox.TabIndex = 9;
            this.userIdTextbox.Text = "user_id";
            this.userIdTextbox.Visible = false;
            this.userIdTextbox.Click += new System.EventHandler(this.userIdTextbox_Click);
            this.userIdTextbox.Leave += new System.EventHandler(this.userIdTextbox_Leave);
            // 
            // altLoginBG
            // 
            this.altLoginBG.DoWork += new System.ComponentModel.DoWorkEventHandler(this.altLoginBG_DoWork);
            // 
            // aboutLabel
            // 
            this.aboutLabel.AutoSize = true;
            this.aboutLabel.BackColor = System.Drawing.Color.Transparent;
            this.aboutLabel.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.aboutLabel.ForeColor = System.Drawing.Color.Black;
            this.aboutLabel.Location = new System.Drawing.Point(241, 0);
            this.aboutLabel.Name = "aboutLabel";
            this.aboutLabel.Size = new System.Drawing.Size(15, 23);
            this.aboutLabel.TabIndex = 35;
            this.aboutLabel.Text = "i";
            this.aboutLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.aboutLabel.Click += new System.EventHandler(this.aboutLabel_Click);
            // 
            // LoginFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.ClientSize = new System.Drawing.Size(282, 392);
            this.Controls.Add(this.altLoginTutLabel);
            this.Controls.Add(this.altLoginLabel);
            this.Controls.Add(this.visableCheckbox);
            this.Controls.Add(this.loginText);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.passwordTextbox);
            this.Controls.Add(this.emailTextbox);
            this.Controls.Add(this.loginButton);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.userIdTextbox);
            this.Controls.Add(this.userAuthTokenTextbox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LoginFrm";
            this.Text = "QobuzDLX | Login";
            this.Load += new System.EventHandler(this.LoginFrm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button loginButton;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.TextBox emailTextbox;
        private System.Windows.Forms.TextBox passwordTextbox;
        private System.Windows.Forms.Label exitLabel;
        private System.Windows.Forms.Button md5Button;
        private System.Windows.Forms.Label loginText;
        private System.ComponentModel.BackgroundWorker getSecretBG;
        private System.ComponentModel.BackgroundWorker loginBG;
        private System.Windows.Forms.Label verNumLabel2;
        private System.Windows.Forms.CheckBox visableCheckbox;
        private System.Windows.Forms.CheckBox disableLogin;
        private System.Windows.Forms.TextBox userAuthTokenTextbox;
        private System.Windows.Forms.Label altLoginLabel;
        private System.Windows.Forms.Label altLoginTutLabel;
        private System.Windows.Forms.TextBox userIdTextbox;
        private System.ComponentModel.BackgroundWorker altLoginBG;
        private System.Windows.Forms.Label aboutLabel;
    }
}