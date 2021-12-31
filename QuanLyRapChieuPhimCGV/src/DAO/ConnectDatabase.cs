using System.Data.SqlClient;

namespace QuanLyRapChieuPhimCGV.src.DAO
{
    public class ConnectDatabase
    {
        public SqlConnection cnn = new SqlConnection(@"Data Source=DESKTOP-NIULDEP\SQLEXPRESS;Initial Catalog=QLRapChieuPhimCGV;User ID=sa;Password=password");
    }
}
