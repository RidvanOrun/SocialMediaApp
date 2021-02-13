using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocialMediaApp.Application.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaApp.Presentation.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IAppUserService _appUserService;
        public ProfileController(IAppUserService appUserService) => this._appUserService = appUserService;


        public IActionResult Index()
        {
            return View();
        }

        //ViewBag, Controller'da oluşturulan bir yapıyı View kısmına taşımak için kullanılır. Kendi içerisinde birden fazla yapının aktarılmasına olanak sunmaktadır. İçerisine bir string ifade, integer ifade yada list gönderebilmek ya da eşitleyebilmek mümkündür.
        public IActionResult Details(string userName)
        {
            ViewBag.userName = userName;
            return View();
        }

        public IActionResult Followings(string userName)
        {
            ViewBag.userName = userName;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Followings(string userName,int pageIndex)
        {
            var findUser = await _appUserService.GetUserIdFromName(userName);
            if (findUser >0)
            {
                var Followings = await _appUserService.UsersFollowings(findUser, pageIndex);
                return Json(Followings, new JsonSerializerSettings());
                //return View();
            }
            else
            {
                return NotFound();
            }
        }


        public IActionResult Followers(string userName)
        {
            ViewBag.userName = userName;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Followers(string userName, int pageındex)
        {
            var findUser = await _appUserService.GetUserIdFromName(userName);

            if (findUser >0)
            {
                var followers = await _appUserService.UsersFollowers(findUser, pageındex);
                return Json(followers, new JsonSerializerSettings());
            }
            else
            {
                return NotFound();
            }

        }
    }
}

