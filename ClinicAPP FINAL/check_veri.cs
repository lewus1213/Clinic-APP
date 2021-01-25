using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace ClinicAPP_FINAL
{
    class check_veri
    {
        public static string veri;
        public static void check()
        {
            MySqlConnection conn = connection_config.GetDBConnection();
            string query = "SELECT check_pwd FROM users WHERE id='" + login_session.id + "'";
            MySqlCommand command = new MySqlCommand(query, conn);
            conn.Open();
            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                veri = Convert.ToString(reader.GetBoolean(0));
            }
            conn.Close();
        }
    }
}
