using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;

namespace ClinicAPP_FINAL
{
    public partial class adddoctor : Form
    {
        public adddoctor()
        {
            InitializeComponent();
        }

        static string query;
        MySqlConnection conn = connection_config.GetDBConnection();
        MySqlCommand command;

        private void button1_Click_1(object sender, EventArgs e)
        {
            bool result = mail_valid.IsValidEmail(textBox8.Text);

            if(result)
            {
                string query;
                try
                {
                    conn.Open();
                    int drcount;
                    int count = textBox1.Text.Length;
                    int count2 = textBox6.Text.Length;
                    query = "SELECT COUNT(*) FROM personel WHERE dr_pesel='" + textBox1.Text + "'";
                    command = new MySqlCommand(query, conn);
                    drcount = Convert.ToInt32(command.ExecuteScalar());

                    conn.Close();
                    conn.Open();

                    query = "SELECT COUNT(*) FROM users WHERE username='" + textBox5.Text + "'";
                    command.CommandText = query;
                    int account = Convert.ToInt32(command.ExecuteScalar());

                    if (drcount > 0)
                    {
                        MessageBox.Show("Lekarz o podanym numerze PESEL już istnieje w bazie.");
                        conn.Close();
                    }

                    else
                    {
                        if (String.IsNullOrEmpty(textBox1.Text) || String.IsNullOrEmpty(comboBox3.Text) || String.IsNullOrEmpty(textBox2.Text) || String.IsNullOrEmpty(textBox3.Text) || String.IsNullOrEmpty(comboBox1.Text) || String.IsNullOrEmpty(comboBox2.Text) || String.IsNullOrEmpty(textBox4.Text) || String.IsNullOrEmpty(textBox6.Text) || String.IsNullOrEmpty(textBox10.Text) || String.IsNullOrEmpty(textBox7.Text) || String.IsNullOrEmpty(textBox9.Text) || String.IsNullOrEmpty(textBox5.Text) || String.IsNullOrEmpty(textBox8.Text))
                        {
                            MessageBox.Show("Uzupełnij wszystkie pola");
                            return;
                        }

                        else if (count < 11)
                        {
                            MessageBox.Show("Niepoprawny PESEL");
                        }
                        else if (count2 < 9)
                        {
                            MessageBox.Show("Niepoprawny numer telefonu");
                        }

                        else if (account > 0)
                        {
                            MessageBox.Show("Użytkownik o tej nazwie już istnieje");
                            conn.Close();
                        }

                        else
                        {
                            conn.Close();
                            conn.Open();
                            int role_id = comboBox2.SelectedIndex + 1;
                            int ward = comboBox3.SelectedIndex + 1;
                            string pass = "";
                            var encryptedString = pswd.EncryptString(pass);
                            query = "INSERT INTO personel (id,dr_pesel,name,surname,gender,birth_city,birthday,phone_nr,email,address,post_code,city, role_id, ward_id) values(NULL,'" + textBox1.Text + "','" + textBox2.Text + "','" + textBox3.Text + "','" + comboBox1.Text + "' , '" + textBox4.Text + "','" + dateTimePicker1.Text + "','" + textBox6.Text + "','" + textBox8.Text + "','" + textBox10.Text + "','" + textBox7.Text + "','" + textBox9.Text + "','" + role_id + "','" + ward + "'); INSERT INTO users (id, username, password, role_id, check_pwd) VALUES (NULL, '" + textBox5.Text + "','" + encryptedString + "','" + role_id + "','" + 1 + "')";
                            command = new MySqlCommand(query, conn);
                            command.ExecuteNonQuery();
                            MessageBox.Show("Pomyślnie dodano nowy personel");
                        }
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Żadne zadanie nie zostało wykonane");
                }
                finally
                {
                    conn.Close();
                }
            }
            else
            {
                MessageBox.Show("Niepoprawny adres e-mail");
            }
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            comboBox1.Text = "";
            comboBox2.Text = "";
            comboBox3.Text = "";
            textBox6.Text = "";
            textBox8.Text = "";
            dateTimePicker1.Text = "";
            textBox10.Text = "";
            textBox7.Text = "";
            textBox9.Text = "";
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(textBox1.Text, "  ^ [0-9]"))
            {
                textBox1.Text = "";
            }
        }

        private void textBox1_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(textBox1.Text, "  ^ [0-9]"))
            {
                textBox1.Text = "";
            }
        }

        private void textBox7_KeyDown(object sender, KeyEventArgs e)
        {
            string sVal = textBox7.Text;
            int len = textBox7.Text.Length;

            if (len / 2 == 1)
            {
                sVal = sVal.Replace("-", "");
                string newst = Regex.Replace(sVal, ".{2}", "$0-");
                textBox7.Text = newst;
                textBox7.SelectionStart = textBox7.Text.Length;
            }
        }

        private void load_roles()
        {
            try
            {
                string query = "SELECT role FROM role";
                conn.Open();
                MySqlCommand command = new MySqlCommand(query, conn);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    comboBox2.Items.Add(reader.GetString(0));
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void load_wards()
        {
            try
            {
                string query = "SELECT ward FROM hospital_ward";
                conn.Open();
                MySqlCommand command = new MySqlCommand(query, conn);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    comboBox3.Items.Add(reader.GetString(0));
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void adddoctor_Load(object sender, EventArgs e)
        {
            login_session.log();
            load_roles();
            load_wards();
            dateTimePicker1.CustomFormat = "yyyy-MM-dd";
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox5.Text = textBox2.Text + "_" + textBox3.Text;
            textBox5.CharacterCasing = CharacterCasing.Lower;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            textBox5.Text = textBox2.Text + "_" + textBox3.Text;
            textBox5.CharacterCasing = CharacterCasing.Lower;
        }

        private void textBox7_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }
    }
}