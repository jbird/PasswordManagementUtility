namespace PasswordManagementUtility.Forms {
    partial class OpenDatabaseForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if(disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.keyfileCheckBox = new System.Windows.Forms.CheckBox();
            this.masterPasswordGroupBox = new System.Windows.Forms.GroupBox();
            this.showPasswordButton = new System.Windows.Forms.Button();
            this.passwordLabel = new System.Windows.Forms.Label();
            this.primaryPasswordTextBox = new System.Windows.Forms.TextBox();
            this.keyfileGroupBox = new System.Windows.Forms.GroupBox();
            this.browseButton = new System.Windows.Forms.Button();
            this.keyfilePathTextBox = new System.Windows.Forms.TextBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.OKButton = new System.Windows.Forms.Button();
            this.openKeyfileDialog = new System.Windows.Forms.OpenFileDialog();
            this.masterPasswordGroupBox.SuspendLayout();
            this.keyfileGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // keyfileCheckBox
            // 
            this.keyfileCheckBox.AutoSize = true;
            this.keyfileCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.keyfileCheckBox.Location = new System.Drawing.Point(9, 28);
            this.keyfileCheckBox.Name = "keyfileCheckBox";
            this.keyfileCheckBox.Size = new System.Drawing.Size(68, 17);
            this.keyfileCheckBox.TabIndex = 0;
            this.keyfileCheckBox.Text = "Keyfile:";
            this.keyfileCheckBox.UseVisualStyleBackColor = true;
            this.keyfileCheckBox.CheckedChanged += new System.EventHandler(this.keyfileCheckBox_CheckedChanged);
            // 
            // masterPasswordGroupBox
            // 
            this.masterPasswordGroupBox.Controls.Add(this.showPasswordButton);
            this.masterPasswordGroupBox.Controls.Add(this.passwordLabel);
            this.masterPasswordGroupBox.Controls.Add(this.primaryPasswordTextBox);
            this.masterPasswordGroupBox.Location = new System.Drawing.Point(12, 12);
            this.masterPasswordGroupBox.Name = "masterPasswordGroupBox";
            this.masterPasswordGroupBox.Size = new System.Drawing.Size(394, 57);
            this.masterPasswordGroupBox.TabIndex = 1;
            this.masterPasswordGroupBox.TabStop = false;
            this.masterPasswordGroupBox.Text = "Master Password";
            // 
            // showPasswordButton
            // 
            this.showPasswordButton.Location = new System.Drawing.Point(325, 21);
            this.showPasswordButton.Name = "showPasswordButton";
            this.showPasswordButton.Size = new System.Drawing.Size(63, 23);
            this.showPasswordButton.TabIndex = 10;
            this.showPasswordButton.Text = "Show";
            this.showPasswordButton.UseVisualStyleBackColor = true;
            this.showPasswordButton.Click += new System.EventHandler(this.showPasswordButton_Click);
            // 
            // passwordLabel
            // 
            this.passwordLabel.AutoSize = true;
            this.passwordLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.passwordLabel.Location = new System.Drawing.Point(6, 26);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(65, 13);
            this.passwordLabel.TabIndex = 2;
            this.passwordLabel.Text = "Password:";
            // 
            // primaryPasswordTextBox
            // 
            this.primaryPasswordTextBox.Location = new System.Drawing.Point(145, 23);
            this.primaryPasswordTextBox.Name = "primaryPasswordTextBox";
            this.primaryPasswordTextBox.PasswordChar = '●';
            this.primaryPasswordTextBox.Size = new System.Drawing.Size(172, 20);
            this.primaryPasswordTextBox.TabIndex = 1;
            this.primaryPasswordTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.passwordTextBox_KeyPress);
            // 
            // keyfileGroupBox
            // 
            this.keyfileGroupBox.Controls.Add(this.browseButton);
            this.keyfileGroupBox.Controls.Add(this.keyfilePathTextBox);
            this.keyfileGroupBox.Controls.Add(this.keyfileCheckBox);
            this.keyfileGroupBox.Location = new System.Drawing.Point(11, 75);
            this.keyfileGroupBox.Name = "keyfileGroupBox";
            this.keyfileGroupBox.Size = new System.Drawing.Size(394, 63);
            this.keyfileGroupBox.TabIndex = 4;
            this.keyfileGroupBox.TabStop = false;
            this.keyfileGroupBox.Text = "Keyfile";
            // 
            // browseButton
            // 
            this.browseButton.Enabled = false;
            this.browseButton.Location = new System.Drawing.Point(313, 23);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(75, 23);
            this.browseButton.TabIndex = 5;
            this.browseButton.Text = "Browse";
            this.browseButton.UseVisualStyleBackColor = true;
            this.browseButton.Click += new System.EventHandler(this.browseButton_Click);
            // 
            // keyfilePathTextBox
            // 
            this.keyfilePathTextBox.Enabled = false;
            this.keyfilePathTextBox.Location = new System.Drawing.Point(145, 25);
            this.keyfilePathTextBox.Name = "keyfilePathTextBox";
            this.keyfilePathTextBox.Size = new System.Drawing.Size(162, 20);
            this.keyfilePathTextBox.TabIndex = 3;
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(325, 144);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(244, 144);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 4;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // openKeyfileDialog
            // 
            this.openKeyfileDialog.FileName = "openKeyfileDialog";
            this.openKeyfileDialog.Title = "Select Keyfile";
            // 
            // OpenDatabaseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(417, 181);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.keyfileGroupBox);
            this.Controls.Add(this.masterPasswordGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OpenDatabaseForm";
            this.ShowInTaskbar = false;
            this.Text = "Open Database";
            this.masterPasswordGroupBox.ResumeLayout(false);
            this.masterPasswordGroupBox.PerformLayout();
            this.keyfileGroupBox.ResumeLayout(false);
            this.keyfileGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox keyfileCheckBox;
        private System.Windows.Forms.GroupBox masterPasswordGroupBox;
        private System.Windows.Forms.Label passwordLabel;
        private System.Windows.Forms.TextBox primaryPasswordTextBox;
        private System.Windows.Forms.Button showPasswordButton;
        private System.Windows.Forms.GroupBox keyfileGroupBox;
        private System.Windows.Forms.TextBox keyfilePathTextBox;
        private System.Windows.Forms.Button browseButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.OpenFileDialog openKeyfileDialog;
    }
}