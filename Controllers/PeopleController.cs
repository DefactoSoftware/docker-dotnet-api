using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using apicore.Models;
using Newtonsoft.Json;

namespace apicore.Controllers
{
    [Route("api/[controller]")]
    public class PeopleController : Controller
    {
        private ApiContext Context;
        public PeopleController(ApiContext context)
        {
            // Set DbContext
            Context = context;
        }

        // GET api/people
        // Returns JSON array of all People
        [HttpGet]
        public string Get()
        {
            return JsonConvert.SerializeObject(Context.People, Formatting.Indented);
        }

        // GET api/people/{personId}
        // Returns JSON object of a People entry selected by id
        [HttpGet("{id}")]
        public string Get(Guid id)
        {
            // TODO: Return person by id here
            return string.Format("value: {0}", id);
        }

        // POST api/people/list
        // Post an array of JSON-formatted People objects and insert into database
        [HttpPost("List", Name = "PeoplePostList")]
        public IActionResult Post([FromBody] IEnumerable<People> people)
        {
            if (people == null)
            {
                return BadRequest();
            }

            foreach (var person in people)
            {
                Context.People.Add(person);
            }
            Context.SaveChanges();

            return CreatedAtRoute("PeoplePostList", new {controller = "People"}, people);
        }

        [HttpPost(Name = "PeoplePost")]
        // Post a JSON-formatted People object and insert into database
        public IActionResult Post([FromBody] People people)
        {
            if (people == null)
            {
                return BadRequest();
            }

            Context.People.Add(people);
            Context.SaveChanges();

            return CreatedAtRoute("PeoplePost", new {controller = "People"}, people);
        }

        // PUT api/people/{personId}
        // Updates a People entry by id
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
            // TODO: Update People by id here
        }

        // DELETE api/people/{personId}
        // Deletes a People entry by id
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            // TODO: Delete People by id here
        }
    }
}
