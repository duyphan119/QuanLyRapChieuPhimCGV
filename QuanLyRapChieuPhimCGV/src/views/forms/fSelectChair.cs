using QuanLyRapChieuPhimCGV.src.DAO;
using QuanLyRapChieuPhimCGV.src.models;
using QuanLyRapChieuPhimCGV.src.views.usercontrols;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyRapChieuPhimCGV.src.views.forms
{
    public partial class fSelectChair : Form
    {
        private ucBookTicket preComponent;
        private Schedule schedule;
        private DAO_Chair dao_ch = new DAO_Chair();
        private List<Chair> chairs = new List<Chair>();
        private List<ChairType> chairTypes = new List<ChairType>();
        private DAO_ChairType dao_ct = new DAO_ChairType();
        private Button curBtnChair;
        public fSelectChair(ucBookTicket u, Schedule sch)
        {
            preComponent = u;
            schedule = sch;
            InitializeComponent();
            chairs = dao_ch.getAllByRoom(schedule.room);
            chairTypes = dao_ct.getAll();
        }

        private void fSelectChair_Load(object sender, EventArgs e)
        {
            int tongSoHang = schedule.room.totalRows;
            int tongSoCot = schedule.room.totalColumns;
            int btnSize = 50;
            Size = new Size(tongSoCot * (btnSize + 6), tongSoHang * (btnSize+6) + 30);
            Button[,] btnChairs = new Button[tongSoHang, tongSoCot];
            for(int j = 0; j < tongSoHang; j++)
            {
                int z = 1;
                dao_ch.getAllInRow(tongSoHang - j, schedule.room).ForEach(chair =>
                {
                    btnChairs[tongSoHang - j - 1, tongSoCot - chair.column] = new Button();
                    btnChairs[tongSoHang - j - 1, tongSoCot - chair.column].Name = chair.id;
                    btnChairs[tongSoHang - j - 1, tongSoCot - chair.column].Text = "" + (char)((chair.row - 1) + 65) + z++;
                    btnChairs[tongSoHang - j - 1, tongSoCot - chair.column].Size = new Size(btnSize, btnSize);
                    btnChairs[tongSoHang - j - 1, tongSoCot - chair.column].Location = new Point((tongSoCot - chair.column) * btnSize + 6, (chair.row - 1) * btnSize + 6);
                    int i = chairTypes.FindIndex(type => type.id == chair.type.id);
                    if (i != -1)
                    {
                        if (i == 0)
                        {
                            btnChairs[tongSoHang - j - 1, tongSoCot - chair.column].BackColor = Color.FromArgb(244, 248, 255);
                        }
                        else if (i == 1)
                        {
                            btnChairs[tongSoHang - j - 1, tongSoCot - chair.column].BackColor = Color.Firebrick;
                        }
                        else if (i == 2)
                        {
                            btnChairs[tongSoHang - j - 1, tongSoCot - chair.column].BackColor = Color.Green;
                        }
                        else if (i == 3)
                        {
                            btnChairs[tongSoHang - j - 1, tongSoCot - chair.column].BackColor = Color.FromArgb(255, 0, 185);
                        }
                        else
                        {
                            btnChairs[tongSoHang - j - 1, tongSoCot - chair.column].BackColor = Color.FromArgb(15, 20, 25);
                        }
                    }
                    if (dao_ch.isBooked(schedule, chair) == true)
                    {
                        btnChairs[tongSoHang - j - 1, tongSoCot - chair.column].Text = "";
                    }
                    btnChairs[tongSoHang - j - 1, tongSoCot - chair.column].Click += new EventHandler(clickChair);
                    Controls.Add(btnChairs[tongSoHang - j - 1, tongSoCot - chair.column]);
                });
            }

            // 1 ô btnSize
            // margin 6
            //width = tongcot * (btnSize + 6) + 20
            //height = tonghang * (btnSize +6)
        }

        private void clickChair(object sender, EventArgs e)
        {
            curBtnChair = sender as Button;
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (curBtnChair != null)
            {
                preComponent.getChair(dao_ch.getById(curBtnChair.Name), curBtnChair.Text);
                Close();
            }
            else
            {
                MessageBox.Show("Chưa chọn ghế", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
