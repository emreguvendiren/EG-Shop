using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace shopapp.entity
{
    public class Admin
    {
        [Required(ErrorMessage ="Lutfen ID Giriniz")]
        public int AdminID { get; set; }

        public string name { get; set; }
        public string surName { get; set; }
        [Required(ErrorMessage ="Lutfen Sifre Giriniz.")]
        public string password { get; set; }    
    }
}