using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using US_Txes_WebAPI_Core.DbRepositories;

namespace US_Txes_WebAPI_Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InitializerController : ControllerBase
    {
        private readonly IHelperRepository _helperRepository;
        public InitializerController(IHelperRepository helperRepository)
        {
            _helperRepository = helperRepository;
        }

        [HttpPut]
        public async Task<ActionResult> Put()
        {
            await _helperRepository.InitializeData();

            return Ok();
        }
    }
}
