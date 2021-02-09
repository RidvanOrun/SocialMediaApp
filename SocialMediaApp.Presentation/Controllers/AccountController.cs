using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMediaApp.Application.Extensions;
using SocialMediaApp.Application.Model.DTOs;
using SocialMediaApp.Application.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaApp.Presentation.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAppUserService _userService;

        public AccountController(IAppUserService appUserService)
        {
            _userService = appUserService;
        }

        #region Registration
        public IActionResult Register()
        {
            ///?????
            if (User.Identity.IsAuthenticated) return RedirectToAction(nameof(HomeController.Index), "Home");
            return View();         
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.Register(registerDTO);
                //?? neden succeeded oldu
                if (result.Succeeded) return RedirectToAction("Index", "Home");

                foreach (var item in result.Errors)
                    ModelState.AddModelError(string.Empty, item.Description);                
            }

            return View(registerDTO);
        
        }

        #endregion

        #region Login
        //?? neden null oldu
        public IActionResult Login(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction(nameof(HomeController.Index), "Home");

            //readme ye eklenecek gitbook yapılacak viewdata viewbag tempdata
            ViewData["ReturnUrl"] = returnUrl;
            return View();      
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO model, string returnUrl) 
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.LogIn(model);
                //?? RedirectToLocal nedemek gitbook basicknowledge
                if (result.Succeeded) return RedirectToLocal(returnUrl);

                ModelState.AddModelError(String.Empty, "Invalid login attempt....");
              
            }
            return View();

        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl);
            else return RedirectToAction(nameof(HomeController.Index), "Home");

        }

        #endregion

        #region Logout
        [HttpPost]

        public async Task<IActionResult> LogOut() 
        {
            await _userService.LogOut();

            return RedirectToAction(nameof(HomeController.Index), "Home");
        
        }

        #endregion

        #region EditProfile
        public async Task<IActionResult> EditProfile(string userName)
        {
            if (userName == User.Identity.Name)
            {
                var user = await _userService.GetById(User.GetUserId());

                if (user == null) return NotFound();
                return View(user);
            }
            else return RedirectToAction(nameof(HomeController.Index), "Home");            
        }

        [HttpPost]
        //??? Ifrom File nedir gitbook
        public async Task<IActionResult> EditProfile(EditProfileDTO model, IFormFile file) 
        {
            await _userService.EditUser(model);
            return RedirectToAction(nameof(HomeController.Index), "home");           
        }

        #endregion
      
    }
}
