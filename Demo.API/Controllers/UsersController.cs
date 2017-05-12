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
    //[Route("api/Persons")]
    public class UsersController : Controller
    {
        private readonly IAsyncDataRepository<User> _userRepository;
        private readonly IAsyncDataRepository<Person> _personRepository;
        private readonly IChangeLog _log;

        public UsersController(IAsyncDataRepository<Person> personRepository, IAsyncDataRepository<User> userRepository, IChangeLog log)
        {
            this._personRepository = personRepository;
            this._userRepository = userRepository;
            this._log = log;
        }

        [HttpGet("api/[controller]")]
        public async Task<IActionResult> GetAllUsersPersonObjectsAsync()
        {
            var allUsers = await _personRepository.GetFilterByInheritedAsync(typeof(User));
            return Ok(allUsers);
        }

        [HttpGet("api/[controller]/{id}")]
        public async Task<IActionResult> GetUsersPersonObjectAsync(int id)
        {
            var user = await _personRepository.GetFilterByInheritedAsync(typeof(User), id);

            //comment: 
            // User currently User does not contain any fields not implemnted by Person -> No need to get, only validate that the user record exists. 
            // If IUser is populated with fields, replace return an DTO mapped with AutoMapper with values from 
            // _personRepository.GetAsync(id) and _userRepository.GetAsync(id).

            return Ok(user);
        }

        [HttpPost("api/[controller]")]
        public async Task<IActionResult> CreateUserAndPersonAsync([FromBody] Person person)
        {
            if (person == null)
                return BadRequest();

            if (await _personRepository.InsertAsync(person) == -1)
                return StatusCode(StatusCodes.Status500InternalServerError, "Unable to create person (from user creation)");

            var user = new User() { Id = person.Id };
            if (await _userRepository.InsertAsync(user) == -1)
                return StatusCode(StatusCodes.Status500InternalServerError, "Unable to create user - Warning (person created)");

            //comment: 
            // User currently does not contain any field -> No need to update. 
            // If update of User is needed, replace the Person object in '[FromBody] Person' with a DTO with both that implements IPerson and IUser and use Automapper to create Person and User objects

            _log.LogCreated(HttpContext.Request.Method, HttpContext.Request.Path, multipleObjectsToJson(user, person).ToString());

            return CreatedAtRoute(new { person.Id }, person);
        }

        [HttpPut("api/[controller]/{id}")]
        public async Task<IActionResult> UpdateUserAndPersonAsync(int id, [FromBody] Person person)
        {
            if (person == null)
                return BadRequest();

            var oldPerson = await _personRepository.GetFilterByInheritedAsync(typeof(User), id);
            var oldUser = await _userRepository.GetAsync(id);

            if (oldPerson == null || oldUser == null)
                return NotFound();

            person.Id = id;

            if (!await _personRepository.UpdateAsync(person))
                return StatusCode(StatusCodes.Status500InternalServerError, "Unable to update users personal details");


            //comment: 
            // User currently does not contain any field -> No need to update. 
            // If update of User is needed, replace the Person object in '[FromBody] Person' with a DTO with both that implements IPerson and IUser and use Automapper to create Person and User objects. 
            // Then find differences between old and new object on both person and object (using _changeLog.Changes(oldObj, updatedObj) and save to database

            _log.LogUpated(HttpContext.Request.Method, HttpContext.Request.Path, oldPerson, person);

            return NoContent();
        }

        [HttpDelete("api/[controller]/{id}")]
        public async Task<IActionResult> DeleteUserAndPersonAsync(int id)
        {
            var person = await _personRepository.GetAsync(id);
            var user = await _userRepository.GetAsync(id);

            if (user == null || person == null)
                return NotFound();

            if (!await _userRepository.DeleteAsync(user))
                return StatusCode(StatusCodes.Status500InternalServerError, "Unable to delete user");

            if (!await _personRepository.DeleteAsync(person))
                return StatusCode(StatusCodes.Status500InternalServerError, "Unable to delete person");

            _log.LogDeleted(HttpContext.Request.Method, HttpContext.Request.Path, multipleObjectsToJson(user, person).ToString());

            return NoContent();
        }

        private string multipleObjectsToJson(params object[] objects)
        {
            if (objects == null || objects.Length == 0)
                return "";
            if (objects.Length == 1)
                return JsonConvert.SerializeObject(objects[0]);

            return "{ " + string.Join(", ", objects.Select(o => "\"" + o.GetType().Name + "\": \"" + JsonConvert.SerializeObject(o) + "\"")) + " }";
        }

        [HttpGet("api/Persons/{personId}/[controller]")]
        //[HttpGet("{personId}/[controller]")]
        public async Task<IActionResult> GetAsync(int personId)
        {
            if (await _personRepository.GetAsync(personId) == null)
                return NotFound();

            var user = await _userRepository.GetAsync(personId);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost("api/Persons/{personId}/[controller]")]
        //[HttpPost("{personId}/[controller]")]
        public async Task<IActionResult> CreateAsync(int personId, [FromBody]User user)
        {
            if (await _personRepository.GetAsync(personId) == null)
                return NotFound();

            if (await _userRepository.GetAsync(personId) != null)
                return StatusCode(StatusCodes.Status409Conflict, "User allready exists");

            user.Id = personId;

            if (await _userRepository.InsertAsync(user) == -1)
                return StatusCode(StatusCodes.Status409Conflict, "Unable to create user");

            _log.LogCreated(HttpContext.Request.Method, HttpContext.Request.Path, JsonConvert.SerializeObject(user).ToString());

            return CreatedAtRoute(new { user.Id }, user);
        }

        [HttpDelete("api/Persons/{personId}/[controller]")]
        //[HttpDelete("{personId}/[controller]")]
        public async Task<IActionResult> DeleteAsync(int personId)
        {
            if (await _personRepository.GetAsync(personId) == null)
                return NotFound();

            var user = await _userRepository.GetAsync(personId);

            if (user == null)
                return NotFound();

            if (!await _userRepository.DeleteAsync(user))
                return StatusCode(StatusCodes.Status500InternalServerError, "Unable to delete user");

            _log.LogDeleted(HttpContext.Request.Method, HttpContext.Request.Path, JsonConvert.SerializeObject(user).ToString());

            return NoContent();
        }
    }
}