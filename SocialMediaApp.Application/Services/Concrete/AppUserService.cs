using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        private readonly IFollowService _followService;

        public AppUserService(UnitOfWork unitOfWork, IMapper mapper, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IFollowService followService)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
            this._userManager = userManager;
            this._singInManager = signInManager;
            this._followService = followService;
            
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
                if (editProfileDTO.Email!= null)
                {
                    var isEmailExist = _userManager.FindByEmailAsync(editProfileDTO.Email);
                    if (isEmailExist == null) await _userManager.SetEmailAsync(user, editProfileDTO.Email);
                }

                if (editProfileDTO.Name!=null)
                {
                    //var isNameExsist = _unitOfWork.AppUserRepository.FirstOrDefault(x => x.Name == editProfileDTO.Name);
                    user.Name = editProfileDTO.Name;
                }
                _unitOfWork.AppUserRepository.Update(user);
                await _unitOfWork.Commit();  
            }

        }

        public async Task<EditProfileDTO> GetById(int Id) // dışarıdan gelen ıd den userın bilgilerini EditProfileDTO standartlarına göre getir. 
        {
            AppUser user = await _unitOfWork.AppUserRepository.GetById(Id);
            return _mapper.Map<EditProfileDTO>(user);
        }

        public async Task<ProfileSummaryDTO> GetByUserName(string userName) //dışarıdan gelen username e göre userın bilgilerini profilesummary dtoya göre getir.
        {
            //selector kendisine verilen repository'e göre verdiğimiz DTOyu ne ile eşleştireceğini anlar.
            //aşağıda çıkan sonuç selectorun üstüne gelindiğinde görülecektir. appUSer,ProfileSummaryTO

            //expresion dışarıdan gelen verinin bool tipinde kontrolünü sağlar.
            var user = await _unitOfWork.AppUserRepository.GetFilteredFirstOrDefault(
                selector: x => new ProfileSummaryDTO
                {
                    UserName = x.UserName,
                    Name = x.Name,
                    ImagePath = x.ImagePath,
                    TweetCount = x.Tweets.Count,
                    FollowerCount = x.Followers.Count,
                    FollowingCount = x.Followings.Count,

                },
                expression: x => x.UserName == userName);

            return user;
        }

        public async Task<int> GetUserIdFromName(string name)
        {
            var user = await _unitOfWork.AppUserRepository.GetFilteredFirstOrDefault(
                selector: x => x.Id,
                expression: x => x.Name == name
                );
            return user;
        }

        public async Task<SignInResult> LogIn(LoginDTO loginDTO)
        {
            var result = await _singInManager.PasswordSignInAsync(loginDTO.UserName, loginDTO.Password, false, false);
            return result;
        }

        public async Task LogOut()
        {
            await _singInManager.SignOutAsync();
        }

        public async Task<IdentityResult> Register(RegisterDTO registerDTO)
        {
            var user = _mapper.Map<AppUser>(registerDTO);
            var result = await _userManager.CreateAsync(user, registerDTO.Password);

            if (result.Succeeded) await _singInManager.SignInAsync(user, isPersistent: false);
            return result;
        }

        public async Task<List<FollowListVM>> UsersFollowers(int id, int pageIndex)
        {
            List<int> followers = await _followService.Followers(id);

            var followersList = await _unitOfWork.AppUserRepository.GetFilteredList(
                selector: x => new FollowListVM
                {
                    Id = x.Id,
                    ImagePath = x.ImagePath,
                    UserName = x.UserName,
                    Name = x.Name
                },
                expression: x => followers.Contains(x.Id),
                include: x => x.Include(x => x.Followers),
                pageIndex: pageIndex);
               
            return followersList;
        }

        public Task<List<FollowListVM>> UsersFollowings(int id, int pageIndex)
        {
            throw new NotImplementedException();
        }
    }
}
