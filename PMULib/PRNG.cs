/**
 * PseudorandomPasswordGenerator.cs
 * 
 * A Pseudorandom Number Generator using AES-256 in CBC Mode.
 * Used for generating passwords and random data.
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
using System.Security.Cryptography;

namespace PMULib {

    /**
     * <summary>
     * A class that represents a Pseudorandom Number Generator using AES-256 in CBC Mode.
     * </summary>
     */
    public static class PRNG {
        private static RNGCryptoServiceProvider rng = null;
        private static SHA512Managed sha512 = null;

        /**
         * Generate Random Alpha Numeric Password
         * <summary>Generates a cryptographically strong random alpha numeric password.</summary>
         * <returns>A cryptographically strong random alpha numeric password</returns>
         */
        public static string GenerateRandomPassword(int length) {
            char[] alphabet = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 
                                'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
                                '0' , '1' , '2' , '3' , '4' , '5' , '6' , '7' , '8' , '9' };

            char[] password = new char[length];
            byte[] seed = new byte[64];

            rng = new RNGCryptoServiceProvider();
            rng.GetBytes(seed);
            rng = null;

            Random random = new Random(BitConverter.ToInt32(seed, 0));

            Array.Clear(seed, 0, seed.Length);
            for(int i = 0; i < alphabet.Length * 10; i++) {
                SwapElementsInArray(alphabet, GenerateRandomIndex(62), GenerateRandomIndex(62));
            }

            for(int i = 0; i < password.Length; i++) {
                password[i] = alphabet[GenerateRandomIndex(62)];
            }

            Array.Clear(alphabet, 0, alphabet.Length);

            return new string(password);
        }

        private static void SwapElementsInArray<T>(T[] theArray, int index1, int index2) {
            T tempHolder = theArray[index1];
            theArray[index1] = theArray[index2];
            theArray[index2] = tempHolder;
        }

        private static int GenerateRandomIndex(int upperBound) {
            byte[] input = new byte[32], pool = new byte[32], cipherText;
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

            using(SHA256Managed hash = new SHA256Managed()) {
                rng.GetBytes(pool);

                input = hash.ComputeHash(pool);
                hash.Clear();
            }
            Array.Clear(pool, 0, pool.Length);
            rng = null;

            using(AesManaged aes = new AesManaged()) {
                aes.KeySize = 256;
                aes.Padding = PaddingMode.None;
                aes.Mode = CipherMode.CBC;
                aes.GenerateIV();
                aes.GenerateKey();

                using(MemoryStream ms = new MemoryStream()) {
                    ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                    cipherText = new byte[encryptor.OutputBlockSize];

                    using(CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write)) {
                        cs.Write(input, 0, input.Length);
                        cs.FlushFinalBlock();
                        Array.Clear(input, 0, input.Length);

                        cipherText = ms.ToArray();
                        cs.Close();
                    }
                    ms.Close();
                }
                if(aes != null) {
                    aes.Clear();
                }
            }

            Random random = new Random(BitConverter.ToInt32(cipherText, 0));
            Array.Clear(cipherText, 0, cipherText.Length);

            return random.Next(upperBound);
        }

        public static void MixPool(ref byte[] pool, byte[] entropy) {
            byte[] tmp_pool = new byte[pool.Length];
            rng = new RNGCryptoServiceProvider();
            rng.GetBytes(tmp_pool);
            rng = null;

            using(AesManaged aes = new AesManaged()) {
                aes.KeySize = 256;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.None;
                aes.GenerateKey();
                aes.GenerateIV();
                using(MemoryStream ms = new MemoryStream()) {
                    using(CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write)) {
                        cs.Write(tmp_pool, 0, tmp_pool.Length);
                        cs.FlushFinalBlock();

                        int i = 0;
                        while(i < ms.Length) {
                            pool[i++] ^= Convert.ToByte(ms.ReadByte());
                        }
                    }
                }
            }
        }

        public static byte[] GetBytes(int bytes) {
            byte[] pool = new byte[bytes];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(pool);

            return pool;
        }
    }
}
