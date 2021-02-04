using SocialMediaApp.DomainLayer.Repository.EntitytypeRepository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApp.DomainLayer.Repository.UnitOfWork
{
    public interface IUnitOfWork
    {
        IAppUserRepository AppUserRepository { get; }
        IFollowRepository FollowRepository { get; }
        ILikeRepository LikeRepository { get; }
        IMentionRepository MentionRepository { get; }
        IShareRepository ShareRepository { get; }
        ITweetRepository TweetRepository { get; }

        Task Commit(); //Başarılı bir işlemin sonucunda çalıştırılır. İşlemin başlamasından itibaren tüm değişikliklerin veri tabanına uygulamasını temin eder.

        Task ExecuteSqlRaw(string sql, params object[] paramters); //Mevcut sql sorgularımızı doğrudan veri tabanına göndermek için kullanılan methodtur.
    }
}
