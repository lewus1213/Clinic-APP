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
    public partial class kart : Form
    {
        public kart()
        {
            InitializeComponent();
        }

        MySqlConnection conn = connection_config.GetDBConnection();
        MySqlCommand command;
        MySqlDataAdapter da = new MySqlDataAdapter();
        DataTable dt = new DataTable();
        DataTable dt2 = new DataTable();
        MySqlDataReader reader;
        string query, patient_id;
        private void get_patients()
        {
            
            string today = DateTime.Now.ToShortDateString();

            try
            {
                conn.Open();
                dt.Clear();
                query = "SELECT pesel AS 'PESEL', name AS 'Imię', surname AS 'Nazwisko', id, birthday FROM patients";
                command = new MySqlCommand(query, conn);
                da.SelectCommand = command;
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                dataGridView1.AllowUserToAddRows = false;
                this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.Columns[3].Visible = false;
                dataGridView1.Columns[4].Visible = false;
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

        private void kart_Load(object sender, EventArgs e)
        {
            get_patients();
        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            string name, surname, pesel, birth;
            if (e.Button == MouseButtons.Left)
            {
                try
                {
                    textBox2.Text = "";
                    textBox3.Text = "";
                    textBox4.Text = "";
                    numericUpDown1.Value = default;
                    numericUpDown2.Value = default;
                    label4.Text = "Data urodzenia:";

                    dataGridView1.CurrentCell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    dataGridView1.Rows[e.RowIndex].Selected = true;
                    dataGridView1.Focus();
                    pesel = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells[0].Value);

                    conn.Open();
                    dt2.Clear();
                    query = "SELECT  hospital_ward.ward AS 'Oddział', archive.visit_day AS 'Data wizyty', file.diagnosis AS 'Postawiona diagnoza', file.drugs AS 'Zapisane leki'FROM archive, file, hospital_ward WHERE archive.pesel='" + pesel + "' AND archive.id = file.kat_id AND hospital_ward.id=archive.ward_id;";
                    command = new MySqlCommand(query, conn);
                    da.SelectCommand = command;
                    da.Fill(dt2);
                    dataGridView2.DataSource = dt2;
                    dataGridView2.AllowUserToAddRows = false;
                    this.dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    patient_id = Convert.ToString(dataGridView1.SelectedRows[0].Cells[3].Value);
                    DateTime dat1 = Convert.ToDateTime(dataGridView1.SelectedRows[0].Cells[4].Value);
                    label4.Text += dat1.ToString("dd-MM-yyyy");

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
                try
                {
                    string query = "SELECT allergy, chronic, taken_drugs, weight, height FROM health WHERE patient_id = '" + patient_id + "'";
                    command = new MySqlCommand(query, conn);
                    conn.Open();
                    reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        textBox4.Text = reader["allergy"].ToString();
                        textBox3.Text = reader["chronic"].ToString();
                        textBox2.Text = reader["taken_drugs"].ToString();
                        numericUpDown1.Value = Convert.ToInt32(reader["height"].ToString());
                        numericUpDown2.Value = Convert.ToInt32(reader["weight"].ToString());
                    }
                    reader.Close();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                dt.Clear();
                query = "SELECT pesel AS 'PESEL', name AS 'Imię', surname AS 'Nazwisko' FROM patients WHERE pesel LIKE '" + textBox1.Text + "%'";

                command = new MySqlCommand(query, conn);
                da.SelectCommand = command;
                da.Fill(dt);
                dataGridView1.DataSource = dt;
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

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                query = "INSERT INTO health (id, allergy, chronic, taken_drugs, weight, height, patient_id) VALUES(NULL,'" + textBox4.Text + "','" + textBox3.Text + "','" + textBox2.Text + "','" + numericUpDown2.Value + "','" + numericUpDown1.Value + "','" + patient_id + "')" +
                "ON DUPLICATE KEY UPDATE allergy='" + textBox4.Text + "', chronic='" + textBox3.Text + "',taken_drugs='" + textBox2.Text + "', weight='" + numericUpDown2.Value + "', height='" + numericUpDown1.Value + "';";
                command = new MySqlCommand(query, conn);
                command.CommandText = query;
                command.ExecuteNonQuery();

                MessageBox.Show("Pomyślnie zaktualizowano kartę zdrowia pacjenta");
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
    }
}
