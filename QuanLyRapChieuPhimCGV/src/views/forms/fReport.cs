using Microsoft.Reporting.WinForms;
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
    public partial class fReport : Form
    {
        public fReport()
        {
            InitializeComponent();
        }
        public fReport(Ticket ticket)
        {
            InitializeComponent();
            DAO_Chair dao_ch = new DAO_Chair();
            rpv.LocalReport.ReportPath = "../../src/views/report/Ticket.rdlc";
            ReportParameter rpt1 = new ReportParameter("typeCustomer", "Liên 2: Khách Hàng");
            ReportParameter rpt2 = new ReportParameter("ticketId", $"Số: {ticket.id}");
            ReportParameter rpt3 = new ReportParameter("ticketDate", $"{DateTime.Now} {ticket.employee.name}");
            ReportParameter rpt4 = new ReportParameter("roomName", $"{ticket.schedule.room.name}");
            ReportParameter rpt5 = new ReportParameter("chairName", $"{dao_ch.getChairName(ticket.chair)}");
            ReportParameter rpt6 = new ReportParameter("dateStart", $"{ticket.schedule.dateTime}");
            ReportParameter rpt7 = new ReportParameter("movieName", $"{ticket.schedule.movie.name}");
            ReportParameter rpt8 = new ReportParameter("totalPrice", $"Tổng      VNĐ      {ticket.totalPrice.ToString("#,##")}");
            rpv.LocalReport.SetParameters(new ReportParameter[]
                { rpt1, rpt2 , rpt3, rpt4,rpt5 ,rpt1,rpt6, rpt7, rpt8}
            );
            rpv.RefreshReport();
        }

        private void fReport_Load(object sender, EventArgs e)
        {
            this.rpv.RefreshReport();
        }
    }
}
