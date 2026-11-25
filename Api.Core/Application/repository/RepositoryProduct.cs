
using Npgsql;
using Dto;
using Api.Core.Application.utils;
using Microsoft.EntityFrameworkCore;
using models;

namespace Api.Core.Application.repository
{
    public interface IRepositoryProduct
    {
    // Os métodos são definidos como internal para restringir o acesso
    // direto ao repositório fora deste assembly.
    // A implementação usa public apenas para cumprir o contrato da interface.
    internal Task<ListaProduct> GetAllProduct(int limit, int page);
    internal Task<ListaProduct> GetEstoque(int limit, int page);
    internal  Task<List<decimal>> GetValorBruto();
    internal  Task<int> AddProduct(ProdutoDto campos);
    internal Task<int> UpdateProduct(ProdutoDto campos, int id);
    internal  Task<int> DeleteProduct(int id);
    internal Task<ProdutoDto> GetProductById(int id);
    internal Task<bool> IsExistLote(int lote);
    internal Task<bool> IsExistCode(int codigo);
    internal Task<long> QuantidadeProduct();
    }
/// <summary>
/// Repositório para manipulação de produtos e estoque no banco de dados.
/// </summary>
class RepositoryProduct(ConnectHost host):IRepositoryProduct
{
    /// <summary>
    /// Busca um produto pelo ID.
    /// </summary>
    /// <param name="id">ID do produto.</param>
    /// <returns>DTO do produto ou null.</returns>
    public  async Task<ProdutoDto> GetProductById(int id)
    {
        var context = host;
        var resultado= await context.produto.Where(pr => pr.id == id).FirstOrDefaultAsync();
        if (resultado is null)
        {
            return null;
        }
        ProdutoDto campos=new();
        campos.Id=resultado.id;
        campos.Nome=resultado.nome;
        campos.Codigo=resultado.codigo;
        campos.Lote=resultado.lote;
        campos.Quantidade=resultado.quantidade;
        campos.ValorRevenda = resultado.valor_revenda;
        return campos;
    }

    /// <summary>
    /// Verifica se um lote já está cadastrado no sistema.
    /// </summary>
    /// <param name="lote">Número do lote.</param>
    /// <returns>True se existir.</returns>
    public async Task<bool> IsExistLote(int lote)
    {
        var context = host;
        var resultado =await context.produto.Where(pr=> pr.lote==lote).CountAsync();
        //talvez seja mais economico
        //var resultado= context.cliente.Single(cliente => cliente.cpf == cpf);
        if (resultado >0)
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// Verifica se um código de produto já está em uso.
    /// </summary>
    /// <param name="codigo">Código do produto.</param>
    /// <returns>True se existir.</returns>
    public async Task<bool> IsExistCode(int codigo)
    {
        
        var context = host;
        var resultado = context.produto.Where(pr=> pr.codigo==codigo).Count();
       
        //var resultado= context.cliente.Single(cliente => cliente.cpf == cpf);
        if (resultado >= 1)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Lista todos os produtos com todos os detalhes.
    /// </summary>
    /// <param name="limit"></param>
    /// <param name="page"></param>
    /// <returns>ListaProduct com os registros.</returns>
    public  async Task<ListaProduct> GetAllProduct(int limit, int page)
    {
        var context = host;
        var take = limit > 0 ? limit : 100;
        var skip = (page > 1 ? page - 1 : 0) * take;
        var list=await context.produto.OrderBy(pr => pr.id).Skip(skip).Take(take).ToListAsync();
       ListaProduct lista=new();

        foreach(var c in list)
        {
            ProdutoDto campos=new();
            campos.Id=c.id;
            campos.Nome=c.nome;
            campos.Codigo=c.codigo;
            campos.Quantidade=c.quantidade;
            campos.ValorRevenda=c.valor_revenda;
            campos.Lote=c.lote;
            lista.Product.Add(campos);
        }
        return lista;
     }

    /// <summary>
    /// Consulta resumida de estoque (apenas nome e quantidade).
    /// </summary>
    /// <param name="limit"></param>
    /// <param name="page"></param>
    /// <returns>ListaProduct com dados parciais.</returns>
    public  async Task<ListaProduct> GetEstoque(int limit,int page)
    {
        var context = host;
        var take = limit > 0 ? limit : 100;
        var skip = (page > 1 ? page - 1 : 0) * take;
        var list=await context.produto.OrderBy(pr => pr.id).Skip(skip).Take(take)
            .Select(pr=>new {pr.nome,pr.quantidade}).ToListAsync();
        
       
        ListaProduct lista=new();
        foreach(var c in list)
        {
            ProdutoDto campos=new();
            campos.Nome=c.nome;
            campos.Quantidade=c.quantidade;
            lista.Product.Add(campos);
        }
        return lista;
    }
    /// <summary>
    /// Obtém os valores de revenda de todos os produtos do banco.
    /// </summary>
    /// <returns>Lista de valores decimais.</returns>
    public async Task<List<decimal>> GetValorBruto()
    {
        
        var context = host;
        var list=await context.produto.Select(pr=>new {pr.valor_revenda}).ToListAsync();

    
        List<decimal> lista=new();
        foreach(var c in list)
        {
            lista.Add(c.valor_revenda);
        }
        return lista;
    }
    /// <summary>
    /// Insere um novo produto no banco de dados.
    /// </summary>
    /// <param name="campos">Dados do produto.</param>
    /// <returns>Número de linhas afetadas.</returns>
    public  async Task<int> AddProduct(ProdutoDto campos)
    {
        
        var context = host;
        await  context.produto.AddAsync(new Produto(){nome = campos.Nome,codigo = campos.Codigo,quantidade = campos.Quantidade,valor_revenda = campos.ValorRevenda,lote = campos.Lote});
        var result = await context.SaveChangesAsync();
       
        return result;

    }
    /// <summary>
    /// Atualiza os dados de um produto existente.
    /// </summary>
    /// <param name="campos">Novos dados.</param>
    /// <param name="id">ID do produto.</param>
    /// <returns>Número de linhas afetadas.</returns>
    public async Task<int> UpdateProduct(ProdutoDto campos,int id)
    {

        var context = host;
     var result= await  context.produto.Where(pr=>pr.id==id).ExecuteUpdateAsync(setter=>
            setter.SetProperty(pr=>pr.nome,campos.Nome)
            .SetProperty(pr=>pr.codigo,campos.Codigo)
            .SetProperty(pr=>pr.lote,campos.Lote)
            .SetProperty(pr=>pr.quantidade,campos.Quantidade)
            .SetProperty(pr=>pr.valor_revenda,campos.ValorRevenda));
       


        return result;
    }
    /// <summary>
    /// Remove um produto pelo ID.
    /// </summary>
    /// <param name="id">ID do produto.</param>
    /// <returns>Número de linhas afetadas.</returns>
    public  async Task<int> DeleteProduct(int id)
    {
        var context = host;
        int resultado= await context.produto.Where(pr=>pr.id==id).ExecuteDeleteAsync();
        //revisar e colocar pra pegar por id
        return resultado ;
    }

    
    
    public async Task<long> QuantidadeProduct()
    {
        var context = host;
        var resultado = await context.produto.CountAsync();
        return resultado;
    }
}
}
