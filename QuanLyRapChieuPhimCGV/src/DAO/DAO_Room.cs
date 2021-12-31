using QuanLyRapChieuPhimCGV.src.models;
using QuanLyRapChieuPhimCGV.src.utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace QuanLyRapChieuPhimCGV.src.DAO
{
    public class DAO_Room
    {
        private SqlConnection cnn;
        private SqlCommand scm;
        private SqlDataReader reader;
        public DAO_Room()
        {
            cnn = new ConnectDatabase().cnn;
        }

        public List<Room> getAll()//Lấy tất cả danh sách phòng chiếu
        {
            List<Room> rooms = new List<Room>();
            try
            {
                DAO_Screen dao_s = new DAO_Screen();
                cnn.Open();
                string query = "select maphong, tenphong, tongsohang, tongsocot, mamanhinh from phongchieu";
                scm = new SqlCommand(query, cnn);
                reader = scm.ExecuteReader();
                while (reader.Read())
                {
                    Room room = new Room();
                    room.id = reader.GetString(0);
                    room.name = reader.GetString(1);
                    room.totalRows = reader.GetInt32(2);
                    room.totalColumns = reader.GetInt32(3);
                    room.screen = dao_s.getById(reader.GetString(4));
                    rooms.Add(room);
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
            return rooms;
        }

        public Room getById(string roomId)//Lấy phòng chiếu theo mã phòng chiếu
        {
            Room room = null;
            try
            {
                DAO_Screen dao_s = new DAO_Screen();
                cnn.Open();
                string query = $"select maphong, tenphong, tongsohang, tongsocot, mamanhinh from phongchieu where maphong = '{roomId}'";
                scm = new SqlCommand(query, cnn);
                reader = scm.ExecuteReader();
                while (reader.Read())
                {
                    room = new Room();
                    room.id = reader.GetString(0);
                    room.name = reader.GetString(1);
                    room.totalRows = reader.GetInt32(2);
                    room.totalColumns = reader.GetInt32(3);
                    room.screen = dao_s.getById(reader.GetString(4));
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
            return room;
        }

        public void insertOne(Room room)//Thêm phòng chiếu
        {
            try
            {
                cnn.Open();
                string query = $@"insert into phongchieu(maphong, tenphong, tongsohang, tongsocot, mamanhinh)
                           values ('{room.id}',N'{room.name}',{room.totalRows}, {room.totalColumns}, '{room.screen.id}')";
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

        public void updateOne(Room room)//Cập nhật phòng chiếu
        {
            try
            {
                cnn.Open();
                string query = $@"update phongchieu set tenphong = N'{room.name}', tongsohang = {room.totalRows}, 
                    tongsocot = {room.totalColumns}, mamanhinh = '{room.screen.id}' where maphong = '{room.id}'";
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

        public void deleteById(string roomId)//Xoá phòng chiếu theo mã phòng chiếu
        {
            try
            {
                cnn.Open();
                string query = $@"delete from phongchieu where maphong = '{roomId}'";
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

        public string generateId()//Tạo mã phòng chiếu
        {
            Methods methods = new Methods();
            string result = "";

            int i = 1;

            string str = methods.addZero(3, i);
            while (true)
            {
                if (getById(str) == null)
                {
                    result += str;
                    break;
                }
                else
                {
                    str = methods.addZero(3, ++i);
                }
            }

            return result;
        }
    }

    
}
