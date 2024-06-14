using Microsoft.AspNetCore.Mvc;
using WebBanHaiSan.Models;
using Microsoft.EntityFrameworkCore;
using WebBanHaiSan.Helpers;
using WebBanHaiSan.ViewModels;
using Microsoft.AspNetCore.Authorization;
using WebBanHaiSan.Helper;
using WebBanHaiSan.Services;

namespace WebBanHaiSan.Controllers
{
    public class CartController : Controller
    {
        private readonly QlbanhangHsContext _context;
        private readonly IVnPayService _vnPayservice;

        public CartController(QlbanhangHsContext context, IVnPayService vnPayservice) {
            _context = context;
            _vnPayservice = vnPayservice;
        }
        public List<CartItem> Cart => HttpContext.Session.Get<List<CartItem>>
            (MySetting.CART_KEY) ?? new List<CartItem>();
        [Route("cart.html", Name = "Cart")]
        public IActionResult Index()
        {
            return View(Cart);
        }
        public IActionResult AddToCart(int id, int quantity = 1)
        {
            var giohang = Cart;
            try
            {
                var item = giohang.SingleOrDefault(p => p.MaHh == id);
                if (item == null)
                {
                    var SP = _context.Products.Include(p => p.SizeNavigation).SingleOrDefault(p => p.ProductId == id);
                    if (SP == null)
                    {
                        TempData["Message"] = $"Không tìm thấy hàng hóa có mã {id}";
                    }
                    item = new CartItem
                    {
                        MaHh = SP.ProductId,
                        TenHh = SP.Productname,
                        PricePd = SP.Price ?? 0,
                        Image = SP.Thumb ?? string.Empty,
                        Discount = SP.Discount ?? 0,
                        Soluong = quantity,
                        size = SP.SizeNavigation.Decription ?? string.Empty,
                        maxStock = SP.Stock ?? 0
                    };
                    giohang.Add(item);
                }
                else
                {
                    item.Soluong += quantity;
                }
                HttpContext.Session.Set(MySetting.CART_KEY, giohang);
                return RedirectToAction("Index", "ProductHome");
            }
            catch
            {
                return RedirectToAction("Index", "ProductHome");
            }
        }
        [HttpPost]
        [Route("api/cart/update")]
        public IActionResult UpdateToCart(int id, int? quantity)
        {
            var giohang = Cart;
            try
            {
                if (giohang != null)
                {
                    var item = giohang.SingleOrDefault(p => p.MaHh == id);
                    if (item != null && quantity.HasValue)
                    {
                        item.Soluong = quantity.Value;
                    }
                    HttpContext.Session.Set(MySetting.CART_KEY, giohang);
                }
                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
        }
        public IActionResult RemoveCart(int id)
        {
            var giohang = Cart;
            var item = giohang.SingleOrDefault(p => p.MaHh == id);
            if (item != null)
            {
                giohang.Remove(item);
                HttpContext.Session.Set(MySetting.CART_KEY, giohang);
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        [Authorize]
        [Route("checkout.html", Name = "CheckOut")]
        public IActionResult CheckOut(string returnUrl = null)
        {
            var giohang = Cart;
            var taikhoanID = HttpContext.Session.GetString("CustomerId");
            CheckoutVM model = new CheckoutVM();
            if (taikhoanID != null)
            {
                var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.CustomerId == Convert.ToInt32(taikhoanID));
                model.CustomerId = khachhang.CustomerId;
                model.FullName = khachhang.FullName;
                model.Email = khachhang.Email;
                model.Phone = khachhang.Phone;
                model.Address = khachhang.Address;
            }
            ViewBag.Giohang = giohang;
            return View(model);
        }
        [HttpPost]
        [Authorize]
        [Route("checkout.html", Name = "CheckOut")]
        public IActionResult CheckOut(CheckoutVM model, string payment = "COD")
        {
            if (ModelState.IsValid)
            {
                TempData["Address"] = model.Address;
                TempData["Phone"] = model.Phone;
                TempData["Note"] = model.Note;

                if (payment == "Thanh Toán Bằng VNPay")
                {
                    Order order = new Order();
                    var vnPayModel = new VnPaymentRequestModel
                    {
                        Amount = Cart.Sum(p => p.Total),
                        CreatedDate = DateTime.Now,
                        Decription = $"{model.FullName} {model.Phone}",
                        FullName = model.FullName,
                        OrderId = order.OrderId,
                    };
                    return Redirect(_vnPayservice.CreatePaymentUrl(HttpContext, vnPayModel));
                }
                var giohang = Cart;
                var taikhoanID = int.Parse(HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySetting.CLAIM_CUSTOMERID).Value);
                var khachhang = new Customer();
                if (taikhoanID != null)
                {
                    khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.CustomerId == taikhoanID);
                }

                Order donhang = new Order();
                donhang.CustomerId = taikhoanID;
                donhang.Address = model.Address;
                donhang.Phone = model.Phone;
                donhang.OrderDate = DateTime.Now;
                donhang.StatusId = 0;
                donhang.PaymentId = payment;
                donhang.ShipDetail = "Giao hàng Tận Nơi";
                donhang.Note = model.Note;
                donhang.TotalMoney = Convert.ToInt32(Cart.Sum(x => x.Total));

                _context.Database.BeginTransaction();
                try
                {
                    _context.Database.CommitTransaction();
                    _context.Add(donhang);
                    _context.SaveChanges();

                    var cthd = new List<OrderDetail>();
                    foreach (var item in giohang)
                    {
                        cthd.Add(new OrderDetail
                        {
                            OrderId = donhang.OrderId,
                            Amount = item.Soluong,
                            Price = item.PricePd,
                            ProductId = item.MaHh,
                            Discount = (int?)item.Discount,
                            Total = (int?)((item.Soluong * item.PricePd) - ((item.Discount/100) * item.PricePd))
                        });
                        var product = _context.Products.SingleOrDefault(p => p.ProductId == item.MaHh);
                        if (product != null)
                        {
                            product.Stock -= item.Soluong;
                            _context.Update(product);
                        }
                    }
                    _context.AddRange(cthd);
                    _context.SaveChanges();
                    HttpContext.Session.Set<List<CartItem>>(MySetting.CART_KEY, new List<CartItem>());


                    ViewBag.GioHang = giohang;
                    return View("Success");
                }
                catch
                {
                    _context.Database.RollbackTransaction();
                }
            }
            return View(Cart);
        }
        [Authorize]
        public IActionResult PaymentSuccess()
        {
            return View("Success");
        }
        [Authorize]
        public IActionResult PaymentFail()
        {
            return View();
        }
        [Authorize]
        public IActionResult PaymentCallBack(Order order)
        {
            var response = _vnPayservice.PaymentExecute(Request.Query);

            if(response == null || response.VnPayResponseCode != "00")
            {
                TempData["Message"] = $"Lỗi Thanh Toán VNPay: {response.VnPayResponseCode}";
                return RedirectToAction("PaymentFail");
            }
            try
            {
                var address = TempData["Address"];
                var phone = TempData["Phone"];
                var note = TempData["Note"];
                var giohang = Cart;
                var taikhoanID = int.Parse(HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySetting.CLAIM_CUSTOMERID).Value);
                var khachhang = new Customer();
                if (taikhoanID != null)
                {
                    khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.CustomerId == taikhoanID);
                }
                order.CustomerId = taikhoanID;
                order.Address = address.ToString();
                order.Phone = Convert.ToInt32(phone);
                order.OrderDate = DateTime.Now;
                order.StatusId = 1;
                order.PaymentId = "Thanh Toán Bằng VNPay";
                order.ShipDetail = "Giao hàng Tận Nơi";
                order.Note = note.ToString();
                order.TotalMoney = Convert.ToInt32(Cart.Sum(x => x.Total));


