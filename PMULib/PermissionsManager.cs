/**
 * PermissionsManager.cs
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
using System.Security;
using System.Security.Permissions;

namespace PMULib {
    public sealed class PermissionsManager {
        private PermissionsManager() { }

        /**
         * <summary>Test's whether the user has read permission on the given file/folder.</summary>
         * <param name="path">The file/folder location</param>
         * <returns>Returns true if the user has read permission on the given file/folder, else returns false</returns>
         */
        public static bool ReadPermission(string path) {
            FileIOPermission fp = new FileIOPermission(FileIOPermissionAccess.Read, path);
            try {
                fp.Demand();
            } catch(SecurityException) {
                return false;
            }
            return true;
        }

        /**
         * <summary>Test's whether the user has write permission on the given file/folder.</summary>
         * <param name="path">The file/folder location</param>
         * <returns>Returns true if the user has write permission on the given file/folder, else returns false</returns>
         */
        public static bool WritePermission(string path) {
            FileIOPermission fp = new FileIOPermission(FileIOPermissionAccess.Write, path);
            try {
                fp.Demand();
            } catch(SecurityException) {
                return false;
            }
            return true;
        }
    }
}
