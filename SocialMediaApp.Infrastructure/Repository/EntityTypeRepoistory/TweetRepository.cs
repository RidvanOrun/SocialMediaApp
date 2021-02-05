using SocialMediaApp.DomainLayer.Entities.Concrete;
using SocialMediaApp.DomainLayer.Repository.EntitytypeRepository;
using SocialMediaApp.Infrastructure.Context;
using SocialMediaApp.Infrastructure.Repository.BaseRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMediaApp.Infrastructure.Repository.EntityTypeRepoistory
{
    public class TweetRepository:BaseRepository<Tweet>,ITweetRepository
    {
        public TweetRepository(ApplicationDbContext applicationDbContext):base (applicationDbContext)
        {

        }
    }
}
