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

namespace QuanLyRapChieuPhimCGV.src.views.usercontrols
{
    public partial class ucObjectBuyTicket : UserControl
    {
        private const string ADD = "Đang thêm";
        private const string EDIT = "Đang sửa";
        private string action = "";
        private DAO_ObjectPersonBuyTicket dao_o = new DAO_ObjectPersonBuyTicket();
        private List<ObjectPersonBuyTicket> objectPersonBuyTickets = new List<ObjectPersonBuyTicket>();
        public ucObjectBuyTicket()
        {
            InitializeComponent();
            Dock = DockStyle.Fill;

            objectPersonBuyTickets = dao_o.getAll();

            objectPersonBuyTickets.ForEach(objectPersonBuyTicket =>
            {
                addToDataGridView(objectPersonBuyTicket);
                cbId.Items.Add(objectPersonBuyTicket.id);
            });

            setEnabled(false);
            cbId.Enabled = false;
        }

        public void addToDataGridView(ObjectPersonBuyTicket objectPersonBuyTicket)//Thêm 1 hàng vào datagridview
        {
            dgvObjectPersonBuyTicket.Rows.Add(new object[]
            {
                objectPersonBuyTicket.id,
                objectPersonBuyTicket.name
            });
        }

        public void setEnabled(bool status)
        {
            txtName.Enabled = status;
        }

        public void resetTextBox()
        {
            txtName.Text = "";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            resetTextBox();
            if (action != ADD)
            {
                action = ADD;
                cbId.Text = dao_o.generateId();//Tạo id mới
            }
            else
            {
                action = "";
                cbId.Text = "";
            }
            cbId.Enabled = false;
            setEnabled(action == ADD);
        }
        public string validate()
        {
            string error = "";

            if (cbId.Text == "")
            {
                error += "Chưa chọn mã đối tượng";
            }
            if (txtName.Text == "")
            {
                error += "Tên đối tượng không được để trống\n";
            }
            return error;
        }

        public ObjectPersonBuyTicket getData()//Lấy thông tin đối tượng từ giao diện
        {
            string error = validate();
            if (error == "")
            {
                ObjectPersonBuyTicket objectPersonBuyTicket = new ObjectPersonBuyTicket();
                objectPersonBuyTicket.id = cbId.Text;
                objectPersonBuyTicket.name = txtName.Text;
                return objectPersonBuyTicket;
            }
            else
            {
                MessageBox.Show(error, "Lưu ý", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public void addEmployee(ObjectPersonBuyTicket objectPersonBuyTicket)
        {
            dao_o.insertOne(objectPersonBuyTicket);
            objectPersonBuyTickets.Add(objectPersonBuyTicket);
            cbId.Items.Add(objectPersonBuyTicket.id);
            cbId.Text = dao_o.generateId();
        }
        public void setData(ObjectPersonBuyTicket objectPersonBuyTicket)
        {
            txtName.Text = objectPersonBuyTicket.name;
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            resetTextBox();
            if (action != EDIT)
            {
                action = EDIT;
            }
            else
            {
                action = "";
            }
            cbId.Text = "";
            cbId.Enabled = (action == EDIT);
            setEnabled(action == EDIT);
        }
        public void updateDataGridView(ObjectPersonBuyTicket objectPersonBuyTicket)//Cập nhật 1 hàng datagridview
        {
            for (int i = 0; i < dgvObjectPersonBuyTicket.RowCount; i++)
            {
                if (dgvObjectPersonBuyTicket.Rows[i].Cells[0].Value.ToString() == objectPersonBuyTicket.id)
                {
                    dgvObjectPersonBuyTicket.Rows[i].Cells[1].Value = objectPersonBuyTicket.name;
                }
            }
        }

        //Cập nhật đối tượng trong CSDL
        //Tìm vị trí đối tượng vừa cập nhật
        //Sửa datagridview
        public void editEmployee(ObjectPersonBuyTicket objectPersonBuyTicket)
        {
            dao_o.updateOne(objectPersonBuyTicket);
            int index = objectPersonBuyTickets.FindIndex(emp => emp.id == objectPersonBuyTicket.id);
            if (index != -1)
            {
                objectPersonBuyTickets[index] = objectPersonBuyTicket;
                updateDataGridView(objectPersonBuyTicket);
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            ObjectPersonBuyTicket objectPersonBuyTicket = getData();
            if (objectPersonBuyTicket != null)
            {
                if (action == ADD)
                {
                    addEmployee(objectPersonBuyTicket);
                    addToDataGridView(objectPersonBuyTicket);
                }
                if (action == EDIT)
                {
                    editEmployee(objectPersonBuyTicket);
                }
                resetTextBox();
            }
        }

        private void cbId_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbId.SelectedIndex != -1)
            {
                ObjectPersonBuyTicket objectPersonBuyTicket = objectPersonBuyTickets.Find(emp => emp.id == cbId.Text);
                if (objectPersonBuyTicket != null)
                {
                    setData(objectPersonBuyTicket);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            for (int i = dgvObjectPersonBuyTicket.SelectedRows.Count - 1; i >= 0; i--)
            {
                int index = dgvObjectPersonBuyTicket.SelectedRows[i].Index;
                if (index != -1)
                {
                    DialogResult answer = MessageBox.Show($"Bạn có chắc chắn xoá <{dgvObjectPersonBuyTicket.Rows[index].Cells[1].Value}> ?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (answer == DialogResult.Yes)
                    {
                        dao_o.deleteById(dgvObjectPersonBuyTicket.Rows[index].Cells[0].Value.ToString());
                        int index_objectPersonBuyTicket = objectPersonBuyTickets.FindIndex(emp => emp.id == dgvObjectPersonBuyTicket.Rows[index].Cells[0].Value.ToString());
                        if (index_objectPersonBuyTicket != -1)
                        {
                            objectPersonBuyTickets.RemoveAt(index_objectPersonBuyTicket);
                            cbId.Items.RemoveAt(index_objectPersonBuyTicket);
                            dgvObjectPersonBuyTicket.Rows.RemoveAt(index);
                        }
                    }
                }
            }
        }

        private void dgvObjectPersonBuyTicket_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            if (action == EDIT)
            {
                ObjectPersonBuyTicket objectPersonBuyTicket = objectPersonBuyTickets.Find(emp => emp.id == dgvObjectPersonBuyTicket.Rows[index].Cells[0].Value.ToString());
                if (objectPersonBuyTicket != null)
                {
                    cbId.Text = dgvObjectPersonBuyTicket.Rows[index].Cells[0].Value.ToString();
                    setData(objectPersonBuyTicket);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            resetTextBox();
        }

        private void dgvObjectPersonBuyTicket_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            if (action == EDIT)
            {
                ObjectPersonBuyTicket objectPersonBuyTicket = objectPersonBuyTickets.Find(emp => emp.id == dgvObjectPersonBuyTicket.Rows[index].Cells[0].Value.ToString());
                if (objectPersonBuyTicket != null)
                {
                    cbId.Text = dgvObjectPersonBuyTicket.Rows[index].Cells[0].Value.ToString();
                    setData(objectPersonBuyTicket);
                }
            }
        }
    }
}
