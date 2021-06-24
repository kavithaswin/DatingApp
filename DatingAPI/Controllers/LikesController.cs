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
      
        private readonly IUnitOfWork _unitOfWork;

        public LikesController(IUnitOfWork unitOfWork)
        {
                _unitOfWork = unitOfWork;
        }

        [HttpPost("{username}")]
        public async Task<ActionResult> AddLikes(string username)
        {
            var sourceUserId = User.GetUserId();
            var likedUser = await _unitOfWork.UserRepository.GetUserByUserNameAsync(username);
            var sourceUser = await _unitOfWork.LikesRepository.GetUsersWithLike(sourceUserId);
            if(likedUser==null) return NotFound();

            if(sourceUser.UserName == username) return BadRequest("Cannot like yourself");
            var userLike = await _unitOfWork.LikesRepository.GetUserLikes(sourceUserId,likedUser.Id);

            if(userLike!=null) return BadRequest("You already liked this user");

            userLike =  new UserLike{
                SourceUserId = sourceUserId,
                LikedUserId = likedUser.Id
            };
            sourceUser.LikedUsers.Add(userLike);
            if(await _unitOfWork.Complete()) return Ok();

            return BadRequest("Failed to add likes");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LikesDto>>> GetUserLikes([FromQuery] LikesParams likesParams){
                
                likesParams.UserId = User.GetUserId();
                var users =  await _unitOfWork.LikesRepository.GetUserLikes(likesParams);

                Response.AddPaginationHeader(users.CurrentPage, users.PageSize,users.TotalCount, users.TotalPages);
                return Ok(users);
        }

    }
}