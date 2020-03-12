﻿using AdvertApi.Models;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebAdvert.Web.ServiceClients.AdvertApi;

namespace WebAdvert.Web.ServiceClients
{
    public class AdvertApiClient : IAdvertApiClient
    {
        private readonly HttpClient client;
        private readonly IMapper mapper;
        private readonly string baseAddress;

        public AdvertApiClient(
            IConfiguration configuration,
            HttpClient client,
            IMapper mapper
        )
        {
            this.client = client;
            this.mapper = mapper;
            this.baseAddress = configuration.GetSection("AdvertApi").GetValue<string>("BaseUrl");
        }

        public async Task<AdvertResponse> CreateAsync(CreateAdvertModel model)
        {
            var advertApiModel = mapper.Map<CreateAdvertModel>(model);
            var jsonModel = JsonConvert.SerializeObject(advertApiModel);

            var response = await client.PostAsync(new Uri($"{baseAddress}/Create"), 
                new StringContent(jsonModel, Encoding.UTF8, "application/json"));

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var createAdvertResponse = JsonConvert.DeserializeObject<CreateAdvertResponse>(jsonResponse);

            var advertResponse = mapper.Map<AdvertResponse>(createAdvertResponse);

            return advertResponse;
        }

        public async Task<bool> ConfirmAsync(ConfirmAdvertRequest model)
        {
            var confirmAdvertApiModel = mapper.Map<ConfirmAdvertModel>(model);
            var jsonModel = JsonConvert.SerializeObject(confirmAdvertApiModel);

            var response = await client.PutAsync(new Uri($"{baseAddress}/Confirm"), 
                new StringContent(jsonModel, Encoding.UTF8, "application/json"));

            return response.StatusCode == System.Net.HttpStatusCode.OK;
        }

        public async Task<List<Advertisement>> GetAllAsync()
        {
            var apiCallResponse = await client.GetAsync(new Uri($"{baseAddress}/all")).ConfigureAwait(false);
            var allAdvertModels = await apiCallResponse.Content.ReadAsAsync<List<AdvertModel>>().ConfigureAwait(false);
            return allAdvertModels.Select(x => mapper.Map<Advertisement>(x)).ToList();
        }

        public async Task<Advertisement> GetAsync(string advertId)
        {
            var apiCallResponse = await client.GetAsync(new Uri($"{baseAddress}/{advertId}")).ConfigureAwait(false);
            var fullAdvert = await apiCallResponse.Content.ReadAsAsync<AdvertModel>().ConfigureAwait(false);
            return mapper.Map<Advertisement>(fullAdvert);
        }
    }
}
