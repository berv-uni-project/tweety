using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Tweety.Models;
using TweetyCore.Utils;

namespace TweetyCore.Controllers
{
    [Route("api/v1/tweety")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        [HttpPost("find")]
        public IActionResult Index([FromBody] Tags requestBody)
        {
            if (ModelState.IsValid)
            {
                TwitterConnect twitterConnect = new TwitterConnect();
                var result = twitterConnect.ProcessTag(requestBody);

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
                    return Ok(new {
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