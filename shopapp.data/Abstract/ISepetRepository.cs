using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using shopapp.entity;

namespace shopapp.data.Abstract
{
    public interface ISepetRepository:IRepository<Sepet>
    {
        void DeleteFromCart(int cartId, int productId);
        Sepet getByUserId(string userId);

    }
}