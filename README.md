# Controle de Gastos Residenciais

Sistema completo para controle de receitas e despesas de pessoas de uma residência,
com regra especial para menores de idade.

## Tecnologias

**Backend:** .NET 8, ASP.NET Core Web API, Entity Framework Core, SQLite
**Frontend:** React, TypeScript, Vite, React Router, Axios

## Estrutura de Pastas

## Como instalar e executar

### Pré-requisitos
- .NET 8 SDK
- Node.js 18+

### Backend

```bash
cd backend/ControleGastos.Api
dotnet restore
dotnet tool install --global dotnet-ef   # se ainda não tiver
dotnet ef migrations add InitialCreate
dotnet ef database update
dotnet run
```

A API sobe por padrão em algo como `http://localhost:5231` (confira a porta exibida no
terminal e ajuste `baseURL` em `frontend/src/api/api.ts` se for diferente). O Swagger
fica disponível em `/swagger`.

O banco SQLite (`controlegastos.db`) é criado automaticamente na primeira execução, e as
migrations também rodam automaticamente ao iniciar a aplicação (`db.Database.Migrate()`
no `Program.cs`).

### Frontend

```bash
cd frontend/controle-gastos-web
npm install
npm run dev
```

Acesse `http://localhost:5173`.

## Regra de negócio: menor de idade

Pessoas com idade menor que 18 anos só podem cadastrar transações do tipo **Despesa**.
Qualquer tentativa de cadastrar uma **Receita** para um menor de idade é bloqueada pelo
backend (`TransacaoValidator.ValidarRegraMenorIdade`), retornando erro 400 com mensagem
explicativa, exibida no frontend via toast.

## Exclusão em cascata (Cascade Delete)

O relacionamento entre `Pessoa` e `Transacao` é configurado no `AppDbContext` com
`OnDelete(DeleteBehavior.Cascade)`. Isso significa que, ao excluir uma pessoa, o próprio
banco de dados remove automaticamente todas as transações vinculadas a ela — não é
necessário nenhum código adicional no backend para isso.

## Endpoints da API

| Método | Rota              | Descrição                          |
|--------|-------------------|-------------------------------------|
| GET    | /api/pessoas      | Lista todas as pessoas              |
| POST   | /api/pessoas      | Cadastra uma pessoa                 |
| DELETE | /api/pessoas/{id} | Exclui uma pessoa (e suas transações)|
| GET    | /api/transacoes   | Lista todas as transações           |
| POST   | /api/transacoes   | Cadastra uma transação              |
| GET    | /api/totais       | Retorna totais por pessoa e geral   |