using AutoMapper;
using EHealth.Api.Contacts.Model;
using EHealth.Api.Contacts.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;

namespace EHealth.Api.Contacts.Controllers
{
    [Route("/api/contact")]
    [ApiController]
    public class ContactController : Controller
    {

        private readonly ILogger<ContactController> _logger;
        private readonly IContactService _contactService;
        private readonly IMapper _mapper;

        public ContactController(IContactService contactService, IMapper mapper, ILogger<ContactController> logger)
        {
            _contactService = contactService;
            _logger = logger;
            _mapper = mapper;

        }

        //validate service is running
        [HttpGet("Check")]
        public async Task<HttpStatusCode> Check() => await Task.FromResult(HttpStatusCode.OK);

        /// <summary>
        /// ListAsync: This will retrieve all contact lists.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ContactBrowseResponse> ListAsync()
        {
            var contacts = await _contactService.ListAsync();
            return contacts;
        }

        /// <summary>
        /// ListAsync: This will retrieve contact by providing contact id.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ContactResponse> GetAsync(int id)
        {
            var contact = await _contactService.FindByIdAsync(id);

            if (contact == null)
                return new ContactResponse { Contact = contact, IsSuccess = false, Message = "Contact is not present." };
            return new ContactResponse { Contact = contact, IsSuccess = true, Message = "" };
        }


        /// <summary>
        /// PostAsync: This is Contact Add functionality
        /// </summary>
        /// <param name="contactModel"></param>
        /// <returns>
        /// Success: return Added Contact Object
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] ContactModel contactModel)
        {
            var result = await _contactService.CreateAsync(contactModel);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// PutAsync: This is update functionality, Update Contact details except Status
        /// </summary>
        /// <param name="contactModel"></param>
        /// <returns></returns>
        [HttpPut()]
        public async Task<IActionResult> PutAsync([FromBody] ContactModel contactModel)
        {
            var result = await _contactService.UpdateAsync(contactModel);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// DeleteAsync: This is custom delete functionality, this will make active status to inactive
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Siccess: return deleted(InActive) Contact</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _contactService.DeleteAsync(id);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }





    }
}