/**
 * Keyfile.cs
 * 
 * Keyfile class which parses a XML formed Keyfile from the
 * path given. Additional functionality for creating a keyfile
 * and getting the key is provided.
 * 
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
using System.Xml;
using System.Security;

namespace PMULib {
    public sealed class Keyfile {
        private XmlDocument xmlDoc;
        private string path;
        private SecureString key;
        private static readonly string baseXml = "<?xml version=\"1.0\"?><keyfile version=\"" + PwDatabase.VERSION + "\"><key></key></keyfile>";

        private Keyfile() { }

        /**
         * <summary>Initializes a new instance of Keyfile with a given path location to the keyfile.</summary>
         * <param name="path">The location of the keyfile to parse.</param>
         */
        public Keyfile(string path) {
            this.path = path;
            using(FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read)) {
                using(StreamReader reader = new StreamReader(fs)) {
                    xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(reader.ReadToEnd());

                    key = GetKey(xmlDoc);
                }
            }
        }

        ~Keyfile() {
            Clear();
        }

        /**
         * <summary>Creates a new Keyfile and saves it to the path given.</summary>
         * <param name="path">The path where to save the Keyfile.</param>
         * <param name="data">The</param>
         */
        public static bool CreateKeyfile(string path, string key) {
            try {
                using(FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write)) {
                    using(StreamWriter writer = new StreamWriter(fs)) {
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(baseXml);
                        xmlDoc.GetElementsByTagName("key")[0].InnerText = key;
                        writer.Write(xmlDoc.InnerXml);
                    }
                    File.SetAttributes(path, FileAttributes.ReadOnly);
                }
                return true;
            } catch(Exception) { return false; }
        }

        private static SecureString GetKey(XmlDocument xmlDoc) {
            XmlNode node = xmlDoc.SelectSingleNode("keyfile/key");
            return Util.ToSecureString(node.InnerText);
        }

        /**
         * <summary>Gets the key from inside the keyfile.</summary>
         * <returns>Returns the 256-bit key.</returns>
         */
        public SecureString GetKey() { return key; }

        /**
         * <summary>Releases all resources used by PMULib.Keyfile.</summary>
         */
        public void Clear() {
            if(key != null) {
                key.Dispose();
            }
            xmlDoc = null;
            path = null;
        }
    }
}
