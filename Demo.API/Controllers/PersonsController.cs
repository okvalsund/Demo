using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using Demo.Core;
using Demo.Repository.Entities;
using Newtonsoft.Json;

namespace Demo.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Persons")]
    public class PersonsController : Controller
    {
        private readonly IAsyncDataRepository<Person> _personRepository;
        private readonly IChangeLog _log;
        public PersonsController(IAsyncDataRepository<Person> personRepository, IChangeLog log)
        {
            _personRepository = personRepository;
            _log = log;
        }

        [HttpGet()]
        public async Task<IActionResult> GetPersonsAsync()
        {
            var allPersons = await _personRepository.GetAllAsync();
            return Ok(allPersons);
        }

        [HttpGet("{id}", Name = "GetPersonAsync")]
        public async Task<IActionResult> GetPersonAsync(int id)
        {
            var person = await _personRepository.GetAsync(id);

            if (person == null)
                return NotFound();

            return Ok(person);
        }

        [HttpGet("Search/{filter}")]
        public async Task<IActionResult> GetFilteredPersonsAsync(string filter)
        {
            var allPersons = await _personRepository.GetAllAsync();

            //pga dårlig tid implenterte jeg ikke metode i repository på dette.
            // ville laget sql som filtrerer med sqlParameter (for å unngå dependency injection)
            // Muligens join mot andre tabeller.

            var filtered = allPersons.Where(p => p.FirstName.IndexOf(filter, StringComparison.CurrentCultureIgnoreCase) >= 0 ||
                                  p.MiddleName.IndexOf(filter, StringComparison.CurrentCultureIgnoreCase) >= 0 ||
                                  p.LastName.IndexOf(filter, StringComparison.CurrentCultureIgnoreCase) >= 0).ToList();

            return Ok(filtered);
        }


        [HttpPost]
        public async Task<IActionResult> CreatePersonAsync([FromBody] Person person)
        {
            if (person == null)
                return BadRequest();

            if (await _personRepository.InsertAsync(person) == -1)
                return StatusCode(StatusCodes.Status500InternalServerError, "Unable to create person");

            _log.LogCreated(HttpContext.Request.Method, HttpContext.Request.Path, JsonConvert.SerializeObject(person).ToString());

            return CreatedAtRoute(new { person.Id }, person);
        }

        [HttpPut("{id}", Name = "UpdatePersonAsync")]
        public async Task<IActionResult> UpdatePersonAsync(int id, [FromBody] Person person)
        {
            if (person == null)
                return BadRequest();

            var oldPerson = await _personRepository.GetAsync(id);
            if (oldPerson == null)
                return NotFound();


            if (!await _personRepository.UpdateAsync(person))
                return StatusCode(StatusCodes.Status500InternalServerError, "Unable to update person");

            _log.LogUpated(HttpContext.Request.Method, HttpContext.Request.Path, oldPerson, person);

            return NoContent();
        }

        //[HttpPatch("{id}")]
        //public async Task<IActionResult> PatchPersonAsync(int id, [FromBody]JsonPatchDocument<Person> person)
        //{
        //    if (person == null)
        //        return BadRequest();

        //    var currentPerson = await _personRepository.GetAsync(id);

        //    //return StatusCode(StatusCodes.Status204NoContent);
        //    return StatusCode(StatusCodes.Status501NotImplemented);
        //}

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var person = await _personRepository.GetAsync(id);

            if (person == null)
                return NotFound();

            if (!await _personRepository.DeleteAsync(person))
                return StatusCode(StatusCodes.Status500InternalServerError, "Unable to delete person");

            _log.LogDeleted(HttpContext.Request.Method, HttpContext.Request.Path, JsonConvert.SerializeObject(person).ToString());

            return NoContent();
        }
    }
}