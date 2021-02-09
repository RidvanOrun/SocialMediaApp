using SocialMediaApp.Application.Model.DTOs;
using SocialMediaApp.Application.Model.VM;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApp.Application.Services.Interface
{
    public interface ITweetService
    {
        Task<List<TimeLineVM>> GetTimeLine(int userId, int pageIndex);
        Task AddTweet(AddTweetDTO addTweetDTO);
        Task<List<TimeLineVM>> UserTweets(string userName, int pageIndex);
        Task DeleteTweet(int id, int userId);

    }
}
