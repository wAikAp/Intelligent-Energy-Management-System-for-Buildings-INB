using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FYP_WEB_APP.Models.API;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FYP_WEB_APP.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class LightAPIController : ControllerBase
    {
        // GET: api/<LightAPIController>
        [HttpGet]

        public IEnumerable<int> Get()
        {
            //LightAPI
            int[] light = new ControllerLightModel().Light;

            return light;
        }

       /* // GET api/<LightAPIController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<LightAPIController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<LightAPIController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<LightAPIController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }*/
    }
}
