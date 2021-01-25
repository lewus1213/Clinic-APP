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
    public partial class print_rec : Form
    {
        public print_rec()
        {
            InitializeComponent();
        }

        private void print_rec_Load(object sender, EventArgs e)
        {
            login_session.log();
        }
    }
}
