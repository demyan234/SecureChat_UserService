using BlockUserService.Proto;
using ContractualDtos.DTO.BlockUser;
using ContractualDtos.DTO.Pagination;
using ContractualDtos.DTO.User;
using ContractualDtos.DTO.UserProfile;
using Google.Protobuf.WellKnownTypes;
using UserProfileService.Proto;
using UserService.Proto;

namespace SecureChatUserMicroService.Application.Application.Extensions.ProtobufMappers;

public static class ProtobufMapperExtensions
{
    #region User

    /// <summary>
    /// Конвертация UserDtos в Proto UserInfo
    /// </summary>
    public static UserInfo ToProtoUserInfo(this UserDtos dto)
    {
        return new UserInfo
        {
            Id = dto.Id.ToString(),
            Email = dto.Email,
            CreatedTime = Timestamp.FromDateTime(dto.CreatedTime.ToDateTimeUtc()),
            LastUpdateTime = Timestamp.FromDateTime(dto.LastUpdateTime.ToDateTimeUtc()),
            DeleteTime = dto.DeleteTime?.ToDateTimeUtc().ToTimestamp()
        };
    }
    
    public static List<UserInfo> ToProtoUserInfoList(
        this PaginationDtoResponse<UserDtos> pagination)
    {
        return pagination.Items.Select(ToProtoUserInfo).ToList();
    }

    #endregion
    
    #region UserProfile
    
    public static UserProfileInfo ToProtoUserProfileInfo(this UserProfileDtos dto)
    {
        return new UserProfileInfo
        {
            Id = dto.Id.ToString(),
            Name = dto.Name,
            Nickname = dto.Nickname,
            AvatarUrl = dto.AvatarUrl,
            StatusQuote = dto.StatusQuote ?? string.Empty,
            IsBlocked = dto.IsBlocked,
            IsDeleted = dto.IsDeleted,
            Status = dto.Status.ToString(),
            UserId = dto.UserId.ToString()
        };
    }

    public static List<UserProfileInfo> ToProtoUserProfileInfoList(
        this PaginationDtoResponse<UserProfileDtos> pagination)
    {
        return pagination.Items.Select(ToProtoUserProfileInfo).ToList();
    }
    
    #endregion

    #region BlockUser

    /// <summary>
    /// Конвертация BlockUserDtos в Proto BlockUserInfo
    /// </summary>
    public static BlockUserInfo ToProtoBlockUserInfo(this BlockUserDtos dto)
    {
        return new BlockUserInfo
        {
            Id = dto.Id.ToString(),
            StartDate = Timestamp.FromDateTime(dto.StartDate.ToDateTimeUtc()),
            EndDate = Timestamp.FromDateTime(dto.EndDate.ToDateTimeUtc()),
            IsActive = dto.IsActive,
            UserProfileId = dto.UserProfileId.ToString()
        };
    }
    
    public static List<BlockUserInfo> ToProtoBlockUserInfoList(this List<BlockUserDtos> dtos)
    {
        return dtos.Select(ToProtoBlockUserInfo).ToList();
    }

    #endregion
}