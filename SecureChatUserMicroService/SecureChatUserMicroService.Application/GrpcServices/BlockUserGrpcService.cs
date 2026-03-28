using BlockUserService.Proto;
using ContractualDtos.DTO.BlockUser;
using ContractualDtos.DTO.BlockUser.CrudDto;
using Grpc.Core;
using SecureChatUserMicroService.Application.Application.Extensions;
using SecureChatUserMicroService.Application.Application.Extensions.ProtobufMappers;
using SecureChatUserMicroService.Application.Common.Interfaces.IRepository;

namespace SecureChatUserMicroService.Application.GrpcServices;

public class BlockUserGrpcService : BlockUserService.Proto.BlockUserGrpcService.BlockUserGrpcServiceBase
{
    private readonly IBlockUserRepository _blockUserRepository;

    public BlockUserGrpcService(IBlockUserRepository blockUserRepository)
    {
        _blockUserRepository = blockUserRepository ?? throw new ArgumentNullException(nameof(blockUserRepository));
    }

    /// <summary>
    /// Вывод всех блокировок
    /// </summary>
    public override async Task<GetAllBlocksResponse> GetAllBlocks(GetAllBlocksRequest request, ServerCallContext context)
    {
        try
        {
            var blockUsers = await _blockUserRepository.GetAllAsync();
            return new GetAllBlocksResponse
            {
                Success = true,
                BlockUsers = { blockUsers.ToProtoBlockUserInfoList() }
            };
        }
        catch (Exception ex)
        {
            return new GetAllBlocksResponse
            {
                Success = false,
                ErrorMessage = ex.Message
            };
        }
    }

    /// <summary>
    /// Вывод определенной блокировки
    /// </summary>
    public override async Task<GetBlockByIdResponse> GetBlockById(GetBlockByIdRequest request, ServerCallContext context)
    {
        try
        {
            var blockUser = await _blockUserRepository.Read(request.BlockId.ToGuid());
            if (blockUser == null)
            {
                return new GetBlockByIdResponse
                {
                    Success = false,
                    ErrorMessage = "BlockUser not found"
                };
            }
            
            return new GetBlockByIdResponse
            {
                Success = true,
                BlockUser = blockUser.ToProtoBlockUserInfo()
            };
        }
        catch (Exception ex)
        {
            return new GetBlockByIdResponse
            {
                Success = false,
                ErrorMessage = ex.Message
            };
        }
    }

    public override async Task<AddNewBlockUserResponse> AddNewBlockUser(AddNewBlockUserRequest request, ServerCallContext context)
    {
        try
        {
            var newBlockUser = await _blockUserRepository.Create(new CreateBlockUserRequestDto(request.StartDate.ToInstant(),
                    request.EndDate.ToInstant(),
                    request.UserProfileId.ToGuid()));
            
            return new AddNewBlockUserResponse
            {
                Success = true,
                BlockId = newBlockUser.ToString()
            };
        }
        catch (Exception ex)
        {
            return new AddNewBlockUserResponse
            {
                Success = false,
                ErrorMessage = ex.Message
            };
        }
    }

    /// <summary>
    /// Изменение состояния блокировки
    /// </summary>
    public override async Task<UpdateBlockResponse> UpdateBlock(UpdateBlockRequest request, ServerCallContext context)
    {
        try
        {
            var updateBlockUser = await _blockUserRepository.Update(new UpdateBlockUserRequestDto(request.Id.ToGuid(),
                request.StartDate.ToInstant(), request.EndDate.ToInstant(), request.IsActive));
            if (updateBlockUser == null)
            {
                return new UpdateBlockResponse
                {
                    Success = false,
                    ErrorMessage = "Internal server error"
                };
            }
            
            return new UpdateBlockResponse
            {
                Success = true,
                UpdateBlockUser = updateBlockUser.ToProtoBlockUserInfo()
            };
        }
        catch (Exception ex)
        {
            return new UpdateBlockResponse
            {
                Success = false,
                ErrorMessage = ex.Message
            };
        }
    }

    /// <summary>
    /// Деактивация блокировки
    /// </summary>
    public override async Task<DeactivateBlockResponse> DeactivateBlock(DeactivateBlockRequest request, ServerCallContext context)
    {
        try
        {
            _ = await _blockUserRepository.DeactivateRecord(request.BlockId.ToGuid());
            return new DeactivateBlockResponse
            {
                Success = true
            };
        }
        catch (Exception ex)
        {
            return new DeactivateBlockResponse
            {
                Success = false,
                ErrorMessage = ex.Message
            };
        }
    }
}