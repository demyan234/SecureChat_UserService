using NodaTime;

namespace ContractualDtos.DTO.BlockUser.CrudDto
{
    public record UpdateBlockUserRequestDto(
        Guid Id,
        Instant StartDate,
        Instant EndDate,
        bool IsActive);
}