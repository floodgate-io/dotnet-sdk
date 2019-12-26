using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FloodGate.SDK;
using WebApplication_Core.Services;

namespace WebApplication_Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            try
            {
                FloodGateWrapper floodgate = FloodGateWrapper.Instance;

                var customAttributes = new Dictionary<string, string>() {
                    { "name", "Peter Parker" },
                    { "subscription", "Gold" },
                    { "country", "UK" },
                    { "role", "User" }
                };

                User user = new User("d2405fc0-c9cd-49e7-a07e-bf244d6d360b")
                {
                    Email = "peter@marvel.com",
                    CustomAttributes = customAttributes
                };

                var flag1 = floodgate.Client.GetValue("background-colour", "grey", user);

                floodgate.Client.Dispose();

                return new string[] { flag1 };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
