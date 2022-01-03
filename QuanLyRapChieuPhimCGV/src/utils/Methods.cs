using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyRapChieuPhimCGV.src.utils
{
    public class Methods
    {
        public string addZero(int num, int value)
        {
            string result = "";
            for (int i = value.ToString().Length; i < num; i++)
            {
                result += "0";
            }
            return result + value;
        }

        public string getObjectPersonBuyTicketName(int id)
        {
            string result = "Tất cả";
            if(id == 0)
            {
                result = "Có thẻ U22";
            }else if(id == 1)
            {
                result = "Trẻ em";
            }
            else if (id == 2)
            {
                result = "Học sinh";
            }
            else if (id == 3)
            {
                result = "Sinh viên";
            }
            else if (id == 4)
            {
                result = "Người cao tuổi";
            }
            else if (id == 5)
            {
                result = "Người lớn";
            }
            return result;
        }

        public string getPermission(int permission)
        {
            string result = "USER";
            if (permission == 1)
            {
                result = "ADMIN";
            }
            return result;
        }
    }
}
