using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using shopapp.entity;

namespace shopapp.webui.ViewModels
{
    public class AdminProductModel
    {
        public Admin Admins { get; set; }
        public List<Product> Products { get; set; }
    }
}