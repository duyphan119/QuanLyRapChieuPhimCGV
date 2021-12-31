using QuanLyRapChieuPhimCGV.src.models;
using QuanLyRapChieuPhimCGV.src.utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace QuanLyRapChieuPhimCGV.src.DAO
{
    public class DAO_ObjectPersonBuyTicket
    {
        private SqlConnection cnn;
        private SqlCommand scm;
        private SqlDataReader reader;
        public DAO_ObjectPersonBuyTicket()
        {
            cnn = new ConnectDatabase().cnn;
        }

        public List<ObjectPersonBuyTicket> getAll()//Lấy tất cả danh sách màn hình
        {
            List<ObjectPersonBuyTicket> objectPersonBuyTickets = new List<ObjectPersonBuyTicket>();
            try
            {
                cnn.Open();
                string query = "select madt, tendt from doituong";
                scm = new SqlCommand(query, cnn);
                reader = scm.ExecuteReader();
                while (reader.Read())
                {
                    ObjectPersonBuyTicket objectPersonBuyTicket = new ObjectPersonBuyTicket();
                    objectPersonBuyTicket.id = reader.GetString(0);
                    objectPersonBuyTicket.name = reader.GetString(1);
                    objectPersonBuyTickets.Add(objectPersonBuyTicket);
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
            return objectPersonBuyTickets;
        }

        public ObjectPersonBuyTicket getById(string objectPersonBuyTicketId)//Lấy màn hình theo mã màn hình
        {
            ObjectPersonBuyTicket objectPersonBuyTicket = null;
            try
            {
                cnn.Open();
                string query = $"select madt, tendt from doituong where madt = '{objectPersonBuyTicketId}'";
                scm = new SqlCommand(query, cnn);
                reader = scm.ExecuteReader();
                if (reader.Read())
                {
                    objectPersonBuyTicket = new ObjectPersonBuyTicket();
                    objectPersonBuyTicket.id = reader.GetString(0);
                    objectPersonBuyTicket.name = reader.GetString(1);
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
            return objectPersonBuyTicket;
        }

        public void insertOne(ObjectPersonBuyTicket objectPersonBuyTicket)//Thêm màn hình
        {
            try
            {
                cnn.Open();
                string query = $@"insert into doituong(madt, tendt)
                           values ('{objectPersonBuyTicket.id}',N'{objectPersonBuyTicket.name}')";
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

        public void updateOne(ObjectPersonBuyTicket objectPersonBuyTicket)//Cập nhật màn hình
        {
            try
            {
                cnn.Open();
                string query = $@"update doituong set tendt = N'{objectPersonBuyTicket.name}' where madt = '{objectPersonBuyTicket.id}'";
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

        public void deleteById(string objectPersonBuyTicketId)//Xoá màn hình theo mã màn hình
        {
            try
            {
                cnn.Open();
                string query = $@"delete from doituong where madt = '{objectPersonBuyTicketId}'";
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

        public string generateId()//Tạo mã màn hình
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
