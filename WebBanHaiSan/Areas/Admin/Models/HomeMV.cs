namespace WebBanHaiSan.Areas.Admin.Models
{
    public class HomeMV
    {
        public int TotalProducts { get; set; }
        public int? TotalRevenue { get; set; }
        public int TotalCustomers { get; set; }
        public int TotalOrders { get; set; }
        public List<DailyRevenue>? DailyRevenue { get; set; }
    }

    public class DailyRevenue
    {
        public DateTime? Date { get; set; }
        public int? Total { get; set; }
    }
}
