using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shopapp.webui.Models
{
    public class SepetModel
    {
        public int SepetId { get; set; }
        public List<SepetItemModel> SepetItems { get; set; }

        public double? TotalPrice(){
            return SepetItems.Sum(i=>i.Price*i.Quantity);
        }
    }
    public class SepetItemModel{
        public int SepetItemId { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public double? Price { get; set; }
        public string ImageUrl { get; set; }
        public int Quantity { get; set; }
    }
}