using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ClinicAPP_FINAL
{
    class connection_config
    {
        public static MySqlConnection GetDBConnection()
        {
            string host = "46.238.234.240";
            int port = 3306;
            string database = "clinicapp";
            string username = "root";
            string password = "";

            return connection.GetDBConnection(host, port, database, username, password);
        }
    }
}
