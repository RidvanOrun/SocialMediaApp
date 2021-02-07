using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMediaApp.Application.Model.VM
{
    public class FollowListVM
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public List<int> Follow { get; set; }
    }
}
