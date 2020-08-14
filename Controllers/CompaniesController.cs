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
    public class CompaniesController : ControllerBase
    {
        private readonly MonogDBService _companyService;

        public CompaniesController(MonogDBService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet]
        public List<object> GetCompaines()
        {
            var list = _companyService.Get("Companies");
            return list.ConvertAll(BsonTypeMapper.MapToDotNetValue);
        }

        [HttpPost]
        [Route("/api/AddCompany")]
        public IActionResult AddCompany([FromBody] object request)
        {
            if (request != null)
            {
                var document = BsonSerializer.Deserialize<BsonDocument>(request.ToString());
                _companyService.Add(document, "Companies");
            }
            return Ok();
        }

        [HttpPut]
        [Route("/api/UpdateCompany")]
        public async Task<IActionResult> UpdateCompany([FromBody] object request)
        {
            if (request != null)
            {
                var document = BsonSerializer.Deserialize<BsonDocument>(request.ToString());
                await _companyService.Update(document, document["_id"].ToString(), "Companies");
            }

            return Ok();
        }

        [HttpDelete]
        [Route("/api/DeleteCompany/{Id}")]
        public async Task<IActionResult> DeleteCompany(string Id)
        {
            if (string.IsNullOrEmpty(Id))
            {
                return NotFound();
            }
            await _companyService.Remove(Id, "Companies");

            return Ok();
        }

        [HttpGet]
        [Route("/api/FilterCompany/")]
        public ActionResult<List<object>> FilterCompany([FromQuery]string key,string value)
        {
                List<BsonDocument> comp = null;
            if (!string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(key))
            {
                comp = _companyService.FilterData(key, value, "Companies").Result;
            }
            return comp.ConvertAll(BsonTypeMapper.MapToDotNetValue);
        }

    }
}