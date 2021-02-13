using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocialMediaApp.Application.Extensions;
using SocialMediaApp.Application.Model.DTOs;
using SocialMediaApp.Application.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaApp.Presentation.Controllers
{
    public class TweetController : Controller
    {

        private readonly ITweetService _tweetServices;

        public TweetController(ITweetService tweetService)
        {
            this._tweetServices = tweetService;
        }       

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddTweet(AddTweetDTO model) 
        {
            if (ModelState.IsValid)
            {
                if (model.AppUserId == User.GetUserId())
                {
                    await _tweetServices.AddTweet(model);
                    return Json("Success");
                }
                else
                {
                    return Json("Failed");
                }
            }
            else return BadRequest(String.Join(Environment.NewLine, ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage + "" + x.Exception)));            
        }

        [HttpPost]
        public async Task<IActionResult> GetTweets(int pageIndex, int pageSize, string userName)
        {
            if (userName == null) return Json(await _tweetServices.GetTimeLine(User.GetUserId(), pageIndex), new JsonSerializerSettings());
            else return Json(await _tweetServices.UserTweets(userName, pageIndex));            
        
        }

    }
}
