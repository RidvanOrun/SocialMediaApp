using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SocialMediaApp.Application.Model.DTOs;
using SocialMediaApp.Application.Model.VM;
using SocialMediaApp.Application.Services.Interface;
using SocialMediaApp.DomainLayer.Entities.Concrete;
using SocialMediaApp.Infrastructure.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApp.Application.Services.Concrete
{
    public class AppUserService:IAppUserService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _singInManager;

        public AppUserService(UnitOfWork unitOfWork, IMapper mapper, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
            this._userManager = userManager;
            this._singInManager = signInManager;
            
        }

        //??????????????
        //params object[], değişken türü belli olmayan durumlarda her eş object türünden türetilir.
        // parametres: dışardan gelen kullanıcı bilgileri. Bu kullanıcı bilgilerinide şu şekilde elde ediyor düşündüğüm zaman kullanıcıları listelediğimde bilgileri yazılı oluyor ıd, adsoyad status vs bunlar parametrs bilgileri oluyor ve bu bilgileri siliyor. 

        //spDeleteUsers yerine stored procedur yazılacak. 
        public async Task DeleteUser(params object[] parameters) => await _unitOfWork.ExecuteSqlRaw($"spDeleteUsers {0}", parameters);
        

        public async Task EditUser(EditProfileDTO editProfileDTO)
        {
            AppUser user = await _unitOfWork.AppUserRepository.GetById(editProfileDTO.Id);
            if (user!=null)
            {
                if (editProfileDTO.Image!=null)
                {
                    using var image = Image.Load(editProfileDTO.Image.OpenReadStream());
                    image.Mutate(x => x.Resize(256, 256));
                    image.Save("wwwroot/images/users/" + Guid.NewGuid().ToString() + ".jpg");
                    user.ImagePath = ("/images/users/" + Guid.NewGuid().ToString() + ".jpg");
                }
                if (editProfileDTO.Password != null)
                {
                    user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, editProfileDTO.Password);
                    await _userManager.UpdateAsync(user);
                }
                if (editProfileDTO.UserName!=null)
                {
                    var isUserNameExist = _userManager.FindByNameAsync(editProfileDTO.UserName);
                    if (isUserNameExist == null)
                    {
                        await _userManager.SetUserNameAsync(user, editProfileDTO.UserName);
                        //user.UserName = editProfileDTO.UserName;
                        //await _userManager.UpdateAsync(user);
                    }
                }
               

            }

        }

        public Task<EditProfileDTO> GetById(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<ProfileSummaryDTO> GetByUserName(string userName)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetUserIdFromName(string name)
        {
            throw new NotImplementedException();
        }

        public Task<SignInResult> LogIn(LoginDTO loginDTO)
        {
            throw new NotImplementedException();
        }

        public Task LogOut()
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> Register(RegisterDTO registerDTO)
        {
            throw new NotImplementedException();
        }

        public Task<List<FollowListVM>> UsersFollowers(int id, int pageIndex)
        {
            throw new NotImplementedException();
        }

        public Task<List<FollowListVM>> UsersFollowings(int id, int pageIndex)
        {
            throw new NotImplementedException();
        }
    }
}
