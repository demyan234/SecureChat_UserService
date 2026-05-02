using ContractualDtos.DTO.Pagination;
using ContractualDtos.DTO.User;
using ContractualDtos.DTO.User.CRUD;

namespace SecureChatUserMicroService.Application.Common.Interfaces.IRepository
{
    public interface IUserRepository
    {
        /// <summary>
        /// Создание пользователя
        /// </summary>
        Task<UserDtos> CreateUser(CreateUserRequest request);

        /// <summary>
        /// Обновление пользователя
        /// </summary>
        Task<UserDtos> UpdateUser(UpdateUserRequest request);

        /// <summary>
        /// Получение пользователя
        /// </summary>
        Task<UserDtos> GetUser(Guid userId);

        /// <summary>
        /// Получение пользователей через поиск
        /// </summary>
        Task<PaginationDtoResponse<UserDtos>> GetUsers(string search);

        /// <summary>
        /// Получение пользователей по Ids
        /// </summary>
        Task<PaginationDtoResponse<UserDtos>> GetUsers(List<Guid> userIds);

        /// <summary>
        /// Безопасное удаление
        /// </summary>
        Task<bool> DeleteUser(Guid userId);
    }
}