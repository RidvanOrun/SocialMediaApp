using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMediaApp.DomainLayer.Entities.Concrete;
using SocialMediaApp.Infrastructure.Mapping.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMediaApp.Infrastructure.Mapping.Concrete
{
    public class MentionMap:BaseMap<Mention>
    {
        public override void Configure(EntityTypeBuilder<Mention> builder)
        {
            builder.HasKey(x => new { x.Id, x.AppUserId, x.TweetId });
            builder.Property(x => x.Text).IsRequired(false);
            base.Configure(builder);
        }

    }
}
