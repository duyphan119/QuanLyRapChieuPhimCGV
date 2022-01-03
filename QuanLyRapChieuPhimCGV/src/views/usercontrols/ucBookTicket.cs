using QuanLyRapChieuPhimCGV.src.views.forms;
using System;
using System.Collections.Generic;
using QuanLyRapChieuPhimCGV.src.models;
using System.Windows.Forms;
using QuanLyRapChieuPhimCGV.src.DAO;

namespace QuanLyRapChieuPhimCGV.src.views.usercontrols
{
    public partial class ucBookTicket : UserControl
    {
        private const string ADD = "Đang thêm";
        private const string EDIT = "Đang sửa";
        private string action = "";
        private DAO_Schedule dao_s = new DAO_Schedule();
        private DAO_Chair dao_ch = new DAO_Chair();
        private DAO_Ticket dao_t = new DAO_Ticket();
        private List<Schedule> schedules = new List<Schedule>();
        private List<Ticket> tickets = new List<Ticket>();
        private ucAdministration admin;
        private Employee employee;
        private Customer curCustomer;
        private TicketPrice curTicketPrice;
        private Schedule curSchedule;
        private Chair curChair;
        private string curChairName;
        private DAO_Customer dao_cus = new DAO_Customer();
        public ucBookTicket(ucAdministration uc, Employee e)
        {
            admin = uc;
            employee = e;
            InitializeComponent();
            Dock = DockStyle.Fill;
            //Lấy tất cả vé
            tickets = dao_t.getAll();
            tickets.ForEach(ticket =>
            {
                cbId.Items.Add(ticket.id);
                addToDGV(ticket);
            });

            setEnabled(false);
            cbId.Enabled = false;
        }

        private void ucTicket_Load(object sender, EventArgs e)
        {
            
        }

        private void btnSelectChair_Click(object sender, EventArgs e)
        {
            if(curSchedule != null)
            {
                new fSelectChair(this, curSchedule).Visible = true;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (action != ADD)
            {
                action = ADD;
                cbId.Text = dao_t.generateId();
            }
            else
            {
                action = cbId.Text = "";
            }
            setEnabled(action == ADD);
            cbId.Enabled = false;
        }
        public void setEnabled(bool status)
        {
           rbtnNo.Enabled = rbtnYes.Enabled = numPoint.Enabled = btnSelectChair.Enabled = btnSelectMovie.Enabled = btnSelectTicketPrice.Enabled = status;
        }

        private void btnSelectMovie_Click(object sender, EventArgs e)
        {
            new fSelectMovie(this).Visible = true;
        }

        public void getSchedule(Movie movie, DateTime dt)
        {
            curSchedule = dao_s.getByDateAndMovie(movie, dt);
            txtMovie.Text = curSchedule.movie.name;
            txtDateTime.Text = curSchedule.dateTime.ToString("dd/MM/yyyy HH:mm");
            txtRoomName.Text = curSchedule.room.name;
        }

        public void getChair(Chair ch, string name)
        {
            curChair = ch;
            curChairName = name;
            txtChairName.Text = curChairName;
        }

        private void rbtnNo_CheckedChanged(object sender, EventArgs e)
        {
            if(rbtnNo.Checked == true)
            {
                btnSelectCustomer.Enabled = false;
                if (curCustomer != null)
                {
                    curCustomer = null;
                    txtCustomer.Text = "";
                }
            }
            else
            {
                btnSelectCustomer.Enabled = true;
            }
        }

        private void rbtnYes_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnYes.Checked == false)
            {
                btnSelectCustomer.Enabled = false;
            }
            else
            {
                btnSelectCustomer.Enabled = true;
            }
        }

        public void getCustomer(Customer cus)
        {
            curCustomer = cus;
            txtCustomer.Text = curCustomer.id;
            numPoint.Maximum = (decimal)curCustomer.totalPoint;
            label9.Text = $"Điểm ({curCustomer.totalPoint})";
        }
        public void getTicketPrice(TicketPrice ticketPrice)
        {
            curTicketPrice = ticketPrice;
            txtTicketPrice.Text = $"{curTicketPrice.price.ToString("#,##")}đ";
        }
        private void btnSelectCustomer_Click(object sender, EventArgs e)
        {
            new fSelectCustomer(this).Visible = true;
        }

