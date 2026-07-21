using System.Text.Json.Serialization;
using ControleGastos.Api.Data;
using ControleGastos.Api.Middlewares;
using ControleGastos.Api.Repositories;
using ControleGastos.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Banco de dados: SQLite, arquivo local persistente.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Injeção de dependência: Repositories e Services.
builder.Services.AddScoped<IPessoaRepository, PessoaRepository>();
builder.Services.AddScoped<ITransacaoRepository, TransacaoRepository>();
builder.Services.AddScoped<IPessoaService, PessoaService>();
builder.Services.AddScoped<ITransacaoService, TransacaoService>();
builder.Services.AddScoped<ITotaisService, TotaisService>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Permite que o enum TipoTransacao seja enviado/recebido como
        // texto ("Receita"/"Despesa") em vez de número (0/1).
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS liberado para o frontend em desenvolvimento (Vite roda na porta 5173).
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Aplica migrations automaticamente ao iniciar (facilita rodar o projeto).
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("FrontendPolicy");

// Middleware de tratamento global de exceções — captura qualquer erro
// inesperado e devolve uma resposta JSON amigável em vez do erro cru.
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthorization();
app.MapControllers();

app.Run();