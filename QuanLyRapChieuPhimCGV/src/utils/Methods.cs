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
    }
}
