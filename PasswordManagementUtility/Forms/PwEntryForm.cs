using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Security;

using PMULib;

namespace PasswordManagementUtility {
    public partial class PasswordEntryForm : Form {
        private PwEntry entry = null;

        public PasswordEntryForm() {
            InitializeComponent();
        }

        public PasswordEntryForm(PwEntry entry, string[] groups) {
            InitializeComponent();
            this.entry = entry;
            groupComboBox.Items.Add("Password Database");
            groupComboBox.Items.AddRange(groups);

            SetFormFields();
        }

        public PasswordEntryForm(string[] groups) {
            InitializeComponent();
            groupComboBox.Items.Add("Password Database");
            groupComboBox.Items.AddRange(groups);
        }

        public PasswordEntryForm(string[] groups, string[] fields) {
            InitializeComponent();
            groupComboBox.Items.Add("Password Database");
            groupComboBox.Items.AddRange(groups);

            int i = 0;
            foreach(TextBox textBox in tabControl1.Controls) {
                textBox.Text = fields[i];
            }
        }

        public PasswordEntryForm(string[] groups, string selectedGroup) {
            InitializeComponent();
            groupComboBox.Items.Add("Password Database");
            groupComboBox.Items.AddRange(groups);
            groupComboBox.SelectedItem = selectedGroup;
        }

        public PwEntry GetPwEntry() { return this.entry; }

        private void showPasswordButton_Click(object sender, EventArgs e) {
            if(!passwordTextBox1.PasswordChar.Equals('\0')) {
                // Unmask password
                passwordTextBox1.PasswordChar = '\0';
                passwordTextBox2.PasswordChar = '\0';
                showPasswordButton.Text = "Hide";
            } else {
                // Mask password
                passwordTextBox1.PasswordChar = '●';
                passwordTextBox2.PasswordChar = '●';
                showPasswordButton.Text = "Show";
            }
        }

        private void generatePasswordButton_Click(object sender, EventArgs e) {
            string pseudo_password = PRNG.GenerateRandomPassword(24);
            passwordTextBox1.Text = pseudo_password;
            passwordTextBox2.Text = pseudo_password;

            pseudo_password = null;
        }

        private void cancelButton_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void OKButton_Click(object sender, EventArgs e) {
            if(ValidateForm()) {
                this.entry = new PwEntry(Util.ToSecureString(passwordTextBox1.Text), titleTextBox.Text, groupComboBox.Text, 
                                         usernameTextBox.Text, UrlTextBox.Text, commentTextBox.Text);

                this.DialogResult = DialogResult.OK;
            }
        }

        private bool ValidateForm() {
            if(titleTextBox.Text == "" || usernameTextBox.Text == "") {
                MessageBox.Show("Please fill in a Title or Username the field.", "Required Fields", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            

            if(passwordTextBox1.Text != passwordTextBox2.Text) {
                MessageBox.Show("Both password fields must be the same.", "Inconsistant Passwords", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            } else if(groupComboBox.Text == "") {
                MessageBox.Show("Please select a password group.", "No Selected Password Group", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }else {
                return true;
            }
        }

        private void SetFormFields() {
            titleTextBox.Text = entry.Title;
            groupComboBox.SelectedItem = entry.Group;
            usernameTextBox.Text = entry.Username;
            passwordTextBox1.Text = Util.ToSystemString(entry.Password);
            passwordTextBox2.Text = Util.ToSystemString(entry.Password);
            UrlTextBox.Text = entry.URL;
            commentTextBox.Text = entry.Comment;
        }

        private void PasswordEntryForm_Load(object sender, EventArgs e) {

        }
    }
}
