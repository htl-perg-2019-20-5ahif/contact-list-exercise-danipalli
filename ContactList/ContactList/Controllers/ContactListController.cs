using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ContactList.Controllers
{
    [ApiController]
    [Route("contacts")]
    public partial class ContactListController : ControllerBase
    {
        private static readonly List<Person> contacts =
            new List<Person> ();

        [HttpGet]
        public IActionResult GetAllContacts() => Ok(contacts);

        [HttpGet]
        [Route("findByName", Name = "GetSpecificItem")]
        public IActionResult GetContact([Required(ErrorMessage = "Filter argument required.")][FromQuery] string nameFilter)
        {
            List<Person> filteredContacts = new List<Person>();
            foreach (Person person in contacts)
            {
                if(person.firstName.ToLower().Contains(nameFilter.ToLower()) || person.lastName.ToLower().Contains(nameFilter.ToLower()))
                {
                    filteredContacts.Add(person);
                }
            }
            if(filteredContacts.Count > 0)
            {
                return Ok(filteredContacts);
            }

            return BadRequest("Invalid or missing name");
        }

        [HttpPost] 
        public IActionResult AddContact([Required(ErrorMessage = "Data in body required.")][FromBody] Person newContact)
        {
            if(newContact.id == 0 || newContact.email == null)
            {
                return BadRequest("Invalid input (e.g. required field missing or empty)");
            }
            contacts.Add(newContact);
            return Created("Person successfully created", newContact);
        }

        [HttpDelete]
        [Route("{personId}")]
        public IActionResult DeleteContact(int personId)
        {
            if(personId < 0)
            {
                return BadRequest("Invalid ID supplied");
            }

            foreach (Person person in contacts)
            {
                if (person.id == personId)
                {
                    contacts.Remove(person);
                    return NoContent();
                }
            }

            return NotFound("Person not found");
        }
    }
}
