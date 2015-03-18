/**
 * Util.cs
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
using System.Globalization;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Win32;

namespace PMULib {
    public static class Util {
        private static CultureInfo culture = CultureInfo.CurrentCulture;
        private static readonly string[] VersionKeys = new string[] {
            @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full",
            @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Client",
            @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v3.5",
            @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v3.0",
            @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v2.0.50727"
        };

        public static string GetOSInfo() {
            string osText = String.Empty;
            OperatingSystem os = Environment.OSVersion;

            switch(os.Version.Major) {
                case 5:
                    switch(os.Version.Minor) {
                        case 0:
                            osText = "Windows 2000";
                            break;
                        case 1:
                            osText = "Windows XP";
                            break;
                        case 2:
                            osText = "Windows Server 2003";
                            break;
                        default:
                            osText = os.ToString();
                            break;
                    }
                    break;
                
                case 6:
                    switch(os.Version.Minor) {
                        case 0:
                            osText = "Windows Vista";
                            break;
                        case 1:
                            osText = "Windows Server 2008";
                            break;
                        default:
                            osText = os.ToString();
                            break;
                    }
                    break;
            }

            if(!string.IsNullOrEmpty(osText)) {
                osText += String.Format(culture, " {0}", os.ServicePack);
            } else {
                osText = String.Format(culture, "{0}", os.ToString());
            }

            return String.Format(culture, "{0}\r\n{1}", osText, DotNetVersion());
        }

        private static string DotNetVersion() {
            string version = "";
            foreach(string versionKey in VersionKeys) {
                RegistryKey key = Registry.LocalMachine.OpenSubKey(versionKey);

                if(key != null) {
                    version = String.Format(culture, "Microsoft .NET Framework Version {0}\r\n", key.GetValue("Version"));

                    if(key.GetValue("SP") != null) {
                        version += String.Format(culture, "Service Pack {0}", key.GetValue("SP"));
                    }
                    break;
                }
            }
            return version;
        }

        public static bool CompatibleRuntimeVersion() {
            foreach(string versionKey in VersionKeys) {
                RegistryKey key = Registry.LocalMachine.OpenSubKey(versionKey);

                if(key != null) {
                    if(key.GetValue("Install").ToString() == "1") return true;
                }
            }
            return false;
        }

        /**
         * <summary>Tests for surport for Microsofts Cryptographic API.</summary>
         * <returns>Returns true if surport for CAPI is found, else returns false.</returns>
         */
        public static bool CAPIAlgorithmSupport() {
            try {
                new AesCryptoServiceProvider();
            } catch(PlatformNotSupportedException) { return false; }
            return true;
        }

        public static bool CngAlgorithmSupport() {
            try {
                new SHA256Cng();
            } catch(PlatformNotSupportedException) { return false; }
            return true;
        }

        /**
         * <summary>Converts a System String to a SecureString.</summary>
         * <param name="password">The String to be converted into a SecureString</param>
         * <returns>Returns a SecureString of the passed String value</returns>
         */
        public static SecureString ToSecureString(string password) {
            SecureString str = new SecureString();
            char[] passArray = password.ToCharArray();

            for(int i = 0; i < password.Length; i++) {
                str.AppendChar(passArray[i]);
            }
            Array.Clear(passArray, 0, passArray.Length);
            return str;
        }

        /**
         * <summary>Converts a SecureString to a System String.</summary>
         * <param name="password">The SecureString to be converted.</param>
         * <returns>Returns a System String from the passed SecureString value.</returns>
         */
        public static string ToSystemString(SecureString password) {
            IntPtr secureStrPtr = Marshal.SecureStringToBSTR(password);
            string systemStr = Marshal.PtrToStringBSTR(secureStrPtr);

            Marshal.ZeroFreeBSTR(secureStrPtr);
            /*if(!password.IsReadOnly()) {
                password.Clear();
            }*/
            return systemStr;
        }

        public static byte[] GetSystemData() {
            using(MemoryStream memoryStream = new MemoryStream()) {
                byte[] data;
                try {
                    data = UInt32ToBytes((uint)Environment.TickCount);
                    memoryStream.Write(data, 0, data.Length);

                    data = UInt64ToBytes((uint)DateTime.Now.Ticks);
                    memoryStream.Write(data, 0, data.Length);

                    Process p = Process.GetCurrentProcess();
                    data = UInt64ToBytes((ulong)p.Handle.ToInt64());
                    memoryStream.Write(data, 0, data.Length);
                    data = UInt32ToBytes((uint)p.Id);
                    memoryStream.Write(data, 0, data.Length);
                    data = UInt64ToBytes((ulong)p.NonpagedSystemMemorySize64);
                    memoryStream.Write(data, 0, data.Length);
                    data = UInt64ToBytes((ulong)p.PagedMemorySize64);
                    memoryStream.Write(data, 0, data.Length);
                    data = UInt64ToBytes((ulong)p.PeakPagedMemorySize64);
                    memoryStream.Write(data, 0, data.Length);
                    data = UInt64ToBytes((ulong)p.PeakVirtualMemorySize64);
                    memoryStream.Write(data, 0, data.Length);
                    data = UInt64ToBytes((ulong)p.PeakWorkingSet64);
                    memoryStream.Write(data, 0, data.Length);
                    data = UInt64ToBytes((ulong)p.PrivateMemorySize64);
                    memoryStream.Write(data, 0, data.Length);
                    data = UInt64ToBytes((ulong)p.StartTime.ToBinary());
                    memoryStream.Write(data, 0, data.Length);
                    data = UInt64ToBytes((ulong)p.WorkingSet64);
                    memoryStream.Write(data, 0, data.Length);
                } catch(Exception) {
                } finally {
                    data = Guid.NewGuid().ToByteArray();
                    memoryStream.Write(data, 0, data.Length);
                }
                Array.Clear(data, 0, data.Length);
                return memoryStream.ToArray();
            }
        }

        public static byte[] GetSystemData(byte[] r_source) {
            using(MemoryStream memoryStream = new MemoryStream()) {
                byte[] data;
                try {
                    memoryStream.Write(r_source, 0, r_source.Length);

                    data = UInt32ToBytes((uint)Environment.TickCount);
                    memoryStream.Write(data, 0, data.Length);

                    data = UInt64ToBytes((uint)DateTime.Now.Ticks);
                    memoryStream.Write(data, 0, data.Length);

                    Process p = Process.GetCurrentProcess();
                    data = UInt64ToBytes((ulong)p.Handle.ToInt64());
                    memoryStream.Write(data, 0, data.Length);
                    data = UInt32ToBytes((uint)p.Id);
                    memoryStream.Write(data, 0, data.Length);
                    data = UInt64ToBytes((ulong)p.NonpagedSystemMemorySize64);
                    memoryStream.Write(data, 0, data.Length);
                    data = UInt64ToBytes((ulong)p.PagedMemorySize64);
                    memoryStream.Write(data, 0, data.Length);
                    data = UInt64ToBytes((ulong)p.PeakPagedMemorySize64);
                    memoryStream.Write(data, 0, data.Length);
                    data = UInt64ToBytes((ulong)p.PeakVirtualMemorySize64);
                    memoryStream.Write(data, 0, data.Length);
                    data = UInt64ToBytes((ulong)p.PeakWorkingSet64);
                    memoryStream.Write(data, 0, data.Length);
                    data = UInt64ToBytes((ulong)p.PrivateMemorySize64);
                    memoryStream.Write(data, 0, data.Length);
                    data = UInt64ToBytes((ulong)p.StartTime.ToBinary());
                    memoryStream.Write(data, 0, data.Length);
                    data = UInt64ToBytes((ulong)p.WorkingSet64);
                    memoryStream.Write(data, 0, data.Length);
                } catch(Exception) {
                } finally {
                    data = Guid.NewGuid().ToByteArray();
                    memoryStream.Write(data, 0, data.Length);
                }
                Array.Clear(data, 0, data.Length);
                return memoryStream.ToArray();
            }
        }

        private static byte[] UInt32ToBytes(uint data) {
            byte[] b = new byte[4];

            unchecked {
                b[0] = (byte)data;
                b[1] = (byte)(data >> 8);
                b[2] = (byte)(data >> 16);
                b[3] = (byte)(data >> 24);
            }
            return b;
        }

        private static byte[] UInt64ToBytes(ulong data) {
            byte[] b = new byte[8];

            unchecked {
                b[0] = (byte)data;
                b[1] = (byte)(data >> 8);
                b[2] = (byte)(data >> 16);
                b[3] = (byte)(data >> 24);
                b[4] = (byte)(data >> 32);
                b[5] = (byte)(data >> 40);
                b[6] = (byte)(data >> 48);
                b[7] = (byte)(data >> 56);
            }
            return b;
        }

        #region Clipboard Methods

        public static void CopyPasswordToClipboard(PwEntry entry) {
            Clipboard.SetData(DataFormats.Text, Util.ToSystemString(entry.Password));
        }

        public static void CopyUsernameToClipboard(PwEntry entry) {
            Clipboard.SetData(DataFormats.Text, entry.Username);
        }

        public static void CopyURLToClipboard(PwEntry entry) {
            Clipboard.SetData(DataFormats.Text, entry.URL);
        }

        public static void ClearClipboard() { Clipboard.Clear(); }

        #endregion

    }
}
