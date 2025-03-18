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
            return await _service.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorDto>> GetGetById(string id)
        {
            var author = await _service.GetByIdAsync(new AuthorId(id));

            if (author == null)
            {
                return NotFound();
            }

            return author;
        }
    }
}
