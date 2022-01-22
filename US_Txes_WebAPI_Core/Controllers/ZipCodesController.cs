﻿using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using US_Txes_WebAPI_Core.DbRepositories;
using US_Txes_WebAPI_Core.Models;

namespace US_Txes_WebAPI_Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZipCodesController : ControllerBase
    {
        private readonly IDbRepository<ZipCode> _zipCodesRepository;
        private readonly IDbRepository<State> _statesRepository;
        public ZipCodesController(IDbRepository<ZipCode> zipCodesRepository, IDbRepository<State> statesRepository)
        {
            _zipCodesRepository = zipCodesRepository;
            _statesRepository = statesRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ZipCode>>> Get()
        {
            var ZipCodes = await _zipCodesRepository.GetAllEntities();

            return new ObjectResult(ZipCodes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ZipCode>> Get(int id)
        {
            var ZipCode = await _zipCodesRepository.FindByID(id);

            return new ObjectResult(ZipCode);
        }

        [HttpPost]
        public async Task<ActionResult<ZipCode>> Post(ZipCode zipCodeInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                var isKnownZipCode = await _zipCodesRepository.IsEntityExists(zipCodeInfo);

                if (isKnownZipCode)
                {
                    return BadRequest("ZipCode with specified Value already exists.");
                }
                else
                {
                    var knowState = await _statesRepository.FindByID(zipCodeInfo.StateID);
                    if (knowState == null)
                    {
                        return BadRequest("Specified State does not exist.");
                    }

                    var result = await _zipCodesRepository.Create(zipCodeInfo);

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
        public async Task<ActionResult<ZipCode>> Put(ZipCode zipCodeInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                var knownZipCode = await _zipCodesRepository.FindByID(zipCodeInfo.ZipCodeID);

                if (knownZipCode == null)
                {
                    return NotFound();
                }
                else
                {
                    var knowState = await _statesRepository.FindByID(zipCodeInfo.StateID);
                    if (knowState == null)
                    {
                        return BadRequest("Specified State does not exist.");
                    }

                    var result = await _zipCodesRepository.Update(zipCodeInfo);

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
        public async Task<ActionResult<ZipCode>> Delete(int id)
        {
            var isDeleted = await _zipCodesRepository.DeleteByID(id);

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
