namespace ContractualDtos.DTO.User.CRUD
{
    public record CreateUserRequest(
        Guid UserId,
        string UserName,
        string UserEmail,
        string UserNickname
    );
}