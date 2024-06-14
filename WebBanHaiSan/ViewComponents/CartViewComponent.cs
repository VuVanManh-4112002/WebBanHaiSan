using Microsoft.AspNetCore.Mvc;
using WebBanHaiSan.ViewModels;
using WebBanHaiSan.Helpers;
namespace WebBanHaiSan.ViewComponents
{
    public class CartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var cart = HttpContext.Session.Get<List<CartItem>>
                (MySetting.CART_KEY) ?? new List<CartItem> ();
            return View("CartPanel",new CartModel
            {
                quantity = cart.Count(),
                Tong = cart.Sum(p => p.Total),
            });
        }
    }
}
