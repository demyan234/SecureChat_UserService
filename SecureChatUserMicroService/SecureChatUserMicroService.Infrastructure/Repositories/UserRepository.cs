using ContractualDtos.DTO.Pagination;
using ContractualDtos.DTO.User;
using ContractualDtos.DTO.User.CrudDto;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using SecureChatUserMicroService.Application.Common.Interfaces;
using SecureChatUserMicroService.Application.Common.Interfaces.IRepository;
using SecureChatUserMicroService.Domain.Entities;
using SecureChatUserMicroService.Domain.Enums.User;

namespace SecureChatUserMicroService.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IUserServiceDbContext _context;

        public UserRepository(IUserServiceDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        #region crud

        public async Task<PaginationDtoResponse<UserDtos>> GetAllAsync(
            int page,
            int pageSize,
            string? search = null,
            string? sortBy = null,
            string? sortDirection = "Ascending")
        {
            try
            {
                var dbQuery = _context.User.AsNoTracking().AsQueryable();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    var searchLower = search.ToLower();
                    dbQuery = dbQuery.Where(u =>
                        u.Email.ToLower().Contains(searchLower));
                }

                dbQuery = sortBy?.ToLower() switch
                {
                    "email" => sortDirection == "Descending"
                        ? dbQuery.OrderByDescending(u => u.Email)
                        : dbQuery.OrderBy(u => u.Email),
                    /*TODO: проверить*/
                    "nickname" => sortDirection == "Descending"
                        ? dbQuery.OrderByDescending(u => u.UserProfiles.OrderByDescending(x => x.Nickname))
                        : dbQuery.OrderBy(u => u.UserProfiles.OrderBy(x => x.Nickname)),

                    _ => dbQuery.OrderBy(u => u.Email)
                };

                var totalCount = await dbQuery.CountAsync();
                var searchData = await dbQuery
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .AsNoTracking()
                    .ToListAsync();

                return new PaginationDtoResponse<UserDtos>(GetUserProfileDto(searchData), totalCount);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<Guid> Create(CreateUserRequestDto dto)
        {
            try
            {
                var newUser = new UserEntity(dto.Email, SystemClock.Instance.GetCurrentInstant(),
                    SystemClock.Instance.GetCurrentInstant(), null);
                /*TODO: изменить AvatarUrl на дефолтный*/
                var newUserProfile = new UserProfileEntity(dto.Name, dto.Nickname, dto.AvatarUrl, dto.StatusQuote,
                    false, false, UserStatusEnum.User.Id, newUser.Id);

                _context.User.Add(newUser);
                _context.UserProfile.Add(newUserProfile);
                await SaveChanges();

                return newUser.Id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<UserDtos?> Read(Guid id)
        {
            try
            {
                var user = await _context.User.FindAsync(id);
                return user == null ? null : GetUserProfileDto(user);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<UserDtos?> Update(UpdateUserRequestDto dto)
        {
            try
            {
                var user = await _context.User.FindAsync(dto.Id);
                var userProfile = user?.UserProfiles.FirstOrDefault(x => x.Id == dto.Id);

                if (user == null || userProfile == null) return null;
                if (userProfile.Nickname != dto.Nickname)
                {
                    var checkNewNickname = await _context.UserProfile.AnyAsync(t => t.Nickname == dto.Nickname);
                    if (checkNewNickname)
                    {
                        throw new Exception("Такой никнейм уже занят");
                    }
                }

                user.Update(dto.Email, SystemClock.Instance.GetCurrentInstant(), null);
                userProfile.Update(dto.Name, dto.Nickname, dto.AvatarUrl, dto.StatusQuote, dto.IsBlocked, dto.IsDeleted,
                    dto.Status);

                _context.User.Update(user);
                _context.UserProfile.Update(userProfile);
                await SaveChanges();

                return GetUserProfileDto(user);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> SafeDelete(Guid id)
        {
            try
            {
                var user = await _context.User.FindAsync([id]);
                var userProfile = user?.UserProfiles.FirstOrDefault(x => x.Id == user.Id);

                if (user == null || userProfile == null) return false;
                if (userProfile is { IsDeleted: true })
                {
                    return true;
                }

                user.Update(null, SystemClock.Instance.GetCurrentInstant(), SystemClock.Instance.GetCurrentInstant());
                userProfile.Update(null, null, null, null, null, true, null);

                _context.User.Update(user);
                _context.UserProfile.Update(userProfile);
                await SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public Task<bool> GetByEmail(string email)
        {
            return _context.User.AnyAsync(u => u.Email == email);
        }

        #endregion


        private async Task SaveChanges()
        {
            await _context.SaveChangesAsync(CancellationToken.None);
        }

        /// <summary>
        /// Формирование DTO для return
        /// </summary>
        private static UserDtos GetUserProfileDto(UserEntity e)
        {
            return new UserDtos(
                e.Id,
                e.Email,
                e.CreatedTime,
                e.LastUpdateTime,
                e.DeleteTime ?? null
            );
        }

        /// <summary>
        /// Формирование DTOs для return
        /// </summary>
        private static List<UserDtos> GetUserProfileDto(List<UserEntity> entities)
        {
            return entities.Select(e => new UserDtos(
                e.Id,
                e.Email,
                e.CreatedTime,
                e.LastUpdateTime,
                e.DeleteTime ?? null
            )).ToList();
        }
    }
}