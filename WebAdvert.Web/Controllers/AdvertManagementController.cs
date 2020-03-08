using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using WebAdvert.Web.Interfaces;
using WebAdvert.Web.Models.AdvertManagement;
using WebAdvert.Web.ServiceClients;
using WebAdvert.Web.ServiceClients.AdvertApi;

namespace WebAdvert.Web.Controllers
{
    public class AdvertManagementController : Controller
    {
        private readonly IFileUploader fileUploader;
        private readonly IAdvertApiClient advertApiClient;
        private readonly IMapper mapper;

        public AdvertManagementController(
            IFileUploader fileUploader,
            IAdvertApiClient advertApiClient,
            IMapper mapper
        )
        {
            this.fileUploader = fileUploader;
            this.advertApiClient = advertApiClient;
            this.mapper = mapper;
        }

        public IActionResult Create(CreateAdvertViewModel model)
        {
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAdvertViewModel model, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                var createAdvertModel = mapper.Map<CreateAdvertModel>(model);
                var apiCallResponse = await advertApiClient.CreateAsync(createAdvertModel);

                if (string.IsNullOrEmpty(apiCallResponse.Id))
                {
                    throw new Exception($"Cannot create new advert");
                }

                var id = apiCallResponse.Id;

                var fileName = "";
                if (imageFile != null)
                {
                    fileName = !string.IsNullOrEmpty(imageFile.FileName) ? Path.GetFileName(imageFile.FileName) : id;
                    var filePath = $"{id}/{fileName}";

                    try
                    {
                        using (var readStream = imageFile.OpenReadStream())
                        {
                            var result = await fileUploader.UploadFileAsync(filePath, readStream);
                            if (!result)
                                throw new Exception(
                                    "Could not upload the image to file repository. Please see the logs for details.");
                        }

                        var confirmModel = new ConfirmAdvertRequest() { 
                            Id = id,
                            FilePath = filePath,
                            Status = AdvertApi.Models.AdvertStatus.Active
                        };

                        var canConfirm = await advertApiClient.ConfirmAsync(confirmModel);

                        if (!canConfirm)
                        {
                            throw new Exception($"Cannot confirm advert of id={id}");
                        }

                        return RedirectToAction("Index", "Home");
                    }
                    catch (Exception e)
                    {
                        var confirmModel = new ConfirmAdvertRequest()
                        {
                            Id = id,
                            FilePath = filePath,
                            Status = AdvertApi.Models.AdvertStatus.Pending
                        };

                        var canConfirm = await advertApiClient.ConfirmAsync(confirmModel);

                        Console.WriteLine(e); // use logger
                    }
                }
            }

            return View(model);
        }
    }
}