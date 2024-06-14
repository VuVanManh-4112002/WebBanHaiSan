
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebBanHaiSan.ViewModels
{
    public class RegisterVM
    {
        [Key]
        public int CustomerId { get; set; }

        [Display(Name = "Họ Và tên")]
        [Required(ErrorMessage = "Hãy nhập Họ Và Tên")]
        [MaxLength(100, ErrorMessage ="Tối Đa 100 kí tự")]
        public string FullName { get; set; }

        [Display(Name ="Email Đăng Nhập")]
        [EmailAddress(ErrorMessage = "Chưa đúng định dạng Email")]
        [Remote(action: "ValidateEmail", controller:"Acount")]
        public string Email { get; set; }

        [Display(Name = "Số Điện Thoại")]
        [Remote(action: "ValidatePhone", controller: "Acount")]
        public int Phone { get; set; }

        [Display(Name = "Mật Khẩu")]
        [Required(ErrorMessage = "Hãy Nhập Mật Khẩu")]
        [MinLength(5, ErrorMessage = "Bạn cần nhập mật khẩu tối thiểu 5 ký tự")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Xác Nhận Mật Khẩu")]
        [Required(ErrorMessage = "Vui Lòng Nhập Mật Khẩu Giống Nhau")]
        [MinLength(5, ErrorMessage = "Bạn cần nhập mật khẩu tối thiểu 5 ký tự")]
        [Compare("Password", ErrorMessage = "Mật Khẩu Không Giống Nhau")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

    }
}
