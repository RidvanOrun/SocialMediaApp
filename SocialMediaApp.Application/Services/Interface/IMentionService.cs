using SocialMediaApp.Application.Model.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApp.Application.Services.Interface
{
    public interface IMentionService
    {
        Task AddMention(AddMentionDTO addMentionDTO);

    }
}
