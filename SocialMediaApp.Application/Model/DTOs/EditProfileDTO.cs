using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SocialMediaApp.Application.Model.DTOs
{
    public class EditProfileDTO
    {
        //bussines Domain ihtiyaçlarımıza göre hazırladığımız ilk günden beri kullandığımız veri transfer objelerimiz attribute bazında şartlar içerebilir. Eski projelerimizde örneğin cms projemizde bir varlığın prototype hem entity hemde dto gibi kullanıyorduk.
        public int Id { get; set; }
        [Required(ErrorMessage ="You Must to Type into name")]
        public string Name { get; set; }
        [DataType(DataType.Password)]
        public int Password { get; set; }
        [Display(Name="User Name")]
        public int UserName { get; set; }
        public string Email { get; set; }
        public string ImagePath { get; set; }
        public IFormFile Image { get; set; }

    }
}
