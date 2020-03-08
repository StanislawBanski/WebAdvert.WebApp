﻿using System.Threading.Tasks;
using AdvertApi.Models;
using WebAdvert.Web.ServiceClients.AdvertApi;

namespace WebAdvert.Web.ServiceClients
{
    public interface IAdvertApiClient
    {
        Task<AdvertResponse> CreateAsync(CreateAdvertModel model);
        Task<bool> ConfirmAsync(ConfirmAdvertRequest model);
        //Task<List<Advertisement>> GetAllAsync();
        //Task<Advertisement> GetAsync(string advertId);
    }
}
