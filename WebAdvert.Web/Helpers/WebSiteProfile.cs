using AdvertApi.Models;
using AutoMapper;
using WebAdvert.Web.Models;
using WebAdvert.Web.Models.AdvertManagement;
using WebAdvert.Web.Models.Home;
using WebAdvert.Web.ServiceClients;
using WebAdvert.Web.ServiceClients.AdvertApi;

namespace WebAdvert.Web.Helpers
{
    public class WebSiteProfile : Profile
    {
        public WebSiteProfile()
        {
            CreateMap<CreateAdvertViewModel, CreateAdvertModel>().ReverseMap();

            CreateMap<AdvertModel, Advertisement>().ReverseMap();

            CreateMap<Advertisement, IndexViewModel>()
                .ForMember(
                    dest => dest.Title, src => src.MapFrom(f => f.Title)
                )
                .ForMember(
                    dest => dest.Image, src => src.MapFrom(f => f.FilePath)
                );

            CreateMap<AdvertType, SearchViewModel>()
                .ForMember(
                    dest => dest.Id, src => src.MapFrom(f => f.Id)
                )
                .ForMember(
                    dest => dest.Title, src => src.MapFrom(f => f.Title)
                );
        }
    }
}
