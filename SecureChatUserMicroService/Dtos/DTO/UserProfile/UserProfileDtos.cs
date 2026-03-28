namespace ContractualDtos.DTO.UserProfile
{
    public record UserProfileDtos(
        Guid Id,
        string Name,
        string Nickname,
        string AvatarUrl,
        string? StatusQuote,
        bool IsBlocked,
        bool IsDeleted,
        Guid Status,
        Guid UserId
    );
}