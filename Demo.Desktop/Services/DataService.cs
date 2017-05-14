using Demo.Desktop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Desktop.DAL
{
    public class DataService
    {
        private readonly string personsUrl = "http://localhost:58139/api/persons/";
        private readonly string reportsUrl = "http://localhost:58139/api/reports/";

        public DataService()
        {
            
        }

        public async Task<IEnumerable<Person>> GetPersons()
        {
            HttpClient client = getApplicationJsonHttpClient();
            HttpResponseMessage response = await client.GetAsync(personsUrl);
            return await response.Content.ReadAsAsync<IEnumerable<Person>>();
        }

        public async Task<Person> GetPerson(int personId)
        {
            HttpClient client = getApplicationJsonHttpClient();
            HttpResponseMessage response = await client.GetAsync(personsUrl + personId);
            return await response.Content.ReadAsAsync<Person>();
        }

        public async Task<IEnumerable<Person>> GetPersonsByFilter(string search)
        {
            HttpClient client = getApplicationJsonHttpClient();
            HttpResponseMessage response = await client.GetAsync(personsUrl +"search/" +search);
            return await response.Content.ReadAsAsync<IEnumerable<Person>>();
        }

        public async Task<bool> AddPerson(Person person)
        {
            HttpClient client = getApplicationJsonHttpClient();
            HttpResponseMessage post = await client.PostAsJsonAsync<Person>(personsUrl, person);

            var location = post.Headers.Location;
            //hack instead of reload of data..
            int newId = 0;
            int.TryParse(location.Segments.Last(), out newId);
            person.Id = newId;
            return post.IsSuccessStatusCode;
        }
        public async Task<bool> UpdatePerson(Person person)
        {
            HttpClient client = getApplicationJsonHttpClient();
            HttpResponseMessage put = await client.PutAsJsonAsync<Person>(personsUrl + person.Id, person);

            return put.IsSuccessStatusCode;
        }

        public async Task<bool> DeletePerson(Person person)
        {
            HttpClient client = getApplicationJsonHttpClient();
            HttpResponseMessage delete = await client.DeleteAsync(personsUrl + person.Id);
            return delete.IsSuccessStatusCode;
        }

        public async Task<bool> AddUser(User user)
        {
            HttpClient client = getApplicationJsonHttpClient();
            HttpResponseMessage post = await client.PostAsJsonAsync<User>(personsUrl + user.Id + "/users", user);
            return post.IsSuccessStatusCode;
        }

        public async Task<User> GetUser(int userId)
        {
            HttpClient client = getApplicationJsonHttpClient();
            HttpResponseMessage response = await client.GetAsync(personsUrl + userId + "/users/");
            return await response.Content.ReadAsAsync<User>();
        }

        public async Task<bool> DeleteUser(User user)
        {
            HttpClient client = getApplicationJsonHttpClient();
            HttpResponseMessage delete = await client.DeleteAsync(personsUrl + user.Id + "/users/");
            return delete.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<Email>> GetPersonEmails(int personId)
        {
            HttpClient client = getApplicationJsonHttpClient();
            HttpResponseMessage response = await client.GetAsync(personsUrl + personId + "/emails");
            return await response.Content.ReadAsAsync<IEnumerable<Email>>();
        }

        public async Task<bool> AddPersonEmail(Email email)
        {
            HttpClient client = getApplicationJsonHttpClient();
            HttpResponseMessage put = await client.PostAsJsonAsync<Email>(personsUrl + email.PersonId + "/emails", email);
            return put.IsSuccessStatusCode;
        }

        public async Task<bool> DeletePersonEmail(Email email)
        {
            HttpClient client = getApplicationJsonHttpClient();
            HttpResponseMessage delete = await client.DeleteAsync(personsUrl + email.PersonId + "/emails/" + email.Id);
            return delete.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<Address>> GetPersonAddresses(int personId)
        {
            HttpClient client = getApplicationJsonHttpClient();
            HttpResponseMessage response = await client.GetAsync(personsUrl + personId + "/addresses");
            return await response.Content.ReadAsAsync<IEnumerable<Address>>();
        }
        public async Task<bool> AddPersonAddress(Address address)
        {
            HttpClient client = getApplicationJsonHttpClient();
            HttpResponseMessage put = await client.PostAsJsonAsync<Address>(personsUrl + address.PersonId + "/addresses", address);
            return put.IsSuccessStatusCode;
        }

        public async Task<bool> DeletePersonAddress(Address address)
        {
            HttpClient client = getApplicationJsonHttpClient();
            HttpResponseMessage delete = await client.DeleteAsync(personsUrl + address.PersonId + "/addresses/" + address.Id);
            return delete.IsSuccessStatusCode;
        }

        public async Task<bool> PersonsReport()
        {
            System.Diagnostics.Process.Start(reportsUrl + "PersonsReport");
            return true;
        }
        public async Task<bool> UsersReport()
        {
            System.Diagnostics.Process.Start(reportsUrl + "UsersReport");
            return true;
        }

        private HttpClient getApplicationJsonHttpClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }
    }


}   
