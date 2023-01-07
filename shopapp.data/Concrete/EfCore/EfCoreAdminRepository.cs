using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using shopapp.data.Abstract;
using shopapp.entity;

namespace shopapp.data.Concrete.EfCore
{
    public class EfCoreAdminRepository : IAdminRepository
    {
        public void addAdmin(Admin admin)
        {
            using(var context = new ShopContext()){
                context.Admins.Add(admin);
                context.SaveChanges();
            };
        }

        public Admin getAdminById(int id)
        {
            using(var context = new ShopContext()){
                return context.Admins.Where(i=>i.AdminID == id).FirstOrDefault();
                
            }
        }

        public List<Admin> getAllAdmins()
        {
            using(var context = new ShopContext()){
                return context.Admins.ToList();
            }
        }

        public Admin Login(int id, string password)
        {
           
            using(var context = new ShopContext()){
                return context.Admins.Where(i=>i.AdminID == id && i.password == password).FirstOrDefault();
                
            }
           
        }
    }
}