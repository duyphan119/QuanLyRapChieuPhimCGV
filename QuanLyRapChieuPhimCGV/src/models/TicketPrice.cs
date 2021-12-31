using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyRapChieuPhimCGV.src.models
{
    public class TicketPrice
    {
        public string id;
        public int startDate;
        public int endDate;
        public ObjectPersonBuyTicket objectPerson;//0: THÀNH VIÊN TỪ 22 TUỔI TRỞ XUỐNG HOẶC CÓ THẺ U22, 1: TRẺ EM, 2: HỌC SINH, SINH VIÊN, NGƯỜI CAO TUỔI, 3: NGƯỜI LỚN, -1: TẤT CẢ MỌI NGƯỜI
    
        public TicketPrice() { }
    }
}
