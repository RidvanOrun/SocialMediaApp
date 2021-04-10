using Microsoft.AspNetCore.Identity;
using SocialMediaApp.DomainLayer.Entities.Interface;
using SocialMediaApp.DomainLayer.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMediaApp.DomainLayer.Entities.Concrete
{ 
    public class AppRole : IdentityRole<int>, IBaseEntity
    {
        private DateTime _createDate = DateTime.Now;
        public DateTime CreateDate { get => _createDate; set=>_createDate=value; }
        public DateTime? ModifiedDate { get ; set; }
        public DateTime? DeleteDate { get; set; }

        private Status _status = Status.Active;
        public Status Status { get =>_status; set =>_status=value; }
    }
}
