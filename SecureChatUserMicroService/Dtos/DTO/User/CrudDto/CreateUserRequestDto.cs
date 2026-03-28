namespace ContractualDtos.DTO.User.CrudDto
{
    public record CreateUserRequestDto(string Email, string Name, string Nickname,
        string AvatarUrl, string? StatusQuote, Guid Status);
}