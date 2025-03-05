using DDDNetCore.Domain.Authors;
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
    }
}
