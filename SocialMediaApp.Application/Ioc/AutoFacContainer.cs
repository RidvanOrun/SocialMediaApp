using Autofac;
using FluentValidation;
using SocialMediaApp.Application.Model.DTOs;
using SocialMediaApp.Application.Services.Concrete;
using SocialMediaApp.Application.Services.Interface;
using SocialMediaApp.Application.Services.Validation;
using SocialMediaApp.Application.Validation;
using SocialMediaApp.DomainLayer.Repository.UnitOfWork;
using SocialMediaApp.Infrastructure.Repository.UnitOfWork;


namespace SocialMediaApp.Application.Ioc
{
    public class AutoFacContainer:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AppUserService>().As<IAppUserService>().InstancePerLifetimeScope();
            builder.RegisterType<FollowService>().As<IFollowService>().InstancePerLifetimeScope();
            builder.RegisterType<LikeService>().As<ILikeService>().InstancePerLifetimeScope();
            builder.RegisterType<TweetServices>().As<ITweetService>().InstancePerLifetimeScope();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();


            builder.RegisterType<LoginValidation>().As<IValidator<LoginDTO>>().InstancePerLifetimeScope();
            builder.RegisterType<RegisterValidation>().As<IValidator<RegisterDTO>>().InstancePerLifetimeScope();
            builder.RegisterType<TweetValidation>().As<IValidator<AddTweetDTO>>().InstancePerLifetimeScope();
        }
    }
}
