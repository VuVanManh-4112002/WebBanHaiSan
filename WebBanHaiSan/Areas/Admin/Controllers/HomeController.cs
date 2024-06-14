using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebBanHaiSan.Areas.Admin.Models;
using WebBanHaiSan.Models;
using Microsoft.AspNetCore.Authorization;
using WebBanHaiSan.ViewModels;
using WebBanHaiSan.Helper;
using WebBanHaiSan.Helpers;

namespace WebBanHaiSan.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private QlbanhangHsContext _context;

        public HomeController(QlbanhangHsContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var totalProducts = _context.Products.Count();
            var totalCustomers = _context.Customers.Count();
            var totalOrders = _context.Orders.Count();

            // Assuming you have a model called Order with a Date and TotalAmount field
            var dailyRevenue = _context.Orders
                .Where(p => p.StatusId.Value == 1 || p.StatusId.Value == 3)
                .GroupBy(p => p.OrderDate)
                .Select(g => new DailyRevenue
                {
                    Date = g.Key,
                    Total = g.Sum(p => p.TotalMoney)
                })
                .ToList();
            var totalRevenue = _context.Orders.Where(p => p.StatusId.Value == 1 || p.StatusId.Value == 3).Sum(p => p.TotalMoney);
            var model = new HomeMV
            {
                TotalProducts = totalProducts,
                TotalRevenue = totalRevenue,
                TotalCustomers = totalCustomers,
                TotalOrders = totalOrders,
                DailyRevenue = dailyRevenue
            };

            return View(model);
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM account)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool isEmail = Utilities.IsValidEmail(account.UserName);
                    if (!isEmail) { return View("Login"); }
                    var taikhoan = _context.Accounts.AsNoTracking().SingleOrDefault(x => x.Email.Trim() == account.UserName);
                    if (taikhoan == null)
                    {
                        ModelState.AddModelError("loi", "Sai Thông Tin Tài Khoản Hoặc Mật Khẩu");
                    }
                    else
                    {
                        string pass = account.Password;
                        if (taikhoan.Password != pass)
                        {
                            ModelState.AddModelError("loi", "Sai Thông Tin Tài Khoản Hoặc Mật Khẩu");
                        }
                        else
                        {
                            if (taikhoan.Active == false) return RedirectToAction("ThongBao", "Acount");
                            return RedirectToAction("Index", "Home");
                        }

                    }
                }
                return View("Login");
            }
            catch
            {
                return View("Login");
            }
        }
    }
}
