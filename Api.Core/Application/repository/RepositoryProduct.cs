
using Npgsql;
using Dto;
using Api.Core.Application.utils;

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
    }
/// <summary>
/// Repositório para manipulação de produtos e estoque no banco de dados.
/// </summary>
class RepositoryProduct(IConnect host):IRepositoryProduct
{
    /// <summary>
    /// Busca um produto pelo ID.
    /// </summary>
    /// <param name="id">ID do produto.</param>
    /// <returns>DTO do produto ou null.</returns>
    public  async Task<ProdutoDto> GetProductById(int id)
    {
        await using NpgsqlConnection connect=host.Connect(); 

        await connect.OpenAsync();

        await using var cmd = new NpgsqlCommand("SELECT * FROM produto WHERE id = @id", connect);
        cmd.Parameters.AddWithValue("id", id);

        await using var reader = await cmd.ExecuteReaderAsync();
        if (!await reader.ReadAsync()) { return null;}

        ProdutoDto campos=new();
        campos.Nome=(string)reader["nome"];
        campos.Codigo=(int)reader["codigo"];
        campos.Lote=(int)reader["lote"];
        campos.Quantidade=(int)reader["quantidade"];
        campos.ValorRevenda = (decimal)reader["valor_revenda"];


        return campos;
    }

    /// <summary>
    /// Verifica se um lote já está cadastrado no sistema.
    /// </summary>
    /// <param name="lote">Número do lote.</param>
    /// <returns>True se existir.</returns>
    public async Task<bool> IsExistLote(int lote)
    {
        await using NpgsqlConnection connect = host.Connect();
        await connect.OpenAsync();

        await using var cmd = new NpgsqlCommand("SELECT EXISTS (SELECT 1 FROM produto WHERE lote=@lote)",connect);
        cmd.Parameters.AddWithValue("lote", lote); 
        bool resultado=(bool) await cmd.ExecuteScalarAsync();
        return resultado;
    }
    /// <summary>
    /// Verifica se um código de produto já está em uso.
    /// </summary>
    /// <param name="codigo">Código do produto.</param>
    /// <returns>True se existir.</returns>
    public async Task<bool> IsExistCode(int codigo)
    {
       await using NpgsqlConnection connect = host.Connect();
       await connect.OpenAsync();

       await using var cmd = new NpgsqlCommand("SELECT EXISTS (SELECT 1 FROM produto WHERE codigo=@codigo)",connect);
       cmd.Parameters.AddWithValue("codigo", codigo); 
       bool resultado=(bool) await cmd.ExecuteScalarAsync();
       return resultado;
    }

    /// <summary>
    /// Lista todos os produtos com todos os detalhes.
    /// </summary>
    /// <param name="limit"></param>
    /// <param name="page"></param>
    /// <returns>ListaProduct com os registros.</returns>
    public  async Task<ListaProduct> GetAllProduct(int limit, int page)
    {
        
        var offset = (page - 1) * limit;
        
       await using NpgsqlConnection connect=host.Connect();;
       await connect.OpenAsync();
       await using var cmd = new NpgsqlCommand("SELECT * FROM produto LIMIT @limit OFFSET @offset",connect);
       cmd.Parameters.AddWithValue("limit", limit);
       cmd.Parameters.AddWithValue("offset", offset);
       await using var read= await  cmd.ExecuteReaderAsync();
       ListaProduct lista=new();

        while (await read.ReadAsync())
        {
            ProdutoDto campos=new();
            campos.Nome=(string)read["nome"];
            campos.Codigo=(int)read["codigo"];
            campos.Quantidade=(int)read["quantidade"];
            campos.ValorRevenda=(decimal)read["valor_revenda"];
            campos.Lote=(int)read["lote"];
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
        var offset = (page - 1) * limit;
        
    
       await using NpgsqlConnection connect=host.Connect();
       await connect.OpenAsync();

       await using var cmd=new NpgsqlCommand("SELECT nome,quantidade FROM produto LIMIT @limit OFFSET @offset",connect);
       cmd.Parameters.AddWithValue("limit", limit);
       cmd.Parameters.AddWithValue("offset", offset);
       var read=await cmd.ExecuteReaderAsync();
        ListaProduct lista=new();
        while(await read.ReadAsync())
        {
            ProdutoDto campos=new();
            campos.Nome=(string)read["nome"];
            campos.Quantidade=(int)read["quantidade"];

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
        
      await using  NpgsqlConnection connect=host.Connect();
      await connect.OpenAsync();

      await  using var cmd= new NpgsqlCommand("SELECT valor_revenda FROM produto",connect);
      var read= await cmd.ExecuteReaderAsync();
        List<decimal> lista=new();
        while(await read.ReadAsync())
        {
            lista.Add((decimal)read["valor_revenda"]);
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

      await using  NpgsqlConnection connect=  host.Connect();
      await   connect.OpenAsync();

       await using var cmd = new NpgsqlCommand("INSERT INTO produto (nome ,codigo,quantidade,valor_revenda,lote) VALUES (@nome,@codigo,@quantidade,@valor_revenda,@lote) ",connect);
        cmd.Parameters.AddWithValue("nome",campos.Nome);
        cmd.Parameters.AddWithValue("codigo", campos.Codigo);
        cmd.Parameters.AddWithValue("quantidade",campos.Quantidade);
        cmd.Parameters.AddWithValue("valor_revenda",campos.ValorRevenda);
        cmd.Parameters.AddWithValue("lote",campos.Lote);
        int resultado=await cmd.ExecuteNonQueryAsync();
        return resultado;

    }
    /// <summary>
    /// Atualiza os dados de um produto existente.
    /// </summary>
    /// <param name="campos">Novos dados.</param>
    /// <param name="id">ID do produto.</param>
    /// <returns>Número de linhas afetadas.</returns>
    public async Task<int> UpdateProduct(ProdutoDto campos,int id)
    {

      await using  NpgsqlConnection connect= host.Connect();
      await connect.OpenAsync();

       await using var cmd = new NpgsqlCommand("UPDATE produto set nome = @nome,codigo=@codigo,quantidade=@quantidade,valor_revenda=@valor_revenda,lote=@lote WHERE id = @id",connect);
       cmd.Parameters.AddWithValue("nome",campos.Nome);
       cmd.Parameters.AddWithValue("codigo",campos.Codigo);
       cmd.Parameters.AddWithValue("lote",campos.Lote);
       cmd.Parameters.AddWithValue("quantidade",campos.Quantidade);
       cmd.Parameters.AddWithValue("valor_revenda",campos.ValorRevenda);
       cmd.Parameters.AddWithValue("id", id);
       int resultado= await cmd.ExecuteNonQueryAsync();

       return resultado;

    }
    /// <summary>
    /// Remove um produto pelo ID.
    /// </summary>
    /// <param name="id">ID do produto.</param>
    /// <returns>Número de linhas afetadas.</returns>
    public  async Task<int> DeleteProduct(int id)
    {
       await using NpgsqlConnection connect=host.Connect(); 
       await connect.OpenAsync();
       await using var cmd=new NpgsqlCommand("DELETE FROM produto WHERE id=@id",connect);
       cmd.Parameters.AddWithValue("id", id);
       int resultado= await cmd.ExecuteNonQueryAsync();
       return resultado;
    }

    
    
}
}
