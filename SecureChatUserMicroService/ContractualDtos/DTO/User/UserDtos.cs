using NodaTime;

namespace ContractualDtos.DTO.User
{
    public record UserDtos(
        Guid Id,
        string Email,
        Instant CreatedTime,
        Instant LastUpdateTime,
        Instant? DeleteTime);
}