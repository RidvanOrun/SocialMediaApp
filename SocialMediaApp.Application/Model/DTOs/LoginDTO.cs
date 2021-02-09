using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMediaApp.Application.Model.DTOs
{
    public class LoginDTO
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }

    }
}
