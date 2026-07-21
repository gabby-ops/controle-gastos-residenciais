using ControleGastos.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ControleGastos.Api.Controllers;

[ApiController]
[Route("api/totais")]
public class TotaisController : ControllerBase
{
    private readonly ITotaisService _totaisService;

    public TotaisController(ITotaisService totaisService)
    {
        _totaisService = totaisService;
    }

    [HttpGet]
    public async Task<IActionResult> ObterTotais()
    {
        var totais = await _totaisService.ObterTotaisAsync();
        return Ok(totais);
    }
}