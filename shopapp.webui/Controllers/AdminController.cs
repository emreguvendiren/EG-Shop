using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using shopapp.business.Abstract;
using shopapp.entity;
using shopapp.webui.ViewModels;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using shopapp.webui.Identity;

namespace shopapp.webui.Controllers
{
    public class AdminController:Controller
    {
        private UserManager<User> _userManager;
        public Admin admins;
        IAdminService _adminService;
         IProductService _productService;
         private IHttpContextAccessor _accessor;
        public AdminController(IAdminService adminService,IProductService productService,IHttpContextAccessor accessor,UserManager<User> userManager)
        {
            _adminService = adminService;
            this._productService=productService;
            admins = _adminService.getAdminById(1);
            _accessor = accessor;
            this._userManager = userManager;
        }
        [HttpGet]
        public IActionResult Login(){
            
            return View();
        } 
        // [HttpPost]
        // public IActionResult Login(int AdminID,string password){
        //     Admin control = _adminService.Login(AdminID,password);
        //     if(control!=null){
        //         //giris basarili
        //         System.Console.WriteLine("Hosgeldiniz");
        //     }
        //     else
        //     {
        //         System.Console.WriteLine("Kullanici adi ya da sifre yanlis");
        //     }
        //     return View(control);
        // } 
        [HttpPost]
        public IActionResult Login(Admin admin){
            Admin admincontrol = _adminService.Login(admin.AdminID,admin.password);
            ViewBag.admin = admincontrol;
            
            if (admincontrol!=null)
            {
                DateTime dt = DateTime.Now;
                try
                {
                    FileStream fs = new FileStream(@"C:\Users\Emre\Desktop\bitirme\shopapp.webui\Log.txt",FileMode.Append,FileAccess.Write,FileShare.Write);
                    string ipAddress = _accessor.HttpContext.Connection.RemoteIpAddress.ToString();   
                    StreamWriter sw = new StreamWriter(fs);
                    sw.WriteLine("["+dt.ToString()+"]"+" [ADMIN] "+admincontrol.name.ToUpper()+" "+admincontrol.surName.ToUpper()+" Giris Yapti!" +" IP : "+ipAddress);
                    sw.Close();
                }
                catch (System.Exception)
                {
                    
                    
                }
                // return RedirectToAction("index","Admin",admin);
                return View("index",admincontrol);

            }
            else
            {
                ViewBag.LoginControl =0;
            }
            return View();
        }
        
        public IActionResult index(int? AdminID){
            return View(admins);
        } 
        public IActionResult products(int AdminID){
            Admin admin = _adminService.getAdminById(AdminID);
            List<Product> products = _productService.GetAll();
            ViewBag.Products = products;
            ViewBag.admin = _adminService.getAdminById(AdminID);
            return View(admins);
        }
        public IActionResult addProduct(){
            
                 
            return View();
        }
        [HttpPost]
        public IActionResult addProduct(Product product){
            if (product.Name!=null && product.Price!=null && product.Description!=null && product.ImageUrl!=null )
            {
                string[] split = product.Name.Split(" ");
                string newurl = split[0].ToLower()+"-"+split[1].ToLower();
                product.Url = newurl;
                _productService.Create(product);
                ViewBag.control = 1;
                return View();
            }
            
            return View();
        }
        public async Task<IActionResult> users(){
            List<Admin> adminn = _adminService.getAllAdmins();
            var users = _userManager.Users;
            List<String> userList = new List<string>();
            User list2;
            List<User> users2 = new List<User>();
            foreach (var item in users)
            {
                userList.Add(item.ToString());
                list2 = await _userManager.FindByNameAsync(item.ToString());
                users2.Add(list2);
            }
            ViewBag.users= users2;
            ViewBag.userList = userList;
            ViewBag.adminler = adminn;
            return View(admins);
        }
        public IActionResult Edit(int? id){
            if(id==null){
                return NotFound();
            }
            var entity = _productService.GetById((int)id);
            if (entity==null)
            {
                return NotFound();
            }
            ViewBag.entity = entity;
            return View();
        }
        [HttpPost]
        public IActionResult Edit(Product product,string control){
            System.Console.WriteLine(control);
            if (product.Name!=null && product.Price!=null && product.Description!=null && product.ImageUrl!=null )
            {
                ViewBag.control = 1;
                ViewBag.entity = product;
                string[] split = product.Name.Split(" ");
                string newurl = split[0].ToLower()+"-"+split[1].ToLower();
                product.Url = newurl;
                _productService.Update(product);
                ViewBag.control = 1;
                return View();
            }
            
            
            return View();
        }
        public IActionResult DeleteProduct(int ProductId){
            var entity = _productService.GetById(ProductId);
            if(entity !=null){
                _productService.Delete(entity);
            }
            return RedirectToAction("products");
        }
        public IActionResult addAdmin(){
            return View();
        }
        [HttpPost]
        public IActionResult addAdmin(Admin admin){
            if (admin.AdminID!= 0 && admin.name !=null && admin.surName!= null && admin.password!=null)
            {
                _adminService.addAdmin(admin);
                return RedirectToAction("users");
            }
            return View();
        }
        
    }
}