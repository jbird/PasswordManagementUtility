namespace PasswordManagementUtility {
    partial class MasterKeyCreationForm {
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
            this.passwordlabel = new System.Windows.Forms.Label();
            this.confirmPasswordlabel = new System.Windows.Forms.Label();
            this.confirmPasswordTextBox = new System.Windows.Forms.TextBox();
            this.PasswordTextBox = new System.Windows.Forms.TextBox();
            this.showPasswordButton = new System.Windows.Forms.Button();
            this.masterPasswordGroupBox = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.strengthLabel1 = new System.Windows.Forms.Label();
            this.strengthContainerPanel = new System.Windows.Forms.Panel();
            this.strengthPanel = new System.Windows.Forms.Panel();
            this.strengthLabel2 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.OKButton = new System.Windows.Forms.Button();
            this.keyfileGroupBox = new System.Windows.Forms.GroupBox();
            this.createKeyFileButton = new System.Windows.Forms.Button();
            this.browseKeyfileButton = new System.Windows.Forms.Button();
            this.keyfilePathTextBox = new System.Windows.Forms.TextBox();
            this.keyfileLabel = new System.Windows.Forms.Label();
            this.keyfileCheckBox = new System.Windows.Forms.CheckBox();
            this.openKeyfileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveKeyfileDialog = new System.Windows.Forms.SaveFileDialog();
            this.masterPasswordGroupBox.SuspendLayout();
            this.strengthContainerPanel.SuspendLayout();
            this.keyfileGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // passwordlabel
            // 
            this.passwordlabel.AutoSize = true;
            this.passwordlabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.passwordlabel.Location = new System.Drawing.Point(19, 22);
            this.passwordlabel.Name = "passwordlabel";
            this.passwordlabel.Size = new System.Drawing.Size(65, 13);
            this.passwordlabel.TabIndex = 0;
            this.passwordlabel.Text = "Password:";
            // 
            // confirmPasswordlabel
            // 
            this.confirmPasswordlabel.AutoSize = true;
            this.confirmPasswordlabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.confirmPasswordlabel.Location = new System.Drawing.Point(19, 48);
            this.confirmPasswordlabel.Name = "confirmPasswordlabel";
            this.confirmPasswordlabel.Size = new System.Drawing.Size(53, 13);
            this.confirmPasswordlabel.TabIndex = 3;
            this.confirmPasswordlabel.Text = "Confirm:";
            // 
            // confirmPasswordTextBox
            // 
            this.confirmPasswordTextBox.Location = new System.Drawing.Point(152, 45);
            this.confirmPasswordTextBox.Name = "confirmPasswordTextBox";
            this.confirmPasswordTextBox.PasswordChar = '●';
            this.confirmPasswordTextBox.Size = new System.Drawing.Size(265, 20);
            this.confirmPasswordTextBox.TabIndex = 2;
            this.confirmPasswordTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.confirmPasswordTextBox_KeyPress);
            // 
            // PasswordTextBox
            // 
            this.PasswordTextBox.Location = new System.Drawing.Point(152, 19);
            this.PasswordTextBox.Name = "PasswordTextBox";
            this.PasswordTextBox.PasswordChar = '●';
            this.PasswordTextBox.Size = new System.Drawing.Size(265, 20);
            this.PasswordTextBox.TabIndex = 1;
            this.PasswordTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.passwordTextBox_KeyPress);
            this.PasswordTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.passwordTextBox_KeyUp);
            // 
            // showPasswordButton
            // 
            this.showPasswordButton.AutoSize = true;
            this.showPasswordButton.Location = new System.Drawing.Point(423, 17);
            this.showPasswordButton.Name = "showPasswordButton";
            this.showPasswordButton.Size = new System.Drawing.Size(63, 23);
            this.showPasswordButton.TabIndex = 10;
            this.showPasswordButton.Text = "Show";
            this.showPasswordButton.UseVisualStyleBackColor = true;
            this.showPasswordButton.Click += new System.EventHandler(this.showPasswordButton_Click);
            // 
            // masterPasswordGroupBox
            // 
            this.masterPasswordGroupBox.Controls.Add(this.label1);
            this.masterPasswordGroupBox.Controls.Add(this.strengthLabel1);
            this.masterPasswordGroupBox.Controls.Add(this.strengthContainerPanel);
            this.masterPasswordGroupBox.Controls.Add(this.strengthLabel2);
            this.masterPasswordGroupBox.Controls.Add(this.passwordlabel);
            this.masterPasswordGroupBox.Controls.Add(this.confirmPasswordlabel);
            this.masterPasswordGroupBox.Controls.Add(this.confirmPasswordTextBox);
            this.masterPasswordGroupBox.Controls.Add(this.showPasswordButton);
            this.masterPasswordGroupBox.Controls.Add(this.PasswordTextBox);
            this.masterPasswordGroupBox.Location = new System.Drawing.Point(12, 83);
            this.masterPasswordGroupBox.Name = "masterPasswordGroupBox";
            this.masterPasswordGroupBox.Size = new System.Drawing.Size(500, 120);
            this.masterPasswordGroupBox.TabIndex = 10;
            this.masterPasswordGroupBox.TabStop = false;
            this.masterPasswordGroupBox.Text = "Master Key Password";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(19, 74);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "Total Key Strength:";
            // 
            // strengthLabel1
            // 
            this.strengthLabel1.Location = new System.Drawing.Point(229, 93);
            this.strengthLabel1.Name = "strengthLabel1";
            this.strengthLabel1.Size = new System.Drawing.Size(36, 13);
            this.strengthLabel1.TabIndex = 0;
            this.strengthLabel1.Text = "0%";
            this.strengthLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // strengthContainerPanel
            // 
            this.strengthContainerPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.strengthContainerPanel.Controls.Add(this.strengthPanel);
            this.strengthContainerPanel.Location = new System.Drawing.Point(154, 72);
            this.strengthContainerPanel.Name = "strengthContainerPanel";
            this.strengthContainerPanel.Size = new System.Drawing.Size(263, 15);
            this.strengthContainerPanel.TabIndex = 10;
            // 
            // strengthPanel
            // 
            this.strengthPanel.BackColor = System.Drawing.Color.Red;
            this.strengthPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.strengthPanel.Location = new System.Drawing.Point(-1, -1);
            this.strengthPanel.Name = "strengthPanel";
            this.strengthPanel.Size = new System.Drawing.Size(265, 15);
            this.strengthPanel.TabIndex = 11;
            this.strengthPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.strengthPanel_Paint);
            // 
            // strengthLabel2
            // 
            this.strengthLabel2.Location = new System.Drawing.Point(262, 93);
            this.strengthLabel2.Name = "strengthLabel2";
            this.strengthLabel2.Size = new System.Drawing.Size(146, 13);
            this.strengthLabel2.TabIndex = 9;
            this.strengthLabel2.Text = "Not Rated";
            this.strengthLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBox4
            // 
            this.textBox4.BackColor = System.Drawing.SystemColors.Control;
            this.textBox4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox4.Location = new System.Drawing.Point(12, 12);
            this.textBox4.Multiline = true;
            this.textBox4.Name = "textBox4";
            this.textBox4.ReadOnly = true;
            this.textBox4.Size = new System.Drawing.Size(500, 31);
            this.textBox4.TabIndex = 15;
            this.textBox4.Text = "Specify a Master Key Password and a (optional) keyfile for addtional authenticati" +
                "on. The details provided here will be used everytime to access your password dat" +
                "abase. So don\'t lose them!";
            // 
            // textBox5
            // 
            this.textBox5.BackColor = System.Drawing.SystemColors.Control;
            this.textBox5.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox5.Location = new System.Drawing.Point(12, 49);
            this.textBox5.Multiline = true;
            this.textBox5.Name = "textBox5";
            this.textBox5.ReadOnly = true;
            this.textBox5.Size = new System.Drawing.Size(500, 28);
            this.textBox5.TabIndex = 16;
            this.textBox5.Text = "The Master Key Password should consist of a minimum of 8 characters and contain a" +
                "t mixture of upper and lower case characters (a-z, A-Z), numbers (0-9) and ASCII" +
                " characters (#, £, _, -) etc.";
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(423, 262);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 7;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(342, 262);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 6;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // keyfileGroupBox
            // 
            this.keyfileGroupBox.Controls.Add(this.createKeyFileButton);
            this.keyfileGroupBox.Controls.Add(this.browseKeyfileButton);
            this.keyfileGroupBox.Controls.Add(this.keyfilePathTextBox);
            this.keyfileGroupBox.Controls.Add(this.keyfileLabel);
            this.keyfileGroupBox.Controls.Add(this.keyfileCheckBox);
            this.keyfileGroupBox.Location = new System.Drawing.Point(12, 209);
            this.keyfileGroupBox.Name = "keyfileGroupBox";
            this.keyfileGroupBox.Size = new System.Drawing.Size(500, 47);
            this.keyfileGroupBox.TabIndex = 11;
            this.keyfileGroupBox.TabStop = false;
            this.keyfileGroupBox.Text = "Keyfile";
            // 
            // createKeyFileButton
            // 
            this.createKeyFileButton.Enabled = false;
            this.createKeyFileButton.Location = new System.Drawing.Point(330, 14);
            this.createKeyFileButton.Name = "createKeyFileButton";
            this.createKeyFileButton.Size = new System.Drawing.Size(75, 23);
            this.createKeyFileButton.TabIndex = 10;
            this.createKeyFileButton.Text = "Create";
            this.createKeyFileButton.UseVisualStyleBackColor = true;
            this.createKeyFileButton.Click += new System.EventHandler(this.createKeyFileButton_Click);
            // 
            // browseKeyfileButton
            // 
            this.browseKeyfileButton.Enabled = false;
            this.browseKeyfileButton.Location = new System.Drawing.Point(411, 14);
            this.browseKeyfileButton.Name = "browseKeyfileButton";
            this.browseKeyfileButton.Size = new System.Drawing.Size(75, 23);
            this.browseKeyfileButton.TabIndex = 10;
            this.browseKeyfileButton.Text = "Browse";
            this.browseKeyfileButton.UseVisualStyleBackColor = true;
            this.browseKeyfileButton.Click += new System.EventHandler(this.browseKeyfileButton_Click);
            // 
            // keyfilePathTextBox
            // 
            this.keyfilePathTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.keyfilePathTextBox.Enabled = false;
            this.keyfilePathTextBox.Location = new System.Drawing.Point(142, 16);
            this.keyfilePathTextBox.Name = "keyfilePathTextBox";
            this.keyfilePathTextBox.Size = new System.Drawing.Size(182, 20);
            this.keyfilePathTextBox.TabIndex = 5;
            // 
            // keyfileLabel
            // 
            this.keyfileLabel.AutoSize = true;
            this.keyfileLabel.Enabled = false;
            this.keyfileLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.keyfileLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.keyfileLabel.Location = new System.Drawing.Point(49, 20);
            this.keyfileLabel.Name = "keyfileLabel";
            this.keyfileLabel.Size = new System.Drawing.Size(49, 13);
            this.keyfileLabel.TabIndex = 10;
            this.keyfileLabel.Text = "Keyfile:";
            // 
            // keyfileCheckBox
            // 
            this.keyfileCheckBox.AutoSize = true;
            this.keyfileCheckBox.Location = new System.Drawing.Point(22, 19);
            this.keyfileCheckBox.Name = "keyfileCheckBox";
            this.keyfileCheckBox.Size = new System.Drawing.Size(15, 14);
            this.keyfileCheckBox.TabIndex = 9;
            this.keyfileCheckBox.UseVisualStyleBackColor = true;
            this.keyfileCheckBox.CheckedChanged += new System.EventHandler(this.keyfileCheckBox_CheckedChanged);
            // 
            // openKeyfileDialog
            // 
            this.openKeyfileDialog.Title = "Select Keyfile";
            // 
            // saveKeyfileDialog
            // 
            this.saveKeyfileDialog.FileName = "keyfile";
            this.saveKeyfileDialog.Title = "Create Keyfile";
            // 
            // MasterKeyCreationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(524, 301);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.keyfileGroupBox);
            this.Controls.Add(this.masterPasswordGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MasterKeyCreationForm";
            this.Text = "Create Master Key";
            this.Load += new System.EventHandler(this.MasterKeyCreationForm_Load);
            this.masterPasswordGroupBox.ResumeLayout(false);
            this.masterPasswordGroupBox.PerformLayout();
            this.strengthContainerPanel.ResumeLayout(false);
            this.keyfileGroupBox.ResumeLayout(false);
            this.keyfileGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label passwordlabel;
        private System.Windows.Forms.Label confirmPasswordlabel;
        private System.Windows.Forms.TextBox confirmPasswordTextBox;
        private System.Windows.Forms.TextBox PasswordTextBox;
        private System.Windows.Forms.Button showPasswordButton;
        private System.Windows.Forms.GroupBox masterPasswordGroupBox;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.GroupBox keyfileGroupBox;
        private System.Windows.Forms.Button createKeyFileButton;
        private System.Windows.Forms.Button browseKeyfileButton;
        private System.Windows.Forms.TextBox keyfilePathTextBox;
        private System.Windows.Forms.Label keyfileLabel;
        private System.Windows.Forms.CheckBox keyfileCheckBox;
        private System.Windows.Forms.OpenFileDialog openKeyfileDialog;
        private System.Windows.Forms.SaveFileDialog saveKeyfileDialog;
        private System.Windows.Forms.Label strengthLabel2;
        private System.Windows.Forms.Panel strengthContainerPanel;
        private System.Windows.Forms.Panel strengthPanel;
        private System.Windows.Forms.Label strengthLabel1;
        private System.Windows.Forms.Label label1;
    }
}