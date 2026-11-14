
using Npgsql;
using Dto;
using Api.Core.Application.utils;
using Microsoft.EntityFrameworkCore;
using models;

namespace Api.Core.Application.repository
{ 
    

public interface IRepositoryFuncionario
{ 
    // Os métodos são definidos como internal para restringir o acesso
    // direto ao repositório fora deste assembly.
    // A implementação usa public apenas para cumprir o contrato da interface.
   internal Task<bool> ExistsCpf(string cpf);
   internal Task<ListaFuncionario> GetFuncionario(int limit, int page);
   internal Task<int> AddFuncionario(FuncionarioDto campos);//
   internal Task<int> UpdateFuncionario(FuncionarioDto campos,int id);
   internal Task<int> DeleteFuncionario(int id);
   internal Task<FuncionarioLoginDto> GetAdmin(string cpf);
   internal Task<FuncionarioDto> GetById(int id);
   internal Task<long> QuantidadeFuncionario();
    }
/// <summary>
/// Repositório para gestão de funcionários no banco de dados.
/// </summary>
internal class RepositoryFuncionario(ConnectHost host):IRepositoryFuncionario
{
    /// <summary>
    /// Recupera dados essenciais de um funcionário para autenticação via CPF.
    /// </summary>
    /// <param name="cpf">CPF do funcionário.</param>
    /// <returns>Dados de login (Admin, SenhaHash, CPF) ou null.</returns>
    public async Task<FuncionarioLoginDto> GetAdmin(string cpf)
    {
        await using var connect = host;
        var result= connect.funcionario.Where(f=>f.cpf==cpf).Select(f=> new {f.isadmin,f.senha_hash,f.cpf}).First();
        
        FuncionarioLoginDto resultado = new();
        resultado.isadmin =result.isadmin;
        resultado.cpf = result.cpf; 
        resultado.SenhaHash = result.senha_hash;
        
        return resultado;
    }
    
    /// <summary>
    /// Verifica se um CPF já existe na tabela de funcionários.
    /// </summary>
    /// <param name="cpf">CPF a pesquisar.</param>
    /// <returns>True se existir.</returns>
    public async Task<bool> ExistsCpf(string cpf)
    {
        await using var connect = host;
        var resultado= connect.funcionario.Where(f => f.cpf == cpf).Select(cl => cl.id).First();
        if (resultado >= 1)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Lista todos os funcionários cadastrados.
    /// </summary>
    /// <returns>ListaFuncionario com todos os registros.</returns>
    public async Task<ListaFuncionario> GetFuncionario(int limit, int page)
    {
        
        await using var context = host;
        var list=await context.funcionario.ToListAsync();
        ListaFuncionario lista=new();
        foreach(var c in list)
        {
            FuncionarioDto campos=new();
            campos.Id=c.id;
            campos.Nome=c.nome;
            campos.Cpf=c.cpf;
            campos.QuantidadeAtestado=c.quantidade_atestado;
            campos.Isadmin=c.isadmin;
            campos.Data=c.data;
            campos.Senha="";
            lista.Funcionarios.Add(campos);
        }
        return lista;
    }
    /// <summary>
    /// Cadastra um novo funcionário no banco.
    /// </summary>
    /// <param name="campos">Dados do funcionário.</param>
    /// <returns>Número de linhas afetadas.</returns>
    public  async Task<int> AddFuncionario(FuncionarioDto campos)
    {
        await using var context=host;
        await context.funcionario.AddAsync(new Funcionario( ){nome=campos.Nome,cpf = campos.Cpf,quantidade_atestado = campos.QuantidadeAtestado,senha_hash= campos.Senha,isadmin = campos.Isadmin});
        int resultado=await context.SaveChangesAsync();
        
        return resultado;
    }  
    
    /// <summary>
    /// Atualiza o registro de um funcionário existente.
    /// </summary>
    /// <param name="campos">Dados atualizados.</param>
    /// <param name="id">ID do funcionário.</param>
    /// <returns>Número de linhas afetadas.</returns>
    public  async Task<int> UpdateFuncionario(FuncionarioDto campos,int id)
    {
     
        await using var context=host;
        context.funcionario.Update(new Funcionario(){nome=campos.Nome,cpf = campos.Cpf,quantidade_atestado = campos.QuantidadeAtestado,senha_hash= campos.Senha,isadmin = campos.Isadmin});
        int resultado= await context.SaveChangesAsync();
        
        return  resultado;

    }
    /// <summary>
    /// Exclui um funcionário pelo ID.
    /// </summary>
    /// <param name="id">ID do funcionário.</param>
    /// <returns>Número de linhas afetadas.</returns>
    public  async Task<int> DeleteFuncionario(int id)
    {
        await using var context=host;
        int resultado= await context.funcionario.Where(f=>f.id==id).ExecuteDeleteAsync();
        //revisar e colocar pra pegar por id
        return resultado ;
    }
    /// <summary>
    /// Busca um funcionário específico pelo ID.
    /// </summary>
    /// <param name="id">ID do funcionário.</param>
    /// <returns>DTO com dados do funcionário ou null.</returns>
    public  async Task<FuncionarioDto> GetById(int id)
    {
        await using var context=host;
        var resultado= context.funcionario.Where(f => f.id == id).First();
        
        
        FuncionarioDto campos=new();
        campos.Id=resultado.id;
        campos.Nome=resultado.nome;
        campos.Cpf=resultado.cpf;
        campos.QuantidadeAtestado=resultado.quantidade_atestado;
        campos.Isadmin=resultado.isadmin;
        campos.Data=resultado.data;
        campos.Senha=resultado.senha_hash;

        return campos;
    }


    public async Task<long> QuantidadeFuncionario()
    {
        await using var context=host;
        var resultado = await context.funcionario.ToListAsync();
        return resultado.Count;
    }
}
}
