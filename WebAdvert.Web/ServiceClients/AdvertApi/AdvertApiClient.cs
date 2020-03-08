using AdvertApi.Models;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebAdvert.Web.ServiceClients.AdvertApi;

namespace WebAdvert.Web.ServiceClients
{
    public class AdvertApiClient : IAdvertApiClient
    {
        private readonly IConfiguration configuration;
        private readonly HttpClient client;
        private readonly IMapper mapper;

        public AdvertApiClient(
            IConfiguration configuration,
            HttpClient client,
            IMapper mapper
        )
        {
            this.configuration = configuration;
            this.client = client;
            this.mapper = mapper;
            client.BaseAddress = new Uri(configuration.GetSection("AdvertApi").GetValue<string>("BaseUrl"));
        }

        public async Task<AdvertResponse> CreateAsync(CreateAdvertModel model)
        {
            var advertApiModel = mapper.Map<CreateAdvertModel>(model);
            var jsonModel = JsonConvert.SerializeObject(advertApiModel);

            var response = await client.PostAsync(client.BaseAddress + "Create", 
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

            var response = await client.PutAsync(client.BaseAddress + "Confirm", 
                new StringContent(jsonModel, Encoding.UTF8, "application/json"));

            return response.StatusCode == System.Net.HttpStatusCode.OK;
        }
    }
}
