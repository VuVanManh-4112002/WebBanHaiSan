using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebBanHaiSan.ViewModels
{
    public class AcountVM
    {
        public int CustomerID { get; set; }
        [Display(Name = "Họ Và tên")]
        [Required(ErrorMessage = "Hãy nhập Họ Và Tên")]
        [MaxLength(100, ErrorMessage = "Tối Đa 100 kí tự")]
        public string? FullName { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Birthday { get; set; }

        public string? Gender { get; set; }

        public string? Avatar { get; set; }

        public string? Address { get; set; }
        [Display(Name = "Email Đăng Nhập")]
        [EmailAddress(ErrorMessage = "Chưa đúng định dạng Email")]
        [Remote(action: "ValidateEmail", controller: "Acount")]
        public string? Email { get; set; }
        [Display(Name = "Số Điện Thoại")]
        [Remote(action: "ValidatePhone", controller: "Acount")]
        public int? Phone { get; set; }
    }
}
