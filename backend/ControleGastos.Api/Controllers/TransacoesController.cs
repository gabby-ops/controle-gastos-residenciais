using ControleGastos.Api.DTOs;
using ControleGastos.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ControleGastos.Api.Controllers;

[ApiController]
[Route("api/transacoes")]
public class TransacoesController : ControllerBase
{
    private readonly ITransacaoService _transacaoService;

    public TransacoesController(ITransacaoService transacaoService)
    {
        _transacaoService = transacaoService;
    }

    [HttpGet]
    public async Task<IActionResult> ObterTodas()
    {
        var transacoes = await _transacaoService.ObterTodasAsync();
        return Ok(transacoes);
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarTransacaoDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var (sucesso, erro, transacaoDto) = await _transacaoService.CriarAsync(dto);

        if (!sucesso) return BadRequest(new { erro });

        return CreatedAtAction(nameof(ObterTodas), new { id = transacaoDto!.Id }, transacaoDto);
    }
}