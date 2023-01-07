using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shopapp.entity
{
    public class Sepet
    {
        public int id { get; set; }
        public string UserId { get; set; }
        public List<Sepetitem> Sepetitems { get; set; }
    }
}