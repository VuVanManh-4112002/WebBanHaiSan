using WebBanHaiSan.Models;

namespace WebBanHaiSan.ViewModels
{
    public class XemDonHang
    {
        public Order DonHang { get; set; }
        public List<OrderDetail> ChiTietDonHang { get; set; }
    }
}
