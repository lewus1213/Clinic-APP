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
using MySqlX.XDevAPI;
using System.Text.RegularExpressions;

namespace ClinicAPP_FINAL
{
    public partial class modifypatient : Form
    {
        public modifypatient()
        {
            InitializeComponent();
        }

        public void listpatient()
        {
            MySqlConnection conn = connection_config.GetDBConnection();
            MySqlCommand command = conn.CreateCommand();
            MySqlDataAdapter da = new MySqlDataAdapter();
            DataTable dt = new DataTable();
            string query;
            string today = DateTime.Now.ToShortDateString();

            try
            {
                conn.Open();
                dt.Clear();
                query = "SELECT id,pesel AS 'PESEL',name AS 'Imię',surname AS 'Nazwisko',gender AS 'Płeć',birth_city AS 'Miejsce urodzenia',birthday AS 'Data urodzenia',father_name AS 'Imię ojca',mother_name AS 'Imię matki',phone AS 'Numer telefonu',email AS 'E-mail',post_code AS 'Kod pocztowy',city AS 'Miasto',address AS 'Adres' FROM patients";
                command.Connection = conn;
                command.CommandText = query;

                da = new MySqlDataAdapter();
                da.SelectCommand = command;
                dt = new DataTable();

                da.Fill(dt);

                dataGridView1.DataSource = dt;
                dataGridView1.AllowUserToAddRows = false;
                this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[4].Visible = false;
                dataGridView1.Columns[5].Visible = false;
                dataGridView1.Columns[6].Visible = false;
                dataGridView1.Columns[7].Visible = false;
                dataGridView1.Columns[8].Visible = false;
                dataGridView1.Columns[9].Visible = false;
                dataGridView1.Columns[10].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                da.Dispose();
                conn.Close();
            }
        }

        private void modifypatient_Load(object sender, EventArgs e)
        {
            login_session.log();
            listpatient();
            dateTimePicker1.CustomFormat = "yyyy-MM-dd";
        }

        public void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                string date = row.Cells[6].Value.ToString();
                DateTime dateValue = DateTime.Parse(date);
                int year = dateValue.Year;
                int month = dateValue.Month;
                int day = dateValue.Day;

