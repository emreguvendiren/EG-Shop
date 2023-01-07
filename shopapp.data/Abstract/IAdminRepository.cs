using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using shopapp.entity;

namespace shopapp.data.Abstract
{
    public interface IAdminRepository
    {
        Admin Login(int id,string password);
        Admin getAdminById(int id);
        List<Admin> getAllAdmins();
        void addAdmin(Admin admin);
    }
}