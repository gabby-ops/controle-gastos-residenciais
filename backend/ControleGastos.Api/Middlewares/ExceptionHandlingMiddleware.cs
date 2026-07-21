using System.Net;
using System.Text.Json;

namespace ControleGastos.Api.Middlewares;

/// <summary>
/// Captura qualquer exceção não tratada nos controllers e devolve
/// uma resposta JSON padronizada, em vez do erro cru do ASP.NET Core.
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro não tratado ao processar a requisição {Path}", context.Request.Path);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var resposta = new
            {
                erro = "Ocorreu um erro inesperado no servidor. Tente novamente mais tarde."
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(resposta));
        }
    }
}