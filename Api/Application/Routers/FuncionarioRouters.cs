using Api.Core.Application.service;
using auth.Models;
using Dto;

using auth.Services;
namespace Api.Routers;

public class FuncionarioRouters
{
        
        public  async  Task Router(WebApplication app)
        {
            app.MapPost("/login/", async Task<IResult> (FuncionarioJsonBody body,IServiceFuncionario service,IConfiguration config,TokenService token) =>
            {
                
                var n1 = await service.Admin(body.cpf);
               
                
                
                if (Crypto.VerificarHash(body.Senha,n1.SenhaHash))
                {
                   return Results.Ok(token.Generate(new Users(body.cpf, body.Senha, new[] { "Admin" }), n1.isadmin));
                    
                }
                return  Results.Ok(token.Generate(new Users(body.cpf,body.Senha,new []{"User"}),n1.isadmin));
            
            });
            app.MapGet("/funcionario/get",async Task<IResult> (IServiceFuncionario service) =>
            {
              
                var funcionario= await service.GetAll();
                return Results.Ok(funcionario) ;
                
            }).WithTags("Funcionario").WithSummary("Lista os funcionarios").WithDescription("Retorna uma lista de todos os funcionários cadastrados, incluindo seus IDs para operações futuras.").RequireAuthorization("Admin");
            
        
            app.MapGet("/funcionario/get/{id}",async Task<IResult>(int id,IServiceFuncionario n1) =>
            {
                var campo =await n1.GetByIdService(id);
                return Results.Ok(campo);
            }).WithTags("Funcionario").WithSummary("Lista o funcionario com o ID").WithDescription("Busca e retorna os detalhes de um funcionário específico através do seu ID único.").RequireAuthorization();

            app.MapDelete("/Funcionario/delete/{id}", async Task<IResult> (int id,IServiceFuncionario n1) =>
            {
                bool resultado =await n1.DeleteFuncionarioService(id);
              
                if (resultado)
                {
                    return  Results.Ok();
                }
                return  Results.BadRequest();
            }).WithTags("Funcionario").WithSummary("Excluir funcionario por ID").WithDescription("Remove o registro de um funcionário do sistema utilizando seu ID. Retorna OK se removido com sucesso.").RequireAuthorization();

            app.MapPost("/funcionario/add/",async Task<IResult> (FuncionarioDto campos, IServiceFuncionario service) =>
                    {
                     bool resultado=  await service.AddService(campos);
                     if (resultado)
                     {
                       return  Results.Ok("adicionado com sucesso");
                     }

                     return Results.BadRequest("erro ao adicionar");
                     
                    }).WithTags("Funcionario").WithSummary("Adiciona funcionario").WithDescription("Cadastra um novo funcionário no sistema com os dados fornecidos. Retorna sucesso ou erro no processamento.").RequireAuthorization();

            app.MapPut("funcionario/update/{id}/", async Task<IResult> (int id,FuncionarioDto campos, IServiceFuncionario service) =>
            {
            bool resultado=  await  service.UpdateFuncionarioService(campos, id);
            if (resultado)
            {
                return  Results.Ok("atualizado com sucesso");
            }
            return Results.BadRequest("erro ao atualizar");
            }).WithTags("Funcionario").WithSummary("Atualiza o funcionario").WithDescription("Atualiza as informações de um funcionário já existente com base no ID. Retorna o status da atualização.").RequireAuthorization();

        }
        
        
}