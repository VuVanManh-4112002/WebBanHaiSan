using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBanHaiSan.Models;
using WebBanHaiSan.ViewModels;

namespace WebBanHaiSan.ViewComponents
{
    public class MenuDanhMucViewComponent : ViewComponent
    {
        private readonly QlbanhangHsContext db;
        public MenuDanhMucViewComponent(QlbanhangHsContext context) => db = context;
        public IViewComponentResult Invoke()
        {
            //var data = db.Carts.AsNoTracking()
            //.OrderByDescending(x => x.CartId)
            //.ToList();
            var data = db.Carts.Select(lo => new DanhmucVM
            {
                CatID = lo.CartId,
                CatName = lo.CartName,
                Soluong = lo.Products.Count
            });
            return View(data);

        }
    }
}
