using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.API2.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]

    public class DummyDataController : Controller
    {
        [HttpGet]
        [Authorize]
        public IActionResult GetDummyData()
        {
            var Data = new List<DummyModel>()
            {new DummyModel
                {
                    Author = "Author is me",
                    Description = "Description",
                    Name = "Name",
                    Id = 1
              },
              new DummyModel
                {
                    Author = "Author is someoneelse",
                    Description = "Description",
                    Name = "Name",
                    Id = 2
              },
            };
            return Ok(Data);
        }
    }
}
