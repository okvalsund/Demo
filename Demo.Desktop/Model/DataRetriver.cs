using Demo.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Desktop.Model
{
    public class DataRetriver
    {
        public DataRetriver()
        {
            
        }

        public async Task<Person> GetPerson(int personID)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Person person = null;
            HttpResponseMessage response = await client.GetAsync("http://localhost:58139/api/persons/" + personID);
            return await response.Content.ReadAsAsync<Person>();
        }

        public async Task<Person> GetFilteredPerson(string search)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Person person = null;
            HttpResponseMessage response = await client.GetAsync("http://localhost:58139/api/persons/search/"+search);
            return await response.Content.ReadAsAsync<Person>();
        }

        public async Task<bool> InsertPerson(Person person)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage put = await client.PostAsJsonAsync<Person>("http://localhost:58139/api/persons/", person);

            if (put.IsSuccessStatusCode)
                return true;
            else
                return false;
        }
        public async Task<bool> UpdatePerson(Person person)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage put = await client.PutAsJsonAsync<Person>("http://localhost:58139/api/persons/" + person.Id, person);

            if (put.IsSuccessStatusCode)
                return true;
            else
                return false;
        }

        public async Task<bool> Delete(Person person)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage put = await client.DeleteAsync("http://localhost:58139/api/persons/" + person.Id);

            if (put.IsSuccessStatusCode)
                return true;
            else
                return false;
        }
    }
}   
