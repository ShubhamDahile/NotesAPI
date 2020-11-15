using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NotesAPI.Controllers
{
    [Authorize]
    public class WelcomeController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetWelcomeMessage()
        {
            return Ok("Welcome To This Page");
        }
    }
}
