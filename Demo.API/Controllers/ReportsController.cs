using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Demo.Core;
using Demo.Repository.Entities;
using System.Text;

namespace Demo.API.Controllers
{
    [Produces("text/html")]
    [Route("api/Reports")]
    public class ReportsController : Controller
    {
        private readonly IAsyncDataRepository<Person> _personRepository;
        private readonly IAsyncDataRepository<User> _userRepository;
        public ReportsController(IAsyncDataRepository<Person> personRepository, IAsyncDataRepository<User> userRepository)
        {
            _personRepository = personRepository;
            _userRepository = userRepository;
        }

        [HttpGet("PersonsReport")]
        public async Task<IActionResult> GetPersonReport()
        {
            var persons = await _personRepository.GetAllAsync();

            if (persons == null)
                return NotFound();

            return Ok(generateHtml(persons.ToList(), "Persons"));
        }

        [HttpGet("UsersReport")]
        public async Task<IActionResult> GetUserReport()
        {
            var users = await _personRepository.GetFilterByInheritedAsync(typeof(User));

            if (users == null)
                return NotFound();

            return Ok(generateHtml(users.ToList(), "Users"));
        }

        private string generateHtml(List<Person> persons, string type)
        {
            //could include related info -> email, address..
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"<h1>{type} - Report</h1>");
            builder.AppendLine("<ul>");
            foreach (var person in persons)
                builder.AppendLine($"<li>First Name: {person.FirstName}, Middle Name: {person.MiddleName}, Last Name: {person.LastName}</li>");
            builder.AppendLine("</ul>");

            return builder.ToString();
        }
    }
}