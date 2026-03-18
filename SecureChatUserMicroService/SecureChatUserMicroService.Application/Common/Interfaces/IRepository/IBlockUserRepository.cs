using ContractualDtos.DTO.BlockUser;
using ContractualDtos.DTO.BlockUser.CrudDto;
using ContractualDtos.DTO.Pagination;

namespace SecureChatUserMicroService.Application.Common.Interfaces.IRepository
{
    public interface IBlockUserRepository
    {
        /// <summary>
        /// Вывод всех блокировок
        /// </summary>
        Task<List<BlockUserDtos>> GetAllAsync();

        /// <summary>
        /// Добавление новой блокировки
        /// </summary>
        /// <param name="dto">передаваемые параметры из запроса</param>
        Task<Guid> Create(CreateBlockUserRequestDto dto);

        /// <summary>
        /// Вывод определенной блокировки
        /// </summary>
        /// <param name="id">передаваемые параметры из запроса</param>
        Task<BlockUserDtos?> Read(Guid id);

        /// <summary>
        /// Изменение определенной блокировки
        /// </summary>
        /// <param name="dto">передаваемые параметры из запроса</param>
        Task<BlockUserDtos?> Update(UpdateBlockUserRequestDto dto);

        /// <summary>
        /// Деактивация определенной блокировки
        /// </summary>
        /// <param name="id">передаваемые параметры из запроса</param>
        Task<bool> DeactivateRecord(Guid id);
    }
}