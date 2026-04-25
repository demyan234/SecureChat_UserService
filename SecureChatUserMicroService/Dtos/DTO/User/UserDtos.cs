namespace ContractualDtos.DTO.User
{
    public record UserDtos(
        Guid UserId,
        string UserName,
        string UserEmail,
        string UserNickname,
        string UserAvatarUrl,
        bool IsActive
    );
}