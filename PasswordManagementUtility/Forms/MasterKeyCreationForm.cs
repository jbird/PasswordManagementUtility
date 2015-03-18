/*
 * MasterKeyCreationForm.cs
 * Copyright (C) 2009-2010 John Bird <https://github.com/jbird>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Security;
using System.Threading;

using PMULib;
using PasswordManagementUtility.Forms;
using System.Drawing.Drawing2D;

namespace PasswordManagementUtility {
    public partial class MasterKeyCreationForm : Form {
        private SecureString password = null;
        private string save_path = null;
        private bool useKeyfile = false;

        public SecureString Password {
            private set { Password = value; }
            get {
                return Util.ToSecureString(PasswordTextBox.Text);
            }
        }

        public MasterKeyCreationForm() {
            InitializeComponent();
        }

        public MasterKeyCreationForm(string save_path) {
            InitializeComponent();
            // Used to set the initial save path directory for the keyfile generator.
            this.save_path = save_path;
        }

        private void MasterKeyCreationForm_Load(object sender, EventArgs e) {
            strengthPanel.Width = 1;
            PasswordTextBox.Focus();
        }

        /**
         * <summary>Gets the MasterKey created from the passwords and/or keyfile.</summary>
         * <returns>Returns the MasterKey containing the passwords and/or keyfile</returns>
         */
        public MasterKey GetMasterKey() {
            if(useKeyfile) {
                return new MasterKey(Password, new Keyfile(keyfilePathTextBox.Text));
            } else {
                return new MasterKey(Password);
            }
        }

        private bool UseKeyfile() { return useKeyfile; }

        #region Show Passwords
        private void showPasswordButton_Click(object sender, EventArgs e) {
            if(!PasswordTextBox.PasswordChar.Equals('\0')) {
                // Unmask password
                PasswordTextBox.PasswordChar = '\0';
                confirmPasswordTextBox.PasswordChar = '\0';
                showPasswordButton.Text = "Hide";
            } else {
                // Mask password
                PasswordTextBox.PasswordChar = '●';
                confirmPasswordTextBox.PasswordChar = '●';
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
                    }
                }
                useKeyfile = false;
            }
        }

        private void cancelButton_Click(object sender, EventArgs e) {
            Close();
        }

        private void createKeyFileButton_Click(object sender, EventArgs e) {
            try {
                Thread thread = new Thread(new ThreadStart(delegate {
                    saveKeyfileDialog.InitialDirectory = save_path;
                    if(saveKeyfileDialog.ShowDialog() == DialogResult.OK) {
                        using(KeyfileGeneratorForm keyfileGen = new KeyfileGeneratorForm()) {
                            keyfileGen.Focus();

                            if(keyfileGen.ShowDialog() == DialogResult.OK) {
                                if(Keyfile.CreateKeyfile(saveKeyfileDialog.FileName, keyfileGen.GetKey())) {
                                    keyfilePathTextBox.Invoke(new MethodInvoker(delegate { keyfilePathTextBox.Text = saveKeyfileDialog.FileName; }));
                                } else {
                                    MessageBox.Show("Unable to create keyfile", "Creating Keyfile Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                    }
                }));
                thread.SetApartmentState(System.Threading.ApartmentState.STA);
                thread.Start();
                
                if(!thread.IsAlive) {
                    thread.Abort();
                }
            } catch(Exception te) {
                MessageBox.Show(te.Message);
            }
        }

        private void browseKeyfileButton_Click(object sender, EventArgs e) {
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

        private void OKButton_Click(object sender, EventArgs e) {
            if(ValidateForm()) {
                this.password = Util.ToSecureString(PasswordTextBox.Text);
                this.DialogResult = DialogResult.OK;
            }
        }

        private bool ValidateForm() {
            string primary = PasswordTextBox.Text, secondary = confirmPasswordTextBox.Text;

            if(primary == "") {
                MessageBox.Show("Please fill in all the fields.", "Empty Field(s)", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            } else if(primary != secondary) {
                MessageBox.Show("Primary Password doesn't match the confirmation password.", "Password Mismatch", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            } else if(keyfileCheckBox.Checked && !File.Exists(keyfilePathTextBox.Text)) {
                MessageBox.Show("Keyfile does not exist.", "Invalid Keyfile", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            } else {
                return true;
            }
        }

        private void TestPasswordStrength() {
            int primary_score = 0;

            if(!(PasswordTextBox.Text.Length < 8)) {
                primary_score = PasswordStrength.TestPasswordStrength(PasswordTextBox.Text);
            }

            int total_score = primary_score;
            strengthPanel.Width = 265 * total_score / 100;

            Font font = this.Font;
            strengthLabel1.Text = total_score + "%";
            if(total_score < 34) {
                // Weak password
                strengthLabel2.ForeColor = Color.Red;
                strengthLabel2.Text = "Weak Passwords";
            } else if(total_score < 68) {
                // Good password
                strengthLabel2.ForeColor = Color.Red;
                strengthLabel2.Text = "Good Passwords";
            } else {
                // Strong password
                strengthLabel2.ForeColor = Color.Green;
                strengthLabel2.Text = "Strong Passwords";
            }
        }

        private void passwordTextBox_KeyUp(object sender, KeyEventArgs e) {
            TestPasswordStrength();
        }

        private void secondaryPasswordTextBox1_KeyUp(object sender, KeyEventArgs e) {
            TestPasswordStrength();
        }

        private void passwordTextBox_KeyPress(object sender, KeyPressEventArgs e) {
            if(e.KeyChar == 13) OKButton_Click(sender, e);
        }

        private void confirmPasswordTextBox_KeyPress(object sender, KeyPressEventArgs e) {
            if(e.KeyChar == 13) OKButton_Click(sender, e);
        }

        // Password Strength
        private void strengthPanel_Paint(object sender, PaintEventArgs e) {
            Rectangle rect = new Rectangle(0, 0, strengthContainerPanel.Width - 1, strengthContainerPanel.Height - 1);
            Brush gradient_brush = new LinearGradientBrush(rect, Color.Red, Color.Green, LinearGradientMode.Horizontal);

            e.Graphics.FillRectangle(gradient_brush, rect);
        }
    }
}