                textBox5.Text = row.Cells[0].Value.ToString();
                textBox1.Text = row.Cells[1].Value.ToString();
                textBox2.Text = row.Cells[2].Value.ToString();
                textBox3.Text = row.Cells[3].Value.ToString();
                comboBox1.Text = row.Cells[4].Value.ToString();
                textBox4.Text = row.Cells[5].Value.ToString();
                dateTimePicker1.Value = new DateTime(year, month, day);
                textBox6.Text = row.Cells[9].Value.ToString();
                textBox7.Text = row.Cells[11].Value.ToString();
                textBox8.Text = row.Cells[10].Value.ToString();
                textBox9.Text = row.Cells[12].Value.ToString();
                textBox10.Text = row.Cells[13].Value.ToString();
                textBox11.Text = row.Cells[7].Value.ToString();
                textBox12.Text = row.Cells[8].Value.ToString();
            }
        }

        public void button1_Click(object sender, EventArgs e)
        {
            MySqlConnection conn = connection_config.GetDBConnection();
            MySqlCommand command = conn.CreateCommand();
            string query;
            try
            {
                conn.Open();
                int ptcount;
                int count = textBox1.Text.Length;
                int count2 = textBox6.Text.Length;
                query = "SELECT COUNT(*) FROM patients WHERE pesel='" + textBox1.Text + "'";
                command.CommandText = query;
                ptcount = Convert.ToInt32(command.ExecuteScalar());
                conn.Close();

                conn.Open();
                string test;
                query = "SELECT pesel FROM patients WHERE id=" + textBox5.Text;
                command.CommandText = query;
                test = Convert.ToString(command.ExecuteScalar());
                conn.Close();

                if (textBox1.Text == test)
                {
                    if (String.IsNullOrEmpty(textBox1.Text) || String.IsNullOrEmpty(textBox2.Text) || String.IsNullOrEmpty(textBox3.Text) || String.IsNullOrEmpty(comboBox1.Text) || String.IsNullOrEmpty(textBox6.Text) || String.IsNullOrEmpty(textBox4.Text) || String.IsNullOrEmpty(textBox10.Text) || String.IsNullOrEmpty(textBox7.Text) || String.IsNullOrEmpty(textBox9.Text) || String.IsNullOrEmpty(textBox12.Text) || String.IsNullOrEmpty(textBox11.Text) || String.IsNullOrEmpty(textBox6.Text))
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
                        conn.Open();
                        query = "UPDATE patients SET pesel='" + textBox1.Text + "',name='" + textBox2.Text + "',surname='" + textBox3.Text + "',gender='" + comboBox1.Text + "',birth_city='" + textBox4.Text + "',address='" + textBox10.Text + "',post_code='" + textBox7.Text + "',city='" + textBox9.Text + "',birthday='" + dateTimePicker1.Text + "',mother_name='" + textBox12.Text + "',father_name='" + textBox11.Text + "',phone='" + textBox6.Text + "', email='" + textBox8.Text + "' WHERE id='"+ textBox5.Text+"'";
                        command.CommandText = query;
                        command.ExecuteNonQuery();
                        conn.Close();
                        MessageBox.Show("Pomyślnie zaktualizowano dane");
                    }
                }

                else if (ptcount > 0)
                {
                    MessageBox.Show("Pacjent o podanym numerze PESEL już istnieje w bazie.");
                    listpatient();
                }
                else
                {
                    if (String.IsNullOrEmpty(textBox1.Text) || String.IsNullOrEmpty(textBox2.Text) || String.IsNullOrEmpty(textBox3.Text) || String.IsNullOrEmpty(comboBox1.Text) || String.IsNullOrEmpty(textBox6.Text) || String.IsNullOrEmpty(textBox4.Text) || String.IsNullOrEmpty(textBox10.Text) || String.IsNullOrEmpty(textBox7.Text) || String.IsNullOrEmpty(textBox9.Text) || String.IsNullOrEmpty(textBox12.Text) || String.IsNullOrEmpty(textBox11.Text) || String.IsNullOrEmpty(textBox6.Text))
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
                        conn.Open();
                        query = "UPDATE patients SET pesel='" + textBox1.Text + "',name='" + textBox2.Text + "',surname='" + textBox3.Text + "',gender='" + comboBox1.Text + "',birth_city='" + textBox4.Text + "',address='" + textBox10.Text + "',post_code='" + textBox7.Text + "',city='" + textBox9.Text + "',birthday='" + dateTimePicker1.Text + "',mother_name='" + textBox12.Text + "',father_name='" + textBox11.Text + "',phone='" + textBox6.Text + "', email='" + textBox8.Text + "' WHERE id='" + textBox5.Text + "'";
                        command.CommandText = query;
                        command.ExecuteNonQuery();
                        conn.Close();
                        MessageBox.Show("Pomyślnie zaktualizowano dane");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nie dokonano żadnych zmian");
            }
            finally
            {
                conn.Close();
                listpatient();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MySqlConnection conn = connection_config.GetDBConnection();
            MySqlCommand command = conn.CreateCommand();
            string query;

            try
            {
                conn.Open();
                query = "DELETE FROM patients WHERE id='" + textBox5.Text + "'";
                command.CommandText = query;
                command.ExecuteNonQuery();
                MessageBox.Show("Pomyślnie usunięto pacjenta");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
                listpatient();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(textBox1.Text, "  ^ [0-9]"))
            {
                textBox1.Text = "";
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(textBox1.Text, "  ^ [0-9]"))
            {
                textBox1.Text = "";
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
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

        private void textBox13_TextChanged(object sender, EventArgs e)
        {
            MySqlConnection conn = connection_config.GetDBConnection();
            MySqlCommand command = conn.CreateCommand();
            MySqlDataAdapter da = new MySqlDataAdapter();
            DataTable dt = new DataTable();
            string query;
            string today = DateTime.Now.ToShortDateString();
            try
            {
                conn.Open();
                dt.Clear();
                query = "SELECT id,pesel,name,surname,gender,birth_city,birthday,father_name,mother_name,phone,email,post_code,city,address FROM patients WHERE pesel LIKE '" + textBox13.Text + "%'";
                command.Connection = conn;
                command.CommandText = query;

                da = new MySqlDataAdapter();
                da.SelectCommand = command;
                dt = new DataTable();

                da.Fill(dt);

                dataGridView1.DataSource = dt;
                dataGridView1.AllowUserToAddRows = false;
                this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[4].Visible = false;
                dataGridView1.Columns[5].Visible = false;
                dataGridView1.Columns[6].Visible = false;
                dataGridView1.Columns[7].Visible = false;
                dataGridView1.Columns[8].Visible = false;
                dataGridView1.Columns[9].Visible = false;
                dataGridView1.Columns[10].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                da.Dispose();
                conn.Close();
            }

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
