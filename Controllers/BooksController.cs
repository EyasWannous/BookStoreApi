using BookStoreApi.Models;
using BookStoreApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly BooksServices _booksServices;

    public BooksController(BooksServices booksServices) =>
      _booksServices = booksServices;

    [HttpGet]
    public async Task<List<Book>> Get() =>
        await _booksServices.GetAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Book>> Get([FromRoute] string id)
    {
        var book = await _booksServices.GetAsync(id);

        if (book is null)
            return NotFound();

        return book;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Book newBook)
    {
        await _booksServices.CreateAsync(newBook);

        return CreatedAtAction(nameof(Get), new { id = newBook.Id }, newBook);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update([FromRoute] string id, [FromBody] Book updatedBook)
    {
        var book = await _booksServices.GetAsync(id);

        if (book is null)
            return NotFound();

        updatedBook.Id = book.Id;
        await _booksServices.UpdateAsync(id, updatedBook);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        var book = await _booksServices.GetAsync(id);

        if (book is null)
            return NotFound();

        await _booksServices.RemoveAsync(id);

        return NoContent();
    }

    [HttpGet("count")]
    public async Task<IActionResult> Count()
    {
        var count = await _booksServices.CountAsync(_ => true);
        
        return Ok(count);
    }

    [HttpPost("DeleteMany")]
    public async Task<IActionResult> DeleteManyAsync([FromBody] List<string> ids) 
    {
        var deleteResult = await _booksServices.DeleteManyAsync(x => ids.Contains(x.Id!));

        return Ok(deleteResult); 
    }
    //TODO
    //public async Task<IActionResult> FindAsync() { return Ok(); }
    //public async Task<IActionResult> InsertManyAsync() { return Ok(); }
    //public async Task<IActionResult> UpdateManyAsync() { return Ok(); }
}