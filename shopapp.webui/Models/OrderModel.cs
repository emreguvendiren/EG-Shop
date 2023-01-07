using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shopapp.webui.Models
{
    public class OrderModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Adress { get; set; }
        public string Ctiy { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string ExpYear { get; set; }
        public string Cvc { get; set; }

        public string ExpMonth { get; set; }



        public SepetModel SepetModel { get; set; }
    }
}