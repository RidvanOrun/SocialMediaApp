using AutoMapper;
using SocialMediaApp.Application.Model.DTOs;
using SocialMediaApp.Application.Services.Interface;
using SocialMediaApp.DomainLayer.Entities.Concrete;
using SocialMediaApp.DomainLayer.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApp.Application.Services.Concrete
{
    public class FollowService: IFollowService
    {
        private readonly IUnitOfWork _unitOfwork;
        private readonly IMapper _mapper;

        public FollowService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfwork = unitOfWork;
            this._mapper = mapper;
        }

        /// <summary>
        /// Takibe almak için yapıyorum. öncelikle üzerine tıkladığım kişi ıd sinden kontrol edilip following lerimin arasındamı var mı yokmu eğer yoksa onu ekliyorum.
        /// </summary>
        /// <param name="Follow"></param>
        /// <returns> Geri dönüş tipi yok sadece folowerkaydediyor.</returns>
        public async Task Follow(FollowDTO followDTO)
        {
            var isFollowExist = await _unitOfwork.FollowRepository.FirstOrDefault(x => x.FollowerId == followDTO.FollowerId && x.FollowingId==followDTO.FollowingId);

            if (isFollowExist==null)
            {
                var follow = _mapper.Map<FollowDTO, Follow>(followDTO);
                await _unitOfwork.FollowRepository.Add(follow);
                await _unitOfwork.Commit();
            }
        }

        /// <summary> ????????????
        /// FollowerIdsi dışarıdan gelen ıd ye eşit olan ıd yiye göre listele
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<int>> Followers(int id)
        {
            var followerList = await _unitOfwork.FollowRepository.GetFilteredList(
                selector:x=>x.FollowerId,
                expression: x=>x.FollowerId==id             
                );
            return followerList;
        }

        public async Task<List<int>> Followings(int id)
        {
            var followerList = await _unitOfwork.FollowRepository.GetFilteredList(
                selector: x => x.FollowerId,
                expression: x => x.FollowerId == id
                );
            return followerList;
        }

        public async Task<bool> IsFollowing(FollowDTO followDTO)
        {
            var isFollowExist = await _unitOfwork.FollowRepository.Any(x => x.FollowerId == followDTO.FollowerId && x.FollowerId == followDTO.FollowingId);
            return isFollowExist;
        }

        /// <summary>
        /// takipten çıkarmak demek eğer takiplistemde ise 
        /// </summary>
        /// <param name="UnFollow"></param>
        /// <returns></returns>
        public async Task UnFollow(FollowDTO followDTO)
        {
            var isFollowExsist = await _unitOfwork.FollowRepository.FirstOrDefault(x => x.FollowingId == followDTO.FollowingId);

            if (isFollowExsist!=null)
            {
                var follow = _mapper.Map<FollowDTO, Follow>(followDTO);
                _unitOfwork.FollowRepository.Delete(follow);
                await _unitOfwork.Commit();
            }
        }
    }
}
