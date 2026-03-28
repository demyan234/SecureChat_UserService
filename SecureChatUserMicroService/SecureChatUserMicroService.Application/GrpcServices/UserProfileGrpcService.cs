using ContractualDtos.DTO.UserProfile;
using Grpc.Core;
using SecureChatUserMicroService.Application.Application.Extensions;
using SecureChatUserMicroService.Application.Application.Extensions.ProtobufMappers;
using SecureChatUserMicroService.Application.Common.Interfaces.IRepository;
using UserProfileService.Proto;

namespace SecureChatUserMicroService.Application.GrpcServices
{
    /// <summary>
    /// GRPC-Сервис для UserProfileEntity
    /// </summary>
    public class UserProfileGrpcService(IUserProfileRepository userProfileRepository)
        : UserProfileService.Proto.UserProfileGrpcService.UserProfileGrpcServiceBase
    {
        private readonly IUserProfileRepository _userProfileRepository =
            userProfileRepository ?? throw new ArgumentNullException(nameof(userProfileRepository));

        /// <summary>
        /// Получение всех профилей
        /// </summary>
        public override async Task<GetAllUserProfilesResponse> GetAllUserProfiles(GetAllUserProfilesRequest request,
            ServerCallContext context)
        {
            try
            {
                var userProfiles = await _userProfileRepository.GetAllAsync(request.Page, request.PageSize,
                    request.Search,
                    request.SortBy, request.SortDirection);

                return new GetAllUserProfilesResponse
                {
                    Success = true,
                    UserProfiles = { userProfiles.ToProtoUserProfileInfoList() }
                };
            }
            catch (Exception ex)
            {
                return new GetAllUserProfilesResponse
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Получение профиля по Id
        /// </summary>
        public override async Task<GetUserProfileByIdResponse> GetUserProfileById(GetUserProfileByIdRequest request,
            ServerCallContext context)
        {
            try
            {
                var userProfile = await _userProfileRepository.Read(request.UserId.ToGuid());
                if (userProfile == null)
                {
                    return new GetUserProfileByIdResponse
                    {
                        Success = false,
                        ErrorMessage = "UserProfile not found"
                    };
                }

                return new GetUserProfileByIdResponse
                {
                    Success = true,
                    UserProfile = userProfile.ToProtoUserProfileInfo()
                };
            }
            catch (Exception ex)
            {
                return new GetUserProfileByIdResponse
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }
    }
}