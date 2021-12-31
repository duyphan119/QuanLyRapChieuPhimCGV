using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyRapChieuPhimCGV.src.models
{
    public class Ticket
    {
        public string id;
        public Schedule schedule;
        public Customer customer;
        public Employee employee;
        public TicketPrice price;

        public Ticket() { }
    }
}
