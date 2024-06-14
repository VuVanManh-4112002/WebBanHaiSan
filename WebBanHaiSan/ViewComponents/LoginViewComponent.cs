using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBanHaiSan.Models;
using WebBanHaiSan.ViewModels;

namespace WebBanHaiSan.ViewComponents
{
    public class LoginViewComponent : ViewComponent
    {
        private readonly QlbanhangHsContext db;
        public LoginViewComponent(QlbanhangHsContext context) => db = context;
        public IViewComponentResult Invoke()
        {
            var taikhoanID = HttpContext.Session.GetString("CustomerId");

            if (taikhoanID == null)
            {
                return View(new AcountHomeVM
                {
                    Name = null
                });
            }
            else
            {
                var khachang = db.Customers.AsNoTracking().SingleOrDefault(x => x.CustomerId == Convert.ToInt32(taikhoanID));
                return View(new AcountHomeVM
                {
                    Name = khachang.FullName
                });
            }
        }
    }
}
