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
using System.Globalization;

namespace ClinicAPP_FINAL
{
    public partial class home : Form
    {
        public home()
        {
            InitializeComponent();
        }

        string name, surname, pesel, data, hour, dname, dsurname, dnr, ward, query;

        MySqlConnection conn = connection_config.GetDBConnection();
        MySqlCommand command;
        MySqlDataAdapter da = new MySqlDataAdapter();
        DataTable dt = new DataTable();

        private void load_data()
        {

            string query;

            try
            {
                roles.role(button1, button2, button3, button7, drop_cancel, drop_visit, button4, button5);
                conn.Open();
                dt.Clear();
                query = roles.rq+=" AND personel.dr_pesel=visits.dr_pesel AND visits.visit_day='" + dateTimePicker1.Text + "'ORDER BY visits.prio_id DESC";
                command = new MySqlCommand(query, conn);
                da.SelectCommand = command;
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                dataGridView1.AllowUserToAddRows = false;
                this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.Columns[6].DefaultCellStyle.Format = "yyyy-MM-dd";
                dataGridView1.Columns[8].Visible = false;
                dataGridView1.Columns[5].Visible = false;
                dataGridView1.Columns[10].Visible = false;
                dataGridView1.Columns[9].HeaderText = "";
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

        private void button1_Click_1(object sender, EventArgs e)
        {
           doctor doctor = new doctor();
           doctor.Show();

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            patient patient = new patient();
            patient.Show();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            register register = new register();
            register.FormClosing += new FormClosingEventHandler(ChildFormClosing);
            register.Show();
        }

        private void home_Load(object sender, EventArgs e)
        {
            login_session.log();
            label2.Text +=login_session.name + " " + login_session.surname;
            dateTimePicker1.CustomFormat = "yyyy-MM-dd";
            load_data();
            dataGridView1.Columns[9].Width = 36;
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            user user = new user();
            user.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            kart kart = new kart();
            kart.Show();
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            var folderBrowserDialog1 = new FolderBrowserDialog();

            DialogResult result = folderBrowserDialog1.ShowDialog();
            if(result == DialogResult.OK)
            {
                string folder = folderBrowserDialog1.SelectedPath;

                try
                {
                    DateTime dt = DateTime.Now;
                    command = new MySqlCommand();
                    MySqlBackup mb = new MySqlBackup(command);
                    command.Connection = conn;
                    conn.Open();
                    mb.ExportToFile(folder + "\\ClinicAPP db backup-" + dt.ToString("yyyy-MM-dd") + ".sql");
                    conn.Close();
                    MessageBox.Show("Pomyślnie utworzono kopię zapasową bazy.");
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 9 && e.Value != null)
            {
                int prio = Convert.ToInt32(e.Value);

                if (prio == 6)
                {
                    e.CellStyle.BackColor = Color.Red;
                    e.CellStyle.ForeColor = Color.Red;
                    e.CellStyle.SelectionBackColor = Color.Red;
                    e.Value = "";
                }
                else if (prio == 5)
                {
                    e.CellStyle.BackColor = Color.Orange;
                    e.CellStyle.ForeColor = Color.Orange;
                    e.CellStyle.SelectionBackColor = Color.Orange;
                    e.Value = "";
                }
                else if (prio == 4)
                {
                    e.CellStyle.BackColor = Color.Yellow;
                    e.CellStyle.ForeColor = Color.Yellow;
                    e.CellStyle.SelectionBackColor = Color.Yellow;
                    e.Value = "";
                }
                else if (prio == 3)
                {
                    e.CellStyle.BackColor = Color.Green;
                    e.CellStyle.ForeColor = Color.Green;
                    e.CellStyle.SelectionBackColor = Color.Green;
                    e.Value = "";
                }
                else if (prio == 2)
                {
                    e.CellStyle.BackColor = Color.Blue;
                    e.CellStyle.ForeColor = Color.Blue;
                    e.CellStyle.SelectionBackColor = Color.Blue;
                    e.Value = "";
                }
                else if (prio == 1) e.Value = "";
            }
            else if (e.ColumnIndex == 7 && e.Value != null)
            {
                string tim = Convert.ToString(e.Value);

                if (tim == "00:00:00") e.Value = "";
            }
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            string query;
            try
            {
                roles.role(button1, button2, button3, button7, drop_cancel, drop_visit, button4, button5);
                conn.Open();
                dt.Clear();
                query = roles.rq+=" AND personel.dr_pesel=visits.dr_pesel AND visits.pesel LIKE '" + textBox1.Text + "%'  ORDER BY visits.prio_id DESC";
                command = new MySqlCommand(query, conn);
                da.SelectCommand = command;
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                dataGridView1.AllowUserToAddRows = false;
                this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

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

        private void dateTimePicker1_ValueChanged_1(object sender, EventArgs e)
        {
            string query;
            try
            {
                roles.role(button1, button2, button3, button7, drop_cancel, drop_visit, button4, button5);
                conn.Open();
                dt.Clear();
                query = roles.rq+=" AND personel.dr_pesel=visits.dr_pesel AND visits.visit_day='" + dateTimePicker1.Text + "%' ORDER BY prio_id DESC";
                command = new MySqlCommand(query, conn);
                da.SelectCommand = command;

                da.Fill(dt);

                dataGridView1.DataSource = dt;
                dataGridView1.AllowUserToAddRows = false;
                this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
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

        private void home_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Show(Cursor.Position.X, Cursor.Position.Y);
            }
        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                try
                {
                    dataGridView1.CurrentCell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    dataGridView1.Rows[e.RowIndex].Selected = true;
                    dataGridView1.Focus();
                    name = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
                    surname = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells[1].Value);
                    pesel = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells[2].Value);
                    data = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells[6].Value);
                    var parsedate = DateTime.Parse(data);
                    data = parsedate.ToString("yyyy-MM-dd");
                    
                    hour = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells[7].Value);
                    var parsehour = DateTime.Parse(hour);
                    hour = parsehour.ToString("HH:mm");

                    dname = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells[3].Value);
                    dsurname = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells[4].Value);
                    dnr = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells[8].Value);
                    ward = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells[10].Value);
                    start_visit.drpesel = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells[5].Value);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }

        private void ChildFormClosing(object sender, FormClosingEventArgs e)
        {
            load_data();
        }

        private void drop_visit_Click(object sender, EventArgs e)
        {
            start_visit start_visit = new start_visit();
            start_visit.FormClosing += new FormClosingEventHandler(ChildFormClosing);
            start_visit.Show();
            start_visit.load_patient(name, surname, pesel, data, hour, dname, dsurname,dnr, ward);

        }

        private void drop_cancel_Click_1(object sender, EventArgs e)
        {
            try
            {
                string pesel = Convert.ToString(dataGridView1.SelectedRows[0].Cells[2].Value);
                string hour = Convert.ToString(dataGridView1.SelectedRows[0].Cells[7].Value);
                string day = Convert.ToString(dataGridView1.SelectedRows[0].Cells[6].Value);
                var parsedate = DateTime.Parse(day);
                string dat = parsedate.ToString("yyyy-MM-dd");
                string query = "DELETE FROM visits WHERE pesel='" + pesel + "' AND hour='" + hour + "' AND visit_day='" + dat + "'";
                command = new MySqlCommand(query, conn);
                conn.Open();
                command.ExecuteNonQuery();
                MessageBox.Show("Pomyślnie odwołano wizytę");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd");
            }
            finally
            {
                conn.Close();
                dt.Clear();
                load_data();
            }
        }
    }
}