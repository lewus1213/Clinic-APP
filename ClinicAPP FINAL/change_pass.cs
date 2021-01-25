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
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text.RegularExpressions;


namespace ClinicAPP_FINAL
{
    public partial class change_pass : Form
    {
        public change_pass()
        {
            InitializeComponent();
        }

        public static string email;
        public static string nsur;
        public static string veri;
        public static string pass;
        private void mail_send()
        {

            var fromAddress = new MailAddress("clinicapp.k25@gmail.com", "ClinicAPP");
            var toAddress = new MailAddress(email, nsur);
            const string fromPassword = "Clinicappk25";
            const string subject = "Weryfikacja konta";
            string body = "Twój kod weryfikacyjny to: <b>" + veri + "</b>";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
            {
                smtp.Send(message);
            }
        }

        public static void randomizer()
        {
            Random generator = new Random();
            veri = generator.Next(0, 99999).ToString("D5");
        }

        private void change_pass_Load(object sender, EventArgs e)
        {
            MySqlConnection conn = connection_config.GetDBConnection();
            string query = "SELECT name, surname FROM personel WHERE id='" + login_session.id + "'";
            MySqlCommand command = conn.CreateCommand();
            command.CommandText = query;
            conn.Open();

            try
            {
                MySqlDataReader reader;
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    nsur = " " + reader.GetString("name") + " " + reader.GetString("surname");
                    label2.Text += " " + reader.GetString("name") + " " + reader.GetString("surname");
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

        private void button1_Click(object sender, EventArgs e)
        {

            MySqlConnection conn = connection_config.GetDBConnection();
            string query = "SELECT email FROM personel WHERE id='" + login_session.id + "'";
            MySqlCommand command = conn.CreateCommand();
            command.CommandText = query;
            conn.Open();

            try
            {
                MySqlDataReader reader;
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    email = " " + reader.GetString("email");
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
            password_str.CheckStrength(textBox1.Text);

            if(textBox1.Text != textBox2.Text)
            {
                MessageBox.Show("Podane hasła nie są takie same");
            }

            else if(password_str.score < 3)
            {
                MessageBox.Show("Hasło powinno mieć przynajmniej 8 znaków, w tym jedną cyfrę i wielką literę");
            }

            else
            {
                pass = textBox1.Text;
                randomizer();
                mail_send();
                this.Hide();
                veri veri = new veri();
                veri.Show();
            }


        }
    }
}
