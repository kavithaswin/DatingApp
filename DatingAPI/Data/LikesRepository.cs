using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingAPI.DTOs;
using DatingAPI.Entities;
using DatingAPI.Extensions;
using DatingAPI.Helpers;
using DatingAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DatingAPI.Data
{
    public class LikesRepository : ILikesRepository
    {
        private readonly DataContext _context;

        public LikesRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<UserLike> GetUserLikes(int SourceUserId, int likedUserId)
        {
            return await _context.Likes.FindAsync(SourceUserId,likedUserId);
        }

        public async Task<PagedList<LikesDto>> GetUserLikes(LikesParams likesParams)
        {
            var users = _context.Users.OrderBy(u=>u.UserName).AsQueryable();
            var likes = _context.Likes.AsQueryable();

            if(likesParams.Predicate=="liked") //get all users liked by logged in user
            {
                likes = likes.Where(like=> like.SourceUserId==likesParams.UserId);
                users = likes.Select(like=>like.LikedUser);
            }
             if(likesParams.Predicate=="likedBy") //get users liked by user id
            {
                likes = likes.Where(like=> like.LikedUserId==likesParams.UserId);
                users = likes.Select(like=>like.SourceUser);
            }

            var likedUsers=  users.Select(user=>new LikesDto{

                Username = user.UserName,
                KnownAs = user.KnownAs,
                Age = user.DateOfBirth.CalculateAge(),
                PhotoUrl = user.Photos.FirstOrDefault(p=>p.IsMain).Url,
                City = user.City

            } );

            return await PagedList<LikesDto>.CreateAsync(likedUsers,likesParams.PageNumber,likesParams.PageSize);
        }

        public async Task<AppUser> GetUsersWithLike(int userId)
        {
            return await _context.Users
            .Include(x=>x.LikedUsers).FirstOrDefaultAsync(x=>x.Id==userId);
            
        }
    }
}