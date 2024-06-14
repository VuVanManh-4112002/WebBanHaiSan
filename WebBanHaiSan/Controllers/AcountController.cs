using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebBanHaiSan.Helper;
using WebBanHaiSan.Helpers;
using WebBanHaiSan.Models;
using WebBanHaiSan.ViewModels;

namespace WebBanHaiSan.Controllers
{
    [Authorize]
    public class AcountController : Controller
    {
        private QlbanhangHsContext _context;

        public AcountController(QlbanhangHsContext context)
        {
            _context = context;
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ValidatePhone(int Phone)
        {
            try
            {
                var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.Phone == Phone);
                if (khachhang != null)
                {
                    return Json(data: "Số Điện Thoại:" + Phone + "Đã được sử dụng");
                }
                return Json(data: true);
            }
            catch 
            {
                return Json(data: true);
            }
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ValidateEmail(string Email)
        {
            try
            {
                var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.Email.ToLower() == Email.ToLower());
                if (khachhang != null)
                {
                    return Json(data: "Số Điện Thoại:" + Email + "Đã được sử dụng");
                }
                return Json(data: true);
            }
            catch
            {
                return Json(data: true);
            }
        }
        [Route("TaiKhoanCuaToi.html", Name = "TaiKhoanCuaToi")]
        public IActionResult Dashboard()
        {
            var taikhoanID = HttpContext.Session.GetString("CustomerId");
            if (taikhoanID != null)
            {
                var khachang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.CustomerId == Convert.ToInt32(taikhoanID));
                if(khachang != null)
                {
                    var LsDonHang = _context.Orders.AsNoTracking()
                        .Include(x => x.Status)
                        .Where(x => x.CustomerId == khachang.CustomerId)
                        .OrderByDescending(x => x.OrderId)
                        .ToList();
                    ViewBag.DSDH = LsDonHang;
                    //var LsSp = _context.OrderDetails.AsNoTracking()
                    //    .Include(p => p.OrderId)
                    //    .Include(p => p.ProductId)
                    //    .OrderByDescending(p => p.ProductId) 
                    //    .ToList();
                    //ViewBag.DS = LsSp;
                    return View(khachang);
                }
            }
            return RedirectToAction("Login");
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Edit()
        {
            var taikhoanID = HttpContext.Session.GetString("CustomerId");
            AcountVM model = new AcountVM();
            if (taikhoanID != null)
            {
                var khachang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.CustomerId == Convert.ToInt32(taikhoanID));
                model.CustomerID = khachang.CustomerId;
                model.FullName = khachang.FullName;
                model.Birthday = khachang.Birthday;              
                model.Avatar = khachang.Avatar ?? string.Empty;
                model.Address = khachang.Address;
                model.Email = khachang.Email;
                model.Phone = khachang.Phone;
            }
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Edit(AcountVM taikhoan, IFormFile Avatar)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var taikhoanID = int.Parse(HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySetting.CLAIM_CUSTOMERID).Value);
                    AcountVM model = new AcountVM();
                    if (taikhoanID != null)
                    {
                        var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.CustomerId == Convert.ToInt32(taikhoanID));
                        khachhang.CustomerId = taikhoanID;
                        khachhang.FullName = taikhoan.FullName;
                        khachhang.Birthday = taikhoan.Birthday;
                        if (taikhoan.Gender == "Nam")
                        {
                            khachhang.Gender = true;
                        }
                        else
                        {
                            khachhang.Gender = false;
                        }
                        if (Avatar != null)
                        {
                            string extension = Path.GetExtension(Avatar.FileName);
                            string image = Utilities.SEOUrl(khachhang.FullName) + extension;
                            khachhang.Avatar = await Utilities.UploadFile(Avatar, @"customers", image.ToLower());
                        }
                        khachhang.Address = taikhoan.Address;
                        khachhang.Email = taikhoan.Email;
                        khachhang.Phone = taikhoan.Phone;
                        _context.Customers.Update(khachhang);
                        await _context.SaveChangesAsync();
                        HttpContext.Session.SetString("CustomerId", khachhang.CustomerId.ToString());

