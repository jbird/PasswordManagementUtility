/**
 * PwDatabase.cs
 * 
 * Password Database class for instansiating a password database
 * and providing the functionality to open, authenticate, save
 * and return password data.

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
using System.Security;
using System.IO;
using System.Windows.Forms;
using System.Data;
using System.Security.Cryptography;
[assembly:CLSCompliant(true)]

namespace PMULib {
    /**
     * <summary>Represents a password database for opening, creating, saving and manipulating content.</summary>
     */
    public sealed class PwDatabase {
        public static readonly string VERSION = "0.1";
        
        private MasterKey masterKey;
        private Keyfile keyfile;
        private string path;
        private bool saved = true;
        
        private XMLDocument xmlDoc;

        /**
         * <summary>Gets whether the password database has been saved.</summary>
         */
        public bool Saved {
            get { return saved; }
            set { saved = value; }
        }

        /**
         * <summary>Gets or sets the password database save location.</summary>
         */
        public string Path {
            get { return path; }
            set { path = value; }
        }

        /**
         * <summary>Initializes a new instance of PwDatabase with a given masterkey password and the location of the database.</summary>
         * <param name="password">The master password that will be used for encryption.</param>
         * <param name="path">The file location for the given password database.</param>
         */
        public PwDatabase(MasterKey masterKey, string path) {
            this.path = path;
            this.masterKey = masterKey;
        }

        /**
         * <summary>Initializes a new instance of PwDatabase with a given masterkey password, keyfile and the location of the database.</summary>
         * <param name="password">The master password that will be used for encryption.</param>
         * <param name="keyfile">The file location for the given keyfile.</param>
         * <param name="path">The file location for the given password database.</param>
         */
        public PwDatabase(MasterKey masterKey, Keyfile keyfile, string path) {
            this.path = path;
            this.keyfile = keyfile;
            this.masterKey = masterKey;
        }

        ~PwDatabase() {
            Close();
        }

        /**
         * <summary>Creates a new password database, encrypted with the given password.</summary>
         * <param name="password">The password given to encrypt the password database with.</param>
         * <param name="path">The file location for where the database will be saved.</param>
         */
        public static void CreateDatabase(MasterKey masterKey, string path) {
            try {
                //MasterKey masterKey = new MasterKey(password);
                XMLDocument xmlDoc = new XMLDocument();
                XMLWriter writer = new XMLWriter(masterKey, path);
                writer.Save(xmlDoc);
                writer.Close();
            } catch(Exception e) { throw new Exception(e.Message); }
        }

        /**
         * <summary>Creates a new password database, encrypted with the given password.</summary>
         * <param name="password">The password given to encrypt the password database with.</param>
         * <param name="path">The file location for where the database will be saved.</param>
         * <param name="username">The username to be given for the database.</param>
         * <param name="description">The description to be given for the database.</param>
         */
        public static void CreateDatabase(SecureString password, string path, string username, string description) {
            try {
                MasterKey masterKey = new MasterKey(password);
                XMLDocument xmlDoc = new XMLDocument(username, description);
                XMLWriter writer = new XMLWriter(masterKey, path);
                writer.Save(xmlDoc);
                writer.Close();
            } catch(Exception e) { throw new Exception(e.Message); }
        }

        /**
         * <summary>Opens a password database.</summary>
         * <returns>Returns true if the password database was opened successfully, else returns false.</returns>
         */
        public bool Open() {
            try {
                xmlDoc = GetXMLDocument(masterKey, path);
                return true;
            } catch(CryptographicException) {
                return false;
            } catch(Exception) {
                return false;
            }
        }

        /**
         * <summary>Sets the MasterKey.</summary>
         * <param name="masterKey">The new MasterKey to encrypt the database with.</param>
         */
        public void SetMasterKey(MasterKey masterKey) {
            this.masterKey = masterKey;
        }

        /**
         * <summary>Gets a XML Document using the given password and database file location.</summary>
         * <param name="masterKey">The password given to decrypt the database.</param>
         * <param name="path">The file location for the database.</param>
         * <returns>Returns a XMLDocument.</returns>
         */
        private static XMLDocument GetXMLDocument(MasterKey masterKey, string path) {
            XMLReader reader = new XMLReader(masterKey, path);
            return reader.GetXMLDocument();
        }

        /**
         * <summary>Gets a list of passwords in the root database.</summary>
         * <returns>Returns a DataTable containing the passwords in the root database.</returns>
         */
        public DataTable GetRootDataTable() {
            return DataGridViewManager.GetRootDataTable(xmlDoc.GetXmlDocument());
        }

        /**
         * <summary>Gets a list of passwords in the given password group.</summary>
         * <param name="group">The group were the list of passwords will be retrieved.</param>
         * <returns>Returns a DataTable containing all the passwords in the given group.</returns>
         */
        public DataTable GetPwDataTable(string group) {
            return DataGridViewManager.GetPwDataTable(xmlDoc.GetXmlDocument(), group);
        }

        /**
         * <summary>Adds a new password entry.</summary>
         * <param name="entry">The PwEntry to be added to the password database.</param>
         * <returns>Returns true if the new password was added to the database successfully, else returns false.</returns>
         */
        public bool AddPwEntry(PwEntry entry) {
            try {
                xmlDoc.AddPwEntry(entry);
                return true;
            } catch(Exception) { return false; }
        }

        /**
         * <summary>Edit a password entry.</summary>
         * <param name="entry">The PwEntry to be edit.</param>
         * <param name="index">The row index of the password.</param>
         * <returns>Returns true if the password is edited successfully, else returns false.</returns>
         */
        public bool EditPwEntry(PwEntry entry, int index) {
            try {
                xmlDoc.EditPwEntry(entry, index);
                return true;
            } catch(Exception) { return false; }
        }

        /**
         * <summary>Removes a password entry.</summary>
         * <param name="entry">The PwEntry to be removed from the password database in the given group and index.</param>
         */
        public bool RemovePwEntry(string group, int index) {
            try {
                xmlDoc.RemovePwEntry(group, index);
                return true;
            } catch(Exception) { return false; }
        }

        /**
         * <summary>Gets a password entry in a specific group and row index.</summary>
         * <param name="group">The group were the password is stored.</param>
         * <param name="index">The row index of the password.</param>
         * <returns>Returns a PwEntry of a password.</returns>
         */
        public PwEntry GetPwEntry(string group, int index) {
            return xmlDoc.GetPwEntry(group, index);
        }

        

        /**
         * <summary>Adds a new password group to the password database.</summary>
         * <param name="group">The name of the new group to add.</param>
         */
        public bool AddPwGroup(string group) {
            try {
                xmlDoc.AddPwGroup(group);
                return true;
            } catch(Exception) { return false; }
        }

        /**
         * <summary>Edits a password group and gives it a new name.</summary>
         * <param name="group">The name of the groups name to change.</param>
         * <param name="new_group">The new name to give to the group.</param>
         */
        public bool EditPwGroup(string group, string new_group) {
            try {
                xmlDoc.EditPwGroup(group, new_group);
                return true;
            } catch(Exception) { return false; }
        }

        /**
         * <summary>Removes a password group.</summary>
         * <param name="group">The name of the group to remove.</param>
         * <returns>Returns true if the password group is removed successfully, else returns false.</returns>
         */
        public void RemovePwGroup(string group) {
            xmlDoc.RemovePwGroup(group);
        }

        /**
         * <summary>Gets all the password groups in the database.</summary>
         * <returns>Returns a TreeNode containing all the different password groups.</returns>
         */
        public TreeNode GetPwGroups() {
            return xmlDoc.GetPwGroups();
        }

        /**
         * <summary>Gets all the password groups in the database.</summary>
         * <returns>Returns a string array containing all the different password groups.</returns>
         */
        public string[] GetPwGroupsArray() {
            return xmlDoc.GetPwGroupsArray();
        }

        /**
         * <summary>Move a password group.</summary>
         * <param name="group_name">The name of the group to move.</param>
         * <param name="index">The index where to move the group.</param>
         */
        public bool MovePwGroup(string group_name, int index) {
            try {
                xmlDoc.MovePwGroup(group_name, index);
                return true;
            } catch(Exception) { return false; }
        }

        /**
         * <summary>Saves the current state of the password database.</summary>
         */
        public void Save() {
            XMLWriter writer = new XMLWriter(masterKey, path);
            writer.Save(xmlDoc);
            writer.Close();
        }

        /**
         * <summary>Releases all resources used by PMULib.PwDatabase.</summary>
         */
        public void Close() {
            if(masterKey != null) masterKey.Clear();
            masterKey = null;
            if(keyfile != null) keyfile.Clear();
            keyfile = null;
            path = null;
            xmlDoc = null;
        }
    }
}
