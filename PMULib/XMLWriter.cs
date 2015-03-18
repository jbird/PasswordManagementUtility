/**
 * XMLWriter.cs
 * 
 * XML Writer class for compressing the password database using GZip, 
 * encrypts the data using AES-256 in CBC Mode with the give master
 * key[and key file] nand saves the data to the given path location.
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
using System.IO;
using System.IO.Compression;
using System.Security.Permissions;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace PMULib {
    /**
     * <summary>Represents a XML Writer class for parsing and encrypting the contents of a password database.</summary>
     */
    internal sealed class XMLWriter {
        private MasterKey masterKey;
        private FileStream fs;
        private GZipStream cs;
        private XMLDocument xmlDoc;

        private XMLWriter() { }

        /**
         * <summary>Initializes a new instance of XMLWriter with the given masterkey and password database location.</summary>
         * <param name="masterKey">The masterkey used for encrypting the database.</param>
         * <param name="path">The file location of the given password database.</param>
         */
        public XMLWriter(MasterKey masterKey, string path) {
            this.masterKey = masterKey;
            fs = new FileStream(path, FileMode.Create, FileAccess.Write);
            cs = new GZipStream(fs, CompressionMode.Compress);
        }

        /**
         * <summary>Write's a new XMLDocument to the XMLWriter.</summary>
         * <param name="xmlDoc">The XMLDocument to be written to the XMLWriter.</param>
         */
        public void Write(XMLDocument xmlDoc) {
            this.xmlDoc = xmlDoc;
        }

        /**
         * <summary>Save's the current state by compressing and encrypting the password database.</summary>
         */
        public void Save() {
            lock(this) {
                using(AesManaged aes = new AesManaged()) {
                    aes.KeySize = 256;
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;
                    aes.Key = masterKey.GenerateMasterKey();
                    aes.IV = masterKey.GenerateIV();

                    using(CryptoStream cryptoStream = new CryptoStream(cs, aes.CreateEncryptor(), CryptoStreamMode.Write)) {
                        using(StreamWriter writer = new StreamWriter(cryptoStream)) {
                            writer.Write(xmlDoc.XMLToString());
                            writer.Close();
                        }
                        cryptoStream.Close();
                    }
                    aes.Clear();
                }
            }
        }

        /**
         * <summary>Save's the password database provided in the XMLDocument into the database path location.</summary>
         * <param name="xmlDoc">The XMLDocument to be saved.</param>
         */
        public void Save(XMLDocument xmlDoc) {
            lock(this) {
                Aes aes = null;
                if(Util.CAPIAlgorithmSupport()) {
                    aes = new AesCryptoServiceProvider();
                } else {
                    aes = new AesManaged();
                }
                aes.KeySize = 256;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = masterKey.GenerateMasterKey();
                aes.IV = masterKey.GenerateIV();

                using(CryptoStream cryptoStream = new CryptoStream(cs, aes.CreateEncryptor(), CryptoStreamMode.Write)) {
                    using(StreamWriter writer = new StreamWriter(cryptoStream)) {
                        writer.Write(xmlDoc.XMLToString());
                        writer.Close();
                    }
                    cryptoStream.Close();
                }
                aes.Clear();
            }
        }

        /**
         * <summary>Closes and released all streams/resources.</summary>
         */
        public void Close() {
            masterKey.Clear();
            xmlDoc = null;
            if(cs != null) cs.Close();
            if(fs != null) fs.Close();
        }
    }
}
