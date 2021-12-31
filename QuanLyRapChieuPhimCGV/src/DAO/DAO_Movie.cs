using QuanLyRapChieuPhimCGV.src.models;
using QuanLyRapChieuPhimCGV.src.utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace QuanLyRapChieuPhimCGV.src.DAO
{
    public class DAO_Movie
    {
        private SqlConnection cnn;
        private SqlCommand scm;
        private SqlDataReader reader;
        public DAO_Movie()
        {
            cnn = new ConnectDatabase().cnn;
        }

        public List<Movie> getAll()//Lấy tất cả danh sách phim
        {
            List<Movie> movies = new List<Movie>();
            try
            {
                DAO_Distributor dao_d = new DAO_Distributor();
                cnn.Open();
                string query = "select maphim, tenphim, hinhanh, daodien, ngaykhoichieu, ngonngu, mota, manph, camtuoiduoi, thoiluong from phim";
                scm = new SqlCommand(query, cnn);
                reader = scm.ExecuteReader();
                while (reader.Read())
                {
                    Movie movie = new Movie()
                    {
                        id = reader.GetString(0),
                        name = reader.GetString(1),
                        image = reader.GetString(2),
                        producer = reader.GetString(3),
                        dateStart = reader.GetDateTime(4),
                        language = reader.GetString(5),
                        description = reader.GetString(6),
                        distributor = dao_d.getById(reader.GetString(7)),
                        ageLimit = reader.GetInt32(8),
                        length = reader.GetInt32(9)
                    };
                    movies.Add(movie);
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
            return movies;
        }

        public Movie getById(string movieId)//Lấy phim theo mã phim
        {
            Movie movie = null;
            try
            {
                DAO_Distributor dao_d = new DAO_Distributor();
                cnn.Open();
                string query = $"select maphim, tenphim, hinhanh, daodien, ngaykhoichieu, ngonngu, mota, manph, camtuoiduoi, thoiluong from phim where maphim = '{movieId}'";
                scm = new SqlCommand(query, cnn);
                reader = scm.ExecuteReader();
                while (reader.Read())
                {
                    movie = new Movie()
                    {
                        id = reader.GetString(0),
                        name = reader.GetString(1),
                        image = reader.GetString(2),
                        producer = reader.GetString(3),
                        dateStart = reader.GetDateTime(4),
                        language = reader.GetString(5),
                        description = reader.GetString(6),
                        distributor = dao_d.getById(reader.GetString(7)),
                        ageLimit = reader.GetInt32(8),
                        length = reader.GetInt32(9)
                    };
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
            return movie;
        }

        public void insertOne(Movie movie)//Thêm phim
        {
            try
            {
                cnn.Open();
                string query = $@"insert into phim(maphim, tenphim, hinhanh, daodien,ngaykhoichieu, ngonngu, mota, manph, camtuoiduoi, thoiluong)
                           values ('{movie.id}',N'{movie.name}','{movie.image}', N'{movie.producer}','{movie.dateStart.ToString("yyyy-MM-dd")}',
                                N'{movie.language}', N'{movie.description}', '{movie.distributor.id}', {movie.ageLimit}, {movie.length})";
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

        public void updateOne(Movie movie)//Cập nhật phim
        {
            try
            {
                cnn.Open();
                string query = $@"update phim set tenphim = N'{movie.name}', hinhanh = {movie.image}, daodien = N'{movie.producer}',
                    ngaykhoichieu = '{movie.dateStart.ToString("yyyy-MM-dd")}', ngonngu = N'{movie.language}', mota = N'{movie.description}',
                    manph ='{movie.distributor.id}',camtuoiduoi = {movie.ageLimit},thoiluong={movie.length}  where maphim = '{movie.id}'";
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

        public void deleteById(string movieId)//Xoá phim theo mã phim
        {
            try
            {
                cnn.Open();
                string query = $@"delete from phim where maphim = '{movieId}'";
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

        public string generateId()//Tạo mã phim
        {
            Methods methods = new Methods();
            string result = "";
            string str_now = DateTime.Now.Year.ToString().Substring(2, 2);//2 chữ số đầu tiên là năm hiện tại
            int i = 1;

            string str = str_now + methods.addZero(7, i);
            while (true)
            {
                if (getById(str) == null)
                {
                    result += str;
                    break;
                }
                else
                {
                    str = str_now + methods.addZero(7, ++i);
                }
            }

            return result;
        }
    }

    
}
