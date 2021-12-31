using QuanLyRapChieuPhimCGV.src.models;
using QuanLyRapChieuPhimCGV.src.utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace QuanLyRapChieuPhimCGV.src.DAO
{
    public class DAO_Distributor
    {
        private SqlConnection cnn;
        private SqlCommand scm;
        private SqlDataReader reader;
        public DAO_Distributor()
        {
            cnn = new ConnectDatabase().cnn;
        }

        public List<Distributor> getAll()//Lấy tất cả danh sách nhà phát hành
        {
            List<Distributor> distributors = new List<Distributor>();
            try
            {
                cnn.Open();
                string query = "select * from nhaphathanh";
                scm = new SqlCommand(query, cnn);
                reader = scm.ExecuteReader();
                while (reader.Read())
                {
                    Distributor distributor = new Distributor();
                    distributor.id = reader.GetString(0);
                    distributor.name = reader.GetString(1);
                    distributors.Add(distributor);
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
            return distributors;
        }

        public Distributor getById(string distributorId)//Lấy nhà phát hàng theo mã nhà phát hành
        {
            Distributor distributor = null;
            try
            {
                cnn.Open();
                string query = $"select * from nhaphathanh where manph = '{distributorId}'";
                scm = new SqlCommand(query, cnn);
                reader = scm.ExecuteReader();
                if (reader.Read())
                {
                    distributor = new Distributor();
                    distributor.id = reader.GetString(0);
                    distributor.name = reader.GetString(1);
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
            return distributor;
        }

        public void insertOne(Distributor distributor)//Thêm nhà phát hành
        {
            try
            {
                cnn.Open();
                string query = $@"insert into nhaphathanh(manph, tennph)
                           values ('{distributor.id}',N'{distributor.name}')";
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

        public void updateOne(Distributor distributor)//Cập nhật nhà phát hành
        {
            try
            {
                cnn.Open();
                string query = $@"update nhaphathanh set tennph = N'{distributor.name}' where manph = '{distributor.id}'";
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

        public void deleteById(string distributorId)//Xoá nhà phát hàng theo mã nhà phát hành
        {
            try
            {
                cnn.Open();
                string query = $@"delete from nhaphathanh where manph = '{distributorId}'";
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

        public string generateId()//Tạo mã nhà phát hành
        {
            Methods methods = new Methods();
            string result = "";

            string str_now = DateTime.Now.Year.ToString().Substring(2, 2);//2 chữ số đầu tiên là năm hiện tại
            int i = 1;

            string str = str_now + methods.addZero(4, i);
            while (true)
            {
                if (getById(str) == null)
                {
                    result += str;
                    break;
                }
                else
                {
                    str = str_now + methods.addZero(4, ++i);
                }
            }

            return result;
        }
    }

    
}
