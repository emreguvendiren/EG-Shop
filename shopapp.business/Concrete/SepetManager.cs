using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using shopapp.business.Abstract;
using shopapp.data.Abstract;
using shopapp.entity;

namespace shopapp.business.Concrete
{
    public class SepetManager : ISepetService
    {
        private ISepetRepository _sepetRepository;
        public SepetManager(ISepetRepository sepetRepository)
        {
            this._sepetRepository = sepetRepository;
        }

        public void AddToCart(string userId, int productId, int quantity)
        {
            var cart = GetCartByUserId(userId);
            if (cart!=null)
            {
                var index = cart.Sepetitems.FindIndex(i=>i.ProductId==productId);
                if (index<0)
                {
                    cart.Sepetitems.Add(new Sepetitem(){
                        ProductId = productId,
                        Quantity = quantity,
                        SepetId = cart.id
                    });
                }
                else
                {
                    cart.Sepetitems[index].Quantity+=quantity;
                }
            }
            _sepetRepository.Update(cart);
        }

        public void DeleteFromCart(string userId, int productId)
        {
            var cart = GetCartByUserId(userId);
            if (cart!=null)
            {
                _sepetRepository.DeleteFromCart(cart.id,productId);
            }
        }

        public Sepet GetCartByUserId(string userId)
        {
            return _sepetRepository.getByUserId(userId);
        }

        public void InitializeCart(string userId)
        {
            _sepetRepository.Create(new Sepet(){UserId = userId});
        }
    }
}