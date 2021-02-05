using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMediaApp.Application.Model.DTOs
{
    public class FollowDTO
    {
        public int FollowerId { get; set; }
        public int FollowingId { get; set; }
        public bool isExist { get; set; }
    }
}
