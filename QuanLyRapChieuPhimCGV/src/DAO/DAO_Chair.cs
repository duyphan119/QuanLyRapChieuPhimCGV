using QuanLyRapChieuPhimCGV.src.models;
using QuanLyRapChieuPhimCGV.src.utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace QuanLyRapChieuPhimCGV.src.DAO
{
    public class DAO_Chair
    {
        private SqlConnection cnn;
        private SqlCommand scm;
        private SqlDataReader reader;
        public DAO_Chair()
        {
            cnn = new ConnectDatabase().cnn;
        }

        public List<Chair> getAll()//Lấy tất cả danh sách ghế
        {
            List<Chair> chairs = new List<Chair>();
            try
            {
                DAO_ChairType dao_ct = new DAO_ChairType();
                DAO_Room dao_r = new DAO_Room();
                cnn.Open();
                string query = "select maghe, vitrihang, vitricot, trangthai, maphong, maloai from ghe";
                scm = new SqlCommand(query, cnn);
                reader = scm.ExecuteReader();
                while (reader.Read())
                {
                    Chair chair = new Chair();
                    chair.id = reader.GetString(0);
                    chair.row = reader.GetInt32(1);
                    chair.column = reader.GetInt32(2);
                    chair.status = reader.GetBoolean(3);
                    chair.room = dao_r.getById(reader.GetString(4));
                    chair.type = dao_ct.getById(reader.GetString(5));
                    chairs.Add(chair);
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
            return chairs;
        }

        public Chair getById(string chairId)//Lấy ghế theo mã ghế
        {
            Chair chair = null;
            try
            {
                DAO_ChairType dao_ct = new DAO_ChairType();
                DAO_Room dao_r = new DAO_Room();
                cnn.Open();
                string query = $"select maghe, vitrihang, vitricot, trangthai, maphong, maloai from ghe where maghe = '{chairId}'";
                scm = new SqlCommand(query, cnn);
                reader = scm.ExecuteReader();
                while (reader.Read())
                {
                    chair = new Chair();
                    chair.id = reader.GetString(0);
                    chair.row = reader.GetInt32(1);
                    chair.column = reader.GetInt32(2);
                    chair.status = reader.GetBoolean(3);
                    chair.room = dao_r.getById(reader.GetString(4));
                    chair.type = dao_ct.getById(reader.GetString(5));
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
            return chair;
        }

        public void insertOne(Chair chair)//Thêm ghế
        {
            try
            {
                cnn.Open();
                string query = $@"insert into ghe(maghe, vitrihang, vitricot, trangthai, maphong, maloai)
                           values ('{chair.id}',{chair.row}, {chair.column}, {(chair.status == true?1:0)}, '{chair.room.id}', '{chair.type.id}')";
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

        public void updateOne(Chair chair)//Cập nhật loại ghế
        {
            try
            {
                cnn.Open();
                string query = $@"update ghe set vitrihang = {chair.row}, vitricot = {chair.column}, trangtrai ={(chair.status == true ? 1 : 0)},
                    maphong = '{chair.room.id}', maloai = '{chair.type.id}' where maghe = '{chair.id}'";
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

        public void deleteById(string chairId)//Xoá loại ghế theo mã loại ghế
        {
            try
            {
                cnn.Open();
                string query = $@"delete from ghe where maloai = '{chairId}'";
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

        public string generateId()//Tạo mã loại ghế
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
