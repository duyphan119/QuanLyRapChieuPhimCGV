using QuanLyRapChieuPhimCGV.src.models;
using QuanLyRapChieuPhimCGV.src.utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyRapChieuPhimCGV.src.DAO
{
    public class DAO_Customer
    {
        private SqlConnection cnn;
        private SqlCommand scm;
        private SqlDataReader reader;
        public DAO_Customer()
        {
            cnn = new ConnectDatabase().cnn;
        }

        public List<Customer> getAll()//Lấy tất cả danh sách khách hàng
        {
            List<Customer> customers = new List<Customer>();
            try
            {
                DAO_Card dao_c = new DAO_Card();
                cnn.Open();
                string query = "select makhach, sdt, email, ngaysinh, gioitinh, mathe, tongdiem from khachhang";
                scm = new SqlCommand(query, cnn);
                reader = scm.ExecuteReader();
                while(reader.Read()){
                    Customer customer = new Customer();
                    customer.id = reader.GetString(0);
                    customer.phone = reader.GetString(1);
                    customer.email = reader.GetString(2);
                    customer.dayOfBirth = reader.GetDateTime(3);
                    customer.gender = (reader.GetBoolean(4) == true)?"Nam":"Nữ";
                    customer.card = dao_c.getById(reader.GetString(5));
                    customer.totalPoint = (float)reader.GetDouble(6);
                    customers.Add(customer);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                cnn.Close();
            }
            return customers;
        }

        public Customer getByEmail(string email)//Lấy khách hàng theo email
        {
            Customer customer = null;
            try
            {
                DAO_Card dao_c = new DAO_Card();
                cnn.Open();
                string query = $"select makhach, sdt, email, ngaysinh, gioitinh, mathe, tongdiem from khachhang where email = '{email}'";
                scm = new SqlCommand(query, cnn);
                reader = scm.ExecuteReader();
                while (reader.Read())
                {
                    customer = new Customer();
                    customer.id = reader.GetString(0);
                    customer.phone = reader.GetString(1);
                    customer.email = reader.GetString(2);
                    customer.dayOfBirth = reader.GetDateTime(3);
                    customer.gender = (reader.GetBoolean(4) == true) ? "Nam" : "Nữ";
                    customer.card = dao_c.getById(reader.GetString(5));
                    customer.totalPoint = (float)reader.GetDouble(6);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                cnn.Close();
            }
            return customer;
        }

        public Customer getByPhone(string phone)//Lấy khách hàng theo sdt
        {
            Customer customer = null;
            try
            {
                DAO_Card dao_c = new DAO_Card();
                cnn.Open();
                string query = $"select makhach, sdt, email, ngaysinh, gioitinh, mathe, tongdiem from khachhang where sdt = '{phone}'";
                scm = new SqlCommand(query, cnn);
                reader = scm.ExecuteReader();
                while (reader.Read())
                {
                    customer = new Customer();
                    customer.id = reader.GetString(0);
                    customer.phone = reader.GetString(1);
                    customer.email = reader.GetString(2);
                    customer.dayOfBirth = reader.GetDateTime(3);
                    customer.gender = (reader.GetBoolean(4) == true) ? "Nam" : "Nữ";
                    customer.card = dao_c.getById(reader.GetString(5));
                    customer.totalPoint = (float)reader.GetDouble(6);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                cnn.Close();
            }
            return customer;
        }

        public Customer getById(string customerId)//Lấy khách hàng theo mã khách hàng
        {
            Customer customer = null;
            try
            {
                DAO_Card dao_c = new DAO_Card();
                cnn.Open();
                string query = $"select makhach, sdt, email, ngaysinh, gioitinh, mathe, tongdiem from khachhang where makhach = '{customerId}'";
                scm = new SqlCommand(query, cnn);
                reader = scm.ExecuteReader();
                while (reader.Read())
                {
                    customer = new Customer();
                    customer.id = reader.GetString(0);
                    customer.phone = reader.GetString(1);
                    customer.email = reader.GetString(2);
                    customer.dayOfBirth = reader.GetDateTime(3);
                    customer.gender = (reader.GetBoolean(4) == true) ? "Nam" : "Nữ";
                    customer.card = dao_c.getById(reader.GetString(5));
                    customer.totalPoint = (float)reader.GetDouble(6);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                cnn.Close();
            }
            return customer;
        }

        public void insertOne(Customer customer)//Thêm khách hàng
        {
            try
            {
                cnn.Open();
                string query = $@"insert into khachhang(makhach, sdt, email, gioitinh, ngaysinh, mathe, tongdiem)
                           values ('{customer.id}','{customer.phone}','{customer.email}',{(customer.gender == "Nam"?1:0)},
                            '{customer.dayOfBirth.ToString("yyyy-MM-dd")}','{customer.card.id}', {customer.totalPoint})";
                scm = new SqlCommand(query, cnn);
                scm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                cnn.Close();
            }
        }

        public void updateOne(Customer customer)//Cập nhật khách hàng
        {
            try
            {
                cnn.Open();
                string query = $@"update khachhang set sdt = '{customer.phone}', email = '{customer.email}', 
                    gioitinh = {(customer.gender == "Nam" ? 1 : 0)}, mathe = '{customer.card.id}', 
                    ngaysinh = '{customer.dayOfBirth.ToString("yyyy-MM-dd")}', tongdiem = {customer.totalPoint} where makhach = '{customer.id}'";
                scm = new SqlCommand(query, cnn);
                scm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                cnn.Close();
            }
        }

        public void deleteById(string customerId)//Xoá khách hàng theo mã khách hàng
        {
            try
            {
                cnn.Open();
                string query = $@"delete from khachhang where makhach = '{customerId}'";
                scm = new SqlCommand(query, cnn);
                scm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                cnn.Close();
            }
        }

        public string generateId()//Tạo mã khách hàng
        {
            Methods methods = new Methods();
            string result = "";

            string str_now = DateTime.Now.Year.ToString().Substring(2, 2);//2 chữ số đầu tiên là năm hiện tại
            int i = 1;

            string str = str_now + methods.addZero(8, i);
            while (true)
            {
                if (getById(str) == null)
                {
                    result += str;
                    break;
                }
                else
                {
                    str = str_now + methods.addZero(8, ++i);
                }
            }

            return result;
        }
    }
}
