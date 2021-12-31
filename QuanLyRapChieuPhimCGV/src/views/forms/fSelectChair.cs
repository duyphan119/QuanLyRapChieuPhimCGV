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
    public partial class fSelectChair : Form
    {
        public fSelectChair()
        {
            InitializeComponent();
        }

        private void fSelectChair_Load(object sender, EventArgs e)
        {
            int tongSoHang = 10;
            int tongSoCot = 17;

            for (int i = 0; i < tongSoHang; i++)
            {
                for (int j = 0; j < tongSoCot; j++)
                {
                    Button btnChair = new Button();
                    btnChair.Text = "" + (char)(i + 65) + (j + 1);
                    btnChair.Size = new Size(60, 60);

                    fpnlRoom.Controls.Add(btnChair);
                }
            }
        }
    }
}
