using HealthChatBox.Models.UserModel;
using HealthChatBox.Models;
using HealthChatBox.Models.ProfileModel;

namespace HealthChatBox.Core.Application.Interfaces.Services
{
    public interface IProfileService
    {
        Task<BaseResponse<ProfileResponse>> GetProfile(int id);
        Task<BaseResponse<ICollection<ProfileResponse>>> GetAllProfiles();
        Task<BaseResponse> RemoveProfile(int id);
        Task<BaseResponse<ProfileResponse>> CreateProfile(int Userid, ProfileRequest request);
        Task<BaseResponse> UpdateProfile(int id, ProfileRequest request);
    }
}
