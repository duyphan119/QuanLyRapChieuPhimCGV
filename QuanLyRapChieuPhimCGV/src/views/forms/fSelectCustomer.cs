using QuanLyRapChieuPhimCGV.src.DAO;
using QuanLyRapChieuPhimCGV.src.models;
using QuanLyRapChieuPhimCGV.src.views.usercontrols;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace QuanLyRapChieuPhimCGV.src.views.forms
{
    public partial class fSelectCustomer : Form
    {
        private ucBookTicket preComponent;
        private ucBill preComponentBill;
        private DAO_Customer dao_cus = new DAO_Customer();
        private List<Customer> customers = new List<Customer>();
        public fSelectCustomer(ucBookTicket uc)
        {
            preComponent = uc;
            InitializeComponent();

            customers = dao_cus.getAll();
            customers.ForEach(customer =>
            {
                dgvCustomer.Rows.Add(new object[]
                {
                    customer.id, customer.phone,customer.email,customer.dayOfBirth.ToString("dd-MM-yyyy"),customer.gender, customer.card.name, customer.totalPoint
                });
            });
        }
        public fSelectCustomer(ucBill uc)
        {
            preComponentBill = uc;
            InitializeComponent();

            customers = dao_cus.getAll();
            customers.ForEach(customer =>
            {
                dgvCustomer.Rows.Add(new object[]
                {
                    customer.id, customer.phone,customer.email,customer.dayOfBirth.ToString("dd-MM-yyyy"),customer.gender, customer.card.name, customer.totalPoint
                });
            });
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if(dgvCustomer.SelectedRows.Count > 0)
            {
                Customer customer = customers.Find(cus => cus.id == dgvCustomer.Rows[dgvCustomer.SelectedRows[0].Index].Cells[0].Value.ToString());
                if(customer != null)
                {
                    if (preComponent != null)
                    {
                        preComponent.getCustomer(customer);
                    }
                    if (preComponentBill != null)
                    {
                        preComponentBill.getCustomer(customer);
                    }
                }
            }
            Close();
        }
    }
}
