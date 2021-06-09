using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingAPI.Data;
using DatingAPI.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingAPI.Controllers
{
    
    public class UsersController : BaseAPIController
    {
        private readonly DataContext context;
        public UsersController(DataContext context)
        {
            this.context = context;
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers(){
            return await context.Users.ToListAsync();
            
        }
        [Authorize]
         [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetUsers(int id){
            return await context.Users.FindAsync(id);
            
        }
    }
}