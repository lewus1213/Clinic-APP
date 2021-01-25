using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClinicAPP_FINAL
{
    class login_session
    {
        public static string name = "";
        public static string surname = "";
        public static string mail = "";
        public static string role = "";
        public static string username = "";
        public static string id = "";
        public static bool logged;

        public static void log()
        {
            if(logged==false)
            {
                Application.Exit();
            }
        }
    }
}
