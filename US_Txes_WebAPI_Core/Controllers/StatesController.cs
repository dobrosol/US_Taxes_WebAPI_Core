using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using US_Txes_WebAPI_Core.DbRepositories;
using US_Txes_WebAPI_Core.Extensions;
using US_Txes_WebAPI_Core.Models;

namespace US_Txes_WebAPI_Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatesController : ControllerBase
    {
        private readonly IDbRepository<State> _statesRepository;
        private readonly IDbRepository<ZipCode> _zipCodesRepository;
        public StatesController(IDbRepository<State> statesRepository, IDbRepository<ZipCode> zipCodesRepository)
        {
            _statesRepository = statesRepository;
            _zipCodesRepository = zipCodesRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<State>>> Get()
        {
            var states = await _statesRepository.GetAllEntities();

            return new ObjectResult(states);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<State>> Get(int id)
        {
            var state = await _statesRepository.FindByID(id);

            return new ObjectResult(state);
        }

        [HttpPost]
        public async Task<ActionResult<State>> Post(State stateInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                var isKnownState = await _statesRepository.IsEntityExists(stateInfo);

                if (isKnownState)
                {
                    return BadRequest("State with specified Name/Abbrevation already exists.");
                }
                else
                {
                    if (stateInfo.ZipCodes.IsAny())
                    {
                        foreach (var zipCode in stateInfo.ZipCodes)
                        {
                            var isKnownZipCode = await _zipCodesRepository.IsEntityExists(zipCode);

                            if (isKnownZipCode)
                            {
                                return BadRequest($"ZipCode {zipCode.Value} already exists.");
                            }
                        }
                    }
                    
                    var result = await _statesRepository.Create(stateInfo);

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
        public async Task<ActionResult<State>> Put(State stateInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                var knownState = await _statesRepository.FindByID(stateInfo.StateID);

                if (knownState == null)
                {
                    return NotFound();
                }
                else
                {
                    var result = await _statesRepository.Update(stateInfo);

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
        public async Task<ActionResult<State>> Delete(int id)
        {
            var associatedZipCodes = await _zipCodesRepository.GetAllEntitiesByParentID(id);
            if (associatedZipCodes.IsAny())
            {
                return Problem("Cannot remove state with specified id. It contains at least 1 associated ZipCode.");
            }

            var isDeleted = await _statesRepository.DeleteByID(id);

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
