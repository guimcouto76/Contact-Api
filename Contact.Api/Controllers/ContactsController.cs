using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Contact.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Contact.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly ILogger<ContactsController> _logger;
        private readonly IContactService _contactService;
        public ContactsController(ILogger<ContactsController> logger, IContactService contactService)
        {
            _logger = logger;
            _contactService = contactService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Models.Contact>>> Get()
        {
            var contacts = await _contactService.FindAll();
            return Ok(contacts);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<WeatherForecast>> Insert([FromBody] Models.Contact contact)
        {
            var id = await _contactService.Insert(contact);

            if (id != default) return Created("", await _contactService.FindOne(id));

            _logger.LogDebug($"Bad POST request. Payload: {JsonConvert.SerializeObject(contact)}");
            return BadRequest();
            
        }

        [Route("{id}")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Models.Contact>> Get(int id)
        {
            var result = await _contactService.FindOne(id);
            if (result != default) return Ok(result);

            _logger.LogDebug($"No contact found using Id {id}");
            return NotFound();
        }

        [Route("{id}")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<WeatherForecast>> Update(int id, [FromBody] Models.Contact contact)
        {
            var result = await _contactService.Update(id, contact);
            if (result) return NoContent();

            _logger.LogDebug($"No contact found using Id {id}");
            return NotFound();
        }

        [Route("{id}")]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<WeatherForecast>> Delete(int id)
        {
            var result = await _contactService.Delete(id);

            if (result) return NoContent();

            _logger.LogDebug($"No contact found using Id {id}");
            return NotFound();
        }

    }
}
