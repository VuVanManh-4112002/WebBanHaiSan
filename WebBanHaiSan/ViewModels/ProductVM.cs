namespace WebBanHaiSan.ViewModels
{
    public class ProductVM
    {
        public int PdId { get; set; }
        public string PdName { get; set; }
        public string Image { get; set; }
        public int PricePD { get; set; }
        public string Mota { get; set; }
        public string CatName { get; set;}
    }

    public class ProductDetailVM
    {
        public int PdId { get; set; }
        public string PdName { get; set; }
        public string Image { get; set; }
        public string SizePD { get; set; }
        public int PricePD { get; set; }
        public string Mota { get; set; }
        public string CatName { get; set; }
        public int SoluongTon { get; set; }
    }
}
