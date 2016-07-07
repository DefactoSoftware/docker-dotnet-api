using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using apicore.Models;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.SwaggerGen.Annotations;

namespace apicore.Controllers
{
    [Route("api/people")]
    [Produces("application/json")]
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
        [SwaggerOperation("GetAllPeople")]
        [HttpGet]
        public string GetAll()
        {
            return JsonConvert.SerializeObject(Context.People);
        }

        // GET api/people/{personId}
        // Returns JSON object of a People entry selected by id
        [SwaggerOperation("GetPersonById")]
        [SwaggerResponse(404, "Person not found")]
        [HttpGet("{id}")]
        public string GetById(Guid id)
        {
            var person = Context.People.SingleOrDefault(p => p.personId == id);
            return JsonConvert.SerializeObject(person);
        }

        // POST api/people/list
        // Post an array of JSON-formatted People objects and insert into database
        [SwaggerOperation("AddPeopleList")]
        [SwaggerResponse(400, "No data posted or incorrect format")]
        [HttpPost("List", Name = "PeoplePostList")]
        public IActionResult PostList([FromBody] IEnumerable<People> people)
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

        [SwaggerOperation("AddPerson")]
        [SwaggerResponse(400, "No data posted or incorrect format")]
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
        [SwaggerOperation("UpdatePerson")]
        [SwaggerResponse(400, "No data posted or incorrect format")]
        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody]People updatePerson)
        {
            if (updatePerson == null)
            {
                return BadRequest();
            }

            var person = Context.People.SingleOrDefault(p => p.personId == id);
            if (person == null) return NotFound();

            updatePerson.personId = id;
            var update = Context.People.Update(updatePerson);
            Context.SaveChanges();

            return CreatedAtRoute("PeoplePut", new {controller = "People"}, updatePerson);
        }

        // DELETE api/people/{personId}
        // Deletes a People entry by id
        [SwaggerOperation("DeletePerson")]
        [SwaggerResponse(400, "No data posted or incorrect format")]
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var deletePerson = Context.People.SingleOrDefault(p => p.personId == id);
            if (deletePerson == null)
            {
                return NotFound();
            }

            Context.People.Remove(deletePerson);
            Context.SaveChanges();

            return Ok();
        }
    }
}
