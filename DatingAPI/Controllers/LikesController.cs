using System.Collections.Generic;
using System.Threading.Tasks;
using DatingAPI.DTOs;
using DatingAPI.Entities;
using DatingAPI.Extensions;
using DatingAPI.Helpers;
using DatingAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingAPI.Controllers
{
    [Authorize]
    public class LikesController : BaseAPIController
    {
        private readonly IUserRepository _userRepository;
        private readonly ILikesRepository _likesRepository;

        public LikesController(IUserRepository userRepository,
         ILikesRepository likesRepository)
        {
            _userRepository = userRepository;
            _likesRepository = likesRepository;
        }

        [HttpPost("{username}")]
        public async Task<ActionResult> AddLikes(string username)
        {
            var sourceUserId = User.GetUserId();
            var likedUser = await _userRepository.GetUserByUserNameAsync(username);
            var sourceUser = await _likesRepository.GetUsersWithLike(sourceUserId);
            if(likedUser==null) return NotFound();

            if(sourceUser.UserName == username) return BadRequest("Cannot like yourself");
            var userLike = await _likesRepository.GetUserLikes(sourceUserId,likedUser.Id);

            if(userLike!=null) return BadRequest("You already liked this user");

            userLike =  new UserLike{
                SourceUserId = sourceUserId,
                LikedUserId = likedUser.Id
            };
            sourceUser.LikedUsers.Add(userLike);
            if(await _userRepository.SaveAllAsync()) return Ok();

            return BadRequest("Failed to add likes");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LikesDto>>> GetUserLikes([FromQuery] LikesParams likesParams){
                
                likesParams.UserId = User.GetUserId();
                var users =  await _likesRepository.GetUserLikes(likesParams);

                Response.AddPaginationHeader(users.CurrentPage, users.PageSize,users.TotalCount, users.TotalPages);
                return Ok(users);
        }

    }
}