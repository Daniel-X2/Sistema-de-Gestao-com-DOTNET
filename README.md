<div align="center">

# Sistema de Gestão com .NET

**API REST para gestão de clientes, funcionários, estoque e dashboard.**

[![Deploy](https://img.shields.io/github/actions/workflow/status/Daniel-X2/Sistema-de-Gestao-com-DOTNET/deploy.yaml?branch=main&style=flat-square&label=Deploy)](https://github.com/Daniel-X2/Sistema-de-Gestao-com-DOTNET/actions/workflows/deploy.yaml)
[![Docker](https://img.shields.io/github/actions/workflow/status/Daniel-X2/Sistema-de-Gestao-com-DOTNET/docker.yaml?branch=main&style=flat-square&label=Docker)](https://github.com/Daniel-X2/Sistema-de-Gestao-com-DOTNET/actions/workflows/docker.yaml)
[![Testes](https://img.shields.io/github/actions/workflow/status/Daniel-X2/Sistema-de-Gestao-com-DOTNET/tests.yaml?branch=main&style=flat-square&label=Testes)](https://github.com/Daniel-X2/Sistema-de-Gestao-com-DOTNET/actions/workflows/tests.yaml)

[Deploy](https://api-gestao-dotnet-ef.onrender.com/) · [LinkedIn](https://www.linkedin.com/in/daniel-silva-dev1/) · [GitHub](https://github.com/Daniel-X2)

</div>

> [!NOTE]
> Duas implementações: `ADO` (ADO.NET/Npgsql) e `ef` (Entity Framework Core). A **`main` agora é a `ef`**. Ambas compartilham o mesmo banco PostgreSQL.

## Backend

- Minimal APIs em .NET 10, em camadas: rotas → serviços → repositórios → `DbContext`.
- EF Core 10 + Npgsql com consultas assíncronas, paginação no banco, operações em lote e projeções que não expõem hashes de senha.
- Autenticação JWT Bearer, senhas com BCrypt e policy `Admin` na listagem de funcionários.
- Validações de domínio (CPF, código, lote, quantidade) e atualização parcial de registros.
- Middleware global de exceções com respostas padronizadas e logs estruturados com Serilog.
- Testes unitários da camada de serviço com mocks de repositório (xUnit, Moq e Bogus).

## Endpoints

Todas as rotas exigem `Authorization: Bearer <token>`, exceto o login. `GET /funcionario/get` exige a role `Admin`.

| Método | Rota | Descrição |
| --- | --- | --- |
| POST | `/funcionario/login` | Login por CPF e senha; retorna o JWT |
| GET | `/client/get/` | Lista clientes (paginado) |
| GET | `/client/quantidade` | Total de clientes |
| POST | `/client/add/` | Cadastra cliente |
| PUT | `/client/update/{id}/` | Atualiza cliente |
| DELETE | `/client/delete/{id}` | Remove cliente |
| GET | `/funcionario/get` | Lista funcionários (Admin, paginado) |
| GET | `/funcionario/get/{id}` | Busca funcionário |
| POST | `/funcionario/add/` | Cadastra funcionário |
| PUT | `/funcionario/update/{id}/` | Atualiza funcionário |
| DELETE | `/funcionario/delete/{id}` | Remove funcionário |
| GET | `/product/get` | Lista produtos (paginado) |
| GET | `/product/get/{id}` | Busca produto |
| POST | `/product/add` | Cadastra produto |
| PUT | `/product/update/{id}/` | Atualiza produto |
| DELETE | `/product/delete/{id}` | Remove produto |
| GET | `/estoque/get` | Nome e quantidade em estoque (paginado) |
| GET | `/estoque/valorBruto` | Valor bruto por produto |
| GET | `/dashboard/stats` | Estatísticas para dashboard |

Listagens aceitam `page` (padrão `1`) e `limit` (padrão `100`).

> [!TIP]
> Credencial de demonstração: `POST /funcionario/login` com `{"cpf":"78069320036","senha":"123"}`. Copie o token e use **Authorize** no Swagger.

## Executar

```bash
cd Compose
docker compose up -d
```

Sobe a API e o PostgreSQL em containers, com a imagem `danizura/api-gestao-dotnet:EF`. Configure `DB_CONNECTION` e as variáveis JWT no `.env` do Compose.

## Estrutura

```text
Api/        rotas, autenticação, middleware e Swagger
Api.Core/   DTOs, serviços, repositórios, DbContext e validações
Api.Test/   testes unitários
Compose/    API + PostgreSQL
```

## CI/CD

GitHub Actions publica a imagem EF e dispara o deploy automaticamente por webhook.
