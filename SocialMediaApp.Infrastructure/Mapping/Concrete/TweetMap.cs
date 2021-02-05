using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMediaApp.DomainLayer.Entities.Concrete;
using SocialMediaApp.Infrastructure.Mapping.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMediaApp.Infrastructure.Mapping.Concrete
{
    public class TweetMap:BaseMap<Tweet>
    {
        public override void Configure(EntityTypeBuilder<Tweet> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Text).HasMaxLength(256).IsRequired(true);
            builder.Property(x => x.ImagePath).IsRequired(false);

            builder.HasMany(x => x.Mentions).WithOne(x => x.Tweet).HasForeignKey(x => x.TweetId);
            builder.HasMany(x => x.Likes).WithOne(x => x.Tweet).HasForeignKey(x => x.TweetId);
            builder.HasMany(x => x.Shares).WithOne(x => x.Tweet).HasForeignKey(x => x.TweetId);

            builder.HasOne(x => x.AppUser).WithMany(x => x.Tweets).HasForeignKey(x => x.AppUserId);

            base.Configure(builder);
        }
    }
}
