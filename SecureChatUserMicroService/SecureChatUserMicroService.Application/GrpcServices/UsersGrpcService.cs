using Grpc.Core;
using SecureChatUserMicroService.Application.Application.Extensions;
using SecureChatUserMicroService.Application.Application.Extensions.ProtobufMappers;
using SecureChatUserMicroService.Application.Common.Interfaces.IRepository;
using UserService.Proto;

namespace SecureChatUserMicroService.Application.GrpcServices
{
    /// <summary>
    /// GRPC-Сервис для UserProfileEntity
    /// </summary>
    public class UsersGrpcService(IUserRepository userRepository) : UserGrpcService.UserGrpcServiceBase
    {
        private readonly IUserRepository _userRepository =
            userRepository ?? throw new ArgumentNullException(nameof(userRepository));

        /// <summary>
        /// Добавление нового пользователя
        /// </summary>
        public override async Task<UserResponse> CreateUser(CreateUserRequest request, ServerCallContext context)
        {
            try
            {
                var newUser = await _userRepository.CreateUser(
                    new ContractualDtos.DTO.User.CRUD.CreateUserRequest(request.UserId.ToGuid(), request.Username,
                        request.Email, request.DisplayName));
                return new UserResponse
                {
                    UserId = newUser.UserId.ToString(),
                    Username = newUser.UserName,
                    Email = newUser.UserEmail,
                    DisplayName = newUser.UserNickname,
                    AvatarUrl = newUser.UserAvatarUrl,
                    IsActive = newUser.IsActive
                };
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }

        /// <summary>
        /// Изменение данных пользователя
        /// </summary>
        public override async Task<UserResponse> UpdateUser(UpdateUserRequest request, ServerCallContext context)
        {
            try
            {
                var updateUser = await _userRepository.UpdateUser(new(request.UserId.ToGuid(), request.Email,
                    request.DisplayName, request.AvatarUrl, request.Username));
                return new UserResponse
                {
                    UserId = updateUser.UserId.ToString(),
                    Username = updateUser.UserName,
                    Email = updateUser.UserEmail,
                    DisplayName = updateUser.UserNickname,
                    AvatarUrl = updateUser.UserAvatarUrl,
                    IsActive = updateUser.IsActive
                };
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }

        /// <summary>
        /// Получение пользователя
        /// </summary>
        public override async Task<UserResponse> GetUser(GetUserRequest request, ServerCallContext context)
        {
            try
            {
                var user = await _userRepository.GetUser(request.UserId.ToGuid());
                return new UserResponse
                {
                    UserId = user.UserId.ToString(),
                    Username = user.UserName,
                    Email = user.UserEmail,
                    DisplayName = user.UserNickname,
                    AvatarUrl = user.UserAvatarUrl,
                    IsActive = user.IsActive
                };
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }

        /// <summary>
        /// Получение пользователей по их "Id"
        /// </summary>
        public override async Task<UsersListResponse> GetUsers(GetUsersRequest request, ServerCallContext context)
        {
            try
            {
                var userIds = request.UserIds.Select(id => id.ToGuid()).ToList();
                var users = await _userRepository.GetUsers(userIds);
                return new UsersListResponse
                {
                    Users = { users.ToProtoUserProfileInfoList() },
                    Total = users.TotalCount
                };
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }

        /// <summary>
        /// Удаление пользователя
        /// </summary>
        public override async Task<DeleteUserResponse> DeleteUser(DeleteUserRequest request, ServerCallContext context)
        {
            try
            {
                var deleteUser = await _userRepository.DeleteUser(request.UserId.ToGuid());
                return new DeleteUserResponse
                {
                    Success = deleteUser
                };
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }
    }
}