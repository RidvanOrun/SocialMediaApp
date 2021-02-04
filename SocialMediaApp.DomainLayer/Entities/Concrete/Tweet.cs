using SocialMediaApp.DomainLayer.Entities.Interface;
using SocialMediaApp.DomainLayer.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMediaApp.DomainLayer.Entities.Concrete
{
    public class Tweet : IBaseEntity, IBase<int>
    {
        public Tweet()
        {
            Likes = new List<Like>();
            Mentions = new List<Mention>();
            Shares = new List<Share>();
        }

        public int Id { get; set; }
        public string Text { get; set; }
        public string ImagePath { get; set; }


        private DateTime _createDate = DateTime.Now;
        public DateTime CreateDate { get => _createDate; set => _createDate = value; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? DeleteDate { get; set; }

        private Status _status = Status.Active;
        public Status Status { get => _status; set => _status = value; }

        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public List<Like> Likes { get; set; }
        public List<Mention> Mentions { get; set; }
        public List<Share> Shares { get; set; }
    }
}
