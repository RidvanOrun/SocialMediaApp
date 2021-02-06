using Microsoft.EntityFrameworkCore;
using SocialMediaApp.DomainLayer.Repository.EntitytypeRepository;
using SocialMediaApp.DomainLayer.Repository.UnitOfWork;
using SocialMediaApp.Infrastructure.Context;
using SocialMediaApp.Infrastructure.Repository.EntityTypeRepoistory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApp.Infrastructure.Repository.UnitOfWork
{
    //unitOfWork u kullanma amacımız neydi; bankamatik örneğindeki gibi her seferinde db ye gönderip işlem yapmasını önlemek ve en son onaydan sonra db ile bağlantı kurmak. Burada da Repositorylerin bağlantısını tek elden yürütmek amacıyla UnitOfWork kullanılıyor. 
    public class UnitOfWork:IUnitOfWork //unit of work u implement ettik. 
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public UnitOfWork(ApplicationDbContext applicationDbContext)
        {
            this._applicationDbContext = applicationDbContext ?? throw new ArgumentNullException("database can not be null");
            //ctor injection yönetminde hata çıkarsa. ?? karar mekanizması else benzeri db dönsün yada cümlecik fırlatsın
        }

        private IAppUserRepository _appUserRepository;
        public IAppUserRepository AppUserRepository
        {
            //get { return _appUserRepository ?? (_appUserRepository = new AppUserRepository(_applicationDbContext)); }

            get
            {
                if (_appUserRepository == null) _appUserRepository = new AppUserRepository(_applicationDbContext);
                return _appUserRepository; 
            }


        }

        private IFollowRepository _followRepository;
        public IFollowRepository FollowRepository { get => _followRepository ?? (_followRepository = new FollowRepository(_applicationDbContext)); }


        private ILikeRepository _likeRepository;
        public ILikeRepository LikeRepository { get => _likeRepository ?? (_likeRepository = new LikeRepository(_applicationDbContext)); }


        private IMentionRepository _mentionRepository;
        public IMentionRepository MentionRepository { get => _mentionRepository ?? (_mentionRepository = new MentionRepository(_applicationDbContext)); }


        private IShareRepository _shareRepository;
        public IShareRepository ShareRepository { get => _shareRepository ?? (_shareRepository = new ShareRepository(_applicationDbContext)); }

        private ITweetRepository _tweetRepository;
        public ITweetRepository TweetRepository { get => _tweetRepository ?? (_tweetRepository = new TweetRepository(_applicationDbContext)); }

        public async Task Commit() => await _applicationDbContext.SaveChangesAsync();
        public async Task ExecuteSqlRaw(string sql, params object[] paramters) => await _applicationDbContext.Database.ExecuteSqlRawAsync(sql, paramters);
        private bool isDisposing = false;

        //????? bunlar neden kullanıldı UNİT OF WORK için önemli bir içerik.... 
        //  İşlemler bittiğinden GC kullanılması için yazıldı. Nesne yönetimi için kullanılan bir yapıdır. 

        public async ValueTask DisposeAsync()
        {
            if (!isDisposing)
            {
                isDisposing = true;
                await DisposeAsync(true); //db.Dispose yazabildirdik ama burada farklı bir kullanım.
                GC.SuppressFinalize(this);
            }
        }

        private async Task DisposeAsync(bool disposing)
        {
            if (disposing) await _applicationDbContext.DisposeAsync();

        }



    }
}
