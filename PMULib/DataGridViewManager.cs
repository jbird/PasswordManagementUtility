/*
 * DataGridViewManager.cs
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
using System.Data;
using System.Xml;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

namespace PMULib {
    /**
     * <summary>DataGridView Manager a helper class for providing data to the DataGridView control.</summary>
     */
    public sealed class DataGridViewManager {
        private DataGridViewManager() { }

        public static DataTable DefaultDataGridView() {
            DataColumn[] columns = new DataColumn[] {
                    new DataColumn("Title"), new DataColumn("Username"), new DataColumn("Password"), new DataColumn("URL"), new DataColumn("Comment")
                };
            DataTable dt = new DataTable();
            dt.Columns.AddRange(columns);

            return dt;
        }

        /**
         * <summary>Gets a list of passwords in the root database.</summary>
         * <param name="xmlDoc">The XmlDocument to retrieve the passwords from.</param>
         * <returns>Returns a DataTable containing the passwords in the root database.</returns>
         */
        public static DataTable GetRootDataTable(XmlDocument xmlDoc) {
            XmlNodeList list = xmlDoc.SelectNodes("/passwordDatabase/passwordGroup/root/entry");
            System.Windows.Forms.DataGridViewLinkColumn links = new System.Windows.Forms.DataGridViewLinkColumn();
            links.HeaderText = "URL";
            links.DataPropertyName = "URL";

            
            DataColumn[] columns = new DataColumn[] {
                    new DataColumn("Title"), new DataColumn("Username"), new DataColumn("Password"), new DataColumn("URL"), new DataColumn("Comment")
                };
            DataTable dt = new DataTable();
            dt.Columns.AddRange(columns);

            for(int x = 0; x < list.Count; x++) {
                //dt.NewRow();
                string[] tmp = new string[list[x].ChildNodes.Count];
                for(int y = 0; y < columns.Length; y++) {
                    if(y != 2) {
                        tmp[y] = list[x].ChildNodes[y].InnerText;
                    } else {
                        // Mask the password field.
                        for(int i = 0; i < list[x].ChildNodes[y].InnerText.Length; i++) tmp[y] += "*";
                    }
                }
                dt.Rows.Add(tmp);
            }
            
            return dt;
        }

        /**
         * <summary>Gets a list of passwords in the given password group.<summary>
         * <param name="xmlDoc">The XmlDocument to retrieve the passwords from.</param>
         * <param name="group">The group were the list of passwords will be retrieved.</param>
         * <returns>Returns a DataTable containing all the passwords in the given group.</returns>
         */
        public static DataTable GetPwDataTable(XmlDocument xmlDoc, string group) {
            XmlNodeList list = xmlDoc.SelectNodes("/passwordDatabase/passwordGroup/group[@name='" + group + "']/entry");

            DataColumn[] columns = new DataColumn[] {
                    new DataColumn("Title"), new DataColumn("Username"), new DataColumn("Password"), new DataColumn("URL"), new DataColumn("Comment")
                };

            DataTable dt = new DataTable();
            dt.Columns.AddRange(columns);
            if(list.Count == 0) return dt;

            for(int x = 0; x < list.Count; x++) {
                //dt.NewRow();
                string[] tmp = new string[list[x].ChildNodes.Count];
                for(int y = 0; y < columns.Length; y++) {
                    if(y != 2) {
                        tmp[y] = list[x].ChildNodes[y].InnerText;
                    } else {
                        // Mask the password field.
                        for(int i = 0; i < list[x].ChildNodes[y].InnerText.Length; i++) tmp[y] += "*";
                    }
                }
                dt.Rows.Add(tmp);
            }
            
            return dt;
        }

        public static void DataBindingCompleteStyle(DataGridView dataGridView) {
            dataGridView.BackgroundColor = Color.White;
            dataGridView.Columns[3].CellTemplate.ToolTipText = "Click to go to the URL";

            DataGridViewCellStyle style = new DataGridViewCellStyle();
            style.Font = new Font(dataGridView.Font, FontStyle.Underline);
            style.ForeColor = Color.Blue;
            style.SelectionForeColor = Color.White;

            dataGridView.Columns[3].DefaultCellStyle = style;
        }
    }
}
