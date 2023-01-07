using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using shopapp.business.Abstract;
using shopapp.entity;
using shopapp.webui.Identity;
using shopapp.webui.Models;

namespace shopapp.webui.Controllers
{
    [Authorize]
    public class CartController:Controller
    {
        private ISepetService _sepetService;
        private UserManager<User> _userManager;
        private IOrderService _orderService;
        public CartController(ISepetService sepetService,UserManager<User> userManager,IOrderService orderService)
        {
            _sepetService = sepetService;
            _userManager = userManager;
            _orderService = orderService;
        }
        public IActionResult Index(){
            var cart = _sepetService.GetCartByUserId(_userManager.GetUserId(User));
            
            return View(new SepetModel(){
                SepetId = cart.id,
                SepetItems = cart.Sepetitems.Select(i=>new SepetItemModel(){
                    SepetItemId = i.id,
                    Name = i.Product.Name,
                    Price = (double)i.Product.Price,
                    ImageUrl = i.Product.ImageUrl,
                    Quantity = i.Quantity,
                    ProductId = i.ProductId
                    
                }).ToList()
            });
        }
        [HttpPost]
        public IActionResult AddToCart(int productId,int quantity){
            var userId = _userManager.GetUserId(User);
            _sepetService.AddToCart(userId,productId,quantity);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult DeleteFromCart(int productId){
            var userId = _userManager.GetUserId(User);
            _sepetService.DeleteFromCart(userId,productId);
            return RedirectToAction("Index");
        }
        public IActionResult Checkout(){
            var cart = _sepetService.GetCartByUserId(_userManager.GetUserId(User));
            OrderModel orderModel = new OrderModel();
            orderModel.SepetModel = new SepetModel(){
                SepetId = cart.id,
                SepetItems = cart.Sepetitems.Select(i=>new SepetItemModel(){
                    SepetItemId = i.id,
                    Name = i.Product.Name,
                    Price = (double)i.Product.Price,
                    ImageUrl = i.Product.ImageUrl,
                    Quantity = i.Quantity,
                    ProductId = i.ProductId
                    
                }).ToList()
            };
            return View(orderModel);
        }

        [HttpPost]
        public IActionResult Checkout(OrderModel model){
            
            
            if(ModelState.IsValid){
                var userId = _userManager.GetUserId(User);
                var cart = _sepetService.GetCartByUserId(userId);

                
                model.SepetModel = new SepetModel(){
                SepetId = cart.id,
                SepetItems = cart.Sepetitems.Select(i=>new SepetItemModel(){
                    SepetItemId = i.id,
                    Name = i.Product.Name,
                    Price = (double)i.Product.Price,
                    ImageUrl = i.Product.ImageUrl,
                    Quantity = i.Quantity,
                    ProductId = i.ProductId
                        
                    }).ToList()
                    };
                
                Payment payment2 = PaymentProcess(model);              
                System.Console.WriteLine(payment2.Status);
                if (payment2.Status == "success")
                {                   
                    SaveOrder(model,payment2,userId);
                    ClearCart(userId);
                    return View("Success");
                }
            }
            
            return View(model);
        }

        private void ClearCart(string userId)
        {
            
        }

        private void SaveOrder(OrderModel model, Payment payment2, string userId)
        {
            var order = new Order();
            order.OrderNumber = new Random().Next(111111,999999).ToString();
            order.OrderState=EnumOrderState.complated;
            order.PaymentId = payment2.PaymentId;
            order.ConversationId = payment2.ConversationId;
            order.OrderDate = new DateTime();
            order.FirstName = model.FirstName;
            order.LastName = model.LastName;
            order.Email= model.Email;
            order.UserId = userId;
            order.Adress = model.Adress;
            order.Phone = model.Phone;
            order.City = model.Ctiy;
            
            order.OrderItems = new List<entity.OrderItem>();

            foreach (var item in model.SepetModel.SepetItems)
            {
                var orderItem = new shopapp.entity.OrderItem(){
                    Price = (double)item.Price,
                    Quantity = item.Quantity,
                    ProductId = item.ProductId
                };
                order.OrderItems.Add(orderItem);
                
            }
            _orderService.Create(order);
        }

        private Payment PaymentProcess(OrderModel model)
        {
            Options options = new Options();
        options.ApiKey = "sandbox-KfmdBLr9THl3V6a4ogaGHZrUI64LXBft";
        options.SecretKey = "sandbox-8j6DdcSmO1GL2CYQnYH1EYxa5Fej8TkC";
        options.BaseUrl = "https://sandbox-api.iyzipay.com";
                
        CreatePaymentRequest request = new CreatePaymentRequest();
        request.Locale = Locale.TR.ToString();
        request.ConversationId = new Random().Next(111111111,999999999).ToString();
        request.Price = model.SepetModel.TotalPrice().ToString();
        request.PaidPrice = model.SepetModel.TotalPrice().ToString();
        request.Currency = Currency.TRY.ToString();
        request.Installment = 1;
        request.BasketId = "B67832";
        request.PaymentChannel = PaymentChannel.WEB.ToString();
        request.PaymentGroup = PaymentGroup.PRODUCT.ToString();

        PaymentCard paymentCard = new PaymentCard();
        paymentCard.CardHolderName = model.CardName;
        paymentCard.CardNumber = model.CardNumber;
        paymentCard.ExpireMonth = model.ExpMonth;
        paymentCard.ExpireYear = model.ExpYear;
        paymentCard.Cvc = model.Cvc;
        paymentCard.RegisterCard = 0;
        request.PaymentCard = paymentCard;

        // paymentCard.CardHolderName = "Emre Guvendiren";
        // paymentCard.CardNumber = "5528790000000008";
        // paymentCard.ExpireMonth = "12";
        // paymentCard.ExpireYear = "2030";
        // paymentCard.Cvc = "123";


        Buyer buyer = new Buyer();
        buyer.Id = "BY789";
        buyer.Name = model.FirstName;
        buyer.Surname = model.LastName;
        buyer.GsmNumber = "+905350000000";
        buyer.Email = "email@email.com";
        buyer.IdentityNumber = "74300864791";
        buyer.LastLoginDate = "2015-10-05 12:43:35";
        buyer.RegistrationDate = "2013-04-21 15:12:09";
        buyer.RegistrationAddress = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
        buyer.Ip = "85.34.78.112";
        buyer.City = "Istanbul";
        buyer.Country = "Turkey";
        buyer.ZipCode = "34732";
        request.Buyer = buyer;

        Address shippingAddress = new Address();
            shippingAddress.ContactName = "Jane Doe";
            shippingAddress.City = "Istanbul";
            shippingAddress.Country = "Turkey";
            shippingAddress.Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            shippingAddress.ZipCode = "34742";
            request.ShippingAddress = shippingAddress;

        Address billingAddress = new Address();
            billingAddress.ContactName = "Jane Doe";
            billingAddress.City = "Istanbul";
            billingAddress.Country = "Turkey";
            billingAddress.Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            billingAddress.ZipCode = "34742";
            request.BillingAddress = billingAddress;

        List<BasketItem> basketItems = new List<BasketItem>();
        BasketItem basketitem;
        foreach (var item in model.SepetModel.SepetItems)
        {
            basketitem = new BasketItem();
            basketitem.Id = item.ProductId.ToString();
            basketitem.Name = item.Name;
            basketitem.Category1="Telefon";
            basketitem.Price = item.Price.ToString();
            basketitem.ItemType= BasketItemType.PHYSICAL.ToString();
            basketItems.Add(basketitem);
        }
        request.BasketItems = basketItems;

        return Payment.Create(request, options);
        }
    }
}