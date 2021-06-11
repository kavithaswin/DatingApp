using DatingAPI.Data;
using DatingAPI.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingAPI.Controllers
{
    public class BuggyController : BaseAPIController
    {
        private readonly DataContext _context ;
        public BuggyController(DataContext Context)
        {
            _context = Context;
        }

        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetSecret(){
            return "secret test";
        }
       
        [HttpGet("not-found")]
        public ActionResult<AppUser> GetNotFound(){
            var thing = _context.Users.Find(-1);
            if(thing==null) return NotFound();
            return Ok(thing);
        }
        
        [HttpGet("server-error")]
        public ActionResult<string> GetServerError(){
           var thing = _context.Users.Find(-1);
           var thingToReturn = thing.ToString();
           return thingToReturn;
        }
        
        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest(){
            return BadRequest("This was not a good request");
        }
       
    }
}