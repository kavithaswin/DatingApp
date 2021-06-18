using System.Collections.Generic;
using System.Threading.Tasks;
using DatingAPI.DTOs;
using DatingAPI.Entities;
using DatingAPI.Helpers;

namespace DatingAPI.Interfaces
{
    public interface ILikesRepository
    {
        Task<UserLike> GetUserLikes(int SourceUserId,int likedUserId);
        Task<AppUser> GetUsersWithLike(int userId);
        Task<PagedList<LikesDto>> GetUserLikes(LikesParams likesParams);
    }
}