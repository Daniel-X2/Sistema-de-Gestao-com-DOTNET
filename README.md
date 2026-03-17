# API de Gestão Assíncrona

Este projeto consiste em uma API robusta desenvolvida em .NET para a gestão de clientes, funcionários e produtos, utilizando PostgreSQL como banco de dados. A aplicação segue uma arquitetura modular com separação clara de responsabilidades entre as camadas de roteamento, serviços e repositórios.

## Estrutura do Projeto

O projeto está dividido em dois módulos principais:

- **Api**: Contém o ponto de entrada da aplicação, definições de rotas (Minimal APIs), middlewares para tratamento de exceções e configurações globais.
- **Api.Core**: Módulo central que abriga a lógica de negócio (Services), acesso a dados (Repositories), objetos de transferência de dados (DTOs), utilitários e definições de exceções personalizadas.

## Tecnologias Utilizadas

- **Plataforma**: .NET Core
- **Banco de Dados**: PostgreSQL
- **Acesso a Dados**: Npgsql (Driver ADO.NET para PostgreSQL)
- **Gestão de Configuração**: DotNetEnv para carregamento de variáveis de ambiente via arquivo .env
- **Padrões**: Injeção de Dependência, Minimal APIs, Repositories e Services.

## Funcionalidades Principais

### Gestão de Clientes
- Cadastro completo com validação de CPF.
- Atualização de dados cadastrais.
- Consulta de lista de clientes e busca individual.
- Remoção de registros.
- Identificação de clientes VIP e controle de contas.

### Gestão de Funcionários
- Cadastro de funcionários com verificação de privilégios administrativos.
- Controle de atestados e registros de nascimento.
- Operações completas de CRUD.

### Gestão de Produtos
- Controle de estoque com código de produto e lote.
- Gestão de valores de revenda.
- Validações para garantir que códigos e quantidades sejam válidos.

## Arquitetura de Tratamento de Erros

A API possui um middleware centralizado para tratamento de exceções, garantindo que o cliente receba respostas padronizadas em caso de erros:
- Dados não encontrados (400 Bad Request).
- CPF, nomes ou contas inválidas (422 Unprocessable Entity).
- Erros de conexão com banco de dados (500 Internal Server Error).
- Validações específicas de lógica de negócio.

## Configuração do Ambiente

1. **Banco de Dados**:
   - O esquema do banco de dados está localizado no arquivo sql.sql dentro do módulo Api.Core.
   - Execute o script sql.sql no seu servidor PostgreSQL para criar as tabelas: cliente, funcionario e produto.

2. **Variáveis de Ambiente**:
   - Crie um arquivo chamado .env na raiz do projeto Api.
   - Defina a variável DB_CONNECTION com a sua string de conexão para o banco de dados PostgreSQL.

3. **Execução**:
   - Compile o projeto utilizando o SDK do .NET.
   - Execute a aplicação. O sistema carregará automaticamente as configurações do arquivo .env e iniciará o servidor.

## Endpoints Principais

As rotas estão organizadas por contexto:
- /client/ - Operações relacionadas a clientes.
- /funcionario/ - Operações relacionadas a funcionários.
- /product/ - Operações relacionadas a produtos.

Cada contexto suporta os métodos HTTP padrão: GET (consulta), POST (criação), PUT (atualização) e DELETE (remoção).
## .ENV
O projeto usa o .env para armazenar os segredos do projeto
DB_CONNECTION=Host=localhost;Database=nome;Username=user;Password=senha