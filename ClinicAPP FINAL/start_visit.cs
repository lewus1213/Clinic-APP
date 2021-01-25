using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using MySql.Data.MySqlClient;


namespace ClinicAPP_FINAL
{
    public partial class start_visit : Form
    {
        public start_visit()
        {
            InitializeComponent();
        }

        MySqlConnection conn = connection_config.GetDBConnection();
        MySqlCommand command;

        string drphone, query;
        static string code;
        public static string control_date, control_hour, drpesel, ward_id;
        public void load_patient(string name, string surname, string pesel, string data, string hour, string dname, string dsurname, string dnr, string ward)
        {
            
            textBox1.Text = name + " " + surname;
            textBox4.Text = data;
            textBox5.Text = hour;
            textBox11.Text = pesel;
            textBox2.Text = dname + " " + dsurname;
            drphone = dnr;
            ward_id = ward;
        }

        public static void randomizer()
        {
            Random generator = new Random();
            code = generator.Next(0, 9999).ToString("D4");
        }

        private void autofill()
        {
            try
            {
                query = "SELECT name FROM drugs";
                MySqlConnection conn = connection_config.GetDBConnection();
                conn.Open();
                MySqlCommand command = new MySqlCommand(query, conn);
                MySqlDataReader reader = command.ExecuteReader();
                AutoCompleteStringCollection autotext = new AutoCompleteStringCollection();

                while (reader.Read())
                {
                    autotext.Add(reader.GetString(0));
                }
                textBox8.AutoCompleteMode = AutoCompleteMode.Suggest;
                textBox8.AutoCompleteSource = AutoCompleteSource.CustomSource;
                textBox8.AutoCompleteCustomSource = autotext;

                conn.Close();
                reader.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void start_visit_Load(object sender, EventArgs e)
        {
            login_session.log();
            this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            autofill();


        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (dataGridView1.Rows.Count == 5)
            {
                MessageBox.Show("Można zapisać maksymalnie 5 lektów na jedną receptę");
            }
            else if (textBox8.Text == "" || textBox10.Text == "")
            {
                MessageBox.Show("Nie wpisano leku lub dakowania");
            }
            else
            {
                try
                {
                    query = "INSERT INTO drugs (id, name) VALUES (NULL,'" + textBox8.Text + "')";
                    MySqlConnection conn = connection_config.GetDBConnection();
                    conn.Open();
                    MySqlCommand command = new MySqlCommand(query, conn);
                    command.ExecuteNonQuery();

                    conn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                dataGridView1.Rows.Add(textBox8.Text, textBox10.Text);
                textBox8.Clear();
                textBox10.Clear();
            }
        }

        private void listBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Show(Cursor.Position.X, Cursor.Position.Y);
            }
            
        }

        private void drop_delete_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow r in dataGridView1.SelectedRows)
            {
                if (!r.IsNewRow)
                {
                    dataGridView1.Rows.RemoveAt(r.Index);
                }
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
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                contextMenuStrip1.Show(Cursor.Position.X, Cursor.Position.Y);
            }
        }

