using ContractualDtos.DTO.Pagination;
using ContractualDtos.DTO.UserProfile;
using Microsoft.EntityFrameworkCore;
using SecureChatUserMicroService.Application.Common.Interfaces;
using SecureChatUserMicroService.Application.Common.Interfaces.IRepository;
using SecureChatUserMicroService.Domain.Entities;

namespace SecureChatUserMicroService.Infrastructure.Repositories
{
    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly IUserServiceDbContext _context;

        public UserProfileRepository(IUserServiceDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        #region crud

        public async Task<PaginationDtoResponse<UserProfileDtos>> GetAllAsync(
            int page,
            int pageSize,
            string? search = null,
            string? sortBy = null,
            string? sortDirection = "Ascending")
        {
            try
            {
                var dbQuery = _context.UserProfile.AsNoTracking().AsQueryable();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    var searchLower = search.ToLower();
                    dbQuery = dbQuery.Where(u =>
                        u.Nickname.ToLower().Contains(searchLower));
                }

                dbQuery = sortBy?.ToLower() switch
                {
                    "nickname" => sortDirection == "Descending"
                        ? dbQuery.OrderByDescending(u => u.Nickname)
                        : dbQuery.OrderBy(u => u.Nickname),
                    "email" => sortDirection == "Descending"
                        ? dbQuery.OrderByDescending(u => u.User.Email)
                        : dbQuery.OrderBy(u => u.User.Email),

                    _ => dbQuery.OrderBy(u => u.Nickname)
                };

                var totalCount = await dbQuery.CountAsync();
                var searchData = await dbQuery
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .AsNoTracking()
                    .ToListAsync();

                return new PaginationDtoResponse<UserProfileDtos>(GetUserProfileDto(searchData), totalCount);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<UserProfileDtos?> Read(Guid id)
        {
            try
            {
                var user = await _context.UserProfile.FindAsync(id);
                return user == null ? null : GetUserProfileDto(user);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        /// <summary>
        /// Формирование DTO для return
        /// </summary>
        private static UserProfileDtos GetUserProfileDto(UserProfileEntity e)
        {
            return new UserProfileDtos(
                e.Id,
                e.Name,
                e.Nickname,
                e.AvatarUrl,
                e.StatusQuote,
                e.IsBlocked,
                e.IsDeleted,
                e.Status,
                e.UserId
            );
        }

        /// <summary>
        /// Формирование DTOs для return
        /// </summary>
        private static List<UserProfileDtos> GetUserProfileDto(List<UserProfileEntity> entities)
        {
            return entities.Select(e => new UserProfileDtos(
                e.Id,
                e.Name,
                e.Nickname,
                e.AvatarUrl,
                e.StatusQuote,
                e.IsBlocked,
                e.IsDeleted,
                e.Status,
                e.UserId
            )).ToList();
        }
    }
}