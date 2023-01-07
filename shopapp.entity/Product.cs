using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace shopapp.entity
{
    public class Product
    {
        
        public int ProductId { get; set; } 
        [Required(ErrorMessage ="Ürün ismi boş olamaz")] 
        public string Name { get; set; }       
        public string Url { get; set; }   
        [Required(ErrorMessage ="Ürün fiyatı boş olamaz")]     
        public double? Price { get; set; } 
        [Required(ErrorMessage ="Ürün açıklaması boş olamaz")] 
        public string Description { get; set; }  
        [Required(ErrorMessage ="Ürün resmi boş olamaz")]        
        public string ImageUrl { get; set; }
        public bool IsApproved { get; set; }
        public bool IsHome { get; set; }

        public List<ProductCategory> ProductCategories { get; set; }
    }
}