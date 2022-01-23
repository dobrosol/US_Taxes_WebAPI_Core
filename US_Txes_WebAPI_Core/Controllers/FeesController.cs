using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using US_Txes_WebAPI_Core.DbRepositories;
using US_Txes_WebAPI_Core.Models;

namespace US_Txes_WebAPI_Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeesController : ControllerBase
    {
        private readonly IDbEntityRepository<ZipCode> _zipCodesRepository;
        private readonly IDbEntityRepository<Fee> _feesRepository;
        public FeesController(IDbEntityRepository<ZipCode> zipCodesRepository, IDbEntityRepository<Fee> feesRepository)
        {
            _zipCodesRepository = zipCodesRepository;
            _feesRepository = feesRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Fee>>> Get()
        {
            var fees = await _feesRepository.GetAllEntities();

            return new ObjectResult(fees);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Fee>> Get(int id)
        {
            var fee = await _feesRepository.FindByID(id);

            return new ObjectResult(fee);
        }

        [HttpPost]
        public async Task<ActionResult<Fee>> Post(Fee feeInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                var isKnownFee = await _feesRepository.IsEntityExists(feeInfo);

                if (isKnownFee)
                {
                    return BadRequest($"Fee for {feeInfo.ZipCodeID} ZipCode already exists.");
                }
                else
                {
                    var knowZipCode = await _zipCodesRepository.FindByID(feeInfo.ZipCodeID);
                    if (knowZipCode == null)
                    {
                        return BadRequest("Specified ZipCode does not exist.");
                    }

                    var result = await _feesRepository.Create(feeInfo);

                    if (result != null)
                    {
                        return Ok(result);
                    }
                    else
                    {
                        return Problem("Internal Server Error");
                    }
                }
            }
        }

        [HttpPut]
        public async Task<ActionResult<Fee>> Put(Fee feeInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                var knownFee = await _feesRepository.FindByID(feeInfo.FeeID);

                if (knownFee == null)
                {
                    return NotFound();
                }
                else
                {
                    var knowZipCode = await _zipCodesRepository.FindByID(feeInfo.ZipCodeID);
                    if (knowZipCode == null)  
                    {
                        return BadRequest("Specified ZipCode does not exist.");
                    }

                    var result = await _feesRepository.Update(feeInfo);

                    if (result != null)
                    {
                        return Ok(result);
                    }
                    else
                    {
                        return Problem("Internal Server Error");
                    }
                }
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Fee>> Delete(int id)
        {
            var isDeleted = await _feesRepository.DeleteByID(id);

            if (!isDeleted)
            {
                return NotFound();
            }
            else
            {
                return Ok();
            }
        }
    }
}
