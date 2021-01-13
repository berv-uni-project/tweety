using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweety.Models;
using TweetyCore.Utils.Twitter;

namespace TweetyCore.Controllers
{
    [Route("api/v1/tweety")]
    [ApiController]
    //[Authorize]
    public class ApiController : ControllerBase
    {
        private readonly ITwitterConnect _twitter;
        public ApiController(ITwitterConnect twitter)
        {
            _twitter = twitter;
        }
        [HttpPost("find")]
        public async Task<ActionResult> Index([FromBody] Tags requestBody)
        {
            if (ModelState.IsValid)
            {
                var result = await _twitter.ProcessTag(requestBody);

                if (result.Count > 0)
                {
                    return Ok(new
                    {
                        status = 200,
                        message = "Success",
                        result
                    });
                }
                else
                {
                    return Ok(new
                    {
                        status = 200,
                        message = "Empty Result",
                        result = new List<string>()
                    });
                };

            }
            return BadRequest();
        }
    }
}