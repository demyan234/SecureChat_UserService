using NodaTime;

namespace ContractualDtos.DTO.BlockUser.CrudDto
{
    public record CreateBlockUserRequestDto(
        Instant StartDate,
        Instant EndDate,
        Guid UserProfileId);
}