        private void btnSelectTicketPrice_Click(object sender, EventArgs e)
        {
            new fSelectTicketPrice(this).Visible = true;
        }
       
        public string validate()
        {
            string error = "";

            return error;
        }
        public Ticket getData()
        {
            string error = validate();
            if(error == "")
            {
                return new Ticket()
                {
                    id = cbId.Text,
                    schedule = curSchedule,
                    chair = curChair,
                    employee = employee,
                    price = curTicketPrice,
                    customer = curCustomer,
                    totalPrice = curTicketPrice.price,
                    date = DateTime.Now,
                    point = (int)numPoint.Value
                };
            }
            else
            {
                MessageBox.Show(error, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return null;
        }
        public void addTicket(Ticket ticket)
        {
            dao_t.insertOne(ticket);
            if(curCustomer != null)//Nếu khách hàng có thẻ
            {
                if (numPoint.Value > 0)//Trả bằng điểm
                {
                    curCustomer.totalPoint -= (float)numPoint.Value;
                    if (curCustomer.totalPoint < 0)
                    {
                        curCustomer.totalPoint = 0;
                    }
                }
                //Tính điểm
                float point = (curCustomer.card.percentForTicket * (float)ticket.price.price) / 100000;
                if (point % Math.Floor(point) >= 0.5)
                {
                    point = (float)Math.Floor(point);
                    point++;
                }
                else
                {
                    point = (float)Math.Floor(point);
                }
                //Cập nhật điểm
                curCustomer.totalPoint += point;
                dao_cus.updateOne(curCustomer);
            }
          
            addToDGV(ticket);
            cbId.Items.Add(ticket.id);
            MessageBox.Show("Lưu thành công", "Thành công");
            cbId.Text = dao_t.generateId();
        }

        public void editTicket(Ticket ticket)
        {
            dao_t.updateOne(ticket);
            int index = tickets.FindIndex(ti => ti.id == ticket.id);
            if (index != -1)
            {
                if (curCustomer != null)//Nếu khách hàng có thẻ
                {
                    if (numPoint.Value > 0)//Trả bằng điểm
                    {
                        curCustomer.totalPoint -= (float)numPoint.Value;
                        if (curCustomer.totalPoint < 0)
                        {
                            curCustomer.totalPoint = 0;
                        }
                    }
                    //Tính điểm
                    float point = getPoint(ticket, curCustomer);
                    //Cập nhật điểm
                    curCustomer.totalPoint += point;
                    dao_cus.updateOne(curCustomer);
                }
                else
                {
                    //Tính điểm
                    float point = getPoint(ticket, tickets[index].customer);
                    //Cập nhật điểm
                    tickets[index].customer.totalPoint -= point;
                    dao_cus.updateOne(tickets[index].customer);
                }
                tickets[index] = ticket;
                editDGV(ticket);
            }
        }

        public float getPoint(Ticket ticket , Customer customer)
        {
            float point = (customer.card.percentForTicket * (float)ticket.price.price) / 100000;
            if (point % Math.Floor(point) >= 0.5)
            {
                point = (float)Math.Floor(point);
                point++;
            }
            else
            {
                point = (float)Math.Floor(point);
            }
            return point;
        }

        public void editDGV(Ticket ticket)
        {
            for (int i = 0; i < dgvTicket.RowCount; i++)
            {
                if (dgvTicket.Rows[i].Cells[0].Value.ToString() == ticket.id)
                {
                    dgvTicket.Rows[i].Cells[1].Value = ticket.customer == null ? "" : ticket.customer.id;
                    dgvTicket.Rows[i].Cells[2].Value = ticket.employee.name;
                    dgvTicket.Rows[i].Cells[3].Value = ticket.price.price.ToString("#,##");
                    dgvTicket.Rows[i].Cells[4].Value = ticket.schedule.dateTime.ToString("dd-MM-yyyy HH:mm");
                    dgvTicket.Rows[i].Cells[5].Value = ticket.schedule.room.name;
                    dgvTicket.Rows[i].Cells[6].Value = dao_ch.getChairName(ticket.chair);
                }
            }
        }

        public void addToDGV(Ticket ticket)
        {
            dgvTicket.Rows.Add(new object[]
            {
                ticket.id, ticket.customer == null ? "" : ticket.customer.id, ticket.employee.name, ticket.price.price.ToString("#,##"), ticket.schedule.dateTime.ToString("dd-MM-yyyy HH:mm"), ticket.chair.room.name, dao_ch.getChairName(ticket.chair)
            });
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            Ticket ticket = getData();
            if (ticket != null)
            {
                if(action == ADD)
                {
                    addTicket(ticket);
                }
                if(action == EDIT)
                {
                    editTicket(ticket);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            for (int i = dgvTicket.SelectedRows.Count - 1; i >= 0; i--)
            {
                int index = dgvTicket.SelectedRows[i].Index;
                if (index != -1)
                {
                    DialogResult answer = MessageBox.Show($"Bạn có chắc chắn xoá <{dgvTicket.Rows[index].Cells[0].Value}> ?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (answer == DialogResult.Yes)
                    {
                        dao_t.deleteById(dgvTicket.Rows[index].Cells[0].Value.ToString());
                        int index_ticket = tickets.FindIndex(cus => cus.id == dgvTicket.Rows[index].Cells[0].Value.ToString());
                        if (index_ticket != -1)
                        {
                            if (tickets[index_ticket].customer != null)
                            {
                                float point = getPoint(tickets[index_ticket], tickets[index_ticket].customer);
                                tickets[index_ticket].customer.totalPoint -= point;
                                dao_cus.updateOne(tickets[index_ticket].customer);
                            }
                            tickets.RemoveAt(index_ticket);
                            cbId.Items.RemoveAt(index_ticket);
                            dgvTicket.Rows.RemoveAt(index);
                        }
                    }
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void txtCustomer_TextChanged(object sender, EventArgs e)
        {

        }

        public void setData(Ticket ticket)
        {
            curCustomer = ticket.customer;
            curSchedule = ticket.schedule;
            curTicketPrice = ticket.price;
            curChair = ticket.chair;
            if(curCustomer == null)
            {
                rbtnNo.Checked = true;
                txtCustomer.Text = "";
            }
            else
            {
                rbtnYes.Checked = true;
                txtCustomer.Text = curCustomer.id;
            }
            txtMovie.Text = curSchedule.movie.name;
            txtRoomName.Text = curSchedule.room.name;
            txtChairName.Text = dao_ch.getChairName(curChair);
            txtTicketPrice.Text = ticket.price.price.ToString("#,##");
            txtDateTime.Text = curSchedule.dateTime.ToString("dd/MM/yyyy HH:mm");
            numPoint.Maximum = (decimal)ticket.customer.totalPoint;
        }

        public void reset()
        {
            curChair = null;
            curCustomer = null;
            curSchedule = null;
            curTicketPrice = null;
            txtChairName.Text = txtCustomer.Text = txtDateTime.Text = txtMovie.Text = txtRoomName.Text = txtTicketPrice.Text = "";
            numPoint.Value = 0;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            reset();
            if(action != EDIT)
            {
                action = EDIT;
            }
            else
            {
                action = "";
            }
            cbId.Text = "";
            cbId.Enabled = action == EDIT;
            setEnabled(action == EDIT);
        }

        private void dgvTicket_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(action == EDIT)
            {
                int index = e.RowIndex;
                if (index != -1)
                {
                    Ticket ticket = tickets.Find(tik => tik.id == dgvTicket.Rows[index].Cells[0].Value.ToString());
                    if (ticket != null)
                    {
                        cbId.Text = dgvTicket.Rows[index].Cells[0].Value.ToString();
                        setData(ticket);
                    }
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            reset();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            Ticket ticket = getData();
            if (ticket != null)
            {
                if (action == ADD)
                {
                    addTicket(ticket);
                }
                if (action == EDIT)
                {
                    editTicket(ticket);
                }
                new fReport(ticket).Visible = true;
            }
        }
    }
}
