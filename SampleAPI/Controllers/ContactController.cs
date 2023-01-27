using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SampleAPI.Models;

namespace SampleAPI.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ContactController : Controller
    {
        private readonly SampleDbContext sampleDbContext;
        public ContactController(SampleDbContext sampleDbContext)
        {
            this.sampleDbContext = sampleDbContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllContacts()
        {
            var contacts = await sampleDbContext.Contacts.ToListAsync();
            return Ok(contacts);
        }

        [HttpGet]
        [Route("{id:int}")]
        [ActionName("GetContact")]
        public async Task<IActionResult> GetContact([FromRoute] int id)
        {
            var contact = await sampleDbContext.Contacts.FirstOrDefaultAsync(x => x.ContactId == id);
            if (contact != null)
            {
                return Ok(contact);
            }
            return NotFound("Contact is not found");
        }
        [HttpPost]
        public async Task<IActionResult> AddContact([FromBody] Contact contact)
        {
            await sampleDbContext.Contacts.AddAsync(contact);
            await sampleDbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetContact), new { id = contact.ContactId }, contact);
        }
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateContact([FromRoute] int id, [FromBody] Contact contact)
        {
            var exsistingContact = await sampleDbContext.Contacts.FirstOrDefaultAsync(x => x.ContactId == id);
            if (exsistingContact != null)
            {
                exsistingContact.FirstName = contact.FirstName;
                exsistingContact.LastName = contact.LastName;
                exsistingContact.EmailAddress = contact.EmailAddress;
                exsistingContact.Phone = contact.Phone;


                await sampleDbContext.SaveChangesAsync();
                return Ok(exsistingContact);
            }
            return NotFound("Contact is not found");
        }
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteContact([FromRoute] int id)
        {
            var exsistingContact = await sampleDbContext.Contacts.FirstOrDefaultAsync(x => x.ContactId == id);
            if (exsistingContact != null)
            {
                sampleDbContext.Contacts.Remove(exsistingContact);
                await sampleDbContext.SaveChangesAsync();
                return Ok(exsistingContact);
            }
            return NotFound("Contact is  not found");
        }
    }
}

