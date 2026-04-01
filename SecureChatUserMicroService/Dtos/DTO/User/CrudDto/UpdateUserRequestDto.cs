namespace ContractualDtos.DTO.User.CrudDto
{
    public record UpdateUserRequestDto(
        Guid Id,
        string? Email,
        string? Name,
        string? Nickname,
        string? AvatarUrl,
        string? StatusQuote,
        bool? IsBlocked,
        bool? IsDeleted);
}