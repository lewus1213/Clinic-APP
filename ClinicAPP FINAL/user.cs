using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace ClinicAPP_FINAL
{
    public partial class user : Form
    {
        static string query, pwd;
        MySqlConnection conn = connection_config.GetDBConnection();
        MySqlCommand command;
        MySqlDataReader reader;

        public user()
        {
            InitializeComponent();
        }

        private void ch_pass()
        {
            
            try
            {
                string pass;
                string query = "SELECT password FROM users WHERE id='" + login_session.id + "';";
                command = new MySqlCommand(query, conn);
                conn.Open();
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    pwd = reader["password"].ToString();
                }
                reader.Close();
                conn.Close();

                if (textBox7.Text == textBox8.Text)
                {
                    pass = pswd.EncryptString(textBox6.Text);

                    if(pwd != pass)
                    {
                        MessageBox.Show("Podane hasło jest niepoprawne");
                        return;
                    }
                    else 
                    {
                        password_str.CheckStrength(textBox7.Text);

                        if(password_str.score<3)
                        {
                            MessageBox.Show("Hasło powinno mieć przynajmniej 8 znaków, w tym jedną cyfrę i wielką literę");
                            return;
                        }

                        else
                        {
                            pass = pswd.EncryptString(textBox7.Text);
                            query = "UPDATE users SET password='" + pass + "' WHERE id='"+login_session.id+"'";
                            conn.Open();
                            command.CommandText = query;
                            command.ExecuteNonQuery();
                            conn.Close();

                            MessageBox.Show("Pomyślnie zmieniono hasło");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Podane hasła nie zgadzają się");
                    return;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void update_profile()
        {
            bool result = mail_valid.IsValidEmail(textBox1.Text);

            if(result)
            {
                if(textBox2.Text.Length < 9)
                {
                    MessageBox.Show("Niepoprawny numer telefonu");
                }
                else if(textBox5.Text.Length < 6)
                {
                    MessageBox.Show("Niepoprawny kod pocztowy");
                }
                else
                {
                    try
                    {
                        query = "UPDATE personel SET phone_nr='" + textBox2.Text + "', email='" + textBox1.Text + "', post_code='" + textBox5.Text + "', city='" + textBox4.Text + "', address='" + textBox3.Text + "' WHERE id='" + login_session.id + "';";
                        conn.Open();
                        command.CommandText = query;
                        command.ExecuteNonQuery();
                        conn.Close();

                        MessageBox.Show("Pomyślnie zaktualizowano dane");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    
                }
            }
            else
            {
                MessageBox.Show("Niepoprawny adres e-mail");
            }
        }

        private void load_data()
        {
            textBox1.Text = login_session.mail;

            try
            {
                string query = "SELECT phone_nr, address, city, post_code FROM personel WHERE id='" + login_session.id + "';";
                command = new MySqlCommand(query, conn);
                conn.Open();
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    textBox2.Text = reader["phone_nr"].ToString();
                    textBox3.Text = reader["address"].ToString();
                    textBox4.Text = reader["city"].ToString();
                    textBox5.Text = reader["post_code"].ToString();
                }
                reader.Close();
                conn.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void user_Load(object sender, EventArgs e)
        {
            login_session.log();
            load_data();
        }

        private void textBox5_KeyDown(object sender, KeyEventArgs e)
        {
            string sVal = textBox5.Text;
            int len = textBox5.Text.Length;

            if (len / 2 == 1)
            {
                sVal = sVal.Replace("-", "");
                string newst = Regex.Replace(sVal, ".{2}", "$0-");
                textBox5.Text = newst;
                textBox5.SelectionStart = textBox5.Text.Length;
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ch_pass();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            update_profile();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }
    }
}
