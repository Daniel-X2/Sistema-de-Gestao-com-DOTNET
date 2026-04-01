using Npgsql;
using Dto;
using Api.Core.Application.utils;

namespace Api.Core.Application.repository
{ 
    

public interface  IRepositoryClient
{
    // Os métodos são definidos como internal para restringir o acesso
    // direto ao repositório fora deste assembly.
    // A implementação usa public apenas para cumprir o contrato da interface.    
   internal Task<ListaClient>  GetAllClient();
   internal Task<ClientDto> GetById(int id);
   internal Task<int> AddClient(ClientDto campos);
   internal Task<bool> ExistsAccount(int conta);
   internal Task<bool> ExistsCpf(string cpf);
   internal Task<int> UpdateClient(ClientDto campos, int id);
   internal Task<int> DeleteClient(int id);
   internal Task<int> GetIdByCpf(string cpf);

}
/// <summary>
/// Repositório para manipulação de dados da tabela 'cliente' no PostgreSQL.
/// </summary>
internal class RepositoryClient(IConnect host):IRepositoryClient
{
    
    /// <summary>
    /// Consulta todos os clientes na tabela 'cliente'.
    /// </summary>
    /// <returns>Lista de clientes (ListaClient).</returns>
    public async Task<ListaClient>  GetAllClient()
    {

        await using NpgsqlConnection connect=host.Connect(); 
        
        await connect.OpenAsync();

        await using var cmd = new NpgsqlCommand("SELECT * FROM cliente ", connect);

        ListaClient lista=new();
        
        await using var reader = await cmd.ExecuteReaderAsync();
        while(await reader.ReadAsync())
        {
            
            ClientDto campos=new();
            campos.Nome=(string)reader["nome"];
            campos.Cpf=(string)reader["cpf"];
            campos.Conta=(int)reader["conta"];
            campos.Isvip=(bool)reader["isvip"];
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
        await using NpgsqlConnection connect=host.Connect(); 
        
        await connect.OpenAsync();

        await using var cmd = new NpgsqlCommand("SELECT * FROM cliente WHERE id = @id", connect);
        cmd.Parameters.AddWithValue("id", id);
        
        await using var reader = await cmd.ExecuteReaderAsync();
        if (!await reader.ReadAsync()) { return null;}
        
        ClientDto campos=new();
        campos.Nome=(string)reader["nome"];
        campos.Cpf=(string)reader["cpf"];
        campos.Conta=(int)reader["conta"];
        campos.Isvip=(bool)reader["isvip"];
           
        
         
        return campos;
    }
    /// <summary>
    /// Insere um novo cliente no banco de dados.
    /// </summary>
    /// <param name="campos">Dados do cliente.</param>
    /// <returns>Número de linhas afetadas.</returns>
    public async Task<int> AddClient(ClientDto campos)
    {
        int resultado;
        
        await using NpgsqlConnection connect = host.Connect();
        
        await connect.OpenAsync();

        await using (var cmd = new NpgsqlCommand("INSERT INTO cliente (nome ,cpf, conta,isvip) VALUES (@nome, @cpf, @conta,@isvip)", connect))
        {
            cmd.Parameters.AddWithValue("nome", campos.Nome);
            cmd.Parameters.AddWithValue("cpf", campos.Cpf);
            cmd.Parameters.AddWithValue("conta", campos.Conta);
            cmd.Parameters.AddWithValue("isvip", campos.Isvip);
            resultado=await cmd.ExecuteNonQueryAsync();
        }
        
        return resultado;
    }  
    /// <summary>
    /// Verifica se uma conta já existe no banco.
    /// </summary>
    /// <param name="conta">Número da conta.</param>
    /// <returns>True se existir.</returns>
    public async Task<bool> ExistsAccount(int conta)
    {
        await using NpgsqlConnection connect=host.Connect();
        await connect.OpenAsync();
        string sql="SELECT EXISTS (SELECT 1 FROM cliente WHERE conta=@conta)";
        await using var cmd=new NpgsqlCommand(sql,connect);
        cmd.Parameters.AddWithValue("conta",conta);
       bool resultado=(bool) await cmd.ExecuteScalarAsync();
       return resultado;
       
    }
    /// <summary>
    /// Verifica se um CPF já está cadastrado para algum cliente.
    /// </summary>
    /// <param name="cpf">CPF a verificar.</param>
    /// <returns>True se existir.</returns>
    public async Task<bool> ExistsCpf(string cpf)
    {
        await using NpgsqlConnection connect =host.Connect();
        await connect.OpenAsync();
        string sql ="SELECT EXISTS (SELECT 1 FROM cliente WHERE cpf=@cpf)";
        await using var cmd=new NpgsqlCommand(sql,connect);
        cmd.Parameters.AddWithValue("cpf",cpf);
        bool resultado=(bool) await cmd.ExecuteScalarAsync();
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
        
        await using NpgsqlConnection connect=host.Connect();

        await connect.OpenAsync();
        int resultado;
        
        
       await using var cmd = new NpgsqlCommand("UPDATE  cliente set nome=@nome,cpf=@cpf,conta=@conta,isvip=@isvip  WHERE id = @id", connect);
      
       cmd.Parameters.AddWithValue("conta",campos.Conta);
       cmd.Parameters.AddWithValue("isvip",campos.Isvip);
       cmd.Parameters.AddWithValue("nome", campos.Nome);
       cmd.Parameters.AddWithValue("cpf", campos.Cpf);
       cmd.Parameters.AddWithValue("id", id);
       
        
         resultado=await cmd.ExecuteNonQueryAsync();   
        
         return  resultado;
    }
    /// <summary>
    /// Exclui um cliente pelo ID.
    /// </summary>
    /// <param name="id">ID do cliente.</param>
    /// <returns>Número de linhas afetadas.</returns>
    public  async Task<int> DeleteClient(int id)
    {
        int resultado;
        
        await using NpgsqlConnection connect=host.Connect();
        
        await connect.OpenAsync();
        //revisar e colocar pra pegar por id
       await using (var  cmd = new NpgsqlCommand("DELETE FROM cliente WHERE id=@id ", connect))
        {
            cmd.Parameters.AddWithValue("id",id);
            resultado=await cmd.ExecuteNonQueryAsync();
        }
        
       
        return resultado ;
        
        
    }
    /// <summary>
    /// Obtém o ID de um cliente através do seu CPF.
    /// </summary>
    /// <param name="cpf">CPF do cliente.</param>
    /// <returns>ID do cliente ou 0 se não encontrado.</returns>
    public  async Task<int> GetIdByCpf(string cpf)
    {
        await using NpgsqlConnection connect=host.Connect(); 
        
        await connect.OpenAsync();

        await using var cmd = new NpgsqlCommand("SELECT id FROM cliente WHERE cpf=@cpf ", connect);
        cmd.Parameters.AddWithValue("cpf", cpf);
        
        await using var reader = await cmd.ExecuteReaderAsync();
        if (!await reader.ReadAsync()) { return 0;}




        
        return (int) reader["id"];
    }
    
}
}