using HealthChatBox.Core.Domain.Entities;
using HealthChatBox.Core.Domain.Enum;

namespace HealthChatBox.Models.UserModel
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public Role Role { get; set; }
    }

    public class LoginResponseModel : BaseResponse
    {

        public string Token { get; set; }

        public UserResponse Data { get; set; }


    }


    public class GoogleUser
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
