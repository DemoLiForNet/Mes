using Microsoft.AspNetCore.Mvc;

namespace Mes.API.Controllers
{
    public class ValuesController : BaseController
    {
        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
            return Ok( new string[] { "value1", "value2" });
        }
    }
}
