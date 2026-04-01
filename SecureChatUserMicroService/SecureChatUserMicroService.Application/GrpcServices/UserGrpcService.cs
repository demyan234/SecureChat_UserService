using ContractualDtos.DTO.User;
using ContractualDtos.DTO.User.CrudDto;
using Grpc.Core;
using SecureChatUserMicroService.Application.Application.Extensions;
using SecureChatUserMicroService.Application.Application.Extensions.ProtobufMappers;
using SecureChatUserMicroService.Application.Common.Interfaces.IRepository;
using SecureChatUserMicroService.Domain.Enums.User;
using UserService.Proto;

namespace SecureChatUserMicroService.Application.GrpcServices
{
    /// <summary>
    /// GRPC-Сервис для UserEntity
    /// </summary>
    public class UserGrpcService(IUserRepository userRepository) : UserService.Proto.UserGrpcService.UserGrpcServiceBase
    {
        private readonly IUserRepository _userRepository =
            userRepository ?? throw new ArgumentNullException(nameof(userRepository));

        /// <summary>
        /// Вывод всех пользователей
        /// </summary>
        public override async Task<GetAllUserResponse> GetAllUser(GetAllUserRequest request, ServerCallContext context)
        {
            try
            {
                var users = await _userRepository.GetAllAsync(
                    request.Page,
                    request.PageSize,
                    request.Search,
                    request.SortBy,
                    request.SortDirection);
                return new GetAllUserResponse
                {
                    Success = true,
                    Count = users.TotalCount,
                    Users = { users.ToProtoUserInfoList() }
                };
            }
            catch (Exception ex)
            {
                return new GetAllUserResponse
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Создание пользователя и его профиля
        /// </summary>
        public override async Task<CreateUserResponse> CreateUser(
            CreateUserRequest request,
            ServerCallContext context)
        {
            try
            {
                var validate = ValidateData(request);
                if (!string.IsNullOrEmpty(validate))
                {
                    return new CreateUserResponse
                    {
                        Success = false,
                        ErrorMessage = validate
                    };
                }

                var createUserDto = new CreateUserRequestDto(request.Email, request.Name, request.Nickname,
                    request.AvatarUrl, request.StatusQuote);
                var user = await _userRepository.Create(createUserDto);

                return new CreateUserResponse
                {
                    Success = true,
                    Id = user.ToString()
                };
            }
            catch (Exception ex)
            {
                return new CreateUserResponse
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Получение пользователя по Id
        /// </summary>
        public override async Task<GetUserResponse> GetUser(GetUserRequest request, ServerCallContext context)
        {
            if (request.Id == string.Empty)
            {
                return new GetUserResponse
                {
                    Success = false,
                    ErrorMessage = "Couldn't find the user"
                };
            }

            try
            {
                var user = await _userRepository.Read(request.Id.ToGuid());
                if (user == null)
                {
                    return new GetUserResponse
                    {
                        Success = false,
                        ErrorMessage = "User not found"
                    };
                }

                return new GetUserResponse
                {
                    Success = true,
                    User = user.ToProtoUserInfo()
                };
            }
            catch (Exception ex)
            {
                return new GetUserResponse
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Изменение пользователя и его профиля
        /// </summary>
        public override async Task<UpdateUserResponse> UpdateUser(UpdateUserRequest request, ServerCallContext context)
        {
            try
            {
                _ = await _userRepository.Update(new UpdateUserRequestDto(request.Id.ToGuid(),
                    request.Email, request.Name, request.Nickname, request.AvatarUrl, request.StatusQuote,
                    request.IsBlocked, request.IsDeleted));

                return new UpdateUserResponse
                {
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new UpdateUserResponse
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Удаление пользователя
        /// </summary>
        public override async Task<DeleteUserResponse> DeleteUser(DeleteUserRequest request, ServerCallContext context)
        {
            try
            {
                _ = await _userRepository.SafeDelete(request.Id.ToGuid());

                return new DeleteUserResponse
                {
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new DeleteUserResponse
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Валидация входных значений
        /// TODO: Вынести во Fluent Validate
        /// </summary>
        private static string ValidateData(CreateUserRequest request)
        {
            if (string.IsNullOrEmpty(request.Email))
            {
                return $"{nameof(request.Email)} is null or empty";
            }

            if (string.IsNullOrEmpty(request.Name))
            {
                return $"{nameof(request.Name)} is null or empty";
            }

            if (string.IsNullOrEmpty(request.Nickname))
            {
                return $"{nameof(request.Nickname)} is null or empty";
            }

            if (string.IsNullOrEmpty(request.AvatarUrl))
            {
                return $"{nameof(request.AvatarUrl)} is null or empty";
            }

            return string.Empty;
        }
    }
}