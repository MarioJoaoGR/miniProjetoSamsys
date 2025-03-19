using System.Collections.Generic;
using System.Threading.Tasks;
using DDDNetCore.Domain.Authors;
using DDDNetCore.Domain.Books;
using Microsoft.AspNetCore.Mvc;

namespace DDDNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _service;

        public AuthorController(IAuthorService service)
        {
            _service = service;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAll()
        {
            var result = await _service.GetAllAsync();

            if (!result.Success)
            {
                return BadRequest(new { message = result.Message, errorType = result.ErrorType?.Name });
            }

            return Ok(result.Obj);  // Return the list of authors if success
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorDto>> GetById(string id)
        {
            var result = await _service.GetByIdAsync(new AuthorId(id));

            if (!result.Success)
            {
                return NotFound(new { message = result.Message, errorType = result.ErrorType?.Name });
            }

            return Ok(result.Obj);  // Return the author if success
        }
    }
}
