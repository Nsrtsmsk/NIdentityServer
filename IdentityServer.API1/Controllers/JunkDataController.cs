using IdentityServer.API1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.API1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class JunkDataController : Controller
    {
        [HttpGet]
        [Authorize]
        public IActionResult GetJunkData()
        {
            var JunkDatas = new List<JunkModel>()
            {
                 new JunkModel
                { Author = "Api1",
                    Description = "Api1 Description",
                    Id = 1,
                    SomethingElse = "Some long information about API 1",
                    Name = "JunkDataController/GetJunkDataRun"
                },
                 new JunkModel
                 {
                     Author = "Api1",
                     Description = "Api1 Description is the second Id's data.",
                     Id = 2,
                     SomethingElse = "Some another long long information about API 1",
                     Name = "JunkDataController/GetJunkDataRun"
                 }
            };

            return Ok(JunkDatas);
        }
    }
}
