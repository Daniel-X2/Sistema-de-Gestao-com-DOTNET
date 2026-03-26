using Api.Core.Application.service;

using Dto;


namespace Api.Routers;

public class FuncionarioRouters
{
        
        public  async  Task Router(WebApplication app)
        {
            
            app.MapGet("/funcionario/get",async Task<ListaFuncionario> (IServiceFuncionario n1) =>
            {
              //e bom retornar com id pra se caso eu quiser deletar depois
                var n2= await n1.GetAll();
                return n2 ;
                
            }).WithTags("Funcionario").WithSummary("Lista os funcionarios").WithDescription("Retorna uma lista de todos os funcionários cadastrados, incluindo seus IDs para operações futuras.");
            
        
            app.MapGet("/funcionario/get/{id}",async (int id,IServiceFuncionario n1) =>
            {
                var campo =await n1.GetByIdService(id);
                return campo;
            }).WithTags("Funcionario").WithSummary("Lista o funcionario com o ID").WithDescription("Busca e retorna os detalhes de um funcionário específico através do seu ID único.");
            app.MapDelete("/Funcionario/delete/{id}", async Task<IResult> (int id,IServiceFuncionario n1) =>
            {
                bool resultado =await n1.DeleteFuncionarioService(id);
              
                if (resultado)
                {
                    return  Results.Ok();
                }
                return  Results.BadRequest();
            }).WithTags("Funcionario").WithSummary("Excluir funcionario por ID").WithDescription("Remove o registro de um funcionário do sistema utilizando seu ID. Retorna OK se removido com sucesso.");
            app.MapPost("/funcionario/add/",async Task<IResult> (FuncionarioDto campos, IServiceFuncionario service) =>
                    {
                     bool resultado=  await service.AddService(campos);
                     if (resultado)
                     {
                       return  Results.Ok("adicionado com sucesso");
                     }

                     return Results.BadRequest("erro ao adicionar");
                     
                    }).WithTags("Funcionario").WithSummary("Adiciona funcionario").WithDescription("Cadastra um novo funcionário no sistema com os dados fornecidos. Retorna sucesso ou erro no processamento.");
            app.MapPut("funcionario/update/{id}/", async Task<IResult> (int id,FuncionarioDto campos, IServiceFuncionario service) =>
            {
            bool resultado=  await  service.UpdateFuncionarioService(campos, id);
            if (resultado)
            {
                return  Results.Ok("atualizado com sucesso");
            }
            return Results.BadRequest("erro ao atualizar");
            }).WithTags("Funcionario").WithSummary("Atualiza o funcionario").WithDescription("Atualiza as informações de um funcionário já existente com base no ID. Retorna o status da atualização.");

        }
        
        
}