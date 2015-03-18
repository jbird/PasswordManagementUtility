/**
 * XMLReader.cs
 * 
 * XML Reader class for parsing a given path to a password database
 * location and in turn decrypts the data using AES-256 in CBC mode
 * with the give master key[and key file] nand decompresses the data 
 * using GZip. A XML Document is then returned.
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
using System.IO;
using System.IO.Compression;
using System.Security.Permissions;
using System.Security;
using System.Security.Cryptography;

using System.ComponentModel;
using System.Threading;

namespace PMULib {
    /**
     * <summary>Represents a XML Reader class for parsing and decrypting the contents of a password database.</summary>
     */
    internal sealed class XMLReader {
        private MasterKey masterKey;
        private FileStream fs;
        private GZipStream cs;
        private XMLDocument xmlDoc;
        
        private XMLReader() { }

        /**
         * <summary>Initializes a new instance of XMLReader with the given masterkey and password database location.</summary>
         * <param name="masterKey">The masterkey used for decrypting the database.</param>
         * <param name="path">The file location of the given password database.</param>
         */
        public XMLReader(MasterKey masterKey, string path) {
            this.masterKey = masterKey;
            fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            cs = new GZipStream(fs, CompressionMode.Decompress);
            
            Init();
        }

        

        private XMLDocument LoadXMLDocument() {
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

                using(CryptoStream cryptoStream = new CryptoStream(cs, aes.CreateDecryptor(), CryptoStreamMode.Read)) {
                    using(StreamReader reader = new StreamReader(cryptoStream)) {
                        return new XMLDocument(reader.ReadToEnd());

                    }
                }
            }
        }

        /**
         * <summary>Decrypts and decompresses the password database, then passes the result to a XML Document.</summary>
         */
        private void Init() {
            
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

            using(CryptoStream cryptoStream = new CryptoStream(cs, aes.CreateDecryptor(), CryptoStreamMode.Read)) {
                using(StreamReader reader = new StreamReader(cryptoStream)) {
                    xmlDoc = new XMLDocument(reader.ReadToEnd());
                    reader.Close();
                }
                cryptoStream.Close();
            }
            aes.Clear();
        }

        /**
         * <summary>Get the XML data represented as a string.</summary>
         * <returns>Returns the XML data as a string.</returns>
         */
        public string XMLToString() {
            return xmlDoc.XMLToString();
        }

        /**
         * <summary>Get's a XML Document from the XML Reader.</summary>
         * <returns>Returns a XMLDocument.</returns>
         */
        public XMLDocument GetXMLDocument() {
            return xmlDoc;
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
