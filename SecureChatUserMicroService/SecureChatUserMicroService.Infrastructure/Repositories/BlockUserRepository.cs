using ContractualDtos.DTO.BlockUser;
using ContractualDtos.DTO.BlockUser.CrudDto;
using Microsoft.EntityFrameworkCore;
using SecureChatUserMicroService.Application.Common.Interfaces;
using SecureChatUserMicroService.Application.Common.Interfaces.IRepository;
using SecureChatUserMicroService.Domain.Entities;

namespace SecureChatUserMicroService.Infrastructure.Repositories
{
    public class BlockUserRepository : IBlockUserRepository
    {
        private readonly IUserServiceDbContext _context;

        public BlockUserRepository(IUserServiceDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        #region crud

        public async Task<List<BlockUserDtos>> GetAllAsync()
        {
            try
            {
                var blockUsers = await _context.BlockUser.ToListAsync();
                return GetBlockUserDto(blockUsers);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<Guid> Create(CreateBlockUserRequestDto dto)
        {
            try
            {
                var newUserBlock = new BlockUserEntity(dto.StartDate, dto.EndDate, dto.UserProfileId);

                _context.BlockUser.Add(newUserBlock);
                await SaveChanges();

                return newUserBlock.Id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<BlockUserDtos?> Read(Guid id)
        {
            try
            {
                var user = await _context.BlockUser.FindAsync(id);
                return user == null ? null : GetBlockUserDto(user);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<BlockUserDtos?> Update(UpdateBlockUserRequestDto dto)
        {
            try
            {
                var blockUser = await _context.BlockUser.FindAsync(dto.Id);
                if (blockUser == null) return null;

                blockUser.Update(dto.StartDate, dto.EndDate, dto.IsActive);
                _context.BlockUser.Update(blockUser);
                await SaveChanges();

                return GetBlockUserDto(blockUser);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> DeactivateRecord(Guid id)
        {
            try
            {
                var blockUser = await _context.BlockUser.FindAsync([id]);
                if (blockUser == null) return false;

                blockUser.Update(null, null, false);
                _context.BlockUser.Update(blockUser);
                await SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion
        
        private async Task SaveChanges()
        {
            await _context.SaveChangesAsync(CancellationToken.None);
        }

        /// <summary>
        /// Формирование DTO для return
        /// </summary>
        private static BlockUserDtos GetBlockUserDto(BlockUserEntity e)
        {
            return new BlockUserDtos(
                e.Id,
                e.StartDate,
                e.EndDate,
                e.UserProfileId,
                e.IsActive
            );
        }

        /// <summary>
        /// Формирование DTOs для return
        /// </summary>
        private static List<BlockUserDtos> GetBlockUserDto(List<BlockUserEntity> entities)
        {
            return entities.Select(e => new BlockUserDtos(
                e.Id,
                e.StartDate,
                e.EndDate,
                e.UserProfileId,
                e.IsActive
            )).ToList();
        }
    }
}