using AdvertApi.Models;

namespace WebAdvert.Web.ServiceClients.AdvertApi
{
    public class ConfirmAdvertRequest
    {
        public string Id { get; set; }
        public string FilePath { get; set; }
        public AdvertStatus Status { get; set; }
    }
}
