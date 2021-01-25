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
using System.Security.Cryptography;
using System.IO;
using log4net;

namespace ClinicAPP_FINAL
{
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
        }

        static string query;
        MySqlConnection conn = connection_config.GetDBConnection();
        MySqlCommand command;
        MySqlDataReader reader;

        private void get_data()
        {
            query = "SELECT id, role_id, username FROM users WHERE username='" + textBox1.Text + "'";
            command = new MySqlCommand(query, conn);
            conn.Open();
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                login_session.id = reader["id"].ToString();
                login_session.role = reader["role_id"].ToString();
                login_session.username = reader["username"].ToString();
            }
            reader.Close();
            conn.Close();

            query = "SELECT name, surname, email FROM personel WHERE id='" + login_session.id + "'";
            command = new MySqlCommand(query, conn);
            command.CommandText = query;
            conn.Open();
            reader = command.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    login_session.name = reader.GetString("name");
                    login_session.surname = reader.GetString("surname");
                    login_session.mail = reader.GetString("email");
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            logg();
        }
        private void logg()
        {
            try
            {
                var encryptedString = pswd.EncryptString(textBox2.Text);
                get_data();
                conn.Open();
                command = new MySqlCommand(query, conn);
                command.CommandText = "SELECT username, password, check_pwd FROM users where username='" + textBox1.Text + "' and password='" + encryptedString + "'";
                reader = command.ExecuteReader();
                if (reader.Read())
                {
                    check_veri.check();
                    if (check_veri.veri == "True")
                    {
                        login_session.logged = false;
                        change_pass change_pass = new change_pass();
                        change_pass.Show();
                        this.Hide();
                    }
                    else
                    {
                        reader.Close();
                        login_session.logged = true;
                        home home = new home();
                        home.Show();
                        this.Hide();
                    }
                }
                else
                {
                    MessageBox.Show("Twoja nazwa użytkownika lub hasło jest nieprawidłowe.");
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bład połączenia z bazą danych...\n" + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        
        private void login_Load(object sender, EventArgs e)
        {

        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode== Keys.Enter)
            {
                e.SuppressKeyPress = true;
                logg();
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
                e.SuppressKeyPress = true;
            }
        }
    }
}
