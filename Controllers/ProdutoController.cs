using ApiFuncional.Data;
using ApiFuncional.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiFuncional.Controllers;

[Authorize]
[ApiController]
[Route("api/produtos")]
public class ProdutoController : ControllerBase
{
    private readonly ApiDbContext _context;

    public ProdutoController(ApiDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<IEnumerable<Produto>>> GetProdutos()
    {
        if (_context.Produtos == null) return NotFound();

        return await _context.Produtos.ToListAsync();
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<Produto>> GetProduto(int id)
    {
        if (_context.Produtos == null) return NotFound();

        var produto = await _context.Produtos.FindAsync(id);

        if (produto == null) return NotFound();

        return produto;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<Produto>> PostProduto(Produto produto)
    {
        if (_context.Produtos == null) return Problem("Error ao criar um produto!");

        if (!ModelState.IsValid)
        {
            //return BadRequest(ModelState);
            // return ValidationProblem(ModelState);
            return ValidationProblem(new ValidationProblemDetails(ModelState)
            {
                Title = "Um ou mais erros de validação ocorreram!"
            });
        }

        _context.Produtos.Add(produto);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(PostProduto), new { id = produto.Id }, produto);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<Produto>> PutProduto(int id, Produto produto)
    {
        if (id != produto.Id) return BadRequest();
       
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        _context.Entry(produto).State = EntityState.Modified;
        
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ProdutoExists(id))
            {
                return NotFound();
            }
            else
                throw;
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<Produto>> DeleteProduto(int id)
    {
        if (_context.Produtos == null) return NotFound();
        
        var produto = await _context.Produtos.FindAsync(id);

        if (produto == null)
        {
            return NotFound();
        }

        _context.Produtos.Remove(produto);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ProdutoExists(int id)
    {
        return (_context.Produtos?.Any(produto => produto.Id == id)).GetValueOrDefault();
    }
}