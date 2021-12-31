using QuanLyRapChieuPhimCGV.src.views.forms;
using System;
using System.Windows.Forms;

namespace QuanLyRapChieuPhimCGV
{
    public partial class fLogin : Form
    {
        public fLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            //new fHome(this, null).Visible = true;
            new fNewHome(this, null).Visible = true;
            Visible = false;
        }

        public void login(string username, string password)
        {
            
        }
    }
}
