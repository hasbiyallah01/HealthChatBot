

using HealthChatBox.Models;
using HealthChatBox.Models.UserModel;

namespace HealthChatBox.Core.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<BaseResponse<UserResponse>> GetUser(int id);
        public Task<BaseResponse<UserResponse>> GetUser(string email);
        Task<BaseResponse<ICollection<UserResponse>>> GetAllUsers();
        Task<BaseResponse> RemoveUser(int id);
        Task<BaseResponse> UpdateUser(int id, UserRequest request);
        Task<BaseResponse<UserResponse>> Login(LoginRequestModel model);
        Task<BaseResponse<UserResponse>> CreateUser(UserRequest request);
    }
}
