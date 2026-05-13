# Sistema de Gestão com .NET
![CI](https://github.com/Daniel-X2/Sistema-de-Gestao-com-DOTNET/actions/workflows/desenvolvimento.yaml/badge.svg)

API REST assíncrona desenvolvida em ASP.NET Core para gestão de clientes, funcionários e produtos, utilizando PostgreSQL como banco de dados. O projeto segue uma arquitetura modular com separação de responsabilidades entre as camadas de roteamento, serviços e repositórios.

A documentação interativa (Swagger) está acessível na raiz da URL. Use o login padrão para gerar o token e testar os endpoints diretamente.
## 🎥 Demonstração

Vídeo mostrando autenticação JWT e uso de endpoints protegidos:

https://youtu.be/g3xyDaQyCmI
## Tecnologias

- **Plataforma**: .NET / ASP.NET Core (Minimal APIs)
- **Banco de Dados**: PostgreSQL
- **Acesso a Dados**: ADO.NET com Npgsql
- **Autenticação**: JWT Bearer + RBAC (roles Admin/User)
- **Segurança**: BCrypt para hash de senhas, Rate Limiting
- **Logs**: Serilog (console + arquivo rotativo diário)
- **Configuração**: DotNetEnv (variáveis de ambiente via `.env`)
- **Testes**: xUnit, Moq, Bogus
- **Testes de Carga**: Locust
- **Padrões**: Injeção de Dependência, Repository Pattern, Service Layer, Middleware de Exceções

## Estrutura do Projeto
```
├── Api/                        # Ponto de entrada, rotas, middleware
│   ├── Application/
│   │   ├── Routers/            # Definição dos endpoints (Minimal APIs)
│   │   └── middleware/         # Tratamento centralizado de exceções
│   ├── Auth/                   # JWT TokenService e modelo de usuário
│   ├── Crypt/                  # BCrypt (hash e verificação de senha)
│   └── Program.cs
│
├── Api.Core/                   # Lógica de negócio e acesso a dados
│   └── Application/
│       ├── dto/                # DTOs (ClientDto, FuncionarioDto, ProdutoDto)
│       ├── repository/         # Repositórios com queries SQL
│       ├── service/            # Regras de negócio e validações
│       ├── utils/              # Conexão, DI (AddScope), Validation
│       └── CustomException/    # Exceções personalizadas
│
├── Api.Test/                   # Testes unitários
│   ├── dados.cs                # Geração de dados com Bogus
│   ├── TestServiceClient.cs
│   ├── TestServiceFuncionario.cs
│   ├── TestServiceProduct.cs
│   └── TestUtils.cs
│
│
├── Docker/
│   ├── docker-compose.yml
│   └── sql.sql                 # Script de inicialização do banco
└── Dockerfile                  # Multi-stage build com usuário não-root
```

## Funcionalidades

### Autenticação
- Login via CPF + senha com geração de token JWT
- Roles: `Admin` (acesso total) e `User` (acesso restrito)
- Senhas armazenadas com BCrypt
- Rate limiting: máximo 5 requisições por minuto por IP

### Clientes (`/client/`)
- Cadastro com validação de CPF (algoritmo próprio), conta e nome
- Atualização parcial — campos inválidos são mantidos com o valor anterior e retornados na resposta
- Listagem paginada e busca por ID
- Remoção por ID
- Suporte a marcação de cliente VIP

### Funcionários (`/funcionario/`)
- Cadastro com validação de CPF, nome e ano de nascimento (18–85 anos)
- Controle de atestados e privilégios administrativos
- Listagem paginada e CRUD completo

### Produtos (`/product/`)
- Cadastro com validação de código, lote, quantidade e valor de revenda
- Consulta de estoque e valor bruto total
- Atualização parcial — campos inválidos mantêm o valor anterior
- Listagem paginada e CRUD completo

## Arquitetura de Tratamento de Erros

Middleware centralizado captura todas as exceções, registra via Serilog e retorna respostas padronizadas em JSON:

| Exceção | Status HTTP |
|---|---|
| `InvalidCpfException` | 422 Unprocessable Entity |
| `InvalidNameException` | 422 Unprocessable Entity |
| `InvalidAccountException` | 422 Unprocessable Entity |
| `InvalidCodeException` | 422 Unprocessable Entity |
| `NegativeNumericException` | 422 Unprocessable Entity |
| `ReturnDataIsEmpty` | 400 Bad Request |
| `InvalidIdException` | 400 Bad Request |
| `InvalidNascimentoException` | 400 Bad Request |
| `InvalidLoteException` | 400 Bad Request |
| `ErroAddToDatabaseException` | 400 Bad Request |
| `ErroUpdateToDatabaseException` | 400 Bad Request |
| `InvalidPassword` | 401 Unauthorized |
| `InvalidConnection` | 500 Internal Server Error |

## Logs

Gerados automaticamente via Serilog em dois destinos:
- **Console** — visível em tempo real
- **Arquivo** — `./Logs/logs.txt` com rotação diária

Cada requisição é registrada automaticamente com método HTTP, rota, status code e tempo de resposta.

## Testes de Carga

Realizados com Locust simulando usuários simultâneos com operações mistas de leitura e escrita (GET, POST, PUT, DELETE).

| Usuários | Requisições | Req/s | Tempo médio | Mediana |
|---|---|---|---|---|
| 10 | 1.572 | 4.9 | 9.8ms | 4ms |
| 50 | 8.346 | 23.2 | 7.4ms | 3ms |

**Endpoints de leitura** mantiveram mediana de **3ms** em ambos os cenários com zero falhas.


## Testes Unitários

Os testes cobrem a camada de serviço com mocks dos repositórios, sem necessidade de banco de dados real.

- **Moq** — mock das interfaces de repositório
- **Bogus** — geração de dados falsos para os testes
- **xUnit** — execução dos testes
```bash
dotnet test
```

## Configuração

### 1. Variáveis de Ambiente

Crie um arquivo `.env` com as seguintes variáveis:
```env
DB_CONNECTION=Host=postgres_db;Database=postgres;Username=postgres;Password=senha
POSTGRES_PASSWORD=senha
JWT_KEY=sua_chave_secreta
JWT_ISSUER=MinhaApi
JWT_AUDIENCE=MeuClient
```

### 2. Execução com Docker (recomendado)

Crie o `.env` dentro da pasta `Docker/` e execute:
```bash
cd Docker
docker-compose up -d
```

A API sobe na porta `5153`. O banco é inicializado automaticamente com as tabelas e um funcionário admin padrão.

### 3. Execução manual

Crie o `.env` dentro da pasta `Api/` e execute:
```bash
dotnet run --project Api
```

### 4. Login padrão

Um funcionário admin é inserido automaticamente ao inicializar o banco:
```json
{
  "cpf": "78069320036",
  "senha": "123"
}
```

## Paginação

Todos os endpoints de listagem suportam paginação via query params:
```
GET /client/get?page=1&limit=10
GET /funcionario/get?page=1&limit=10
GET /product/get?page=1&limit=10
GET /estoque/get?page=1&limit=10
```

## Endpoints

### Autenticação

| Método | Rota | Descrição |
|---|---|---|
| POST | `/login/` | Gera token JWT |

### Clientes

| Método | Rota | Descrição |
|---|---|---|
| GET | `/client/get/` | Lista clientes (paginado) |
| POST | `/client/add/` | Adiciona um cliente |
| PUT | `/client/update/{id}/` | Atualiza um cliente |
| DELETE | `/client/delete/{id}` | Remove um cliente |

### Funcionários

| Método | Rota | Descrição |
|---|---|---|
| GET | `/funcionario/get` | Lista funcionários (paginado) |
| GET | `/funcionario/get/{id}` | Busca funcionário por ID |
| POST | `/funcionario/add/` | Adiciona um funcionário |
| PUT | `/funcionario/update/{id}/` | Atualiza um funcionário |
| DELETE | `/funcionario/delete/{id}` | Remove um funcionário |

### Produtos e Estoque

| Método | Rota | Descrição |
|---|---|---|
| GET | `/product/get` | Lista produtos (paginado) |
| GET | `/product/get/{id}` | Busca produto por ID |
| GET | `/estoque/get` | Consulta estoque (paginado) |
| GET | `/estoque/valorBruto` | Lista valores de revenda |
| POST | `/product/add` | Adiciona um produto |
| PUT | `/product/update/{id}/` | Atualiza um produto |
| DELETE | `/product/delete/{id}` | Remove um produto |
