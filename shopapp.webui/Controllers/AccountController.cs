using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using shopapp.business.Abstract;
using shopapp.webui.EmailServices;
using shopapp.webui.Identity;
using shopapp.webui.Models;

namespace shopapp.webui.Controllers
{
    
    [AutoValidateAntiforgeryToken]
    public class AccountController:Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private IHttpContextAccessor _accessor;
        private IEmailSender _emailSender;
        private ISepetService _sepetService;
        public AccountController(UserManager<User> userManager,SignInManager<User> signInManager,IHttpContextAccessor accessor,IEmailSender emailSender,ISepetService sepetService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _accessor = accessor;
            _emailSender = emailSender;
            _sepetService = sepetService;
            
        }
        public IActionResult Login(){
            
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model){
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user==null)
            {
                // ModelState.AddModelError("","Kullanici adi bulunamadi.");
                ViewBag.notfound = 1;
                return View(model);
            }
            if(!await _userManager.IsEmailConfirmedAsync(user)){
                ViewBag.notconfirmed = 1;
                return View(model);
            }
            var result = await _signInManager.PasswordSignInAsync(user,model.Password,true,false);
            if(result.Succeeded){
                DateTime dt = DateTime.Now;
                try
                {
                    FileStream fs = new FileStream(@"C:\Users\Emre\Desktop\bitirme\shopapp.webui\Log.txt",FileMode.Append,FileAccess.Write,FileShare.Write);
                    string ipAddress = _accessor.HttpContext.Connection.RemoteIpAddress.ToString();   
                    StreamWriter sw = new StreamWriter(fs);
                    sw.WriteLine("["+dt.ToString()+"]"+" [USER] "+user.FirstName.ToUpper()+" "+user.LastName.ToUpper()+" Giris yapti!" + "IP : " + ipAddress);
                    sw.Close();
                }
                catch (System.Exception)
                {
                    
                    
                }
                return RedirectToAction("index","home");
            }
            ViewBag.idorpass = 1;
            return View();
        }
        public IActionResult Register(){

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model){
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = new User(){
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Email = model.Email
            };
            
            var result = await _userManager.CreateAsync(user,model.Password);
            if(result.Succeeded){
                //log
                DateTime dt = DateTime.Now;
                try
                {
                    FileStream fs = new FileStream(@"C:\Users\Emre\Desktop\bitirme\shopapp.webui\Log.txt",FileMode.Append,FileAccess.Write,FileShare.Write);
                    string ipAddress = _accessor.HttpContext.Connection.RemoteIpAddress.ToString();   
                    StreamWriter sw = new StreamWriter(fs);
                    sw.WriteLine("["+dt.ToString()+"]"+" "+user.UserName.ToUpper()+" "+user.LastName.ToUpper()+" Kayit oldu!" + "IP : " + ipAddress);
                    sw.Close();
                }
                catch (System.Exception)
                {
                    
                    
                }

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var url = Url.Action("ConfirmEmail","Account",new{
                    userId = user.Id,
                    token = code
                });
                
                await _emailSender.SendEmailAsync(model.Email,"Hesabinizi Onaylayiniz -EG Shop",$"Lutfen Email hesabinizi onaylamak icin linke <a href='https://localhost:5001{url}'>tiklayiniz</a>");
                return RedirectToAction("Login","Account");
            }
            ModelState.AddModelError("Email","Email kullaniliyor.");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Logout(){
            
            await _signInManager.SignOutAsync();
            return RedirectToAction("index","home");
        }
        public async Task<IActionResult> ConfirmEmail(string userId,string token){
            if(userId==null || token == null){
                //"Gecersiz Token";
                ViewBag.gecersiz = 1;
                return View();
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user==null)
            {
                //"Boyle bir kullanici bulunamadi."
                ViewBag.bulunamadi = 1;
                return View();
            }
            var result = await _userManager.ConfirmEmailAsync(user,token);
            if (result.Succeeded)
            {
                _sepetService.InitializeCart(user.Id);
                //Hesabiniz onaylandi
                ViewBag.success=1;
                return View();
            }

            return View();
        }
        public async Task<IActionResult> Manage(){
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            
            return View(user);
        }
        
        
    }
}