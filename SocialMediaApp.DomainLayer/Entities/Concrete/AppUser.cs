using Microsoft.AspNetCore.Identity;
using SocialMediaApp.DomainLayer.Entities.Interface;
using SocialMediaApp.DomainLayer.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SocialMediaApp.DomainLayer.Entities.Concrete
{
    public class AppUser : IdentityUser<int>, IBaseEntity
    {
        //ilgili varlığın initialize edildiğinde ilişkiler
        //NullReferanceException hatası almamak için listeleri hafızaya cıkarmam gerekiyor ctor yaptık
        public AppUser()
        {
            Tweets = new List<Tweet>();
            Likes = new List<Like>();
            Shares = new List<Share>();
            Mentions = new List<Mention>();
        }
        public string Name { get; set; }
        public string ImagePath { get; set; } = "/images/users/default.jp";


        private DateTime _createDate = DateTime.Now;
        public DateTime CreateDate { get => _createDate; set => _createDate = value; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? DeleteDate { get; set; }

        private Status _status = Status.Active;
        public Status Status { get => _status; set => _status = value; }

        public List<Tweet> Tweets { get; set; }
        public List<Like> Likes { get; set; }
        public List<Share> Shares { get; set; }
        public List<Mention> Mentions { get; set; }

        [InverseProperty("Follower")]
        public List<Follow> Followers { get; set; }

        [InverseProperty("Following")]
        public List<Follow> Followings { get; set; }

    }
}
