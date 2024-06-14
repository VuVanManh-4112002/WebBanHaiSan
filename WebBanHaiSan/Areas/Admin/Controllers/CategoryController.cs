using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebBanHaiSan.Helper;
using WebBanHaiSan.Models;

namespace WebBanHaiSan.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly QlbanhangHsContext _context;

        public CategoryController(QlbanhangHsContext context)
        {
            _context = context;
        }

        // GET: Admin/Cart
        public async Task<IActionResult> Index()
        {
            return _context.Carts != null ?
                        View(await _context.Carts.ToListAsync()) :
                        Problem("Entity set 'QlbanhangHsContext.Carts'  is null.");
        }

        // GET: Admin/Cart/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Carts == null)
            {
                return NotFound();
            }

            var carts = await _context.Carts
                .FirstOrDefaultAsync(m => m.CartId == id);
            if (carts == null)
            {
                return NotFound();
            }

            return View(carts);
        }

        // GET: Admin/Cart/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Cart/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CartId,CartName,Title,Cover")] Cart carts, Microsoft.AspNetCore.Http.IFormFile fthumb)
        {
            if (ModelState.IsValid)
            {
                carts.CartName = Utilities.TotitleCase(carts.CartName);
                if (fthumb != null)
                {
                    string extension = Path.GetExtension(fthumb.FileName);
                    string image = Utilities.SEOUrl(carts.CartName) + extension;
                    carts.Cover = await Utilities.UploadFile(fthumb, @"categorys", image.ToLower());
                }
                if (string.IsNullOrEmpty(carts.CartName)) carts.CartName = "default.jpg";
                _context.Add(carts);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(carts);
        }

        // GET: Admin/Cart/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Carts == null)
            {
                return NotFound();
            }

            var carts = await _context.Carts.FindAsync(id);
            if (carts == null)
            {
                return NotFound();
            }
            return View(carts);
        }

        // POST: Admin/Cart/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CartId,CartName,Title,Cover")] Cart carts, Microsoft.AspNetCore.Http.IFormFile fthumb = null)
        {
            if (id != carts.CartId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    carts.CartName = Utilities.TotitleCase(carts.CartName);
                    if (fthumb != null)
                    {
                        string extension = Path.GetExtension(fthumb.FileName);
                        string image = Utilities.SEOUrl(carts.CartName) + extension;
                        carts.Cover = await Utilities.UploadFile(fthumb, @"categorys", image.ToLower());
                    }
                    if (string.IsNullOrEmpty(carts.CartName)) carts.CartName = "default.jpg";
                    _context.Update(carts);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartExists(carts.CartId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(carts);
        }

        // GET: Admin/Cart/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Carts == null)
            {
                return NotFound();
            }

            var carts = await _context.Carts
                .FirstOrDefaultAsync(m => m.CartId == id);
            if (carts == null)
            {
                return NotFound();
            }

            return View(carts);
        }

        // POST: Admin/Cart/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Carts == null)
            {
                return Problem("Entity set 'QlbanhangHsContext.Carts'  is null.");
            }
            var carts = await _context.Carts.FindAsync(id);
            if (carts != null)
            {
                _context.Carts.Remove(carts);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartExists(int id)
        {
            return (_context.Carts?.Any(e => e.CartId == id)).GetValueOrDefault();
        }
    }
}
