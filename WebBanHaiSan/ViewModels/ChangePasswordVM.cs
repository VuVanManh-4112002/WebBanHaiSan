using System.ComponentModel.DataAnnotations;

namespace WebBanHaiSan.ViewModels
{
    public class ChangePasswordVM
    {
        [Key]
        public int CustomerId { get; set; }
        [Display(Name = "Mật Khẩu Hiện Tại")]
        public string PasswordNow { get; set; }
        [Display(Name = "Mật Khẩu Mới")]
        [Required(ErrorMessage ="Vui Lòng Nhập Mật Khẩu")]
        [MinLength(5, ErrorMessage = "Bạn cần nhập mật khẩu tối thiểu 5 ký tự")]
        public string Password { get; set; }
        [MinLength(5, ErrorMessage = "Bạn cần nhập mật khẩu tối thiểu 5 ký tự")]
        [Display(Name = "Nhập Lại Mật Khẩu Mới")]
        [Compare("Password", ErrorMessage ="Mật Khẩu Không Giống Nhau")]
        public string ConfirmPassword { get; set; }
    }
}
