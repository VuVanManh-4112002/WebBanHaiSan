using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using WebBanHaiSan.Models;
using WebBanHaiSan.ViewModels;

namespace WebBanHaiSan.Controllers
{
    public class ProductHomeController : Controller
    {
        private readonly QlbanhangHsContext _context;
        public ProductHomeController(QlbanhangHsContext context)
        {
            _context = context;
        }
        public IActionResult Index(int? page)
        {
            try
            {
                var pageNumber = page == null || page <= 0 ? 1 : page.Value;
                var pageSize = 9;
                var lsProduct = _context.Products
                    .Where(x => x.Stock > 0)
                    .AsNoTracking()
                    .OrderByDescending(x => x.ProductId);
                PagedList<Product> models = new PagedList<Product>(lsProduct, pageNumber, pageSize);
                ViewBag.CurrentPage = pageNumber;
                return View(models);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public IActionResult List(int CatID, int page=1)
        {
            try
            {
                var pageSize = 9;
                var danhmuc = _context.Carts.AsNoTracking().SingleOrDefault(x => x.CartId == CatID);
                var lsProduct = _context.Products
                    .AsNoTracking()
                    .Where(x => x.CartId == CatID && x.Stock > 0)
                    .OrderByDescending(x => x.Price);
                PagedList<Product> models = new PagedList<Product>(lsProduct, page, pageSize);
                ViewBag.CurrentPage = page;
                ViewBag.CurrentCat = danhmuc;
                return View(models);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public IActionResult Details(int id)
        {
            try
            {
                var product = _context.Products.Include(x => x.Cart).Include(x => x.Store).Include(x => x.SizeNavigation).SingleOrDefault(x => x.ProductId == id);
                if (product == null)
                {
                    return RedirectToAction("Index");
                }

                var LsProduct = _context.Products.AsNoTracking().Where(x=>x.CartId==product.CartId && x.ProductId!=id)
                    .OrderByDescending(x=>x.Price)
                    .Take(6)
                    .ToList();

                ViewBag.SanPham = LsProduct;
                return View(product);


            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public IActionResult Search(string query, int page = 1)
        {
            //var product = _context.Products.AsQueryable();
            //if(query != null)
            //{
            //    product = product.Where(p => p.Productname.Contains(query));
            //}
            //var result = product.Select(p => p.ProductId).ToList();
            //return View(result);
            try
            {
                var pageSize = 9;
                var lsProduct = _context.Products
                    .AsNoTracking()
                    .Where(x => x.Productname.Contains(query) && x.Stock > 0)
                    .OrderByDescending(x => x.Price);
                PagedList<Product> models = new PagedList<Product>(lsProduct, page, pageSize);
                ViewBag.CurrentPage = page;
                return View(models);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
