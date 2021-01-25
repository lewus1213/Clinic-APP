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
    public partial class addpatient : Form
    {
        
        public addpatient()
        {
            InitializeComponent();
        }

        static string query;
        MySqlConnection conn = connection_config.GetDBConnection();
        MySqlCommand command;

        private void button1_Click(object sender, EventArgs e)
        {
            
            string query;
            try
            {
                conn.Open();
                int drcount;
                int count = textBox1.Text.Length;
                int count2 = textBox6.Text.Length;
                query = "SELECT COUNT(*) FROM patients WHERE pesel='" + textBox1.Text + "'";
                command = new MySqlCommand(query, conn);
                drcount = Convert.ToInt32(command.ExecuteScalar());

                if (drcount > 0)
                {
                    MessageBox.Show("Pacjent o podanym numerze PESEL już istnieje w bazie.");
                    conn.Close();
                }

                else
                {
                    if (String.IsNullOrEmpty(textBox1.Text) || String.IsNullOrEmpty(textBox2.Text) || String.IsNullOrEmpty(textBox3.Text) || String.IsNullOrEmpty(comboBox1.Text) || String.IsNullOrEmpty(textBox4.Text) || String.IsNullOrEmpty(comboBox1.Text) || String.IsNullOrEmpty(textBox6.Text))
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

                    else
                    {
                        conn.Close();
                        conn.Open();
                        query = "INSERT INTO patients (id,pesel,name,surname,gender,birth_city,birthday,father_name,mother_name,phone,email,post_code,city,address) values(NULL,'"+ textBox1.Text + "','" + textBox2.Text + "','" + textBox3.Text + "','" + comboBox1.Text + "','" + textBox4.Text + "','" + dateTimePicker1.Text + "','" + textBox11.Text + "','" + textBox10.Text + "','" + textBox6.Text + "','" + textBox8.Text + "','" + textBox7.Text + "','" + textBox9.Text + "','" + textBox5.Text + "')";
                        command = new MySqlCommand(query, conn);
                        command.ExecuteNonQuery();
                        MessageBox.Show("Pomyślnie dodano pacjenta");
                    }
                }

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

        private void button6_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            comboBox1.Text = "";
            textBox6.Text = "";
            textBox8.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
            textBox9.Text = "";
            textBox10.Text = "";
            textBox11.Text = "";
            dateTimePicker1.Text = "";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
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

        private void textBox6_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void textBox6_TextChanged_1(object sender, EventArgs e)
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

            if (len/2==1)
            { 
                sVal = sVal.Replace("-", "");
                string newst = Regex.Replace(sVal, ".{2}", "$0-");
                textBox7.Text = newst;
                textBox7.SelectionStart = textBox7.Text.Length;
            }
        }

        private void textBox7_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void addpatient_Load(object sender, EventArgs e)
        {
            login_session.log();
            dateTimePicker1.CustomFormat = "yyyy-MM-dd";
        }
    }
}

