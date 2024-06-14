using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using WebBanHaiSan.Models;

namespace WebBanHaiSan.Controllers
{
    public class PostController : Controller
    {
        private readonly QlbanhangHsContext _context;
        public PostController(QlbanhangHsContext context)
        {
            _context = context;
        }
        public IActionResult Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 10;
            var lsTinDangs = _context.Posts
                .AsNoTracking()
                .OrderByDescending(x => x.PostId);
            PagedList<Post> models = new PagedList<Post>(lsTinDangs, pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;
            return View(models);
        }
        [Route("/tintuc/{id}.html", Name ="TinDetails")]
        public IActionResult Details(int id)
        {
            var tindang = _context.Posts.AsNoTracking().SingleOrDefault(x => x.PostId == id);
            if (tindang == null)
            {
                return RedirectToAction("Index");
            }
            var LsBaivietlienquan = _context.Posts.AsNoTracking().Where(x => x.PostId !=id).Take(3).OrderByDescending(x =>x.Creatdate).ToList();
            ViewBag.BVLQ = LsBaivietlienquan;
            return View(tindang);
        }
    }
}
