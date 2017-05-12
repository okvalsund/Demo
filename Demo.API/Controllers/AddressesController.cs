using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Demo.Core;
using Demo.Repository.Entities;
using Newtonsoft.Json;

namespace Demo.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Persons")]
    public class AddressesController : Controller
    {
        private readonly IAsyncDataRepository<Address> _addressRepository;
        private readonly IAsyncDataRepository<Person> _personRepository;
        private readonly IChangeLog _log;

        public AddressesController(IAsyncDataRepository<Person> personRepository, IAsyncDataRepository<Address> addressRepository, IChangeLog log)
        {
            this._personRepository = personRepository;
            this._addressRepository = addressRepository;
            this._log = log;
        }

        [HttpGet("{personId}/[controller]")]
        public async Task<IActionResult> GetAsync(int personId)
        {
            if (await _personRepository.GetAsync(personId) == null)
                return NotFound();

            var addresses = await _addressRepository.GetFilterByForeignKey(personId, nameof(Address.PersonId));

            if (addresses == null)
                return NotFound();

            return Ok(addresses);
        }
        
        [HttpGet("{personId}/[controller]/{id}", Name = nameof(GetAddressAsync))]
        public async Task<IActionResult> GetAddressAsync(int personId, int id)
        {   
            if (await _personRepository.GetAsync(personId) == null)
                return NotFound();

            var address = await _addressRepository.GetByForeignKeyAsync(personId, nameof(Address.PersonId), id);

            if (address == null)
                return NotFound();

            return Ok(address);
        }
        
        [HttpPost("{personId}/[controller]")]
        public async Task<IActionResult> CreateAsync(int personId, [FromBody]Address address)
        {
            if (address == null)
                return BadRequest();

            if (await _personRepository.GetAsync(personId) == null)
                return NotFound();

            address.PersonId = personId;

            if(await _addressRepository.InsertAsync(address) == -1)
                return StatusCode(StatusCodes.Status500InternalServerError, "Unable to create person");

            _log.LogCreated(HttpContext.Request.Method, HttpContext.Request.Path, JsonConvert.SerializeObject(address).ToString());

            return CreatedAtRoute(nameof(this.GetAddressAsync), new { personId = address.PersonId, id = address.Id }, address);
        }
        
        [HttpPut("{personId}/[controller]/{id}")]
        public async Task<IActionResult> UpdateAsync(int personId, int id, [FromBody]Address address)
        {
            if (address == null)
                return BadRequest();

            if (await _personRepository.GetAsync(personId) == null)
                return NotFound();

            var oldAddress = await _addressRepository.GetByForeignKeyAsync(personId, nameof(Address.PersonId), id);
            if (oldAddress == null)
                return NotFound();

            address.Id = id;
            address.PersonId = personId;

            if (!await _addressRepository.UpdateAsync(address))
                return StatusCode(StatusCodes.Status500InternalServerError, "Unable to update address");

            _log.LogUpated(HttpContext.Request.Method, HttpContext.Request.Path, oldAddress, address);

            return NoContent();
        }
        
        [HttpDelete("{personId}/[controller]/{id}")]
        public async Task<IActionResult> DeleteAsync(int personId, int id)
        {
            if (await _personRepository.GetAsync(personId) == null)
                return NotFound();

            var address = await _addressRepository.GetByForeignKeyAsync(personId, nameof(Address.PersonId), id);

            if (address == null)
                return NotFound();

            if (!await _addressRepository.DeleteAsync(address))
                return StatusCode(StatusCodes.Status500InternalServerError, "Unable to delete address");

            _log.LogDeleted(HttpContext.Request.Method, HttpContext.Request.Path, JsonConvert.SerializeObject(address).ToString());

            return NoContent();
        }
    }
}
