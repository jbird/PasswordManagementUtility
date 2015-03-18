/**
 * MasterKey.cs
 * 
 * Master Key class which parses a given master password and
 * transforms the password using PBKDF2 and SHA-256.
 * Addtional functionality for generating the key and IV is also given.
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
using System.Security;
using System.Security.Cryptography;
using System.Runtime.InteropServices;

namespace PMULib {
    /**
     * <summary>Represents a Master Key class for transforming passwords into usable encryption keys.</summary>
     */
    public sealed class MasterKey : IDisposable {
        private SHA256 sha256;

        private int iterations = 8000;
        private readonly byte[] salt = Encoding.ASCII.GetBytes("6vEY43mUrZxWtGJfB47iTW7eoVzuJusOiLwiIrmzuMfqhdO2NSlGhSpHzaRuak0T");
        private SecureString password;
        private Keyfile keyfile;

        private MasterKey() { }

        /**
         * <summary>Initializes a new instance of MasterKey with a user given password.</summary>
         * <param name="password">A given password to be transformed into a cryptographic hash result.</param>
         */
        public MasterKey(SecureString password) {
            this.password = password;
            
            if(Util.CngAlgorithmSupport()) {
                sha256 = new SHA256Cng();
            } else {
                sha256 = new SHA256Managed();
            }
        }

        /**
         * <summary>Initializes a new instance of MasterKey with a user given password and keyfile.</summary>
         * <param name="password">A given password to be transformed with a keyfile into a cryptographic hash result.</param>
         * <param name="keyfile">A given keyfile to be transformed with the password into a cryptographic hash result.</param>
         */
        public MasterKey(SecureString password, Keyfile keyfile) {
            this.password = password;
            this.keyfile = keyfile;

            if(Util.CngAlgorithmSupport()) {
                sha256 = new SHA256Cng();
            } else {
                sha256 = new SHA256Managed();
            }
        }

        /*
        private int CalculateIterations() {
            DateTime start = new DateTime();
            TimeSpan duration = new TimeSpan();
            int i = 1000;
            string password = Util.ToSystemString(this.password);

            do {
                i += 10000;
                start = DateTime.Now;
                Rfc2898DeriveBytes pkcs5 = new Rfc2898DeriveBytes(password, salt, i);
                byte[] hash = pkcs5.GetBytes(32);
                duration = DateTime.Now - start;
                pkcs5.Reset();
            } while(!(duration.Seconds >= 1));
            
            return i;
        }*/

        /**
         * <summary>Generates a initialization vector.</summary>
         * <returns>Returns an array of bytes derived from the password.</returns>
         */
        public byte[] GenerateIV() {
            Rfc2898DeriveBytes iv = new Rfc2898DeriveBytes(Util.ToSystemString(password), salt, iterations);
            return iv.GetBytes(16);
        }

        /**
         * <summary>Generates a initialization vector to the given byte length.</summary>
         * <returns>Returns an array of bytes derived from the given password.</returns>
         */
        public byte[] GenerateIV(int bytes) {
            Rfc2898DeriveBytes iv = new Rfc2898DeriveBytes(Util.ToSystemString(password), salt, iterations);
            return iv.GetBytes(bytes);
        }

        /**
         * <summary>Generates the Master Key using PKCS #5 and outputs a SHA-256 hash.</summary>
         * <returns>Returns a byte array of the cryptographic hash.</returns>
         */
        public byte[] GenerateMasterKey() {
            if(keyfile != null) {
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(Util.ToSystemString(password) + Util.ToSystemString(keyfile.GetKey()), salt, iterations);
                return sha256.ComputeHash(key.GetBytes(32));
            } else {
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(Util.ToSystemString(password), salt, iterations);
                return sha256.ComputeHash(key.GetBytes(32));
            }
        }

        /**
         * <summary>Releases all resources used by PMULib.MasterKey.</summary>
         */
        public void Clear() {
            password.Dispose();
            if(keyfile != null) {
                keyfile.Clear();
            }
            sha256.Clear();
        }

        private void Dispose(bool disposing) {
            if(disposing) {
                Clear(); // Dispose of managed resources.
            }
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
