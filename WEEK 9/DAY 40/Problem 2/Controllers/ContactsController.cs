using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplication7.Exceptions;
using WebApplication7.Models;
using WebApplication7.Repositories;

namespace WebApplication7.Controllers
{
    [Route("api/contacts")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IContactRepository _repo;
        private readonly ILogger<ContactsController> _logger;

        public ContactsController(IContactRepository repo, ILogger<ContactsController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        // GET ALL CONTACTS
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Fetching all contacts");

            var data = await _repo.GetAllAsync();

            return Ok(data);
        }

        //  GET CONTACT BY ID
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation($"Fetching contact with ID {id}");

            var contact = await _repo.GetByIdAsync(id);

            if (contact == null)
            {
                _logger.LogWarning($"Contact with ID {id} not found");
                throw new NotFoundException($"Contact with ID {id} not found");
            }

            return Ok(contact);
        }

        //  CREATE CONTACT (Admin Only)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(ContactInfo contact)
        {
            _logger.LogInformation("Creating new contact");

            if (contact == null)
                throw new ArgumentException("Contact data is required");

            var created = await _repo.AddAsync(contact);

            _logger.LogInformation($"Contact created with ID {created.ContactId}");

            return CreatedAtAction(nameof(GetById), new { id = created.ContactId }, created);
        }

        //  UPDATE CONTACT (Admin Only)
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, ContactInfo contact)
        {
            _logger.LogInformation($"Updating contact with ID {id}");

            if (id != contact.ContactId)
            {
                _logger.LogWarning("ID mismatch during update");
                throw new ArgumentException("Contact ID mismatch");
            }

            var existing = await _repo.GetByIdAsync(id);

            if (existing == null)
            {
                _logger.LogWarning($"Contact with ID {id} not found for update");
                throw new NotFoundException($"Contact with ID {id} not found");
            }

            await _repo.UpdateAsync(contact);

            _logger.LogInformation($"Contact with ID {id} updated successfully");

            return Ok(new { message = "Contact updated successfully" });
        }

        //  DELETE CONTACT (Admin Only)
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation($"Deleting contact with ID {id}");

            var existing = await _repo.GetByIdAsync(id);

            if (existing == null)
            {
                _logger.LogWarning($"Contact with ID {id} not found for deletion");
                throw new NotFoundException($"Contact with ID {id} not found");
            }

            await _repo.DeleteAsync(id);

            _logger.LogInformation($"Contact with ID {id} deleted successfully");

            return Ok(new { message = "Contact deleted successfully" });
        }
    }
}