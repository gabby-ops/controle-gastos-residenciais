using ControleGastos.Api.DTOs;
using ControleGastos.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ControleGastos.Api.Controllers;

[ApiController]
[Route("api/pessoas")]
public class PessoasController : ControllerBase
{
    private readonly IPessoaService _pessoaService;

    public PessoasController(IPessoaService pessoaService)
    {
        _pessoaService = pessoaService;
    }

    [HttpGet]
    public async Task<IActionResult> ObterTodos()
    {
        var pessoas = await _pessoaService.ObterTodosAsync();
        return Ok(pessoas);
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarPessoaDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var pessoa = await _pessoaService.CriarAsync(dto);
        return CreatedAtAction(nameof(ObterTodos), new { id = pessoa.Id }, pessoa);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Excluir(int id)
    {
        var sucesso = await _pessoaService.ExcluirAsync(id);
        if (!sucesso) return NotFound(new { erro = "Pessoa não encontrada." });

        return NoContent();
    }
}