/*
 * MemoryProtection.cs
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
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Security;

namespace PMULib {
    internal sealed class MemoryProtection {
        private static Aes aes;

        public static void Init() {
            if(Util.CAPIAlgorithmSupport()) {
                aes = new AesCryptoServiceProvider();
            } else {
                aes = new AesManaged();
            }

            aes.KeySize = 128;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.GenerateKey();
            aes.GenerateIV();
        }
        
        public static string Encrypt(string str) {
            byte[] plainText = Encoding.ASCII.GetBytes(str);
            byte[] cipherText = { };

            using(MemoryStream ms = new MemoryStream()) {

                using(ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV)) {
                    using(CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write)) {
                        cs.Write(plainText, 0, plainText.Length);
                        cs.FlushFinalBlock();
                        cipherText = ms.ToArray();
                    }
                }
            }
            return Convert.ToBase64String(cipherText);
        }

        public static string Decrypt(string str) {
            byte[] cipherText = Convert.FromBase64String(str);
            byte[] plainText = { };

            using(MemoryStream ms = new MemoryStream()) {

                using(ICryptoTransform encryptor = aes.CreateDecryptor(aes.Key, aes.IV)) {
                    using(CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write)) {
                        cs.Write(cipherText, 0, cipherText.Length);
                        cs.FlushFinalBlock();
                        plainText = ms.ToArray();
                    }
                }
            }
            return Encoding.ASCII.GetString(plainText);
        }
    }
}
