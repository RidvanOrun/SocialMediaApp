using Microsoft.AspNetCore.Identity;
using SocialMediaApp.Application.Model.DTOs;
using SocialMediaApp.Application.Model.VM;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApp.Application.Services.Interface
{
    public interface IAppUserService
    {
        Task DeleteUser(params object[] parameters);

        Task<IdentityResult> Register(RegisterDTO registerDTO);
        Task<SignInResult> LogIn(LoginDTO loginDTO);
        Task LogOut();

        Task<int> GetUserIdFromName(string name);
        Task<EditProfileDTO> GetById(int Id);
        Task EditUser(EditProfileDTO editProfileDTO);
        Task<ProfileSummaryDTO> GetByUserName(string userName);

        Task<List<FollowListVM>> UsersFollowers(int id, int pageIndex);
        Task<List<FollowListVM>> UsersFollowings(int id, int pageIndex);

    }
}
