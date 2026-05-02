using ContractualDtos.DTO.Pagination;
using ContractualDtos.DTO.User;
using ContractualDtos.DTO.User.CRUD;
using Microsoft.EntityFrameworkCore;
using SecureChatUserMicroService.Application.Common.Interfaces;
using SecureChatUserMicroService.Application.Common.Interfaces.IGrpcClients;
using SecureChatUserMicroService.Application.Common.Interfaces.IRepository;
using SecureChatUserMicroService.Domain.Entities;
using SecureChatUserMicroService.Domain.Enums.User;

namespace SecureChatUserMicroService.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IUserServiceDbContext _context;
        private readonly IChatGrpcClient _chatGrpcServiceClient;

        public UserRepository(IUserServiceDbContext context, IChatGrpcClient chatGrpcServiceClient)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _chatGrpcServiceClient =
                chatGrpcServiceClient ?? throw new ArgumentNullException(nameof(chatGrpcServiceClient));
        }

        #region crud

        public async Task<UserDtos> CreateUser(CreateUserRequest request)
        {
            if (await _context.Users.FirstOrDefaultAsync(un => un.Nickname == request.UserNickname) != null)
            {
                throw new("UserNickname already exists");
            }

            if (await _context.Users.FirstOrDefaultAsync(ue => ue.Email == request.UserEmail) != null)
            {
                throw new("UserEmail already exists");
            }
            
            //TODO: добавить ссылку на дефолт аватар
            var newUser = new UsersEntity(request.UserId,
                request.UserEmail,
                request.UserNickname,
                request.UserName,
                "",
                UserRolesEnum.User.Id);

            //Добавление пользователя в ChatGrpcService
            await _chatGrpcServiceClient.AddUser(new()
            {
                UserId = newUser.Id.ToString(),
            });
            
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync(CancellationToken.None);

            return GetUserDto(newUser);
        }

        public async Task<UserDtos> UpdateUser(UpdateUserRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(un => un.Id == request.UserId) ??
                       throw new ArgumentNullException(nameof(request.UserId));
            user.Update(request.Email, request.Nickname, request.AvatarUrl, request.UserName);
            
            _context.Users.Update(user);
            await _context.SaveChangesAsync(CancellationToken.None);

            return GetUserDto(user);
        }

        public async Task<UserDtos> GetUser(Guid userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(un => un.Id == userId) ??
                       throw new ArgumentNullException(nameof(userId));
            return GetUserDto(user);
        }

        /// <summary>
        /// TODO: сделать минимальную длину символов 2
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public async Task<PaginationDtoResponse<UserDtos>> GetUsers(string search)
        {
            var query = _context.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search) && search.Length >= 2)
            {
                query = query.Where(u => 
                    u.Nickname.Contains(search) || 
                    u.Name.Contains(search));
            }

            var totalCount = await query.CountAsync();
            var users = await query
                .OrderBy(u => u.Id)
                .ToListAsync();

            var dtos = GetUserDto(users).ToList();
            return new(dtos, totalCount);
        }

        public async Task<PaginationDtoResponse<UserDtos>> GetUsers(List<Guid> userIds)
        {
            var users = _context.Users.Where(un => userIds.Contains(un.Id)).ToList();
            return new(GetUserDto(users).ToList(), users.Count);
        }

        public async Task<bool> DeleteUser(Guid userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(un => un.Id == userId) ??
                       throw new ArgumentNullException(nameof(userId));
            await _chatGrpcServiceClient.RemoveUser(new()
            {
                UserId = user.Id.ToString()
            });
            
            _context.Users.Update(user);
            await _context.SaveChangesAsync(CancellationToken.None);
            
            return true;
        }

        #endregion

        /// <summary>
        /// Формирование DTO для return
        /// </summary>
        private static UserDtos GetUserDto(UsersEntity e)
        {
            return new(
                e.Id,
                e.Name,
                e.Email,
                e.Nickname,
                e.AvatarUrl,
                e.DeletedAt == null
            );
        }

        /// <summary>
        /// Формирование DTOs для return
        /// </summary>
        private static List<UserDtos> GetUserDto(List<UsersEntity> entities)
        {
            return entities.Select(e => new UserDtos(
                e.Id,
                e.Name,
                e.Email,
                e.Nickname,
                e.AvatarUrl,
                e.DeletedAt == null
            )).ToList();
        }
    }
}