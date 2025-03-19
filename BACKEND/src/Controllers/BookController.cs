using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDDNetCore.Domain.Books;
using Microsoft.AspNetCore.Authorization;
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

        [HttpPost]
        public async Task<ActionResult<BookDto>> Create(CreatingBookDto dto)
        {
            try
            {
                var result = await _service.AddAsync(dto);

                if (!result.Success)
                {
                    return BadRequest(new { Message = result.Message, ErrorType = result.ErrorType?.Name });
                }

                return CreatedAtAction(nameof(GetGetById), new { id = result.Obj.Id }, result.Obj);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                var result = await _service.DeleteAsync(new BookId(id));

                if (!result.Success)
                {
                    return BadRequest(new { Message = result.Message, ErrorType = result.ErrorType?.Name });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BookDto>> Update(EditingBookDto dto, Guid id)
        {
            if (id != dto.Id)
            {
                return BadRequest(new { Message = "IDs não coincidem." });
            }

            try
            {
                var result = await _service.UpdateAsync(dto);

                if (!result.Success)
                {
                    return BadRequest(new { Message = result.Message, ErrorType = result.ErrorType?.Name });
                }

                return Ok(result.Obj);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<BookDto>>> SearchAsync([FromQuery] BookFilterDto dto)
        {
            var result = await _service.SearchAsync(dto);

            if (!result.Success)
            {
                return BadRequest(new { Message = result.Message, ErrorType = result.ErrorType?.Name });
            }

            return Ok(result.Obj);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookDto>> GetGetById(string id)
        {
            var result = await _service.GetByIdAsync(new BookId(id));

            if (!result.Success)
            {
                return NotFound(new { Message = result.Message, ErrorType = result.ErrorType?.Name });
            }

            return Ok(result.Obj);
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetAll()
        {
            var result = await _service.GetAllAsync();

            if (!result.Success)
            {
                return BadRequest(new { Message = result.Message, ErrorType = result.ErrorType?.Name });
            }

            return Ok(result.Obj);
        }

        [HttpGet("GetAllActive")]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetAllActive()
        {
            var result = await _service.GetAllActiveAsync();

            if (!result.Success)
            {
                return BadRequest(new { Message = result.Message, ErrorType = result.ErrorType?.Name });
            }

            return Ok(result.Obj);
        }

        [HttpGet("GetAllInactive")]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetAllInactive()
        {
            var result = await _service.GetAllInactiveAsync();

            if (!result.Success)
            {
                return BadRequest(new { Message = result.Message, ErrorType = result.ErrorType?.Name });
            }

            return Ok(result.Obj);
        }

        [HttpGet("GetBooksByAuthor")]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetBooksByAuthor([FromQuery] string authorId)
        {
            try
            {
                var result = await _service.GetBooksByAuthorAsync(authorId);

                if (!result.Success)
                {
                    return BadRequest(new { Message = result.Message, ErrorType = result.ErrorType?.Name });
                }

                return Ok(result.Obj);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }


    }
}
