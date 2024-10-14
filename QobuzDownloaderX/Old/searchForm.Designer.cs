namespace QobuzDownloaderX
{
    partial class searchForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(searchForm));
            this.searchInput = new System.Windows.Forms.TextBox();
            this.searchButton = new System.Windows.Forms.Button();
            this.resultTextbox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // searchInput
            // 
            this.searchInput.Location = new System.Drawing.Point(12, 14);
            this.searchInput.Name = "searchInput";
            this.searchInput.Size = new System.Drawing.Size(612, 20);
            this.searchInput.TabIndex = 0;
            this.searchInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.searchInput_KeyDown);
            // 
            // searchButton
            // 
            this.searchButton.Location = new System.Drawing.Point(630, 12);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(98, 23);
            this.searchButton.TabIndex = 1;
            this.searchButton.Text = "Search";
            this.searchButton.UseVisualStyleBackColor = true;
            this.searchButton.Click += new System.EventHandler(this.searchButton_Click);
            // 
            // resultTextbox
            // 
            this.resultTextbox.Location = new System.Drawing.Point(12, 40);
            this.resultTextbox.Multiline = true;
            this.resultTextbox.Name = "resultTextbox";
            this.resultTextbox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.resultTextbox.Size = new System.Drawing.Size(716, 513);
            this.resultTextbox.TabIndex = 2;
            this.resultTextbox.WordWrap = false;
            // 
            // searchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(740, 565);
            this.Controls.Add(this.resultTextbox);
            this.Controls.Add(this.searchButton);
            this.Controls.Add(this.searchInput);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "searchForm";
            this.Text = "QobuzDLX | Search";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.searchForm_FormClosed);
            this.Load += new System.EventHandler(this.searchForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox searchInput;
        private System.Windows.Forms.Button searchButton;
        private System.Windows.Forms.TextBox resultTextbox;
    }
}