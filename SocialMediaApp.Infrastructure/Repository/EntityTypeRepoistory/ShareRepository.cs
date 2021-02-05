using SocialMediaApp.DomainLayer.Entities.Concrete;
using SocialMediaApp.DomainLayer.Repository.EntitytypeRepository;
using SocialMediaApp.Infrastructure.Context;
using SocialMediaApp.Infrastructure.Repository.BaseRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMediaApp.Infrastructure.Repository.EntityTypeRepoistory
{
    public class ShareRepository:BaseRepository<Share>,IShareRepository
    {
        public ShareRepository(ApplicationDbContext applicationDbContext) :base(applicationDbContext) 
        {

        }
    }
}
