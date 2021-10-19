using Application.Models;
using System;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IGameInvitationService
    {
        Task<GameInvitationModel> Add(GameInvitationModel gameInvitationModel);
        Task<GameInvitationModel> Get(Guid id);
        Task Update(GameInvitationModel gameInvitationModel);
    }
}