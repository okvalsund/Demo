using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Demo.Repository.Entities;
using Demo.Core;
using Newtonsoft.Json;

namespace Demo.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Persons")]
    public class EmailsController : Controller
    {
        private readonly IAsyncDataRepository<Email> _emailRepository;
        private readonly IAsyncDataRepository<Person> _personRepository;
        private readonly IChangeLog _log;

        public EmailsController(IAsyncDataRepository<Person> personRepository, IAsyncDataRepository<Email> emailRepository, IChangeLog log)
        {
            this._personRepository = personRepository;
            this._emailRepository = emailRepository;
            this._log = log;
        }

        [HttpGet("{personId}/[controller]")]
        public async Task<IActionResult> GetAsync(int personId)
        {
            if (await _personRepository.GetAsync(personId) == null)
                return NotFound();

            var emails = await _emailRepository.GetFilterByForeignKey(personId, nameof(Email.PersonId));

            if (emails == null)
                return NotFound();

            return Ok(emails);
        }

        [HttpGet("{personId}/[controller]/{id}", Name = nameof(GetEmailAsync))]
        public async Task<IActionResult> GetEmailAsync(int personId, int id)
        {
            if (await _personRepository.GetAsync(personId) == null)
                return NotFound();

            var email = await _emailRepository.GetByForeignKeyAsync(personId, nameof(Email.PersonId), id);

            if (email == null)
                return NotFound();

            return Ok(email);
        }

        [HttpPost("{personId}/[controller]")]
        public async Task<IActionResult> CreateAsync(int personId, [FromBody]Email email)
        {
            if (email == null)
                return BadRequest();

            if (await _personRepository.GetAsync(personId) == null)
                return NotFound();

            email.PersonId = personId;

            if(await _emailRepository.InsertAsync(email) == -1)
                return StatusCode(StatusCodes.Status500InternalServerError, "Unable to create person");

            _log.LogCreated(HttpContext.Request.Method, HttpContext.Request.Path, JsonConvert.SerializeObject(email).ToString());

            return CreatedAtRoute(nameof(this.GetEmailAsync), new { personId = email.PersonId, id = email.Id }, email);
        }

        [HttpPut("{personId}/[controller]/{id}")]
        public async Task<IActionResult> UpdateAsync(int personId, int id, [FromBody]Email email)
        {
            if (email == null)
                return BadRequest();

            if (await _personRepository.GetAsync(personId) == null)
                return NotFound();

            var oldEmail = await _emailRepository.GetByForeignKeyAsync(personId, nameof(Email.PersonId), id);
            if (oldEmail == null)
                return NotFound();

            email.Id = id;
            email.PersonId = personId;

            if (!await _emailRepository.UpdateAsync(email))
                return StatusCode(StatusCodes.Status500InternalServerError, "Unable to update email");

            _log.LogUpated(HttpContext.Request.Method, HttpContext.Request.Path, oldEmail, email);

            return NoContent();
        }

        [HttpDelete("{personId}/[controller]/{id}")]
        public async Task<IActionResult> DeleteAsync(int personId, int id)
        {
            if (await _personRepository.GetAsync(personId) == null)
                return NotFound();

            var email = await _emailRepository.GetByForeignKeyAsync(personId, nameof(Email.PersonId), id);

            if (email == null)
                return NotFound();

            if (!await _emailRepository.DeleteAsync(email))
                return StatusCode(StatusCodes.Status500InternalServerError, "Unable to delete email");

            _log.LogDeleted(HttpContext.Request.Method, HttpContext.Request.Path, JsonConvert.SerializeObject(email).ToString());

            return NoContent();
        }
    }
}
