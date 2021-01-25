using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClinicAPP_FINAL
{
    public partial class doctor : Form
    {
        public doctor()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            adddoctor adddoctor = new adddoctor();
            adddoctor.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            modifydoctor modifydoctor = new modifydoctor();
            modifydoctor.Show();
            this.Hide();
        }

        private void doctor_Load(object sender, EventArgs e)
        {
            login_session.log();
        }
    }
}
