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
using System.Linq.Expressions;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.ViewerObjectModel;

namespace ClinicAPP_FINAL
{
    public partial class register : Form
    {
        public register()
        {
            InitializeComponent();
        }

        public void v_date_down()
        {
            try
            {
                MySqlConnection conn = connection_config.GetDBConnection();
                string query = "SELECT DATE_FORMAT(hour, '%H:%i') FROM visits WHERE dr_pesel='" + textBox3.Text + "' AND visit_day='" + dateTimePicker1.Text + "'";
                conn.Open();
                MySqlCommand command = new MySqlCommand(query, conn);
                MySqlDataReader reader = command.ExecuteReader();
                liste .Items.Clear();
                while (reader.Read())
                {
                    liste .Items.Add(reader.GetString(0).ToString());
                }
                reader.Close();
                conn.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void lock_sor()
        {
            List<Button> lista = new List<Button> { button1, button2, button3, button4, button5, button6, button7, button8, button9, button10, button11, button12, button13, button14, button15, button16, button17, button18, button19, button20, button21, button22, button23, button24, button25, button26, button27, button28, button29, button30, button31, button32, button33, button34, button35, button36 };

            if (comboBox3.Text == "SOR")
            {
                for (int j = 0; j < 36; j++)
                {
                    lista[j].BackColor = Color.Lavender;
                    lista[j].Enabled = false;
                    comboBox1.Enabled = true;
                }
            }
            else
            {
                comboBox1.SelectedIndex = 0;
                lock_but();
                comboBox1.Enabled = false;
            }
        }

        public void lock_but()
        {
            try
            
            {
                List<Button> lista = new List<Button> { button1, button2, button3, button4, button5, button6, button7, button8, button9, button10, button11, button12, button13, button14, button15, button16, button17, button18, button19, button20, button21, button22, button23, button24, button25, button26, button27, button28, button29, button30, button31, button32, button33, button34, button35, button36 };

                for (int k = 0; k < 36; k++)
                {
                    lista[k].Enabled = true;
                    lista[k].BackColor = Color.WhiteSmoke;
                    lista[k].ForeColor = Color.Black;
                }

                liste.Items.Clear();
                v_date_down();
                int count = liste.Items.Count;
                string tim = null;

                for (int i = 0; i < count; i++)
                {
                    for (int j = 0; j < 36; j++)
                    {
                        //buttons[j] = (Button)Controls["button" + (j + 1)];

                        if (liste.Items[i].ToString() == lista[j].Text)
                        {
                            tim = liste.Items[i].ToString();
                            lista[j].BackColor = Color.Lavender;
                            lista[j].Enabled = false;

                        }
                    }
                }
                v_date_down();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void register_Load(object sender, EventArgs e)
        {
            login_session.log();
            load_wards();
            load_prio();
            dateTimePicker1.CustomFormat = "yyyy-MM-dd";

            List<Button> lista = new List<Button> { button1, button2, button3, button4, button5, button6, button7, button8, button9, button10, button11, button12, button13, button14, button15, button16, button17, button18, button19, button20, button21, button22, button23, button24, button25, button26, button27, button28, button29, button30, button31, button32, button33, button34, button35, button36 };

            for (int j = 0; j < 36; j++)
            {
                lista[j].Click += new System.EventHandler(this.test);
            }
        }

        void test(object sender, EventArgs e)
        {
            Button przycisk = (Button)sender;
            textBox2.Text = przycisk.Text;
        }

        private void load_doctors()
        {
            try
            {
                comboBox2.ResetText();
                comboBox2.Items.Clear();
                int index = comboBox3.SelectedIndex + 1;
                MySqlConnection conn = connection_config.GetDBConnection();
                string query = "SELECT name, surname FROM personel WHERE role_id='1' AND ward_id='" +index+ "'";
                conn.Open();
                MySqlCommand command = new MySqlCommand(query, conn);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    comboBox2.Items.Add(reader.GetString(0) + " " + (reader.GetString(1)));
                }
                conn.Close();
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void load_prio()
        {
            try
            {
                MySqlConnection conn = connection_config.GetDBConnection();
                string query = "SELECT prio FROM priority";
                conn.Open();
                MySqlCommand command = new MySqlCommand(query, conn);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    comboBox1.Items.Add(reader.GetString(0));
                }
                conn.Close();
                reader.Close();
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
                MySqlConnection conn = connection_config.GetDBConnection();
                string query = "SELECT ward FROM hospital_ward";
                conn.Open();
                MySqlCommand command = new MySqlCommand(query, conn);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    comboBox3.Items.Add(reader.GetString(0));
                }
                conn.Close();
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            MySqlConnection conn = connection_config.GetDBConnection();
            string query = "SELECT dr_pesel FROM personel WHERE concat(name,' ',surname) = '" + comboBox2.Text + "'";
            conn.Close();
            conn.Open();
            MySqlCommand command = new MySqlCommand(query, conn);
            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                textBox3.Text = (reader.GetString(0));
            }
            conn.Close();
            v_date_down();
            lock_but();
            lock_sor();
        }

        private void insert()
        {
            DateTime date = Convert.ToDateTime(dateTimePicker1.Text);

            string gun = string.Empty;

            gun = date.ToString("dddd");
            if (gun != "sobota" && gun != "niedziela")
            {
                lock_but();
                lock_sor();
                MySqlConnection conn = connection_config.GetDBConnection();
                conn.Close();
                string query = "SELECT pesel FROM patients WHERE pesel = '" + textBox1.Text + "'";
                conn.Open();
                MySqlCommand command = new MySqlCommand(query, conn);
                MySqlDataReader reader = command.ExecuteReader();

                reader.Close();
                string patient = Convert.ToString(command.ExecuteScalar());

                conn.Close();

                if (patient != textBox1.Text)
                {
                    MessageBox.Show("Nie ma takiego pacjenta!");
                }

                else
                {
                    string query2 = "SELECT hour FROM visits WHERE pesel = '" + textBox1.Text + "'";
                    conn.Open();
                    MySqlCommand command2 = new MySqlCommand(query2, conn);
                    MySqlDataReader reader2 = command.ExecuteReader();

                    reader2.Close();
                    conn.Close();

                    v_date_down();
                    int ward_id = comboBox3.SelectedIndex + 1;
                    int prio_id = comboBox1.SelectedIndex + 1;
                    string query3 = "INSERT INTO visits (dr_pesel,visit_day,pesel,hour, ward_id, prio_id) values('" + textBox3.Text + "','" + dateTimePicker1.Text + "','" + textBox1.Text + "','" + textBox2.Text + "','" + ward_id + "','" + prio_id + "')";
                    conn.Open();
                    MySqlCommand command3 = new MySqlCommand(query3, conn);
                    command3.ExecuteNonQuery();
                    MessageBox.Show("Dodano wizytę!");

                    conn.Close();

                    DateTime dt = DateTime.Parse(dateTimePicker1.Text);
                    start_visit.control_date = dt.ToString("yyyy-MM-dd");
                    start_visit.control_hour = textBox2.Text;

                    v_date_down();
                    lock_but();
                    lock_sor();
                }
            }
            else
            {
                MessageBox.Show("Nie można się umówić w sobotę lub niedzielę");
            }
            v_date_down();
        }

        private void print_op()
        {
            int index = comboBox1.SelectedIndex + 1;

            print_rec print = new print_rec();
            CrystalReport2 cr = new CrystalReport2();
            TextObject visit_id = (TextObject)cr.ReportDefinition.Sections["Section1"].ReportObjects["Text1"];

            BoxObject box = (BoxObject)cr.ReportDefinition.Sections["Section1"].ReportObjects["Box1"];


            if (index == 1) box.FillColor = Color.WhiteSmoke;
            else if (index == 2) box.FillColor = Color.Blue;
            else if (index == 3) box.FillColor = Color.Green;
            else if (index == 4) box.FillColor = Color.Yellow;
            else if (index == 5) box.FillColor = Color.Orange;
            else if (index == 6) box.FillColor = Color.Red;

            string query = "SELECT MAX(id) FROM visits";
            try
            {
                MySqlConnection conn = connection_config.GetDBConnection();
                conn.Open();
                MySqlCommand command = new MySqlCommand(query, conn);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    visit_id.Text=(reader.GetString(0));
                }
                conn.Close();
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            print.crystalReportViewer1.ReportSource = cr;
            print.Show();
        }

        private void button37_Click_1(object sender, EventArgs e)
        {
            if(comboBox3.Text == "SOR")
            {
                if(comboBox1.Text=="" || comboBox2.Text==""|| textBox1.Text == "")
                {
                    MessageBox.Show("Uzupełnij wszystkie pola");
                }
                else
                {
                    insert();
                    print_op();
                }
            }
            else
            {
                if ((textBox1.Text == "") || (textBox2.Text == "") || (comboBox2.Text == ""))
                {
                    MessageBox.Show("Uzupełnij wszystkie pola");
                }
                else
                {
                    insert();
                }
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            try
            {

                DateTime date = Convert.ToDateTime(dateTimePicker1.Text);

                string gun = string.Empty;

                gun = date.ToString("dddd");
                if (gun != "sobota" && gun != "niedziela")
                {
                    lock_but();
                    lock_sor();
                }
                else
                {
                    MessageBox.Show("Nie można się umówić w sobotę lub niedzielę");
                }

            }
            catch { }
        }


        private void button38_Click_2(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            load_doctors();
            lock_but();
            lock_sor();
            if (comboBox3.Text == "SOR")
            {
                dateTimePicker1.Value = DateTime.Now;
                dateTimePicker1.Enabled = false;
            }
            else
            {
                dateTimePicker1.Enabled = true;
            }
        }

        private void register_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
    }
}
