using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using US_Txes_WebAPI_Core.DbRepositories;
using US_Txes_WebAPI_Core.Models;

namespace US_Txes_WebAPI_Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleFeesController : ControllerBase
    {
        private readonly IDbEntityRepository<VehicleFee> _vehicleFeesRepository;
        private readonly IDbEntityRepository<State> _statesRepository;
        public VehicleFeesController(IDbEntityRepository<VehicleFee> vehicleFeesRepository, IDbEntityRepository<State> statesRepository)
        {
            _vehicleFeesRepository = vehicleFeesRepository;
            _statesRepository = statesRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VehicleFee>>> Get()
        {
            var vehicleFees = await _vehicleFeesRepository.GetAllEntities();

            return new ObjectResult(vehicleFees);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VehicleFee>> Get(int id)
        {
            var vehicleFee = await _vehicleFeesRepository.FindByID(id);

            return new ObjectResult(vehicleFee);
        }

        [HttpPost]
        public async Task<ActionResult<VehicleFee>> Post(VehicleFee vehicleFeeInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                var isKnownVehicleFee = await _vehicleFeesRepository.IsEntityExists(vehicleFeeInfo);

                if (isKnownVehicleFee)
                {
                    return BadRequest($"{vehicleFeeInfo.Type.ToString()} VehicleFee for {vehicleFeeInfo.StateID} state already exists.");
                }
                else
                {
                    var knowState = await _statesRepository.FindByID(vehicleFeeInfo.StateID);
                    if (knowState == null)
                    {
                        return BadRequest("Specified State does not exist.");
                    }

                    var result = await _vehicleFeesRepository.Create(vehicleFeeInfo);

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
        public async Task<ActionResult<VehicleFee>> Put(VehicleFee vehicleFeeInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                var knownVehicleFee = await _vehicleFeesRepository.FindByID(vehicleFeeInfo.VehicleFeeID);

                if (knownVehicleFee == null)
                {
                    return NotFound();
                }
                else
                {
                    var result = await _vehicleFeesRepository.Update(vehicleFeeInfo);

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
        public async Task<ActionResult<VehicleFee>> Delete(int id)
        {
            var isDeleted = await _vehicleFeesRepository.DeleteByID(id);

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
