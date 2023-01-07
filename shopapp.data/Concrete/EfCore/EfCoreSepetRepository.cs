using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using shopapp.data.Abstract;
using shopapp.entity;

namespace shopapp.data.Concrete.EfCore
{
    public class EfCoreSepetRepository : EfCoreGenericRepository<Sepet, ShopContext>, ISepetRepository
    {
        public void DeleteFromCart(int cartId, int productId)
        {
            using(var context = new ShopContext()){
                var cmd = @"delete from Sepetitems where SepetId=@p0 and ProductId = @p1";
                context.Database.ExecuteSqlRaw(cmd,cartId,productId);
            }
        }

        public Sepet getByUserId(string userId)
        {
            using(var context = new ShopContext()){
                return context.Sepets
                .Include(i=>i.Sepetitems)
                .ThenInclude(i=>i.Product)
                .FirstOrDefault(i=>i.UserId == userId);
            }
        }
        public override void Update(Sepet entity)
        {
            using (var context = new ShopContext())
            {
                context.Sepets.Update(entity);
                context.SaveChanges();
            }
            
        }
    }
}