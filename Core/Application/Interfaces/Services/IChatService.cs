using HealthChatBox.Models;
namespace HealthChatBox.Core.Application.Interfaces.Services
{
    public interface IChatService
    {
        Task<BaseResponse<ChatResponseDto>> SaveChatEntryAsync(ChatEntryDto chatEntryDto);
        Task<ChatResponseDto> GetChat(int id);
        Task<BaseResponse<ICollection<ChatResponseDto>>> GetAllChats();
        Task<BaseResponse> RemoveChat(int id);

    }

}

