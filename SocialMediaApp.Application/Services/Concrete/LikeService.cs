using AutoMapper;
using SocialMediaApp.Application.Model.DTOs;
using SocialMediaApp.Application.Services.Interface;
using SocialMediaApp.DomainLayer.Entities.Concrete;
using SocialMediaApp.Infrastructure.Repository.UnitOfWork;
using System;
using System.Threading.Tasks;

namespace SocialMediaApp.Application.Services.Concrete
{
    public class LikeService : ILikeService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LikeService(UnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        public async Task Like(LikeDTO likeDTO)
        {
            var isLiked = await _unitOfWork.LikeRepository.FirstOrDefault(x => x.AppUserId == likeDTO.AppUserId && x.TweetId == likeDTO.TweetId);
            if (isLiked==null)
            {
                var like = _mapper.Map<LikeDTO, Like>(likeDTO);
                await _unitOfWork.LikeRepository.Add(like);
                await _unitOfWork.Commit();
            }
        }       

        public async Task UnLike(LikeDTO likeDTO)
        {
            var isLiked = await _unitOfWork.LikeRepository.FirstOrDefault(x => x.AppUserId == likeDTO.AppUserId && x.TweetId == likeDTO.TweetId);
            _unitOfWork.LikeRepository.Delete(isLiked);
            await _unitOfWork.Commit();
        }
    }
}
