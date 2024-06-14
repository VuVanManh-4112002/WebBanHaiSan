using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebBanHaiSan.Models;
using WebBanHaiSan.ViewModels;

namespace WebBanHaiSan.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private QlbanhangHsContext _context;

        public HomeController(ILogger<HomeController> logger, QlbanhangHsContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            HomeViewVM model = new HomeViewVM();

            var lsProducts = _context.Products.AsNoTracking()
                .Where(x => x.Homeflag == true)
                .OrderByDescending(p => p.ProductId)
                .Take(16)
                .ToList();

            List<ProductHomeVM> lsProductViews = new List<ProductHomeVM>();

            var lsCats = _context.Carts
                .AsNoTracking()
                .OrderByDescending(x => x.CartId)
                .ToList();
            foreach (var item in lsCats)
            {
                ProductHomeVM productHome = new ProductHomeVM();
                productHome.category = item;
                productHome.lsProducts = lsProducts.Where(x => x.CartId == item.CartId).ToList();
                lsProductViews.Add(productHome);
            }

            var TinTuc = _context.Posts
                .AsNoTracking()
                .OrderByDescending(x => x.PostId)
                .Take(3)
                .ToList();
            model.Product = lsProductViews;
            model.Posts = TinTuc;
            ViewBag.AllProducts = lsProducts;
            return View(model);
        }
        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Danhmuc()
        {
            return View();
        }
        public IActionResult Taikhoan()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}