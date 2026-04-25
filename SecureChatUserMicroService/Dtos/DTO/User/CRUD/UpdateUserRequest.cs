namespace ContractualDtos.DTO.User.CRUD
{
    public record UpdateUserRequest(
        Guid UserId,
        string? Email,
        string? Nickname,
        string? AvatarUrl,
        string? UserName
    );
}