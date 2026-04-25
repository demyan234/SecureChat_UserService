using ContractualDtos.DTO.Pagination;
using ContractualDtos.DTO.User;
using ContractualDtos.DTO.User.CRUD;

namespace SecureChatUserMicroService.Application.Common.Interfaces.IRepository
{
    public interface IUserRepository
    {
        Task<UserDtos> CreateUser(CreateUserRequest request);
        Task<UserDtos> UpdateUser(UpdateUserRequest request);
        Task<UserDtos> GetUser(Guid userId);
        Task<PaginationDtoResponse<UserDtos>> GetUsers(List<Guid> userIds);
        Task<bool> DeleteUser(Guid userId);
    }
}