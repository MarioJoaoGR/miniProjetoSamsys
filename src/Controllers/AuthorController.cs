using System.Collections.Generic;
using System.Threading.Tasks;
using DDDNetCore.Domain.Authors;
using DDDNetCore.Domain.Books;
using Microsoft.AspNetCore.Mvc;

namespace DDDNetCore.Controllers
{
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
    }
}
