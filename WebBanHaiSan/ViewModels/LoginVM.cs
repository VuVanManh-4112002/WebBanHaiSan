using System.ComponentModel.DataAnnotations;

namespace WebBanHaiSan.ViewModels
{
    public class LoginVM
    {
        [Key]
        [MaxLength(100)]
        [Required(ErrorMessage ="Vui lòng nhập Email")]
        [Display(Name ="Email")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string UserName { get; set; }

        [Display(Name ="Mật Khẩu")]
        [Required(ErrorMessage ="Vui lòng nhập mật khẩu")]
        [MinLength(5, ErrorMessage ="Bạn cần nhập mật khẩu tối thiểu 5 ký tự")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
