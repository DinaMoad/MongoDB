using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WorkiomBackendTest.Services;

namespace WorkiomBackendTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly MonogDBService _monogDBService;

        public ContactsController(MonogDBService monogDBService)
        {
            _monogDBService = monogDBService;
        }

        [HttpGet]
        public List<object> GetContacts()
        {
            var list = _monogDBService.Get("Contacts");
            return list.ConvertAll(BsonTypeMapper.MapToDotNetValue);
        }

        [HttpPost]
        [Route("/api/AddContact")]
        public IActionResult AddContact([FromBody] object request)
        {
            if (request != null)
            {
                var document = BsonSerializer.Deserialize<BsonDocument>(request.ToString());
                _monogDBService.Add(document, "Contacts");
            }
            return Ok();
        }

        [HttpPut]
        [Route("/api/UpdateContact")]
        public async Task<IActionResult> UpdateContact([FromBody] object request)
        {
            if (request != null)
            {
                var document = BsonSerializer.Deserialize<BsonDocument>(request.ToString());
                await _monogDBService.Update(document, document["_id"].ToString(), "Contacts");
            }
            return Ok();
        }

        [HttpDelete]
        [Route("/api/DeleteContact/{Id}")]
        public async Task<IActionResult> DeleteContact(string Id)
        {
            if (string.IsNullOrEmpty(Id))
            {
                return NotFound();
            }
            await _monogDBService.Remove(Id, "Contacts");

            return Ok();
        }

        [HttpGet]
        [Route("/api/FilterContacts")]
        public ActionResult<List<object>> FilterContacts([FromQuery]string key,string value)
        {
           List<BsonDocument> comp = null;
            if (!string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(key))
            {
                comp = _monogDBService.FilterData(key, value, "Contacts").Result;
            }
            return comp.ConvertAll(BsonTypeMapper.MapToDotNetValue);
        }

    }
}