using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using shopapp.entity;

namespace shopapp.business.Abstract
{
    public interface IAdminService
    {
        Admin Login(int id,string password);
        Admin getAdminById(int id);

        void addAdmin(Admin admin);
        List<Admin> getAllAdmins();
    }
}