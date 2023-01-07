using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shopapp.entity
{
    public class Sepetitem
    {
        public int id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int SepetId { get; set; }
        public Sepet Sepet { get; set; }
        public int Quantity { get; set; }

    }
}