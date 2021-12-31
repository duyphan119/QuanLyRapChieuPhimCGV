using QuanLyRapChieuPhimCGV.src.DAO;
using QuanLyRapChieuPhimCGV.src.models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyRapChieuPhimCGV.src.views.forms
{
    public partial class fChangePassword : Form
    {
        private DAO_Employee dao_e = new DAO_Employee();
        private Employee employee;
        public fChangePassword(Employee emp)
        {
            InitializeComponent();
            employee = emp; //Lấy thông tin nhân viên được truyền từ form trước
        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            string error = dao_e.changePassword(employee, txtOldPassword.Text, txtNewPassword.Text, txtConfirmNewPassword.Text);
            if(error == "")
            {
                Close();
            }
            else
            {
                MessageBox.Show(error, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
