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
using System.Threading;
using System.IO;

namespace PasswordManagementUtility.Forms {
    public partial class OpenDatabaseForm : Form {
        private SecureString password;
        private string save_path;
        private bool useKeyfile = false;

        public OpenDatabaseForm() {
            InitializeComponent();
            password = new SecureString();
        }

        public OpenDatabaseForm(string save_path) {
            InitializeComponent();
            this.save_path = save_path;
            password = new SecureString();
        }

        /**
         * <summary>Returns whether the Keyfile checkbox is checked.</summary>
         * <returns>Returns true if the checkbox is checked, else returns false.</returns>
         */
        public bool KeyfileChecked() {
            return keyfileCheckBox.Checked;
        }

        public MasterKey GetMasterKey() {
            if(useKeyfile) {
                return new MasterKey(password, new Keyfile(keyfilePathTextBox.Text));
            } else {
                return new MasterKey(password);
            }
        }

        #region Show Passwords
        private void showPasswordButton_Click(object sender, EventArgs e) {
            if(!primaryPasswordTextBox.PasswordChar.Equals('\0')) {
                // Unmask password
                primaryPasswordTextBox.PasswordChar = '\0';
                primaryPasswordTextBox.PasswordChar = '\0';
                showPasswordButton.Text = "Hide";
            } else {
                // Mask password
                primaryPasswordTextBox.PasswordChar = '●';
                primaryPasswordTextBox.PasswordChar = '●';
                showPasswordButton.Text = "Show";
            }
        }
        #endregion

        private void keyfileCheckBox_CheckedChanged(object sender, EventArgs e) {
            if(keyfileCheckBox.Checked) {
                // Enable all controls (except checkbox)
                foreach(Control control in keyfileGroupBox.Controls) {
                    if(!control.Enabled) control.Enabled = true;
                }
                useKeyfile = true;
            } else {
                // Disable all controls (except checkbox)
                foreach(Control control in keyfileGroupBox.Controls) {
                    if(!(control.GetType() == keyfileCheckBox.GetType())) {
                        if(control.Enabled) control.Enabled = false;
                    } else {
                        continue;
                    }
                }
                useKeyfile = false;
            }
        }

        private void cancelButton_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void OKButton_Click(object sender, EventArgs e) {
            if(ValidateForm()) {
                password = Util.ToSecureString(primaryPasswordTextBox.Text);
                DialogResult = DialogResult.OK;
            }
        }

        private void browseButton_Click(object sender, EventArgs e) {
            try {
                Thread thread = new Thread(new ThreadStart(delegate {
                    openKeyfileDialog.InitialDirectory = save_path;
                    if(openKeyfileDialog.ShowDialog() == DialogResult.OK) {
                        keyfilePathTextBox.Invoke(new MethodInvoker(delegate { keyfilePathTextBox.Text = openKeyfileDialog.FileName; }));
                    }
                }));
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();

                if(!thread.IsAlive) {
                    thread.Abort();
                }
            } catch(Exception te) {
                MessageBox.Show(te.Message);
            }
        }

        private bool ValidateForm() {
            if(primaryPasswordTextBox.Text == "") {
                MessageBox.Show("Please fill in all the fields.", "Empty Field(s)", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            } else if(primaryPasswordTextBox.Text.Length < 8) {
                MessageBox.Show("Password(s) must be at least 8 characters long.", "Invalid Password Length", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            } else if(KeyfileChecked() && !File.Exists(keyfilePathTextBox.Text)) {
                MessageBox.Show("Keyfile does not exist.", "Invalid Keyfile", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            } else {
                return true;
            }
        }

        private void passwordTextBox_KeyPress(object sender, KeyPressEventArgs e) {
            // Enter key
            if(e.KeyChar == 13) {
                OKButton_Click(sender, e);
            }
        }
    }
}
