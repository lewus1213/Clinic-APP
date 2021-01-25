using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace ClinicAPP_FINAL
{
    class roles
    {
        public static string rq;
        public static void role(Button button1, Button button2, Button button3, Button button7, ToolStripMenuItem drop_cancel, ToolStripMenuItem start_visit, Button button4, Button button5)
        {
            string role = login_session.role;
            string username = login_session.username;
            string connString = "datasource=192.168.0.108;port=3306;username=root;password=;database=clinicapp";
            MySqlConnection conn = new MySqlConnection(connString);
            MySqlCommand command = conn.CreateCommand();
            string query = "SELECT role FROM user WHERE='" + username + "'";

            if(role=="3") //ordy adm
            {
                rq = "SELECT patients.name AS 'Imię', patients.surname AS 'Nazwisko', patients.pesel AS 'PESEL', personel.name AS 'Imię doktora', personel.surname AS 'Nazwisko doktora', personel.dr_pesel ,visits.visit_day AS 'Data wizyty', visits.hour AS 'Godzina wizyty', personel.phone_nr, visits.prio_id, hospital_ward.id, hospital_ward.ward AS 'Oddział' FROM patients, personel, visits, hospital_ward WHERE hospital_ward.id=visits.ward_id AND patients.pesel=visits.pesel";
               // start_visit.Visible = false;
                button1.Location = new Point(5,100);
                button2.Location = new Point(5, 146);
                button3.Location = new Point(5, 192);
                button7.Location = new Point(5, 238);
                button4.Location = new Point(5, 284);
            }
            else if(role=="2") //piele
            {
                button1.Visible = false;
                button2.Location = new Point(5, 100);
                button3.Location = new Point(5, 146);
                button7.Location = new Point(5, 192);
                button4.Location = new Point(5, 238);
                button5.Visible = false;
                start_visit.Visible = false;
                rq = "SELECT patients.name AS 'Imię', patients.surname AS 'Nazwisko', patients.pesel AS 'PESEL', personel.name AS 'Imię doktora', personel.surname AS 'Nazwisko doktora', personel.dr_pesel ,visits.visit_day AS 'Data wizyty', visits.hour AS 'Godzina wizyty', personel.phone_nr, visits.prio_id, hospital_ward.id, hospital_ward.ward AS 'Oddział' FROM patients, personel, visits, hospital_ward WHERE hospital_ward.id=visits.ward_id AND patients.pesel=visits.pesel";
            }
            else if(role=="1") // dokt
            {
                rq = "SELECT patients.name AS 'Imię', patients.surname AS 'Nazwisko', patients.pesel AS 'PESEL', personel.name AS 'Imię doktora', personel.surname AS 'Nazwisko doktora', personel.dr_pesel ,visits.visit_day AS 'Data wizyty', visits.hour AS 'Godzina wizyty', personel.phone_nr, visits.prio_id, hospital_ward.id, hospital_ward.ward AS 'Oddział' FROM patients, personel, visits, hospital_ward WHERE hospital_ward.id=visits.ward_id AND personel.id ='" + login_session.id + "' AND patients.pesel=visits.pesel";
                button1.Visible = false;
                button2.Visible=false;
                button3.Visible = false;
                drop_cancel.Visible = false;
                button5.Visible = false;
                button7.Location = new Point(5, 100);
                button4.Location = new Point(5, 146);
            }
        }
    }
}
