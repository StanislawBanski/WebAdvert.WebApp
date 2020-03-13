using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAdvert.Web.ServiceClients;

namespace WebAdvert.Web.Controllers
{
    [Route("api")]
    [ApiController]
    [Produces("application/json")]
    public class InternalApis : Controller
    {
        private readonly IAdvertApiClient advertApiClient;

        public InternalApis(IAdvertApiClient advertApiClient)
        {
            this.advertApiClient = advertApiClient;
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetAsync(string id)
        {
            var record = await advertApiClient.GetAsync(id).ConfigureAwait(false);
            return Json(record);
        }
    }
}