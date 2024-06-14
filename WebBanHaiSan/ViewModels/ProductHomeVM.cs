using WebBanHaiSan.Models;

namespace WebBanHaiSan.ViewModels
{
    public class ProductHomeVM
    {
        public Cart category { get; set; }
        public List<Product> lsProducts { get; set; }
    }
}
