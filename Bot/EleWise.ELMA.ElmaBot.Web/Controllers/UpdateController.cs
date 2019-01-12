using System.Threading.Tasks;
using EleWise.ELMA.TelegramBot.Services;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace EleWise.ELMA.ElmaBot.Web.Controllers
{
    [Route("api/[controller]")]
    public class UpdateController : Controller
    {
        private readonly IUpdateService updateService;

        public UpdateController(IUpdateService updateService)
        {
            this.updateService = updateService;
        }

        // POST api/update
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Update update)
        {
            await updateService.EchoAsync(update);
            return Ok();
        }
    }
}
