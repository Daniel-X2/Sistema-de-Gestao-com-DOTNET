using Npgsql;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Dto;
using Api.Core.Application.utils;
using Microsoft.EntityFrameworkCore;
using models;

namespace Api.Core.Application.repository
{ 
    

public interface  IRepositoryClient
{
    // Os métodos são definidos como internal para restringir o acesso
    // direto ao repositório fora deste assembly.
    // A implementação usa public apenas para cumprir o contrato da interface.    
   internal Task<ListaClient> GetAllClient(int page, int limit);
   internal Task<ClientDto> GetById(int id);
   internal Task<int> AddClient(ClientDto campos);
   internal Task<bool> ExistsAccount(int Conta);
   internal Task<bool> ExistsCpf(string cpf);
   internal Task<int> UpdateClient(ClientDto campos, int id);
   internal Task<int> DeleteClient(int id);
   internal Task<int> GetIdByCpf(string cpf);
   internal Task<long> QuantidadeClient();
}
/// <summary>
/// Repositório para manipulação de dados da tabela 'cliente' no PostgreSQL.
/// </summary>
internal class RepositoryClient(ConnectHost host):IRepositoryClient
{
    /// <summary>
    /// Consulta todos os clientes na tabela 'cliente'.
    /// </summary>
    /// <param name="page"></param>
    /// <param name="limit"></param>
    /// <returns>Lista de clientes (ListaClient).</returns>
    public async Task<ListaClient>  GetAllClient(int page,int limit)
    {
        var context = host;
        var take = limit > 0 ? limit : 100;
        var skip = (page > 1 ? page - 1 : 0) * take;
        var list=await context.cliente.OrderBy(cl => cl.id).Skip(skip).Take(take).ToListAsync();

        ListaClient lista=new();


        foreach(var c in list)
        {
            
            ClientDto campos=new();
            campos.Id=c.id;
            campos.Nome=c.nome;
            campos.Cpf=c.cpf;
            campos.Conta=c.conta;
            campos.Isvip=c.isvip;
            campos.Data=c.data;
            campos.Empresa=c.empresa;
            lista.Clients.Add(campos);
        }
         
        return lista;
        
        
    }
    /// <summary>
    /// Busca um cliente específico pelo ID.
    /// </summary>
    /// <param name="id">ID único do cliente.</param>
    /// <returns>Dados do cliente ou null se não encontrado.</returns>
    public  async Task<ClientDto> GetById(int id)
    { 
        
        
        var context = host;
        var resultado=await context.cliente.Where(cl => cl.id == id).FirstOrDefaultAsync();
            if (resultado is null)
            {
                return null;
            }
        
        ClientDto campos=new();
        campos.Id=resultado.id;
        campos.Nome=resultado.nome;
        campos.Cpf=resultado.cpf;
        campos.Conta=resultado.conta;
        campos.Isvip=resultado.isvip;
        campos.Data=resultado.data;
        campos.Empresa=resultado.empresa;

        return campos;
    }
    /// <summary>
    /// Insere um novo cliente no banco de dados.
    /// </summary>
    /// <param name="campos">Dados do cliente.</param>
    /// <returns>Número de linhas afetadas.</returns>
    public async Task<int> AddClient(ClientDto campos)
    {
        
        var context = host;
        await context.cliente.AddAsync(new cliente( ){nome=campos.Nome,cpf = campos.Cpf,conta = campos.Conta,empresa = campos.Empresa,isvip = campos.Isvip});
        int resultado=await context.SaveChangesAsync();
        
        return resultado;
    }  
    /// <summary>
    /// Verifica se uma conta já existe no banco.
    /// </summary>
    /// <param name="Conta">Número da conta.</param>
    /// <returns>True se existir.</returns>
    public async Task<bool> ExistsAccount(int Conta)
    {
        var context = host;
        return await context.cliente.AnyAsync(cliente => cliente.conta == Conta);
    }
    /// <summary>
    /// Verifica se um CPF já está cadastrado para algum cliente.
    /// </summary>
    /// <param name="cpf">CPF a verificar.</param>
    /// <returns>True se existir.</returns>
    public async Task<bool> ExistsCpf(string cpf)
    {
        var context = host;
        return await context.cliente.AnyAsync(cliente => cliente.cpf == cpf);
    }

    public async Task<Int64> QuantidadeClient()
    {
        
        var context = host;
        var resultado = await context.cliente.CountAsync();
        return resultado;
    }
    /// <summary>
    /// Atualiza os dados de um cliente pelo ID.
    /// </summary>
    /// <param name="campos">Dados atualizados.</param>
    /// <param name="id">ID do cliente.</param>
    /// <returns>Número de linhas afetadas.</returns>
    public  async Task<int> UpdateClient(ClientDto campos,int id)
    {
        var context = host;
       var resultado= await  context.cliente.Where(cl=>cl.id==id).ExecuteUpdateAsync(setter=>setter.SetProperty(cl=>cl.nome,campos.Nome)
        .SetProperty(cl=>cl.cpf,campos.Cpf)
        .SetProperty(cl=>cl.conta,campos.Conta).
        SetProperty(cl=>cl.empresa , campos.Empresa)
        .SetProperty(cl=>cl.isvip, campos.Isvip));
        // resultado= await context.SaveChangesAsync();
        
        return  resultado;
    }
    /// <summary>
    /// Exclui um cliente pelo ID.
    /// </summary>
    /// <param name="id">ID do cliente.</param>
    /// <returns>Número de linhas afetadas.</returns>
    public  async Task<int> DeleteClient(int id)
    {
        var context = host;
        int resultado= await context.cliente.Where(cl=>cl.id==id).ExecuteDeleteAsync();
        //revisar e colocar pra pegar por id
        
        return resultado ;
    }
    /// <summary>
    /// Obtém o ID de um cliente através do seu CPF.
    /// </summary>
    /// <param name="cpf">CPF do cliente.</param>
    /// <returns>ID do cliente ou 0 se não encontrado.</returns>
    public  async Task<int> GetIdByCpf(string cpf)
    {
        var context = host;
        var resultado=await context.cliente.Where(cl => cl.cpf == cpf).Select(cl => cl.id).FirstOrDefaultAsync();
    
        return resultado;
    }
    
}
}