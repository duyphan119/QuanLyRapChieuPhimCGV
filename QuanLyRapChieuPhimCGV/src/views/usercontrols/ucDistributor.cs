using QuanLyRapChieuPhimCGV.src.DAO;
using QuanLyRapChieuPhimCGV.src.models;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace QuanLyRapChieuPhimCGV.src.views.usercontrols
{
    public partial class ucDistributor : UserControl
    {
        private ucAdministration admin;
        private Employee employee;
        private const string ADD = "Đang thêm";
        private const string EDIT = "Đang sửa";
        private string action = "";
        private DAO_Distributor dao_s = new DAO_Distributor();
        private List<Distributor> manufacturers = new List<Distributor>();

        public ucDistributor(ucAdministration uc, Employee e)
        {
            admin = uc;
            employee = e;
            InitializeComponent();
            
            Dock = DockStyle.Fill;

            manufacturers = dao_s.getAll();

            manufacturers.ForEach(manufacturer =>
            {
                addToDataGridView(manufacturer);
                cbId.Items.Add(manufacturer.id);
            });

            setEnabled(false);
            cbId.Enabled = false;
        }
        private void ucManufacturer_Load(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            resetTextBox();
        }

        private void dgvManufacturer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            if (action == EDIT)
            {
                Distributor manufacturer = manufacturers.Find(emp => emp.id == dgvManufacturer.Rows[index].Cells[0].Value.ToString());
                if (manufacturer != null)
                {
                    cbId.Text = dgvManufacturer.Rows[index].Cells[0].Value.ToString();
                    setData(manufacturer);
                }
            }
        }

        public void addToDataGridView(Distributor manufacturer)//Thêm 1 hàng vào datagridview
        {
            dgvManufacturer.Rows.Add(new object[]
            {
                manufacturer.id,
                manufacturer.name
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
                cbId.Text = dao_s.generateId();//Tạo id mới
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
                error += "Chưa chọn mã màn hình";
            }
            if (txtName.Text == "")
            {
                error += "Tên màn hình không được để trống\n";
            }
            return error;
        }

        public Distributor getData()//Lấy thông tin nhân viên từ giao diện
        {
            string error = validate();
            if (error == "")
            {
                Distributor manufacturer = new Distributor();
                manufacturer.id = cbId.Text;
                manufacturer.name = txtName.Text;
                return manufacturer;
            }
            else
            {
                MessageBox.Show(error, "Lưu ý", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public void addEmployee(Distributor manufacturer)
        {
            dao_s.insertOne(manufacturer);
            manufacturers.Add(manufacturer);
            cbId.Items.Add(manufacturer.id);
            cbId.Text = dao_s.generateId();
        }
        public void setData(Distributor manufacturer)
        {
            txtName.Text = manufacturer.name;
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
        public void updateDataGridView(Distributor manufacturer)//Cập nhật 1 hàng datagridview
        {
            for (int i = 0; i < dgvManufacturer.RowCount; i++)
            {
                if (dgvManufacturer.Rows[i].Cells[0].Value.ToString() == manufacturer.id)
                {
                    dgvManufacturer.Rows[i].Cells[1].Value = manufacturer.name;
                }
            }
        }

        //Cập nhật nhân viên trong CSDL
        //Tìm vị trí nhân viên vừa cập nhật
        //Sửa datagridview
        public void editEmployee(Distributor manufacturer)
        {
            dao_s.updateOne(manufacturer);
            int index = manufacturers.FindIndex(emp => emp.id == manufacturer.id);
            if (index != -1)
            {
                manufacturers[index] = manufacturer;
                updateDataGridView(manufacturer);
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            Distributor manufacturer = getData();
            if (manufacturer != null)
            {
                if (action == ADD)
                {
                    addEmployee(manufacturer);
                    addToDataGridView(manufacturer);
                }
                if (action == EDIT)
                {
                    editEmployee(manufacturer);
                }
                resetTextBox();
            }
        }

        private void cbId_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbId.SelectedIndex != -1)
            {
                Distributor manufacturer = manufacturers.Find(emp => emp.id == cbId.Text);
                if (manufacturer != null)
                {
                    setData(manufacturer);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            for (int i = dgvManufacturer.SelectedRows.Count - 1; i >= 0; i--)
            {
                int index = dgvManufacturer.SelectedRows[i].Index;
                if (index != -1)
                {
                    DialogResult answer = MessageBox.Show($"Bạn có chắc chắn xoá <{dgvManufacturer.Rows[index].Cells[1].Value}> ?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (answer == DialogResult.Yes)
                    {
                        dao_s.deleteById(dgvManufacturer.Rows[index].Cells[0].Value.ToString());
                        int index_manufacturer = manufacturers.FindIndex(emp => emp.id == dgvManufacturer.Rows[index].Cells[0].Value.ToString());
                        if (index_manufacturer != -1)
                        {
                            manufacturers.RemoveAt(index_manufacturer);
                            cbId.Items.RemoveAt(index_manufacturer);
                            dgvManufacturer.Rows.RemoveAt(index);
                        }
                    }
                }
            }
        }
    }
}
