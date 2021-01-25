using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace ClinicAPP_FINAL
{
    public partial class veri : Form
    {
        public veri()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string ver = textBox1.Text + textBox2.Text + textBox3.Text + textBox4.Text + textBox5.Text;

            if (change_pass.veri == ver)
            {
                var encryptedString = pswd.EncryptString(change_pass.pass);

                MySqlConnection conn = connection_config.GetDBConnection();
                MySqlCommand command = conn.CreateCommand();
                string query = "UPDATE users SET password='" + encryptedString + "', check_pwd='0' WHERE id='" + login_session.id + "'";
                conn.Open();
                command.CommandText = query;
                command.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("Poprawnie ustawiono hasło");
                this.Close();
                login login2 = new login();
                login2.Show();
            }
            else
            {
                MessageBox.Show("Kod weryfikacyjny jest nieprawidłowy");
            }
        }
    }
}
