using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using WebAdvert.Web.Interfaces;
using WebAdvert.Web.Models.AdvertManagement;

namespace WebAdvert.Web.Controllers
{
    public class AdvertManagementController : Controller
    {
        private readonly IFileUploader fileUploader;

        public AdvertManagementController(IFileUploader fileUploader)
        {
            this.fileUploader = fileUploader;
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
                var id = "111111";
                // call advert api to store details to get real id

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

                        // call advert api to confirm advert

                        return RedirectToAction("Index", "Home");
                    }
                    catch (Exception e)
                    {
                        // call advert api to cancel advert
                        Console.WriteLine(e);
                    }
                }
            }

            return View(model);
        }
    }
}