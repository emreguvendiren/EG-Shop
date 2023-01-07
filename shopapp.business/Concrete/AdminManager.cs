using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using shopapp.business.Abstract;
using shopapp.data.Abstract;
using shopapp.entity;

namespace shopapp.business.Concrete
{
    public class AdminManager : IAdminService
    {
        private IAdminRepository _adminRepository;
        public AdminManager(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public void addAdmin(Admin admin)
        {
            _adminRepository.addAdmin(admin);
        }

        public Admin getAdminById(int id)
        {
            return _adminRepository.getAdminById(id);
        }

        public List<Admin> getAllAdmins()
        {
            return _adminRepository.getAllAdmins();
        }

        public Admin Login(int id, string password)
        {
            return _adminRepository.Login(id,password);
        }
    }
}