        private void start_visit_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            register register = new register();
            register.Show();
            register.comboBox2.SelectedText = textBox2.Text;
            register.textBox1.Text = textBox11.Text;
            register.textBox3.Text = drpesel;
            register.textBox1.Enabled = false;
        }

        public void button2_Click(object sender, EventArgs e)
        {
            string[] rec = new string[5] { "", "", "", "", "" };
            string[] dos = new string[5] { "", "", "", "", "" };


            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                rec[row.Index] = row.Cells[0].Value.ToString().Trim();
            }

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                dos[row.Index] = row.Cells[1].Value.ToString().Trim();
            }

            string result = string.Join(",", rec.Where(s => !string.IsNullOrEmpty(s)));

            print_rec print = new print_rec();

            if (textBox3.Text == "" || textBox6.Text == "" || textBox7.Text == "" || textBox5.Text == "")
            {
                MessageBox.Show("Uzupełnij wszystkie pola");
            }
            else
            {
                if (dataGridView1.Rows.Count == 0)
                {
                    //archiwizacja !!!
                    query = "INSERT INTO archive SELECT NULL, dr_pesel, visit_day, pesel, hour, NULL, ward_id FROM visits WHERE pesel = '" + textBox11.Text + "' AND visit_day='" + textBox4.Text + "' AND hour='" + textBox5.Text + "'; " +
                        "SET @id = LAST_INSERT_ID(); " +
                        "INSERT INTO file(id, interview, examination, diagnosis, drugs, kat_id) VALUES(NULL, '" + textBox3.Text + "', '" + textBox6.Text + "', '" + textBox7.Text + "', '" + result + "', @id);" +
                        " DELETE FROM visits WHERE pesel = '" + textBox11.Text + "' AND visit_day='" + textBox4.Text + "' AND hour='" + textBox5.Text + "';";

                    conn.Open();

                    command = new MySqlCommand(query, conn);
                    command.ExecuteNonQuery();
                    MessageBox.Show("Pomyślnie zakończono wizytę");
                    conn.Close();

                    this.Close();
                }
                else
                {
                    randomizer();
                    CrystalReport1 cr = new CrystalReport1();
                    TextObject patient = (TextObject)cr.ReportDefinition.Sections["Section3"].ReportObjects["Text10"];
                    TextObject doctor = (TextObject)cr.ReportDefinition.Sections["Section3"].ReportObjects["Text11"];
                    TextObject drph = (TextObject)cr.ReportDefinition.Sections["Section3"].ReportObjects["Text12"];
                    TextObject datatime = (TextObject)cr.ReportDefinition.Sections["Section3"].ReportObjects["Text13"];
                    TextObject r1 = (TextObject)cr.ReportDefinition.Sections["Section3"].ReportObjects["Text14"];
                    TextObject r2 = (TextObject)cr.ReportDefinition.Sections["Section3"].ReportObjects["Text28"];
                    TextObject r3 = (TextObject)cr.ReportDefinition.Sections["Section3"].ReportObjects["Text30"];
                    TextObject r4 = (TextObject)cr.ReportDefinition.Sections["Section3"].ReportObjects["Text32"];
                    TextObject r5 = (TextObject)cr.ReportDefinition.Sections["Section3"].ReportObjects["Text34"];
                    TextObject d1 = (TextObject)cr.ReportDefinition.Sections["Section3"].ReportObjects["Text15"];
                    TextObject d2 = (TextObject)cr.ReportDefinition.Sections["Section3"].ReportObjects["Text27"];
                    TextObject d3 = (TextObject)cr.ReportDefinition.Sections["Section3"].ReportObjects["Text29"];
                    TextObject d4 = (TextObject)cr.ReportDefinition.Sections["Section3"].ReportObjects["Text31"];
                    TextObject d5 = (TextObject)cr.ReportDefinition.Sections["Section3"].ReportObjects["Text33"];
                    TextObject rec_code = (TextObject)cr.ReportDefinition.Sections["Section3"].ReportObjects["Text3"];
                    r1.Text = rec[0];
                    r2.Text = rec[1];
                    r3.Text = rec[2];
                    r4.Text = rec[3];
                    r5.Text = rec[4];
                    d1.Text = dos[0];
                    d2.Text = dos[1];
                    d3.Text = dos[2];
                    d4.Text = dos[3];
                    d5.Text = dos[4];
                    rec_code.Text = code;
                    patient.Text = textBox1.Text;
                    doctor.Text = textBox2.Text;
                    drph.Text = "+48 " + drphone;
                    DateTime dt = DateTime.Parse(textBox5.Text);
                    textBox5.Text = dt.ToString("HH:mm");
                    datatime.Text = control_date + " godz. " + control_hour;
                    print.crystalReportViewer1.ReportSource = cr;

                    query = "INSERT INTO archive SELECT NULL, dr_pesel, visit_day, pesel, hour, NULL, ward_id FROM visits WHERE pesel = '" + textBox11.Text + "' AND visit_day='" + textBox4.Text + "' AND hour='" + textBox5.Text + "'; " +
                        "SET @id = LAST_INSERT_ID(); " +
                        "UPDATE archive SET rec_code='" + code + "' WHERE id=@id;" +
                        "INSERT INTO file(id, interview, examination, diagnosis, drugs, kat_id) VALUES(NULL, '" + textBox3.Text + "', '" + textBox6.Text + "', '" + textBox7.Text + "', '" + result + "', @id);" +
                        " DELETE FROM visits WHERE pesel = '" + textBox11.Text + "' AND visit_day='" + textBox4.Text + "' AND hour='" + textBox5.Text + "';";

                    conn.Open();

                    command = new MySqlCommand(query, conn);
                    command.ExecuteNonQuery();
                    MessageBox.Show("Pomyślnie zakończono wizytę");
                    conn.Close();

                    this.Close();
                    print.Show();

                    code = "";
                }
            }
            

            
            
        }
    }
}
