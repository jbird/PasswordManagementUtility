using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Security.Cryptography;
using System.Threading;
using PMULib;
using System.IO;

using Microsoft.Win32;
using Microsoft.Internal;

namespace PasswordManagementUtility.Forms {
    public partial class KeyfileGeneratorForm : Form {
        private Aes aes = null;
        private SHA512 sha512 = null;
        private SHA256 sha256 = null;
        private RNGCryptoServiceProvider rng = null;

        private static byte[] pool = null, tmp_pool = null;
        private static byte[] hash = null;
        private static int i, x, y;

        public KeyfileGeneratorForm() {
            InitializeComponent();
            
            if(Util.CngAlgorithmSupport()) {
                aes = new AesCryptoServiceProvider();
                sha512 = new SHA512Cng();
                sha256 = new SHA256Cng();
            } else {
                aes = new AesManaged();
                sha256 = new SHA256Managed();
                sha512 = new SHA512Managed();
            }
        }

        private void KeyFileGenerator_Load(object sender, EventArgs e) {
            x = -1;
            y = -1;

            int w, c;
            ThreadPool.GetMinThreads(out w, out c);
            ThreadPool.SetMinThreads(w, 4);

            pool = new byte[128]; // 1024-bits
            tmp_pool = new byte[128];
            rng = new RNGCryptoServiceProvider();
            rng.GetBytes(pool);

            UpdateForm();
            timer1.Start();
        }

        private void MixPool(Object n) {
            //rng.GetBytes(pool);
            rng.GetBytes(tmp_pool);
            //PRNG.MixPool(ref pool, Util.GetSystemData(new byte[] { (byte)x, (byte)y }));
            
            /*// Insert a 512-bit hash of the pool into the first half of the pool 
            Buffer.BlockCopy(sha512.ComputeHash(tmp_pool), 0, tmp_pool, 0, 64);
            // Insert a 512-bit hash of randomly captured system data and insert into the second half.
            Buffer.BlockCopy(sha512.ComputeHash(Util.GetSystemData(new byte[] { (byte)x, (byte)y })), 0, tmp_pool, 64, 64);*/

            for(int i = 0; i < 128; i++) {
                pool[i] ^= tmp_pool[i];
            }
        }

        private void UpdateForm() {
            i = 0;
            foreach(Control control in dataPoolPanel.Controls) {
                control.Text = pool[i].ToString("X");
                i++;
            }
            Refresh();
        }

        private void keyfilePanel_MouseMove(object sender, MouseEventArgs e) {
            if(e.X != x || e.Y != y) {
                x = e.X; y = e.Y;

                ThreadPool.QueueUserWorkItem(new WaitCallback(MixPool));
                UpdateForm();
            }
        }

        private void dataPoolPanel_MouseMove(object sender, MouseEventArgs e) {
            if(e.X != x || e.Y != y) {
                x = e.X; y = e.Y;

                ThreadPool.QueueUserWorkItem(new WaitCallback(MixPool));
                UpdateForm();
            }
        }

        public string GetKey() {
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }

        private void generateKeyfileButton_Click(object sender, EventArgs e) {
            try {
                hash = sha256.ComputeHash(pool);
                Clear();
            } catch(Exception ex) {
                MessageBox.Show(ex.Message);
            }

            DialogResult = DialogResult.OK;
        }

        private void cancelButton_Click(object sender, EventArgs e) {
            Clear();
            Dispose();
        }

        private void Clear() {
            sha256.Clear();
            sha512.Clear();
            rng = null;
            Array.Clear(pool, 0, 128);
        }

        private void timer1_Tick(object sender, EventArgs e) {
            ThreadPool.QueueUserWorkItem(new WaitCallback(MixPool));
            UpdateForm();
        }
    }
}
