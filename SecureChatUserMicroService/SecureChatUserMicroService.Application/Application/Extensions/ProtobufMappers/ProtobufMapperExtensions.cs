using ContractualDtos.DTO.Pagination;
using ContractualDtos.DTO.User;
using UserService.Proto;

namespace SecureChatUserMicroService.Application.Application.Extensions.ProtobufMappers
{
    public static class ProtobufMapperExtensions
    {
        #region UserProfile
    
        public static UserResponse ToProtoUserProfileInfo(this UserDtos dto)
        {
            return new UserResponse
            {
                UserId = dto.UserId.ToString(),
                Username = dto.UserName,
                Email = dto.UserEmail,
                DisplayName = dto.UserNickname,
                AvatarUrl = dto.UserAvatarUrl,
                IsActive = dto.IsActive
            };
        }

        public static List<UserResponse> ToProtoUserProfileInfoList(
            this PaginationDtoResponse<UserDtos> pagination)
        {
            return pagination.Items.Select(ToProtoUserProfileInfo).ToList();
        }
    
        #endregion
    }
}