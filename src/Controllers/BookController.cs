
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
                var book = await _service.AddAsync(dto);

                return CreatedAtAction(nameof(GetGetById), new { id = book.Id }, book);
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
                var book = await _service.DeleteAsync(new BookId(id));
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
                return BadRequest();
            }

            try
            {
                var book = await _service.UpdateAsync(dto);

                return Ok(book);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<BookDto>>> SearchAsync([FromQuery] BookFilterDto dto)
        {
            Console.WriteLine($"Filtros recebidos -> ISBN: {dto.Isbn}, Title: {dto.Title}, AuthorName: {dto.AuthorName}, ValueOrder: {dto.ValueOrder}");
            return await _service.SearchAsync(dto);
        }




        [HttpGet("{id}")]
        public async Task<ActionResult<BookDto>> GetGetById(string id)
        {
            var book = await _service.GetByIdAsync(new BookId(id));

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }



        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetAll()
        {
            return await _service.GetAllAsync();
        }

    }
}
