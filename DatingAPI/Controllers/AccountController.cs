using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DatingAPI.Data;
using DatingAPI.DTOs;
using DatingAPI.Entities;
using DatingAPI.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingAPI.Controllers
{
    public class AccountController : BaseAPIController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService ;
        private readonly IMapper _mapper;

        public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager, ITokenService tokenService,
         IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
            

        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDTO registerDto)
        {
            if (await IsUserExists(registerDto.Username))
                return BadRequest("User already exists");
            var user = _mapper.Map<AppUser>(registerDto);
          
           
            user.UserName = registerDto.Username.ToLower();
           
            

            var result = await _userManager.CreateAsync(user,registerDto.Password);
            if(!result.Succeeded) return BadRequest(result.Errors);

            var roleResult = await _userManager.AddToRoleAsync(user,"Member");
            if(!result.Succeeded) return BadRequest(result.Errors);
            
             
             return new UserDto{
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user),
                KnownAs = user.KnownAs,
                Gender = user.Gender
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.Users
            .Include(p=>p.Photos)
            .SingleOrDefaultAsync(u => u.UserName == loginDto.Username.ToLower());

            if (user == null)
                return Unauthorized("Invalid UserName");

            var result = await _signInManager.CheckPasswordSignInAsync(user,loginDto.Password,false);

            if(!result.Succeeded) return Unauthorized("Bad Password");
           
            return new UserDto{
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x=> x.IsMain)?.Url,
                KnownAs = user.KnownAs,
                Gender = user.Gender
            };
        }
        private async Task<bool> IsUserExists(string userName)
        {
            return await _userManager.Users.AnyAsync(u => u.UserName.ToLower() == userName.ToLower());
        }
    }
}