using NodaTime;

namespace ContractualDtos.DTO.BlockUser
{
    public record BlockUserDtos(
        Guid Id,
        Instant StartDate,
        Instant EndDate,
        Guid UserProfileId,
        bool IsActive);
}