using Dto;
using Api.Core.Application.service;


namespace Api.Routers;

public class ProductRouters
{
    public async Task Routers(WebApplication app)
    {
        app.MapGet("/estoque/get", async Task<ListaProduct> (IServiceProduct service) =>
        { 
            ListaProduct lista= await service.GetEstoque();
            
            return lista;
        }).WithTags("Product").WithSummary("Lista Quantidade e nome dos produtos no estoque").WithDescription("Retorna uma lista resumida contendo apenas o nome e a quantidade disponível de cada produto no estoque.");
        app.MapGet("/product/get", async Task<ListaProduct> (IServiceProduct service) =>
        {
            ListaProduct lista = await service.GetAllProduct();
            return lista;
            
        }).WithTags("Product").WithSummary("Lista todos os produtos").WithDescription("Retorna os detalhes completos de todos os produtos cadastrados no catálogo.");
        app.MapGet("/estoque/valorBruto", async Task<List<decimal>> (IServiceProduct service) =>
        {
            List<decimal> lista = await service.GetValorBruto();

            return lista;
        }).WithTags("Product").WithSummary("Lista o Valor bruto de todos os produtos").WithDescription("Calcula e retorna o valor bruto total em estoque para cada produto.");
        app.MapGet("/product/get/{id}", async Task<ProdutoDto> (int id,IServiceProduct service) =>
        {
            ProdutoDto produto = await service.GetProdutId(id);
            
            return produto;
        }).WithTags("Product").WithSummary("Lista produtos com o ID").WithDescription("Busca e retorna as informações detalhadas de um produto específico através do seu ID.");
        app.MapPut("/product/update/{id}/", async Task<IResult> (int id, ProdutoDto campos, IServiceProduct service) =>
        {
            await service.UpdateProduct(campos, id);

            return Results.Ok("adicionado com Sucesso");
        }).WithTags("Product").WithSummary("atualiza o produto").WithDescription("Atualiza os dados de um produto existente. Retorna mensagem de sucesso após a operação.");
        app.MapPost("/product/add", async Task<IResult> (ProdutoDto campos, IServiceProduct service) =>
        {
            bool resultado = await service.AddProduct(campos);
            if (resultado)
            {
                return Results.Ok("adicionado com sucesso");
            }

            return Results.BadRequest("Erro ao adicionar");
            
        }).WithTags("Product").WithSummary("Adiciona o produto").WithDescription("Cadastra um novo produto no sistema. Retorna sucesso ou erro caso ocorra algum problema na criação.");
        app.MapDelete("/product/delete/{id}", async Task<IResult> (int id, IServiceProduct service) =>
        {
            bool resultado = await service.DeleteProduct(id);
            if (resultado)
            {
                return Results.Ok("foi deletado com sucesso");
            }

            return Results.BadRequest("erro ao deletar");
            
        }).WithTags("Product").WithSummary("Deleta o produto ").WithDescription("Remove permanentemente um produto do catálogo com base no seu ID.");
    }
}