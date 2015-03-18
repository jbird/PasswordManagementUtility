/**
 * XMLDocument.cs
 * 
 * XML Document class for storing and manipulating the contents of
 * the XML data. Addtional functionality fot setting and getting elements
 * as well as transparently encrypting password elements in the document.
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
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Security;
using System.Security.Cryptography.Xml;
using System.Data;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace PMULib {
    /**
     * <summary>Represents a XML Document class for adding, updaing and removing elements in a XML Document.</summary>
     */
    internal sealed class XMLDocument {
        private XmlDocument xmlDoc;
        private readonly string baseXml = "<?xml version=\"1.0\"?><passwordDatabase version=\"" + PwDatabase.VERSION + "\"><meta><username /><description /></meta><passwordGroup name=\"Password Database\"><root><entry><title>Sample Entry</title><username>John Smith</username><password>" + PRNG.GenerateRandomPassword(24) + "</password><url>www.example.com</url><comment>A example password entry.</comment></entry></root><group name=\"General\"></group><group name=\"Email\"></group><group name=\"Internet\"></group></passwordGroup></passwordDatabase>";
        private readonly string[] tag_names = new string[] { "title", "username", "password", "url", "comment" };

        /**
         * <summary>Initializes a new instance of XMLDocument with a template layout.</summary>
         */
        public XMLDocument() {
            Init();
        }

        /**
         * <summary>Initializes a new instance of XMLDocument with a template layout and username and password database description given.</summary>
         */
        public XMLDocument(string username, string description) {
            Init();

            SetUsername(username);
            SetDescription(description);
        }

        /**
         * <summary>Initializes a new instance of XMLDocument with the given XML string passed.</summary>
         */
        public XMLDocument(string xml) {
            Init(xml);
        }

        #region Initialize
        private void Init() {
            xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(baseXml);

            MemoryProtection.Init();
            EncryptPasswordElements();
        }

        private void Init(string xml) {
            xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            MemoryProtection.Init();
            EncryptPasswordElements();
        }

        #endregion

        #region Password Element Protection
        private void EncryptPasswordElements() {
            XmlNodeList passwordNodes = xmlDoc.SelectNodes("//password");
            foreach(XmlNode node in passwordNodes) {
                node.InnerText = MemoryProtection.Encrypt(node.InnerText);
            }
        }

        private void DecryptPasswordElements() {
            XmlNodeList passwordNodes = xmlDoc.SelectNodes("//password");
            foreach(XmlNode node in passwordNodes) {
                node.InnerText = MemoryProtection.Decrypt(node.InnerText);
            }
        }
        #endregion

        /**
         * <summary>Loads a given XML string into the XMLDocument.</summary>
         */
        public void LoadXML(string xml) {
            xmlDoc.LoadXml(xml);

            EncryptPasswordElements();
        }

        /**
         * <summary>Sets the password database username.<summary>
         * <param name="username">The username to set for the database.</param>
         */
        public void SetUsername(string username) {
            xmlDoc.GetElementsByTagName("username")[0].InnerText = username;
        }

        /**
         * <summary>Sets the password database description.<summary>
         * <param name="username">The description to set for the database.</param>
         */
        public void SetDescription(string description) {
            xmlDoc.GetElementsByTagName("description")[0].InnerText = description;
        }

        /**
         * <summary>Adds a new password entry.</summary>
         * <param name="entry">The PwEntry to be added to the password database.</param>
         */
        public void AddPwEntry(PwEntry entry) {
            DecryptPasswordElements();
            string[] fields = entry.GetFields();
            if(!isRoot(entry.Group)) {
                XmlNode node = xmlDoc.SelectSingleNode("passwordDatabase/passwordGroup/group[@name='" + entry.Group + "']");
                XmlElement new_entry = xmlDoc.CreateElement("entry");

                for(int i = 0; i < tag_names.Length; i++) {
                    XmlElement element = xmlDoc.CreateElement(tag_names[i]);
                    XmlText text = xmlDoc.CreateTextNode(fields[i]);
                    element.AppendChild(text);
                    new_entry.AppendChild(element);
                }


                node.AppendChild(new_entry);
            } else {
                XmlNode group = xmlDoc.SelectSingleNode("passwordDatabase/passwordGroup/root");
                XmlElement new_entry = xmlDoc.CreateElement("entry");

                for(int i = 0; i < tag_names.Length; i++) {
                    XmlElement element = xmlDoc.CreateElement(tag_names[i]);
                    XmlText text = xmlDoc.CreateTextNode(fields[i]);
                    element.AppendChild(text);
                    new_entry.AppendChild(element);
                }
                group.AppendChild(new_entry);
            }
            EncryptPasswordElements();
        }

        /**
         * <summary>Edit a password entry.</summary>
         * <param name="entry">The PwEntry to be edit.</param>
         * <param name="index">The row index of the password.</param>
         */
        public void EditPwEntry(PwEntry entry, int index) {
            DecryptPasswordElements();
            string[] fields = entry.GetFields();
            if(!isRoot(entry.Group)) {
                XmlNode group_node = xmlDoc.SelectSingleNode("passwordDatabase/passwordGroup/group[@name='" + entry.Group + "']/entry[" + index + "]");
                int i = 0;
                foreach(XmlNode node in group_node.ChildNodes) {
                    node.InnerText = fields[i++];
                }
            } else {
                XmlNode group_node = xmlDoc.SelectSingleNode("passwordDatabase/passwordGroup/root/entry[" + index + "]");
                int i = 0;
                foreach(XmlNode node in group_node.ChildNodes) {
                    node.InnerText = fields[i++];
                }
            }
            EncryptPasswordElements();
        }

        /**
         * <summary>Removes a password entry.</summary>
         * <param name="entry">The PwEntry to be removed from the password database in the given group and index.</param>
         */
        public void RemovePwEntry(string group, int index) {
            if(!isRoot(group)) {
                XmlNode node = xmlDoc.SelectSingleNode("passwordDatabase/passwordGroup/group[@name='" + group + "']");
                XmlNode entry = xmlDoc.SelectSingleNode("passwordDatabase/passwordGroup/group[@name='" + group + "']/entry[" + index + "]");
                node.RemoveChild(entry);
            } else {
                XmlNode node = xmlDoc.SelectSingleNode("passwordDatabase/passwordGroup/root");
                XmlNode entry = xmlDoc.SelectSingleNode("passwordDatabase/passwordGroup/root/entry[" + index + "]");
                node.RemoveChild(entry);
            }
        }

        /**
         * <summary>Gets a password entry in a specific group and row index.</summary>
         * <param name="group">The group were the password is stored.</param>
         * <param name="index">The row index of the password.</param>
         * <returns>Returns a PwEntry of a password.</returns>
         */
        public PwEntry GetPwEntry(string group, int index) {
            DecryptPasswordElements();

            PwEntry entry = null;
            if(!isRoot(group)) {
                XmlNode node = xmlDoc.SelectSingleNode("passwordDatabase/passwordGroup/group[@name='" + group + "']/entry[" + index + "]");
                entry = new PwEntry(Util.ToSecureString(node.ChildNodes[2].InnerText), node.ChildNodes[0].InnerText, group,
                                    node.ChildNodes[1].InnerText, node.ChildNodes[3].InnerText, node.ChildNodes[4].InnerText);
            } else {
                XmlNode node = xmlDoc.SelectSingleNode("passwordDatabase/passwordGroup/root/entry[" + index + "]");
                entry = new PwEntry(Util.ToSecureString(node.ChildNodes[2].InnerText), node.ChildNodes[0].InnerText, group,
                                    node.ChildNodes[1].InnerText, node.ChildNodes[3].InnerText, node.ChildNodes[4].InnerText);
            }

            EncryptPasswordElements();
            return entry;
        }

        /**
         * <summary>Adds a new password group to the password database.</summary>
         * <param name="group">The name of the new group to add.</param>
         */
        public void AddPwGroup(string group) {
            XmlNode node = xmlDoc.SelectSingleNode("passwordDatabase/passwordGroup");
            XmlNode group_node = xmlDoc.SelectSingleNode("passwordDatabase/passwordGroup/group[last()]");
            XmlElement new_group = xmlDoc.CreateElement("group");
            XmlAttribute attr = xmlDoc.CreateAttribute("name");
            attr.Value = group;
            new_group.Attributes.Append(attr);
            node.InsertAfter(new_group, group_node);

        }

        /**
         * <summary>Edits a password group and gives it a new name.</summary>
         * <param name="group">The name of the groups name to change.</param>
         * <param name="new_group">The new name to give to the group.</param>
         */
        public void EditPwGroup(string group, string new_group) {
            XmlElement group_node = xmlDoc.SelectSingleNode("passwordDatabase/passwordGroup/group[@name='" + group + "']") as XmlElement;
            group_node.SetAttribute("name", new_group);
        }

        /**
         * <summary>Removes a password group.</summary>
         * <param name="group">The name of the group to remove.</param>
         */
        public void RemovePwGroup(string group) {
            XmlNode node = xmlDoc.SelectSingleNode("passwordDatabase/passwordGroup/group[@name='" + group + "']");
            node.ParentNode.RemoveChild(node);
        }

        /**
         * <summary>Gets all the password groups in the database.</summary>
         * <returns>Returns a TreeNode containing all the different password groups.</returns>
         */
        public TreeNode GetPwGroups() {
            XmlNodeList list = xmlDoc.SelectNodes("passwordDatabase/passwordGroup/group/@name");
            TreeNode[] groups = new TreeNode[list.Count];

            for(int i = 0; i < list.Count; i++) {
                groups[i] = new TreeNode(list[i].Value);
            }
            return new TreeNode(xmlDoc.SelectSingleNode("/passwordDatabase/passwordGroup/@name").Value, groups);
        }

        /**
         * <summary>Gets all the password groups in the database.</summary>
         * <returns>Returns a string array containing all the different password groups.</returns>
         */
        public string[] GetPwGroupsArray() {
            XmlNodeList list = xmlDoc.SelectNodes("passwordDatabase/passwordGroup/group/@name");
            string[] groups = new string[list.Count];

            for(int i = 0; i < list.Count; i++) {
                groups[i] = list[i].Value;
            }
            return groups;
        }

        /**
         * <summary>Move a password group.</summary>
         * <param name="group_name">The name of the group to move.</param>
         * <param name="index">The index where to move the group.</param>
         */
        public void MovePwGroup(string group_name, int index) {
            XmlNode node = xmlDoc.SelectSingleNode("passwordDatabase/passwordGroup/group[@name='" + group_name + "']");
            node.ParentNode.RemoveChild(node);

            XmlNode group_node = xmlDoc.SelectSingleNode("passwordDatabase/passwordGroup/group[" + index + "]");
            group_node.ParentNode.InsertBefore(node, group_node);
        }

        /**
         * <summary>Get the XMLDocument data represented as a string.</summary>
         * <returns>Returns the XMLDocument XML data as a string.</returns>
         */
        public string XMLToString() {
            DecryptPasswordElements();
            string xml = xmlDoc.InnerXml;
            EncryptPasswordElements();

            return xml;
        }

        /**
         * <summary>Gets a XmlDocument.</summary>
         * <returns>Returns a XmlDocument.</returns>
         */
        public XmlDocument GetXmlDocument() {
            DecryptPasswordElements();
            XmlDocument doc = xmlDoc;
            EncryptPasswordElements();

            return doc;
        }

        private bool isRoot(string group) { return ((group == "Password Database") ? true : false); }
    }
}
