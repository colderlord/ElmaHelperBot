using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace EleWise.ELMA.ElmaBot.Web.Controllers
{
    [Route("api/[controller]")]
    public class UpdateController : Controller
    {
        // POST api/update
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]object update)
        {
            return Ok();
        }
    }
}
