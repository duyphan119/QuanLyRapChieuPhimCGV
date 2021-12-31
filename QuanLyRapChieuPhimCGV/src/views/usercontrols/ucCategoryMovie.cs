using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLyRapChieuPhimCGV.src.models;

namespace QuanLyRapChieuPhimCGV.src.views.usercontrols
{
    public partial class ucCategoryMovie : UserControl
    {
        private ucAdministration admin;
        private Employee employee;
        public ucCategoryMovie(ucAdministration uc, Employee e)
        {
            admin = uc;
            employee = e;
            InitializeComponent();
            Dock = DockStyle.Fill;
        }
    }
}
