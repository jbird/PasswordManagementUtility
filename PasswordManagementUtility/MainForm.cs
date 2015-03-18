/**
 * MainForm.cs
 * 
 * Copyright (C) 2009-2010 John Bird <https://github.com/jbird>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Security.Cryptography;

using PMULib;
using PasswordManagementUtility.Forms;
using System.Threading;

namespace PasswordManagementUtility {
    public partial class MainForm : Form {
        private static PwDatabase pwDatabase;
        private static bool databaseOpen = false;

        public MainForm() {
            InitializeComponent();
        }

        private void SetFormTitle(string filename) { this.Text = VersionInfo.GetApplicationTitle(filename); }
        private void SetDefaultFormTitle() { this.Text = VersionInfo.GetApplicationTitle(); }

        private void MainForm_Load(object sender, EventArgs e) {
            SetDefaultFormTitle();
            statusStrip.Items["toolStripProgressBar"].Visible = false;
            statusStrip.Items.Insert(2, new ToolStripSeparator());
            statusStrip.Items[2].Visible = false;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
            if(pwDatabase != null) {
                if(pwDatabase.Saved) {
                    dataGridView.DataSource = DataGridViewManager.DefaultDataGridView();
                    dataGridView.BackgroundColor = Control.DefaultBackColor;
                    treeView.Nodes.Clear();
                    pwDatabase.Close();
                    pwDatabase = null;

                    toolStripStatusLabel1.Text = "";
                    DisableMenuItems();
                    SetDefaultFormTitle();
                    databaseOpen = false;
                } else {
                    if(SaveChangesDialog() == DialogResult.Cancel) {
                        e.Cancel = true;
                    }
                }
            }
        }

        #region ToolStrip Menu Items

        #region File
        private void newToolStripMenuItem_Click(object sender, EventArgs e) {
            CreatePwDatabase();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenPwDatabase();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e) {
            SavePwDatabase();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e) {
            if(saveFileDialog.ShowDialog() == DialogResult.OK) {
                pwDatabase.Path = saveFileDialog.FileName;
                SavePwDatabase();

                SetFormTitle(System.IO.Path.GetFileNameWithoutExtension((saveFileDialog.FileName)));
            }
            saveFileDialog.Dispose();
        }

        private DialogResult SaveChangesDialog() {
            DialogResult message = MessageBox.Show("Save Changes Before Closing?", "Save Changes?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            if(message == DialogResult.Yes) {
                pwDatabase.Save();

                dataGridView.DataSource = DataGridViewManager.DefaultDataGridView();
                dataGridView.BackgroundColor = Control.DefaultBackColor;
                treeView.Nodes.Clear();
                pwDatabase.Close();

                toolStripStatusLabel1.Text = "";
                DisableMenuItems();
                SetDefaultFormTitle();
                databaseOpen = false;
            } else if(message == DialogResult.No) {
                dataGridView.DataSource = DataGridViewManager.DefaultDataGridView();
                dataGridView.BackgroundColor = Control.DefaultBackColor;
                treeView.Nodes.Clear();
                pwDatabase.Close();

                toolStripStatusLabel1.Text = "";
                DisableMenuItems();
                SetDefaultFormTitle();
                databaseOpen = false;
            }

            return message;
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e) {
            if(pwDatabase.Saved) {
                dataGridView.DataSource = DataGridViewManager.DefaultDataGridView();
                dataGridView.BackgroundColor = Control.DefaultBackColor;
                treeView.Nodes.Clear();
                pwDatabase.Close();
                pwDatabase = null;

                toolStripStatusLabel1.Text = "";
                DisableMenuItems();
                SetDefaultFormTitle();
                databaseOpen = false;
            } else {
                SaveChangesDialog();
            }
        }

        private void changeMasterKeyToolStripMenuItem_Click(object sender, EventArgs e) {
            using(MasterKeyCreationForm masterKeyForm = new MasterKeyCreationForm(pwDatabase.Path)) {
                masterKeyForm.Text = "Change Master Key";
                if(masterKeyForm.ShowDialog() == DialogResult.OK) {
                    pwDatabase.SetMasterKey(masterKeyForm.GetMasterKey());

                    SavePwDatabase();
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
            /*if(databaseOpen) {
                if(!pwDatabase.Saved) SaveChangesDialog();
                pwDatabase.Close();
            }*/

            Application.Exit();
        }
        #endregion

        #region Edit
        private void addEntryToolStripMenuItem_Click(object sender, EventArgs e) {
            AddPwEntry();
        }

        private void editViewEntryToolStripMenuItem_Click(object sender, EventArgs e) {
            EditViewPwEntry();
        }

        private void deleteEntryToolStripMenuItem_Click(object sender, EventArgs e) {
            DeletePwEntry();
        }

        private void addGroupToolStripMenuItem_Click(object sender, EventArgs e) {
            AddGroup();
        }

        private void editGroupToolStripMenuItem_Click(object sender, EventArgs e) {
            EditGroup();
        }

        private void deleteGroupToolStripMenuItem_Click(object sender, EventArgs e) {
            DeleteGroup();
        }

        private void copyUsernameToolStripMenuItem_Click(object sender, EventArgs e) {
            CopyUsername();
        }

        private void copyPasswordToolStripMenuItem_Click(object sender, EventArgs e) {
            CopyPassword();
        }

        private void clearClipboardToolStripMenuItem_Click(object sender, EventArgs e) {
            ClearClipboard();
        }

        #endregion 

        #region View
        private void showToolbarToolStripMenuItem_Click(object sender, EventArgs e) {
            if(showToolbarToolStripMenuItem.Checked) {
                showToolbarToolStripMenuItem.Checked = false;
                toolStrip.Hide();
            } else {
                showToolbarToolStripMenuItem.Checked = true;
                toolStrip.Show();
            }
        }

        private void expandAllToolStripMenuItem_Click(object sender, EventArgs e) {
            treeView.ExpandAll();
        }

        private void collapseAllToolStripMenuItem_Click(object sender, EventArgs e) {
            treeView.CollapseAll();
        }

        #region Show Columns

        private void showTitleToolStripMenuItem_Click(object sender, EventArgs e) {
            if(showTitleToolStripMenuItem.Checked) {
                showTitleToolStripMenuItem.Checked = false;
                dataGridView.Columns[0].Visible = false;
            } else {
                showTitleToolStripMenuItem.Checked = true;
                dataGridView.Columns[0].Visible = true;
            }
        }

        private void showUsernameToolStripMenuItem_Click(object sender, EventArgs e) {
            if(showUsernameToolStripMenuItem.Checked) {
                showUsernameToolStripMenuItem.Checked = false;
                dataGridView.Columns[1].Visible = false;
            } else {
                showUsernameToolStripMenuItem.Checked = true;
                dataGridView.Columns[1].Visible = true;
            }
        }

        private void showPasswordToolStripMenuItem_Click(object sender, EventArgs e) {
            if(showPasswordToolStripMenuItem.Checked) {
                showPasswordToolStripMenuItem.Checked = false;
                dataGridView.Columns[2].Visible = false;
            } else {
                showPasswordToolStripMenuItem.Checked = true;
                dataGridView.Columns[2].Visible = true;
            }
        }

        private void showURLToolStripMenuItem_Click(object sender, EventArgs e) {
            if(ShowURLToolStripMenuItem.Checked) {
                ShowURLToolStripMenuItem.Checked = false;
                dataGridView.Columns[3].Visible = false;
            } else {
                ShowURLToolStripMenuItem.Checked = true;
                dataGridView.Columns[3].Visible = true;
            }
        }

        private void showCommentToolStripMenuItem_Click(object sender, EventArgs e) {
            if(showCommentToolStripMenuItem.Checked) {
                showCommentToolStripMenuItem.Checked = false;
                dataGridView.Columns[4].Visible = false;
            } else {
                showCommentToolStripMenuItem.Checked = true;
                dataGridView.Columns[4].Visible = true;
            }
        }
        #endregion

        #endregion

        #region Tools

        private void generateKeyfileToolStripMenuItem_Click(object sender, EventArgs e) {
            try {
                Thread thread = new Thread(new ThreadStart(delegate {
                    using(SaveFileDialog saveKeyfileDialog = new SaveFileDialog()) {
                        saveKeyfileDialog.Title = "Create Keyfile";
                        saveKeyfileDialog.FileName = "Keyfile";
                        if(saveKeyfileDialog.ShowDialog() == DialogResult.OK) {
                            using(KeyfileGeneratorForm keyfileGen = new KeyfileGeneratorForm()) {
                                keyfileGen.Focus();

                                if(keyfileGen.ShowDialog() == DialogResult.OK) {
                                    if(!Keyfile.CreateKeyfile(saveKeyfileDialog.FileName, keyfileGen.GetKey())) {
                                        MessageBox.Show("Unable to create keyfile", "Creating Keyfile Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
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
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        #endregion 

        #region Help

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {
            using(AboutBox about = new AboutBox()) {
                about.ShowDialog();
            }
        }

        #endregion

        #endregion

        #region ToolStripBar

        private void newToolStripButton_Click(object sender, EventArgs e) {
            CreatePwDatabase();
        }

        private void openToolStripButton_Click(object sender, EventArgs e) {
            OpenPwDatabase();
        }

        private void saveToolStripButton_Click(object sender, EventArgs e) {
            SavePwDatabase();
        }

        private void addEntryToolStripButton_Click(object sender, EventArgs e) {
            AddPwEntry();
        }

        private void editViewEntryToolStripButton_Click(object sender, EventArgs e) {
            EditViewPwEntry();
        }

        private void deleteEntryToolStripButton_Click(object sender, EventArgs e) {
            DeletePwEntry();
        }
        
        private void addGroupToolStripButton_Click(object sender, EventArgs e) {
            AddGroup();
        }

        private void editGroupToolStripButton_Click(object sender, EventArgs e) {
            EditGroup();
        }

        private void deleteGroupToolStripButton_Click(object sender, EventArgs e) {
            DeleteGroup();
        }

        private void copyUsernameToolStripButton_Click(object sender, EventArgs e) {
            CopyUsername();
        }

        private void copyPasswordToolStripButton_Click(object sender, EventArgs e) {
            CopyPassword();
        }

        private void clearClipboardtoolStripButton_Click(object sender, EventArgs e) {
            ClearClipboard();
        }

        #endregion

        #region PwDatabase Functionality

        private void CreatePwDatabase() {
            while(true) {
                if(saveFileDialog.ShowDialog() == DialogResult.OK) {
                    if(PermissionsManager.WritePermission(saveFileDialog.FileName)) {
                        backgroundWorker1.RunWorkerAsync("Create");
                        break;
                    } else {
                        MessageBox.Show("You do not have write permission for the given directory.", "Write Permission", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        continue;
                    }
                } else {
                    break;
                }
            }
        }

        private void OpenPwDatabase() {
            while(true) {
                if(openFileDialog.ShowDialog() == DialogResult.OK) {
                    if(PermissionsManager.ReadPermission(openFileDialog.FileName)) {
                        backgroundWorker1.RunWorkerAsync("Open");
                        break;
                    } else {
                        MessageBox.Show("You do not have read permission for the given file.", "Read Permission", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        continue;
                    }
                } else {
                    break;
                }
            }
        }

        private void SavePwDatabase() {
            if(PermissionsManager.WritePermission(pwDatabase.Path)) {
                backgroundWorker1.RunWorkerAsync("Save");
                pwDatabase.Saved = true;
            } else {
                MessageBox.Show("Unable to save database, no write permission.", "Read Permission", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private PwEntry GetSelectedPwEntry() {
            return pwDatabase.GetPwEntry(treeView.SelectedNode.Text, dataGridView.CurrentRow.Index + 1);
        }

        private void AddPwEntry() {
            string selectedGroup = treeView.SelectedNode.Text;
            using(PasswordEntryForm entryForm = new PasswordEntryForm(pwDatabase.GetPwGroupsArray(), selectedGroup)) {
                if(entryForm.ShowDialog() == DialogResult.OK) {
                    if(pwDatabase.AddPwEntry(entryForm.GetPwEntry())) {
                        if(treeView.SelectedNode.Text != treeView.Nodes[0].Text) {
                            dataGridView.Columns.Clear();
                            dataGridView.DataSource = pwDatabase.GetPwDataTable(treeView.SelectedNode.Text);
                        } else {
                            dataGridView.Columns.Clear();
                            dataGridView.DataSource = pwDatabase.GetRootDataTable();
                        }
                        treeView.ExpandAll();
                        pwDatabase.Saved = false;
                    } else {
                        MessageBox.Show("Unable to add password", "Unable to add password", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void EditViewPwEntry() {
            PwEntry entry = GetSelectedPwEntry();
            using(PasswordEntryForm entryForm = new PasswordEntryForm(entry, pwDatabase.GetPwGroupsArray())) {
                if(entryForm.ShowDialog() == DialogResult.OK) {
                    if(pwDatabase.EditPwEntry(entryForm.GetPwEntry(), dataGridView.CurrentRow.Index + 1)) {
                        if(treeView.SelectedNode.Text != treeView.Nodes[0].Text) {
                            dataGridView.Columns.Clear();
                            dataGridView.DataSource = pwDatabase.GetPwDataTable(treeView.SelectedNode.Text);
                            
                        } else {
                            dataGridView.Columns.Clear();
                            dataGridView.DataSource = pwDatabase.GetRootDataTable();
                        }
                        treeView.ExpandAll();
                        pwDatabase.Saved = false;
                    } else {
                        MessageBox.Show("Password Management Utility: Unable to edit password", "Unable to edit password", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void DeletePwEntry() {
            int index = dataGridView.CurrentRow.Index + 1;
            string group = treeView.SelectedNode.Text;
            if(MessageBox.Show("Delete this password?", "Delete Password", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                if(pwDatabase.RemovePwEntry(group, index)) {
                    if(treeView.SelectedNode.Text != treeView.Nodes[0].Text) {
                        dataGridView.Columns.Clear();
                        dataGridView.DataSource = pwDatabase.GetPwDataTable(treeView.SelectedNode.Text);
                    } else {
                        dataGridView.Columns.Clear();
                        dataGridView.DataSource = pwDatabase.GetRootDataTable();
                    }
                    pwDatabase.Saved = false;
                } else {
                    MessageBox.Show("Password Management Utility: Unable to remove password", "Unable to remove password", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void AddGroup() {
            treeView.Nodes[0].Nodes.Add("New Folder");
            treeView.LabelEdit = true;
            pwDatabase.AddPwGroup("New Folder");
            treeView.SelectedNode = treeView.Nodes[0].LastNode;
            treeView.SelectedNode.BeginEdit();
        }

        private void EditGroup() {
            treeView.SelectedNode.BeginEdit();
        }

        private void DeleteGroup() {
            string group = treeView.SelectedNode.Text;
            if(MessageBox.Show("Delete " + group + " Group?", "Delete Group?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                pwDatabase.RemovePwGroup(group);
                treeView.Nodes[0].Nodes.RemoveAt(treeView.SelectedNode.Index);
                pwDatabase.Saved = false;
            }
        }

        #endregion

        #region Enable and Disable Menu and ToolStrip Items
        private void EnableMenuItems() {
            /*foreach(ToolStripItem item in toolStrip.Items) {
                if(!item.Enabled) item.Enabled = true;
            }*/

            foreach(ToolStripItem item in fileToolStripMenuItem.DropDownItems) {
                if(!item.Enabled) item.Enabled = true;
            }

            saveToolStripButton.Enabled = true;
            saveToolStripMenuItem.Enabled = true;
            saveAsToolStripMenuItem.Enabled = true;
            closeToolStripMenuItem.Enabled = true;

            //changeMasterKeyToolStripMenuItem.Enabled = true;

            addEntryToolStripButton.Enabled = true;
            editViewEntryToolStripButton.Enabled = true;
            deleteEntryToolStripButton.Enabled = true;
            addGroupToolStripButton.Enabled = true;
            copyUsernameToolStripButton.Enabled = true;
            copyPasswordToolStripButton.Enabled = true;
            URLToolStripMenuItem.Enabled = true;

            addEntryToolStripMenuItem.Enabled = true;
            editViewEntryToolStripMenuItem.Enabled = true;
            deleteEntryToolStripMenuItem.Enabled = true;
            addGroupToolStripMenuItem.Enabled = true;

            showColumnsToolStripMenuItem.Enabled = true;
        }

        private void DisableMenuItems() {
            saveToolStripButton.Enabled = false;
            saveToolStripMenuItem.Enabled = false;
            saveAsToolStripMenuItem.Enabled = false;
            closeToolStripMenuItem.Enabled = false;

            changeMasterKeyToolStripMenuItem.Enabled = false;

            addEntryToolStripButton.Enabled = false;
            editViewEntryToolStripButton.Enabled = false;
            deleteEntryToolStripButton.Enabled = false;
            addGroupToolStripButton.Enabled = false;
            editGroupToolStripButton.Enabled = false;
            deleteGroupToolStripButton.Enabled = false;
            copyUsernameToolStripButton.Enabled = false;
            copyPasswordToolStripButton.Enabled = false;
            URLToolStripMenuItem.Enabled = false;

            addEntryToolStripMenuItem.Enabled = false;
            editViewEntryToolStripMenuItem.Enabled = false;
            deleteEntryToolStripMenuItem.Enabled = false;
            addGroupToolStripMenuItem.Enabled = false;
            editGroupToolStripMenuItem.Enabled = false;
            deleteGroupToolStripMenuItem.Enabled = false;
            copyUsernameToolStripMenuItem.Enabled = false;
            copyPasswordToolStripMenuItem.Enabled = false;

            showColumnsToolStripMenuItem.Enabled = false;
        }

        public void EnableRowSelectMenuItems() {
            editViewEntryToolStripMenuItem.Enabled = true;
            editViewEntryToolStripButton.Enabled = true;
            deleteEntryToolStripMenuItem.Enabled = true;
            deleteEntryToolStripButton.Enabled = true;
            copyUsernameToolStripMenuItem.Enabled = true;
            copyUsernameToolStripButton.Enabled = true;
            copyPasswordToolStripMenuItem.Enabled = true;
            copyPasswordToolStripButton.Enabled = true;

            contextCopyUsername.Enabled = true;
            contextCopyPassword.Enabled = true;
            contextEditViewEntry.Enabled = true;
            contextDeleteEntry.Enabled = true;
            contextURLToolStripMenuItem.Enabled = true;
        }

        public void DisableRowSelectMenuItems() {
            editViewEntryToolStripMenuItem.Enabled = false;
            editViewEntryToolStripButton.Enabled = false;
            deleteEntryToolStripMenuItem.Enabled = false;
            deleteEntryToolStripButton.Enabled = false;
            copyUsernameToolStripMenuItem.Enabled = false;
            copyUsernameToolStripButton.Enabled = false;
            copyPasswordToolStripMenuItem.Enabled = false;
            copyPasswordToolStripButton.Enabled = false;

            contextCopyUsername.Enabled = false;
            contextCopyPassword.Enabled = false;
            contextEditViewEntry.Enabled = false;
            contextDeleteEntry.Enabled = false;
            contextURLToolStripMenuItem.Enabled = false;
        }
        #endregion

        #region DatagridView Events
        private void dataGridView_CellLeave(object sender, DataGridViewCellEventArgs e) {
            DisableRowSelectMenuItems();
        }

        private void dataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e) {
            DataGridViewManager.DataBindingCompleteStyle(dataGridView);
        }

        private void dataGridView_RowLeave(object sender, DataGridViewCellEventArgs e) {
            dataGridView.ClearSelection();
            DisableRowSelectMenuItems();
        }

        private void dataGridView_RowEnter(object sender, DataGridViewCellEventArgs e) {
            EnableRowSelectMenuItems();
        }

        private void dataGridView_MouseClick(object sender, MouseEventArgs e) {
            DataGridView.HitTestInfo hitTest = dataGridView.HitTest(e.X, e.Y);
            if(hitTest.Type != DataGridViewHitTestType.Cell) {
                if(dataGridView.SelectedRows.Count > 0) {
                    dataGridView.ClearSelection();

                    DisableRowSelectMenuItems();
                }
            } else {
                EnableRowSelectMenuItems();
            }

        }

        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e) {

        }

        private void dataGridView_MouseUp(object sender, MouseEventArgs e) {
            DataGridView.HitTestInfo hitTestInfo = dataGridView.HitTest(e.X, e.Y);

            if(hitTestInfo.Type == DataGridViewHitTestType.Cell) {
                if(!dataGridView.Rows[hitTestInfo.RowIndex].Selected) {
                    dataGridView.ClearSelection();
                    dataGridView.Rows[hitTestInfo.RowIndex].Selected = true;
                    EnableRowSelectMenuItems();
                }


                // NOTE: Change so the current selected row is selected on right click.
                if(GetSelectedPwEntry().URL != "") {
                    contextURLToolStripMenuItem.Enabled = true;
                } else {
                    contextURLToolStripMenuItem.Enabled = false;
                }
            } else {
                if(dataGridView.SelectedCells.Count > 0) {
                    dataGridView.ClearSelection();
                    DisableRowSelectMenuItems();
                }
            }
            
        }

        #endregion

        #region TreeView Events

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e) {
            if(treeView.SelectedNode.Text != treeView.Nodes[0].Text) {
                if(!editGroupToolStripMenuItem.Enabled) {
                    editGroupToolStripButton.Enabled = true;
                    deleteGroupToolStripButton.Enabled = true;
                    editGroupToolStripMenuItem.Enabled = true;
                    deleteGroupToolStripMenuItem.Enabled = true;
                }
                dataGridView.Columns.Clear();
                dataGridView.DataSource = pwDatabase.GetPwDataTable(treeView.SelectedNode.Text);
                
            } else {
                editGroupToolStripButton.Enabled = false;
                deleteGroupToolStripButton.Enabled = false;
                editGroupToolStripMenuItem.Enabled = false;
                deleteGroupToolStripMenuItem.Enabled = false;
                
                dataGridView.Columns.Clear();
                dataGridView.DataSource = pwDatabase.GetRootDataTable();
                
            }
            treeView.ExpandAll();
        }

        private void treeView1_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e) {
            // Disable editting the root (Password Database) Node
            if(treeView.SelectedNode.Text == treeView.Nodes[0].Text) {
                e.CancelEdit = true;
            }
        }

        private void treeView1_AfterLabelEdit(object sender, NodeLabelEditEventArgs e) {
            string new_group = e.Label;
            string group = e.Node.Text;

            if(new_group == null) new_group = "New Folder";
            pwDatabase.EditPwGroup(group, new_group);
            e.Node.EndEdit(false);
            pwDatabase.Saved = false;
        }

        private void treeView1_DragEnter(object sender, DragEventArgs e) {
            e.Effect = DragDropEffects.Move;
        }

        private void treeView1_DragDrop(object sender, DragEventArgs e) {
            if(e.Data.GetDataPresent("System.Windows.Forms.TreeNode", false)) {
                Point pt = ((TreeView)sender).PointToClient(new Point(e.X, e.Y));
                TreeNode DestinationNode = ((TreeView)sender).GetNodeAt(pt);
                TreeNode NewNode = (TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode");

                if(DestinationNode != NewNode) {
                    // Update PwDatabase Groups

                    if(pwDatabase.MovePwGroup(NewNode.Text, DestinationNode.Index + 1)) {
                        string tmp = DestinationNode.Text;
                        treeView.Nodes[0].Nodes[DestinationNode.Index].Text = NewNode.Text;
                        treeView.Nodes[0].Nodes[NewNode.Index].Text = tmp;
                        treeView.SelectedNode = NewNode;

                        pwDatabase.Saved = false;
                    }
                }
            }
        }

        private void treeView1_ItemDrag(object sender, ItemDragEventArgs e) {
            DoDragDrop(e.Item, DragDropEffects.Move);
        }

        #endregion

        #region Copy Username, Password and URL

        private void CopyUsername() {
            Util.CopyUsernameToClipboard(GetSelectedPwEntry());

            toolStripStatusLabel1.Text = "Username will be cleared from the clipboard in 30 seconds...";
            if(toolStripProgressBar.Style != ProgressBarStyle.Blocks) {
                toolStripProgressBar.Style = ProgressBarStyle.Blocks;
            }
            toolStripProgressBar.Value = 300;
            toolStripProgressBar.Visible = true;
            timer.Start();

            if(!clearClipboardtoolStripButton.Enabled) {
                clearClipboardtoolStripButton.Enabled = true;
                clearClipboardToolStripMenuItem.Enabled = true;
            }
        }

        private void CopyPassword() {
            Util.CopyPasswordToClipboard(GetSelectedPwEntry());

            toolStripStatusLabel1.Text = "Password will be cleared from the clipboard in 30 seconds...";
            if(toolStripProgressBar.Style != ProgressBarStyle.Blocks) {
                toolStripProgressBar.Style = ProgressBarStyle.Blocks;
            }
            toolStripProgressBar.Value = 300;
            toolStripProgressBar.Visible = true;
            timer.Start();

            if(!clearClipboardtoolStripButton.Enabled) {
                clearClipboardtoolStripButton.Enabled = true;
                clearClipboardToolStripMenuItem.Enabled = true;
            }
        }

        private void OpenURLInBrowser() {
            try {
                string url = GetSelectedPwEntry().URL;
                if(url != "") {
                    System.Diagnostics.Process.Start(url);
                }
            } catch(System.ComponentModel.Win32Exception noBrowser) {
                if(noBrowser.ErrorCode == -2147467259) {
                    MessageBox.Show(noBrowser.Message);
                }
            } catch(Exception other) {
                MessageBox.Show(other.Message);
            }
        }

        private void CopyURL() {
            int index = dataGridView.SelectedRows[0].Index + 1;
            string group = treeView.SelectedNode.Text;

            Util.CopyURLToClipboard(pwDatabase.GetPwEntry(group, index));

            if(!clearClipboardtoolStripButton.Enabled) {
                clearClipboardtoolStripButton.Enabled = true;
                clearClipboardToolStripMenuItem.Enabled = true;
            }
            
        }

        private void timer_Tick(object sender, EventArgs e) {
            toolStripProgressBar.Value--;

            if(toolStripProgressBar.Value == 0) {
                timer.Stop();
                Util.ClearClipboard();
                ProgressComplete();
            } else if(toolStripProgressBar.Value % 10 == 0 && toolStripProgressBar.Value != 100) {
                toolStripStatusLabel1.Text = String.Format("Password will be cleared from the clipboard in {0} seconds...", (toolStripProgressBar.Value / 10));
            } 
        }

        private void ClearClipboard() {
            if(timer.Enabled) timer.Stop();

            toolStripProgressBar.Value = 1;
            clearClipboardToolStripMenuItem.Enabled = false;
            clearClipboardtoolStripButton.Enabled = false;
            ProgressComplete();
        }

        #endregion 

        #region Progress Bar
        private void LoadProgress(string m) {
            if(m == "Open") {
                toolStripStatusLabel1.Text = "Opening Database...";
            } else if(m == "Create") {
                toolStripStatusLabel1.Text = "Creating Database...";
            } else if(m == "Save") {
                toolStripStatusLabel1.Text = "Saving Database...";
            } else if(m == "Cancel") {
                Invoke(new MethodInvoker(delegate {
                    toolStripProgressBar.Visible = false;
                    toolStripProgressBar.Value = 0;

                    toolStripStatusLabel1.Text = "";
                }));
                return;
            }

            Invoke(new MethodInvoker(delegate {
                if(toolStripProgressBar.Style != ProgressBarStyle.Marquee) {
                    toolStripProgressBar.Style = ProgressBarStyle.Marquee;
                }
                toolStripProgressBar.Value = 50;
                toolStripProgressBar.Visible = true;
            }));
        }

        private void ProgressComplete() {
            toolStripProgressBar.Visible = false;
            toolStripProgressBar.Value = 0;
            toolStripStatusLabel1.Text = "Ready";
        }

        #endregion

        #region BackgroundWorker Events

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e) {
            if(e.Argument.ToString() == "Create") {
                using(MasterKeyCreationForm masterKeyForm = new MasterKeyCreationForm(saveFileDialog.FileName)) {
                    if(masterKeyForm.ShowDialog() == DialogResult.OK) {
                        try {
                            LoadProgress("Create");
                            
                            PwDatabase.CreateDatabase(masterKeyForm.GetMasterKey(), saveFileDialog.FileName);
                            pwDatabase = new PwDatabase(masterKeyForm.GetMasterKey(), saveFileDialog.FileName);
                            if(!pwDatabase.Open()) {
                                MessageBox.Show("Unable to open database", "Error Opening Database", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            e.Result = "true";

                        } catch(Exception create_e) {
                            MessageBox.Show(create_e.Message, "Error Creating Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            e.Result = "false";
                        }
                    } else {
                        e.Result = "false"; // Catch's Thread state exception
                        toolStripStatusLabel1.Text = "";
                    }
                }
            } else if(e.Argument.ToString() == "Open") {
                using(OpenDatabaseForm openDatabaseForm = new OpenDatabaseForm(openFileDialog.FileName)) {
                    while(true) {
                        if(openDatabaseForm.ShowDialog() == DialogResult.OK) {
                            try {
                                LoadProgress("Open");
                                pwDatabase = new PwDatabase(openDatabaseForm.GetMasterKey(), openFileDialog.FileName);

                                if(!pwDatabase.Open()) {
                                    if(!openDatabaseForm.KeyfileChecked()) {
                                        MessageBox.Show("Wrong password(s).", "Wrong Password(s)", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    } else {
                                        MessageBox.Show("Wrong password(s) and/or keyfile.", "Wrong Password/Keyfile", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    }
                                    LoadProgress("Cancel");
                                    continue;
                                }

                                e.Result = "true";
                                break;
                            } catch(Exception oe) {
                                // Invalid password provided
                                MessageBox.Show(oe.Message, "Error opening password database.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                continue;
                            }

                        } else {
                            e.Result = "false";
                            break;
                        }
                    }
                }
            } else if(e.Argument.ToString() == "Save") {
                LoadProgress("Save");
                try {
                    pwDatabase.Save();
                } catch(Exception se) {
                    MessageBox.Show(se.Message, "Error Saving");
                }
                e.Result = "save";
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            if(e.Result.ToString() == "true") {
                ProgressComplete();

                if(openFileDialog.FileName.Length != 0) {
                    SetFormTitle(System.IO.Path.GetFileNameWithoutExtension((openFileDialog.FileName)));
                    openFileDialog.Dispose();
                } else {
                    SetFormTitle(System.IO.Path.GetFileNameWithoutExtension((saveFileDialog.FileName)));
                    saveFileDialog.Dispose();
                }
                
                treeView.Nodes.Add(pwDatabase.GetPwGroups());
                treeView.ExpandAll();
                treeView.SelectedNode = treeView.Nodes[0];

                EnableMenuItems();
                databaseOpen = true;

                dataGridView.ContextMenuStrip = contextMenuStrip;
            } else if(e.Result.ToString() == "save") {
                ProgressComplete();
            } else {
                ProgressComplete();
                toolStripStatusLabel1.Text = "";
                dataGridView.ContextMenuStrip = null;
            }
        }
        #endregion

        #region Context Menu

        private void contextCopyUsername_Click(object sender, EventArgs e) {
            CopyUsername();
        }

        private void contextCopyPassword_Click(object sender, EventArgs e) {
            CopyPassword();
        }

        private void contextCopyURLToolStripMenuItem_Click(object sender, EventArgs e) {
            CopyURL();
        }

        private void contextAddEntry_Click(object sender, EventArgs e) {
            AddPwEntry();
        }

        private void contextEditViewEntry_Click(object sender, EventArgs e) {
            EditViewPwEntry();
        }

        private void contextDeleteEntry_Click(object sender, EventArgs e) {
            DeletePwEntry();
        }

        private void contextOpenInBrowserToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenURLInBrowser();
        }

        #endregion

        private void dataGridView_MouseDown(object sender, MouseEventArgs e) {
            
        }

        private void dataGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e) {

        }
    }
}
