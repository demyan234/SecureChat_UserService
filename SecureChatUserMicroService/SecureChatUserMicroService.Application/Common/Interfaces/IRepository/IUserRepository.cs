using ContractualDtos.DTO.Pagination;
using ContractualDtos.DTO.User;
using ContractualDtos.DTO.User.CrudDto;

namespace SecureChatUserMicroService.Application.Common.Interfaces.IRepository
{
    public interface IUserRepository
    {
        /// <summary>
        /// Вывод всех пользователей
        /// </summary>
        Task<PaginationDtoResponse<UserDtos>> GetAllAsync(
            int page,
            int pageSize,
            string? search = null,
            string? sortBy = null,
            string? sortDirection = "Ascending");

        /// <summary>
        /// Добавление нового пользователя
        /// </summary>
        /// <param name="dto">передаваемые параметры из запроса</param>
        Task<Guid> Create(CreateUserRequestDto dto);

        /// <summary>
        /// Вывод определенного пользователя
        /// </summary>
        /// <param name="id">передаваемые параметры из запроса</param>
        Task<UserDtos?> Read(Guid id);

        /// <summary>
        /// Изменение определенного пользователя
        /// </summary>
        /// <param name="dto">передаваемые параметры из запроса</param>
        Task<UserDtos?> Update(UpdateUserRequestDto dto);

        /// <summary>
        /// Деактивация определенного пользователя
        /// </summary>
        /// <param name="id">передаваемые параметры из запроса</param>
        Task<bool> SafeDelete(Guid id);
    }
}