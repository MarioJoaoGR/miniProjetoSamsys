
using DDDNetCore.Domain.Books;
using Microsoft.AspNetCore.Mvc;

namespace DDDNetCore.Controllers
{

    [Route("api/[controller]")]
    [ApiController]

    public class BookController : ControllerBase
    {

        private readonly IBookService _service;

        public BookController(IBookService service)
        {
            _service = service;
        }
    }
}
