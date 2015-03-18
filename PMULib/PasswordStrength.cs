/**
 * PasswordStrength.cs
 * Author: John Bird
 * 
 * Password Strength class for testing password strength based
 * on upper(A-Z) and lower(a-z) case characters, numbers(0-9) and
 * non-alphanumeric characters(£,$,%,&,(,@) etc.
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
using System.Text.RegularExpressions;

namespace PMULib {

    /**
     * <summary>Represents a method for testing the strength of a given password and returning a score between 0-100%.</summary>
     */
    public static class PasswordStrength {

        /**
         * <summary>Tests a passwords strength based on length, upper/lower case characters, numbers and non-alphanumeric characters.</summary>
         * <param name="password">The passwors to be tested.</param>
         * <returns>The password strength score between 0 and 100%</returns>
         */
        public static int TestPasswordStrength(string password) {
            int score = 0;

            if(password.Length < 4) return score;

            score += password.Length * 4;
            score += CheckRepetition(password, 1);
            score += CheckRepetition(password, 2);
            score += CheckRepetition(password, 3);
            score += CheckRepetition(password, 4);

            // Password has 3 numbers
            if(Regex.IsMatch(password, "/(.*[0-9].*[0-9].*[0-9])/")) score += 5;

            // Password has 2 symbols
            if(Regex.IsMatch(password, "/(.*[!,@,#,$,%,^,&,*,?,_,~].*[!,@,#,$,%,^,&,*,?,_,~])/")) score += 5;

            // Password has upper and lower case characters
            if(Regex.IsMatch(password, "/([a-z].*[A-Z])|([A-Z].*[a-z])/")) score += 10;

            // Password has number and characters
            if(Regex.IsMatch(password, "/([a-zA-Z])/") && Regex.IsMatch(password, "/([0-9])/")) score += 15;

            // Password has number and symbol
            if(Regex.IsMatch(password, "/([!,@,#,$,%,^,&,*,?,_,~])/") && Regex.IsMatch(password, "/([a-zA-Z])/")) score += 15;

            // Password is just a number of characters
            if(Regex.IsMatch(password, "/^\\w+$/") || Regex.IsMatch(password, "/^\\d+$/")) score -= 15;

            // Verifing 0 < score < 100
            if(score < 0) score = 0;
            if(score > 100) score = 100;

            return score;
        }

        private static int CheckRepetition(string str, int len) {
            string result = String.Empty;

            for(int i = 0; i < str.Length; i++) {
                bool repeated = true;
                int j;
                for(j = 0; j < len && (j + i + len) < str.Length; j++ ) {
                    repeated = repeated && (str[j+i] == str[j+i+len]);
                }
                if(j < len) repeated = false;
                if(repeated) {
                    i += len - 1;
                    repeated = false;
                } else {
                    result += str[i];
                }
            }
            return (result.Length - str.Length) * 1;
        }
    }
}
