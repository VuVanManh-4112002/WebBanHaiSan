using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using WebBanHaiSan.Helper;
using WebBanHaiSan.Models;

namespace WebBanHaiSan.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly QlbanhangHsContext _context;

        public ProductController(QlbanhangHsContext context)
        {
            _context = context;
        }

        // GET: Admin/Product
        public IActionResult Index(int page=1, int CartID = 0)
        {
            var pageNumber = page;
            var pageSize = 10;
            List<Product> lsProducts = new List<Product>();
            if (CartID != 0)
            {
                lsProducts = _context.Products
                .AsNoTracking()
                .Where(p => p.CartId==CartID)
                .Include(p => p.Cart)
                .Include(p => p.SizeNavigation)
                .Include(p => p.Store)
                .OrderByDescending(x => x.Price).ToList();
            }
            else
            {
                lsProducts = _context.Products
                .AsNoTracking()
                .Include(p => p.Cart)
                .Include(p => p.SizeNavigation)
                .Include(p => p.Store)
                .OrderByDescending(x => x.ProductId).ToList();
            }
            PagedList<Product> models = new PagedList<Product>(lsProducts.AsQueryable(), pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;
            ViewBag.CurrentCartID = CartID;
            ViewData["DanhMuc"] = new SelectList(_context.Carts, "CartId", "CartName", CartID);
            ViewData["Size"] = new SelectList(_context.SizeIds, "Size", "Decription");
            ViewData["CuaHang"] = new SelectList(_context.Stores, "StoreId", "StoreName");
            return View(models);
        }
        public IActionResult Filtter(int CartID = 0)
        {
            var url = $"/Admin/Product/Index?CartID={CartID}";
            if (CartID == 0)
            {
                url = $"/Admin/Product/Index";
            }
            return Json(new { status = "success", redirectUrl = url});
        }
        // GET: Admin/Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Cart)
                .Include(p => p.SizeNavigation)
                .Include(p => p.Store)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Admin/Product/Create
        public IActionResult Create()
        {
            ViewData["DanhMuc"] = new SelectList(_context.Carts, "CartId", "CartName");
            ViewData["Size"] = new SelectList(_context.SizeIds, "Size", "Decription");
            ViewData["CuaHang"] = new SelectList(_context.Stores, "StoreId", "StoreName");
            return View();
        }

        // POST: Admin/Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,Productname,CartId,Origin,Size,StatusPd,Price,Discount,Thumb,Video,Homeflag,Bestseller,Title,Stock,StoreId")] Product product, Microsoft.AspNetCore.Http.IFormFile fthumb)
        {
            if (ModelState.IsValid)
            {
                product.Productname = Utilities.TotitleCase(product.Productname);
                if (fthumb != null)
                {
                    string extension = Path.GetExtension(fthumb.FileName);
                    string image = Utilities.SEOUrl(product.Productname) + extension;
                    product.Thumb = await Utilities.UploadFile(fthumb, @"products", image.ToLower());
                }
                if (string.IsNullOrEmpty(product.Thumb)) product.Thumb = "default.jpg";
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DanhMuc"] = new SelectList(_context.Carts, "CartId", "CartName", product.CartId);
            ViewData["Size"] = new SelectList(_context.SizeIds, "Size", "Decription", product.Size);
            ViewData["CuaHang"] = new SelectList(_context.Stores, "StoreId", "StoreName", product.StoreId);
            return View(product);
        }

        // GET: Admin/Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["DanhMuc"] = new SelectList(_context.Carts, "CartId", "CartName", product.CartId);
            ViewData["Size"] = new SelectList(_context.SizeIds, "Size", "Decription", product.Size);
            ViewData["CuaHang"] = new SelectList(_context.Stores, "StoreId", "StoreName", product.StoreId);
            return View(product);
        }

        // POST: Admin/Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,Productname,CartId,Origin,Size,StatusPd,Price,Discount,Thumb,Video,Homeflag,Bestseller,Title,Stock,StoreId")] Product product, Microsoft.AspNetCore.Http.IFormFile fthumb = null)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    product.Productname = Utilities.TotitleCase(product.Productname);
                    if (fthumb != null)
                    {
                        string extension = Path.GetExtension(fthumb.FileName);
                        string image = Utilities.SEOUrl(product.Productname) + extension;
                        product.Thumb = await Utilities.UploadFile(fthumb, @"products", image.ToLower());
                    }
                    if (string.IsNullOrEmpty(product.Thumb)) product.Thumb = "default.jpg";
                    if (_context.Carts.Any(c => c.CartId == product.CartId))
                    {
                        _context.Update(product);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("CartId", "CartId không hợp lệ.");
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            ViewData["DanhMuc"] = new SelectList(_context.Carts, "CartId", "CartName", product.CartId);
            ViewData["Size"] = new SelectList(_context.SizeIds, "Size", "Decription", product.Size);
            ViewData["CuaHang"] = new SelectList(_context.Stores, "StoreId", "StoreName", product.StoreId);
            return View(product);
        }

        // GET: Admin/Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Cart)
                .Include(p => p.SizeNavigation)
                .Include(p => p.Store)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Admin/Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'QlbanhangHsContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return (_context.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}
