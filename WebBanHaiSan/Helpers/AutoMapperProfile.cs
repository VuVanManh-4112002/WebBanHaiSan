using AutoMapper;
using WebBanHaiSan.Models;
using WebBanHaiSan.ViewModels;

namespace WebBanHaiSan.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<RegisterVM, Customer>();
                //.ForMember(kh => kh.Fullname, option => option.MapFrom(RegisterVM => RegisterVM.Fullname)).ReverseMap();
        }
    }
}
