using WebBanHaiSan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace WebBanHaiSan.ViewModels
{
    public class CartItem
    {
        public int MaHh { get; set; }
        public string? TenHh { get; set; }
        public string? Price { get; set; }
        public string? Image { get; set; }
        public int PricePd { get; set; }
        public int Soluong { get; set; }
        public double Discount { get; set; }
        public double Total => Soluong * PricePd;
        public double TotalDiscount => (Discount/100) * PricePd;
        public double TotalPD => Total - TotalDiscount;
        public int maxStock { get; set; }
        public string size { get; set; }

    }
}
