using AutoMapper;
using WebAdvert.Web.Models.AdvertManagement;
using WebAdvert.Web.ServiceClients.AdvertApi;

namespace WebAdvert.Web.Helpers
{
    public class WebSiteProfile : Profile
    {
        public WebSiteProfile()
        {
            CreateMap<CreateAdvertViewModel, CreateAdvertModel>().ReverseMap();
        }
    }
}
