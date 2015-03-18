/*
 * PwEntry.cs
 * 
 * Password Entry class for instansiating a password and it's
 * related fields as well as providing the functionality to get it's fields.
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;

namespace PMULib {
    /**
     * <summary>Represents a Password entry class for holding all the fields for a given password entry.</summary>
     */
    public sealed class PwEntry : IDisposable {
        private string title, group, username, url, comment;
        private SecureString password;

        /**
         * <summary>Initializes a new instance of PwEntry.</summary>
         */
        public PwEntry() {
            password = new SecureString();
        }

        /**
         * <summary>Initializes a new instance of PwEntry with the given fields.</summary>
         */
        public PwEntry(SecureString password, string title, string group, string username, string url, string comment) {
            this.password = password;
            this.password.MakeReadOnly();

            this.title = title;
            this.group = group;
            this.username = username;
            this.url = url;
            this.comment = comment;
        }

        /**
         * <summary>Gets the password title for this password entry.</summary>
         */
        public SecureString Password {
            get { return this.password; }
        }

        /**
         * <summary>Gets or Sets the password title for this password entry.</summary>
         */
        public string Title {
            get { return this.title; }
            set { this.title = value; }
        }

        /**
         * <summary>Gets or Sets the password group for this password entry.</summary>
         */
        public string Group {
            get { return this.group; }
            set { this.group = value; }
        }

        /**
         * <summary>Gets or Sets the password username for this password entry.</summary>
         */
        public string Username {
            get { return this.username; }
            set { this.group = value; }
        }

        /**
         * <summary>Gets or Sets the password URL for this password entry.</summary>
         */
        public string URL {
            get { return this.url; }
            set { this.url = value; }
        }

        /**
         * <summary>Gets or Sets the password comment for this password entry.</summary>
         */
        public string Comment {
            get { return this.comment; }
            set { this.comment = value; }
        }

        public string[] GetFields() {
            return new string[] {Title, Username,  Util.ToSystemString(Password), URL, Comment };
        }

        private void Dispose(bool disposing) {
            if(disposing) {
                password.Dispose();
            }
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
