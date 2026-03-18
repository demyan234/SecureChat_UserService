using ContractualDtos.DTO.Pagination;
using ContractualDtos.DTO.UserProfile;

namespace SecureChatUserMicroService.Application.Common.Interfaces.IRepository
{
    public interface IUserProfileRepository
    {
        /// <summary>
        /// Вывод всех профилей пользователей
        /// </summary>
        Task<PaginationDtoResponse<UserProfileDtos>> GetAllAsync(
            int page,
            int pageSize,
            string? search = null,
            string? sortBy = null,
            string? sortDirection = "Ascending");

        /// <summary>
        /// Вывод определенного профиля пользователя
        /// </summary>
        /// <param name="id">передаваемый параметр из запроса</param>
        Task<UserProfileDtos?> Read(Guid id);
    }
}