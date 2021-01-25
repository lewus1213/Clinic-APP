using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace ClinicAPP_FINAL
{
    class password_str
    {
        public static int score;
        public enum PasswordScore
        {
            Blank = 0,
            VeryWeak = 1,
            Weak = 2,
            Medium = 3,
            Strong = 4,
            VeryStrong = 5
        }
        public static PasswordScore CheckStrength(string password)
        {
            score = 0;
            if (password.Length >= 8)
                score++;
            if (Regex.Match(password, @"\d", RegexOptions.ECMAScript).Success)
                score++;
            if (Regex.Match(password, @"[A-Z][a-z]{0,1}\d*", RegexOptions.ECMAScript).Success)
                score++;

            return (PasswordScore)score;
        }
    }
}