                        var claims = new List<Claim>
                            {
                            new Claim(ClaimTypes.Name, khachhang.FullName),
                            new Claim(MySetting.CLAIM_CUSTOMERID, khachhang.CustomerId.ToString())
                            };
                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "login");
                        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                        await HttpContext.SignInAsync(claimsPrincipal);
                        return RedirectToAction("Dashboard", "Acount");
                    }
                }
                catch
                {
                    return View(taikhoan);
                }
            }
            return RedirectToAction("Login");
        }
        [HttpGet]
        [AllowAnonymous]
        [Route("Register.html", Name = "DangKy")]
        public IActionResult Dangkytaikhoan()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("Register.html", Name ="DangKy")]
        public async Task<IActionResult> DangkyTaiKhoan(RegisterVM taikhoan)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (taikhoan.Password != taikhoan.ConfirmPassword)
                    {
                        ModelState.AddModelError("loi", "Mật Khẩu Và Xác Nhận Mật Khẩu phải trùng nhau");
                        return View(taikhoan);
                    }
                    string randomkey = Utilities.GetRandomKey();
                    Customer khachhang = new Customer
                    {
                        FullName = taikhoan.FullName,
                        Phone = taikhoan.Phone,
                        Email = taikhoan.Email.Trim().ToLower(),
                        Password = (taikhoan.Password + randomkey.Trim()).ToMd5Hash(),
                        Active = true,
                        Randomkey = randomkey,
                    };
                    try
                    {
                        _context.Add(khachhang);
                        await _context.SaveChangesAsync();
                        HttpContext.Session.SetString("CustomerId", khachhang.CustomerId.ToString());
                        var taikhoanID = HttpContext.Session.GetString("CustomerId");

                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, khachhang.FullName),
                            new Claim("CustomerId", khachhang.CustomerId.ToString())
                        };
                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "login");
                        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                        await HttpContext.SignInAsync(claimsPrincipal);
                        return RedirectToAction("Dashboard", "Acount");
                    }
                    catch
                    {
                        return RedirectToAction("DangKyTaiKhoan", "Acount");
                    }
                }
                else
                {
                    return View(taikhoan);
                }
            }
            catch
            {
                return View(taikhoan);
            }
        }
        [HttpGet]
        [AllowAnonymous]
        [Route("Login.html", Name = "DangNhap")]
        public IActionResult Login(string? returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            var taikhoanID = HttpContext.Session.GetString("CustomerId");
            if (taikhoanID != null)
            {
                return RedirectToAction("Dashboard", "Acount");
            }
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("Login.html", Name = "DangNhap")]
        public async Task<IActionResult> Login(LoginVM customer, string? returnUrl)
        {
            try
            {
                ViewBag.ReturnUrl = returnUrl;
                if (ModelState.IsValid)
                {
                    bool isEmail = Utilities.IsValidEmail(customer.UserName);
                    if (!isEmail) { return View(customer); }
                    var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.Email.Trim() == customer.UserName);
                    if (khachhang == null)
                    {
                        ModelState.AddModelError("loi", "Sai Thông Tin Tài Khoản Hoặc Mật Khẩu");
                    }
                    else
                    {
                        string pass = (customer.Password + khachhang.Randomkey.Trim()).ToMd5Hash();
                        if (khachhang.Password != pass)
                        {
                            ModelState.AddModelError("loi", "Sai Thông Tin Tài Khoản Hoặc Mật Khẩu");
                        }
                        else
                        {
                            if (khachhang.Active == false) return RedirectToAction("ThongBao", "Acount");

                            HttpContext.Session.SetString("CustomerId", khachhang.CustomerId.ToString());
                            var taikhoanID = HttpContext.Session.GetString("CustomerId");

                            var claims = new List<Claim>
                            {
                            new Claim(ClaimTypes.Name, khachhang.FullName),
                            new Claim(MySetting.CLAIM_CUSTOMERID, khachhang.CustomerId.ToString())
                            };
                            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "login");
                            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                            await HttpContext.SignInAsync(claimsPrincipal);

                            if (Url.IsLocalUrl(returnUrl))
                            {
                                return Redirect(returnUrl);
                            }
                            else
                            {
                                return RedirectToAction("Dashboard", "Acount");
                            }
                        }

                    }
                }
            }
            catch
            {
                return RedirectToAction("DangkyTaiKhoan", "Acount");
            }
            return View(customer);
        }
        [HttpGet]
        [Route("logout.html",Name ="Logout")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            HttpContext.Session.Remove("CustomerId");
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM model)
        {
            try
            {
                var taikhoanID = HttpContext.Session.GetString("CustomerId");
                if (taikhoanID == null)
                {
                    return RedirectToAction("Login", "Acount");
                }
                if (ModelState.IsValid)
                {
                    var taikhoan = _context.Customers.Find(Convert.ToInt32(taikhoanID));
                    if (taikhoan == null) return RedirectToAction("Login", "Acount");
                    var pass = (model.PasswordNow.Trim() + taikhoan.Randomkey.Trim()).ToMd5Hash();
                    if (pass == taikhoan.Password)
                    {
                        string passnew = (model.Password.Trim() + taikhoan.Randomkey.Trim()).ToMd5Hash();
                        taikhoan.Password = passnew;
                        _context.Update(taikhoan);
                        _context.SaveChanges();
                        return RedirectToAction("Dashboard", "Acount");
                    }
                }
                return RedirectToAction("ChangePassword", "Acount");
            }
            catch
            {
                return RedirectToAction("Dashboard", "Acount");
            }
        }
        public async Task<IActionResult> RemoveOrder(int id)
        {
            var donhang = _context.Orders.Find(id);
            if (ModelState.IsValid)
           {
                try
                {
                   var huy = _context.TransactStatusIds.SingleOrDefault(p => p.StatusId == -1);
                    if (huy != null)
                    {
                        donhang.StatusId = huy.StatusId;
                        _context.SaveChanges();
                        return RedirectToAction("Dashboard", "Acount");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Unable to update order status. " + ex.Message);
                    return RedirectToAction("Dashboard", "Acount");
                }
            }
            return RedirectToAction("Dashboard", "Acount");
        }
    }
}
