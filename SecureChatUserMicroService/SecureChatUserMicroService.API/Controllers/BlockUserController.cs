using ContractualDtos.DTO.BlockUser;
using ContractualDtos.DTO.BlockUser.CrudDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecureChatUserMicroService.Application.Common.Interfaces.IRepository;

namespace SecureChatUserMicroService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BlockUserController : ControllerBase
    {
        private readonly IBlockUserRepository _blockUserRepository;

        public BlockUserController(IBlockUserRepository userRepository)
        {
            _blockUserRepository = userRepository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<BlockUserDtos>>> GetAllBlocks()
        {
            try
            {
                var users = await _blockUserRepository.GetAllAsync();
                return Ok(users);
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
        public async Task<ActionResult<BlockUserDtos>> GetBlockById(Guid id)
        {
            try
            {
                var user = await _blockUserRepository.Read(id);
                return Ok(user);
            }
            catch
            {
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<BlockUserDtos>> AddNewBlockUser([FromBody] CreateBlockUserRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var createdBlock = await _blockUserRepository.Create(dto);

                return CreatedAtAction(
                    nameof(GetBlockById),
                    new { id = createdBlock },
                    createdBlock);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BlockUserDtos>> UpdateBlock([FromBody] UpdateBlockUserRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                /*TODO: через ApiGateway сделать получение текущего ID пользователя*/
                var updatedBlockUser = await _blockUserRepository.Update(dto);

                if (updatedBlockUser == null)
                    return NotFound($"Блокировка пользователя с ID - {dto.Id} не найден");

                return Ok(updatedBlockUser);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        [HttpDelete("{id:guid}")]
        /*[Authorize(Roles = "Admin")]*/
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeactivateBlock(Guid id)
        {
            try
            {
                var result = await _blockUserRepository.DeactivateRecord(id);

                if (!result)
                    return NotFound($"Блокировка пользователя с ID {id} не найден");

                return NoContent();
            }
            catch
            {
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }
    }
}