using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestControllerForMergeLearning : ControllerBase
    {
        // GET: api/<TestControllerForMergeLearning>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<TestControllerForMergeLearning>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<TestControllerForMergeLearning>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<TestControllerForMergeLearning>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TestControllerForMergeLearning>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