                _context.Database.BeginTransaction();
                try
                {
                    _context.Database.CommitTransaction();
                    _context.Orders.Update(order);
                    _context.SaveChanges();

                    var cthd = new List<OrderDetail>();
                    foreach (var item in giohang)
                    {
                        cthd.Add(new OrderDetail
                        {
                            OrderId = order.OrderId,
                            Amount = item.Soluong,
                            Price = item.PricePd,
                            ProductId = item.MaHh,
                            Discount = (int?)item.Discount,
                            Total = (int?)((item.Soluong * item.PricePd) - ((item.Discount/100) * item.PricePd))
                        });
                        var product = _context.Products.SingleOrDefault(p => p.ProductId == item.MaHh);
                        if (product != null)
                        {
                            product.Stock -= item.Soluong;
                            _context.Update(product);
                        }
                    }
                    _context.AddRange(cthd);
                    _context.SaveChanges();
                    HttpContext.Session.Set<List<CartItem>>(MySetting.CART_KEY, new List<CartItem>());

                    ViewBag.GioHang = giohang;
                    return View("Success");
                }
                catch
                {
                    _context.Database.RollbackTransaction();
                }
                TempData["Message"] = $"Thanh Toán VNPay thành Công !";
                return View("PaymentSuccess");
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"Lỗi khi cập nhật đơn hàng: {ex.Message}";
                return RedirectToAction("PaymentFail");
            }
        }
    }
}
