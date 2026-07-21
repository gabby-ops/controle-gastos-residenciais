using ControleGastos.Api.DTOs;
using ControleGastos.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ControleGastos.Api.Controllers;

/// <summary>
/// Expõe os endpoints REST para gerenciamento de pessoas (moradores).
/// </summary>
[ApiController]
[Route("api/pessoas")]
public class PessoasController : ControllerBase
{
    private readonly IPessoaService _pessoaService;

    public PessoasController(IPessoaService pessoaService)
    {
        _pessoaService = pessoaService;
    }

    /// <summary>
    /// Lista todas as pessoas cadastradas.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> ObterTodos()
    {
        var pessoas = await _pessoaService.ObterTodosAsync();
        return Ok(pessoas);
    }

    /// <summary>
    /// Cadastra uma nova pessoa. Validações de nome/idade são feitas
    /// via DataAnnotations no CriarPessoaDto; a validação de nome
    /// duplicado é feita no Service.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarPessoaDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var (sucesso, erro, pessoaDto) = await _pessoaService.CriarAsync(dto);

        if (!sucesso) return BadRequest(new { erro });

        return CreatedAtAction(nameof(ObterTodos), new { id = pessoaDto!.Id }, pessoaDto);
    }

    /// <summary>
    /// Exclui uma pessoa pelo Id. O banco de dados aplica Cascade Delete
    /// automaticamente, removendo também todas as transações vinculadas.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Excluir(int id)
    {
        var sucesso = await _pessoaService.ExcluirAsync(id);
        if (!sucesso) return NotFound(new { erro = "Pessoa não encontrada." });

        return NoContent();
    }
}