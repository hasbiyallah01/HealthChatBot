using HealthChatBox.Core.Application.Interfaces.Repositories;
using HealthChatBox.Core.Application.Interfaces.Services;
using HealthChatBox.Core.Domain.Entities;
using HealthChatBox.Infrastructure.Repositories;
using HealthChatBox.Models;

namespace HealthChatBox.Services
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;
        private readonly IUnitOfWork _unitOfWork;
        public ChatService(IChatRepository chatRepository, IUnitOfWork unitOfWork)
        {
            _chatRepository = chatRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse<ChatResponseDto>> SaveChatEntryAsync(ChatEntryDto chatEntryDto)
        {
            var chatEntry = new ChatEntry
            {
                Query = chatEntryDto.Query,
                Response = chatEntryDto.Response,
                Timestamp = DateTime.UtcNow,
                DateCreated = DateTime.UtcNow,
                IsDeleted = false,
            };

             await _chatRepository.AddAsync(chatEntry);
            await _unitOfWork.SaveAsync();

            return new BaseResponse<ChatResponseDto>
            {
                 IsSuccessful = true,   
                  Message = "Completed",
                   Value = new ChatResponseDto
                   {
                       Id = chatEntry.Id,
                       Query = chatEntry.Query,
                       Response = chatEntry.Response,
                   }
            };
        }

        public async Task<ChatResponseDto> GetChat(int id)
        {
            var chat = await _chatRepository.GetAsync(id);
            if (chat == null)
            {
                return null;
            }

            return new ChatResponseDto
            {
                    Id = chat.Id,
                    Query = chat.Query,
                    Response = chat.Response,
            };
        }


        public async Task<BaseResponse<ICollection<ChatResponseDto>>> GetAllChats()
        {
            var chat = await _chatRepository.GetAllAsync();

            return new BaseResponse<ICollection<ChatResponseDto>>
            {
                Message = "List of roles",
                IsSuccessful = true,
                Value = chat.Select(role => new ChatResponseDto
                {
                    Id = role.Id,
                    Query = role.Query,
                    Response = role.Response,
                }).ToList()
            };
        }

        public async Task<BaseResponse> RemoveChat(int id)
        {
            var chat = await _chatRepository.GetAsync(id);
            if (chat == null)
            {
                return new BaseResponse
                {
                    Message = "Role does not exists",
                    IsSuccessful = false
                };
            }

            _chatRepository.Remove(chat);
            await _unitOfWork.SaveAsync();

            return new BaseResponse
            {
                Message = "Role deleted successfully",
                IsSuccessful = true
            };
        }
    }
}


