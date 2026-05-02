using ChatService.Proto;

namespace SecureChatUserMicroService.Application.Common.Interfaces.IGrpcClients
{
    public interface IChatGrpcClient
    {
        /// <summary>
        /// Добавление пользователя в ChatGrpcService
        /// </summary>
        Task<AddUserResponse> AddUser(AddUserRequest request);

        /// <summary>
        /// Безопасное удаление пользователя в ChatGrpcService
        /// </summary>
        Task<RemoveUserResponse> RemoveUser(RemoveUserRequest request);
    }
}