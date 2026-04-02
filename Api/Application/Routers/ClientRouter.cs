using Api.Core.Application.service;
using Api.Test;
using Dto;


namespace Api.Routers;

/// <summary>
/// Responsável por gerenciar as rotas relacionadas aos clientes.
/// </summary>
public class  ClientRouter
{
    
    /// <summary>
    /// Define os endpoints de cliente na aplicação (GET, POST, PUT, DELETE).
    /// </summary>
    /// <param name="app">Instância do WebApplication para mapear as rotas.</param>
    public  async  Task Routers(WebApplication app)
    {
        
        app.MapGet("/client/get/", async Task<IResult> (IServiceClient service,int page=1,int limit=100) =>
        {
           
            ListaClient lista=  await service.GetAllService(page, limit);
            
            return Results.Ok(lista) ;
        }).WithTags("Client").WithSummary("Listar todos os clientes").WithDescription("Retorna uma lista contendo todos os clientes cadastrados no sistema.").RequireAuthorization();

        app.MapDelete("/client/delete/{id}", async  Task<IResult> (int id,IServiceClient service) =>
        {
          bool resultado= await service.DeleteService(id);
          
          if (resultado)
          {
              return Results.Ok(new {resultado="foi deletado com sucesso"});
          }
          return Results.BadRequest(new {erro="erro ao tentar deletar"});
          
          
        }).WithTags("Client").WithSummary("Excluir cliente por ID").WithDescription("Remove um cliente do sistema permanentemente com base no ID fornecido. Retorna sucesso ou erro na operação.").RequireAuthorization();

        app.MapPost("/client/add/", async Task<IResult> (ClientDto campos,IServiceClient service) =>
        {
        bool resultado=  await  service.AddService(campos);
     
        
        if (resultado)
        {
            return Results.Ok(new {resultado="foi adicionado com sucesso"});
        }
        return Results.BadRequest(new {erro="erro ao tentar adicionar"});
      
        }).WithTags("Client").WithSummary("Adiciona clientes").WithDescription("Cria um novo registro de cliente com os dados fornecidos. Retorna uma mensagem de confirmação ou erro.").RequireAuthorization();

        app.MapPut("/client/update/{id}/", async Task<IResult> (int id, ClientDto campos, IServiceClient service) =>
        {
          var resultado= await service.UpdateService(id,campos);
          //_next(context);

          if (resultado.Length==0 )
          {
              return Results.Ok(new {resultado=$"atualizado com sucesso {resultado.Length}"});
          }
          return Results.Ok(new {resultado="teve atualizaçoes parciais",camposNotUpdate=$"campos nao atualizados {resultado}"});
          
        }).WithTags("Client").WithSummary("Atualiza o Client").WithDescription("Atualiza as informações de um cliente existente.").RequireAuthorization();
    }
}
