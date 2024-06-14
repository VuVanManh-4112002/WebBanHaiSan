using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using WebBanHaiSan.Helpers;
using WebBanHaiSan.Models;
using WebBanHaiSan.ViewModels;

namespace WebBanHaiSan.ViewComponents
{
    public class StoreViewComponent : ViewComponent
    {
        private readonly QlbanhangHsContext db;
        public StoreViewComponent(QlbanhangHsContext context) => db = context;
        public IViewComponentResult Invoke()
        {
            var data = db.Stores.Select(lo => new StoreVM
            {
                StoreID = lo.StoreId,
                StoreName = lo.StoreName
            });
            return View(data);
        }
    }
}
