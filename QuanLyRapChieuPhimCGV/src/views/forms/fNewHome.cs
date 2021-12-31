using QuanLyRapChieuPhimCGV.src.models;
using QuanLyRapChieuPhimCGV.src.views.usercontrols;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyRapChieuPhimCGV.src.views.forms
{
    public partial class fNewHome : Form
    {
        private fLogin preComponent;
        private Employee employee;
        private Button btn_Clicked;
        public fNewHome(fLogin f, Employee emp)
        {
            InitializeComponent();
            preComponent = f;
            employee = emp;
            pnlView.Controls.Add(new ucHome());
            btn_Clicked = btnHome;
        }

        private void fNewHome_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Bạn có chắc chắn thoát ?", "Câu hỏi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            e.Cancel = dialogResult == DialogResult.No;
            if(dialogResult == DialogResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                preComponent.Close();
            }
        }

        public void activeBtn(object sender)
        {
            btn_Clicked.BackColor = Color.FromArgb(192, 192, 255);
            Button thisBtn = sender as Button;
            btn_Clicked = thisBtn;
            thisBtn.BackColor = Color.White;
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            activeBtn(sender);
            pnlView.Controls.Clear();
            pnlView.Controls.Add(new ucHome());
        }

        private void btnBookTicket_Click(object sender, EventArgs e)
        {
            activeBtn(sender);
            pnlView.Controls.Clear();
            pnlView.Controls.Add(new ucBookTicket(null, employee));
        }

        private void btnAdministration_Click(object sender, EventArgs e)
        {
            activeBtn(sender);
            pnlView.Controls.Clear();
            pnlView.Controls.Add(new ucAdministration(employee));
        }
    }
}
