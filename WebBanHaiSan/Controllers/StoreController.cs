using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using WebBanHaiSan.Models;

namespace WebBanHaiSan.Controllers
{
    public class StoreController : Controller
    {
        private readonly QlbanhangHsContext _context;
        public StoreController(QlbanhangHsContext context)
        {
            _context = context;
        }
        public IActionResult Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 10;
            var lsStores = _context.Stores
                .Where(x => x.Active == true)
                .AsNoTracking()
                .OrderByDescending(x => x.StoreId);
            PagedList<Store> models = new PagedList<Store>(lsStores, pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;
            return View(models);
        }
        [Route("/cuahang/{id}.html", Name = "CuaHangDetails")]
        public IActionResult Details(int id)
        {
            var cuahang = _context.Stores.AsNoTracking().SingleOrDefault(x => x.StoreId == id);
            if (cuahang == null)
            {
                return RedirectToAction("Index");
            }
            var LsProduct = _context.Products.AsNoTracking().Where(x => x.Store.StoreId == id).OrderByDescending(x => x.ProductId).ToList();
            ViewBag.DSSP = LsProduct;
            return View(cuahang);
        }
    }
}
