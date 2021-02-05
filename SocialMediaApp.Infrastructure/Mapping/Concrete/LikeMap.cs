using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMediaApp.DomainLayer.Entities.Concrete;
using SocialMediaApp.Infrastructure.Mapping.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMediaApp.Infrastructure.Mapping.Concrete
{
    public class LikeMap:BaseMap<Like>
    {
        public override void Configure(EntityTypeBuilder<Like> builder)
        {
            builder.HasKey(x => new { x.AppUserId, x.TweetId });
            base.Configure(builder);
        }

    }
}
