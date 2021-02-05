using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMediaApp.Application.Model.DTOs
{
    public class LikeDTO
    {
        public int AppUserId { get; set; }
        public int TweetId { get; set; }
        public int isExist { get; set; }

    }
}
