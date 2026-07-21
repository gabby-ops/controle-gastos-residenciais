using ControleGastos.Api.DTOs;
using ControleGastos.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ControleGastos.Api.Controllers;

/// <summary>
/// Expõe os endpoints REST para cadastro e listagem de transações
/// financeiras (receitas e despesas) vinculadas a uma pessoa.
/// </summary>
[ApiController]
[Route("api/transacoes")]
public class TransacoesController : ControllerBase
{
    private readonly ITransacaoService _transacaoService;

    public TransacoesController(ITransacaoService transacaoService)
    {
        _transacaoService = transacaoService;
    }

    /// <summary>
    /// Lista todas as transações, incluindo o nome da pessoa vinculada.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> ObterTodas()
    {
        var transacoes = await _transacaoService.ObterTodasAsync();
        return Ok(transacoes);
    }

    /// <summary>
    /// Cadastra uma nova transação. O Service valida se a pessoa existe
    /// e se a regra de negócio do menor de idade é respeitada
    /// (menores de 18 anos só podem cadastrar despesas).
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarTransacaoDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var (sucesso, erro, transacaoDto) = await _transacaoService.CriarAsync(dto);

        if (!sucesso) return BadRequest(new { erro });

        return CreatedAtAction(nameof(ObterTodas), new { id = transacaoDto!.Id }, transacaoDto);
    }
}