using QuanLyRapChieuPhimCGV.src.views.forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLyRapChieuPhimCGV.src.models;
using System.Windows.Forms;

namespace QuanLyRapChieuPhimCGV.src.views.usercontrols
{
    public partial class ucBookTicket : UserControl
    {
        private ucAdministration admin;
        private Employee employee;
        public ucBookTicket(ucAdministration uc, Employee e)
        {
            admin = uc;
            employee = e;
            InitializeComponent();
            Dock = DockStyle.Fill;
        }

        private void ucTicket_Load(object sender, EventArgs e)
        {
            
        }

        private void btnSelectChair_Click(object sender, EventArgs e)
        {
            new fSelectChair().Visible = true;
        }
    }
}
