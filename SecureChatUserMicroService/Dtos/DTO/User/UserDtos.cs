using NodaTime;

namespace ContractualDtos.DTO.User
{
    public record UserDtos(
        Guid Id,
        string Email,
        DateTime CreatedTime,
        DateTime LastUpdateTime,
        DateTime? DeleteTime);
}