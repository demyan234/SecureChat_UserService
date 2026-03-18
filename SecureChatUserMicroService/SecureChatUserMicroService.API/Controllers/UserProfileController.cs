using ContractualDtos.DTO.UserProfile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecureChatUserMicroService.Application.Common.Interfaces.IRepository;

namespace SecureChatUserMicroService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileRepository _userProfile;

        public UserProfileController(IUserProfileRepository userProfile)
        {
            _userProfile = userProfile;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<UserProfileDtos>>> GetAllUserProfiles(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] string? sortDirection = "Ascending")
        {
            try
            {
                var userProfiles = await _userProfile.GetAllAsync(page, pageSize, search, sortBy, sortDirection);
                return Ok(userProfiles);
            }
            catch
            {
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserProfileDtos>> GetUserProfileById(Guid id)
        {
            try
            {
                var userProfile = await _userProfile.Read(id);
                return Ok(userProfile);
            }
            catch
            {
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }
    }
}