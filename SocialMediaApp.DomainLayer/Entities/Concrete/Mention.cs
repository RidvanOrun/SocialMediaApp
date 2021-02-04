using SocialMediaApp.DomainLayer.Entities.Interface;
using SocialMediaApp.DomainLayer.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SocialMediaApp.DomainLayer.Entities.Concrete
{
    public class Mention : IBase<int>, IBaseEntity
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int AppUserId { get; set; }
        [ForeignKey("AppUserId")]
        public AppUser AppUser { get; set; }
        public int TweetId { get; set; }
        [ForeignKey("TweetId")]
        public Tweet Tweet { get; set; }


        private DateTime _createDate = DateTime.Now;
        public DateTime CreateDate { get => _createDate; set => _createDate = value; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? DeleteDate { get; set; }

        private Status _status = Status.Active;
        public Status Status { get => _status; set => _status = value; }
    }
}
