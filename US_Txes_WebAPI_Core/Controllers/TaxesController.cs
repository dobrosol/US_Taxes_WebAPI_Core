using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using US_Txes_WebAPI_Core.DbRepositories;
using US_Txes_WebAPI_Core.Models;

namespace US_Txes_WebAPI_Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaxesController : ControllerBase
    {
        private readonly ITaxesRepository _taxesRepository;
        public TaxesController(ITaxesRepository taxesRepository)
        {
            _taxesRepository = taxesRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tax>>> Get()
        {            
            var taxes = await _taxesRepository.CalculateAllTaxes();

            return new ObjectResult(taxes);
        }

        [HttpPost]
        public async Task<ActionResult<Tax>> Post(TaxRequest request)
        {
            var tax = await _taxesRepository.CalculateTax(request.StateAbbreviation, request.ZipCode, request.VehicleType);

            return new ObjectResult(tax);
        }
    }
}
