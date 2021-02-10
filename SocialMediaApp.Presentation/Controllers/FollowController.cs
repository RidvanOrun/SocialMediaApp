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
    public class FollowController : Controller
    {
        private readonly IFollowService _followService;
        public FollowController(IFollowService followService)
        {
            this._followService = followService;
        }

        public IActionResult Index() => View();

        [HttpPost]
        public async Task<IActionResult> Follow(FollowDTO model)
        {
            if (!model.isExist)
            {
                if (model.FollowerId == User.GetUserId())
                {
                    await _followService.Follow(model);
                    return Json("Success");
                }
                else
                {
                    return Json("Failed");
                }
            }
            else
            {
                if (model.FollowerId == User.GetUserId())
                {
                    await _followService.UnFollow(model);
                    return Json("Success");
                }
                else
                {
                    return Json("Failed");
                }
            }

        }
    }
}
