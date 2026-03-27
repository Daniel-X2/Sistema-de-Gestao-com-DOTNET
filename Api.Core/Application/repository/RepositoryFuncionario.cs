
using Npgsql;
using Dto;
using Api.Core.Application.utils;

namespace Api.Core.Application.repository
{ 
    

public interface IRepositoryFuncionario
{ 
    // Os métodos são definidos como internal para restringir o acesso
    // direto ao repositório fora deste assembly.
    // A implementação usa public apenas para cumprir o contrato da interface.
   internal Task<bool> ExistsCpf(string cpf);
   internal Task<ListaFuncionario> GetFuncionario();
   internal Task<int> AddFuncionario(FuncionarioDto campos);//
   internal Task<int> UpdateFuncionario(FuncionarioDto campos,int id);
   internal Task<int> DeleteFuncionario(int id);
   internal Task<int> GetIdByCpf(string cpf);
   internal Task<FuncionarioDto> GetById(int id);
    }
internal class RepositoryFuncionario(IConnect host):IRepositoryFuncionario
{
    public async Task<int> GetIdByCpf(string cpf)
    {
        await using NpgsqlConnection connect = host.Connect();

        await connect.OpenAsync();

        await using var cmd = new NpgsqlCommand("SELECT id FROM funcionario WHERE cpf=@cpf ", connect);
        cmd.Parameters.AddWithValue("cpf", cpf);

        await using var reader = await cmd.ExecuteReaderAsync();
        if (!await reader.ReadAsync()) { return 0;}
        return (int)reader["id"];
    }

    public async Task<bool> ExistsCpf(string cpf)
    {
        await using NpgsqlConnection connect =host.Connect();
        await connect.OpenAsync();
        string sql ="SELECT EXISTS (SELECT 1 FROM funcionario WHERE cpf=@cpf)";
        await using var cmd=new NpgsqlCommand(sql,connect);
        cmd.Parameters.AddWithValue("cpf",cpf);
        bool resultado=(bool) await cmd.ExecuteScalarAsync();
        return resultado;
    }
    public async Task<ListaFuncionario> GetFuncionario()
    {
        
        await using NpgsqlConnection connect=host.Connect();
        
        await connect.OpenAsync();
        
        await using var cmd = new NpgsqlCommand("SELECT * FROM funcionario", connect);
       
        ListaFuncionario lista=new();

        await using var reader = await cmd.ExecuteReaderAsync();
        while(await reader.ReadAsync())
        {
            FuncionarioDto campos=new();
            campos.Nome=(string)reader["nome"];
            campos.Cpf=(string)reader["cpf"];
            campos.Isadmin=(bool)reader["isadmin"];
            campos.QuantidadeAtestado=(int)reader["quantidade_atestado"];
            campos.Nascimento=(int)reader["nascimento"];
            lista.Funcionarios.Add(campos);
        }
         
        return lista;
    }
    public  async Task<int> AddFuncionario(FuncionarioDto campos)
    {
        int resultado;
        
        await using NpgsqlConnection connect = host.Connect();
        
        await connect.OpenAsync();

        await using (var cmd = new NpgsqlCommand("INSERT INTO funcionario (nome ,cpf, isadmin,quantidade_atestado,nascimento) VALUES (@nome ,@cpf, @isadmin,@quantidade_atestado,@nascimento)", connect))
        {
            cmd.Parameters.AddWithValue("nome", campos.Nome);
            cmd.Parameters.AddWithValue("cpf", campos.Cpf);
            cmd.Parameters.AddWithValue("isadmin", campos.Isadmin);
            cmd.Parameters.AddWithValue("quantidade_atestado", campos.QuantidadeAtestado);
            cmd.Parameters.AddWithValue("nascimento",campos.Nascimento);
            resultado=await cmd.ExecuteNonQueryAsync();
        }
        
        return resultado;
    }  
    
    public  async Task<int> UpdateFuncionario(FuncionarioDto campos,int id)
    {
     
        await using NpgsqlConnection connect=host.Connect();

        await connect.OpenAsync();
        int resultado;
      await  using (var cmd=new NpgsqlCommand("UPDATE  funcionario set nome =@nome,cpf=@cpf,isadmin=@isadmin,quantidade_atestado=@quantidade_atestado,nascimento=@nascimento WHERE id=@id", connect))
        {
            cmd.Parameters.AddWithValue("nome", campos.Nome);
            cmd.Parameters.AddWithValue("cpf", campos.Cpf);
            cmd.Parameters.AddWithValue("isadmin", campos.Isadmin);
            cmd.Parameters.AddWithValue("quantidade_atestado", campos.QuantidadeAtestado);
            cmd.Parameters.AddWithValue("nascimento",campos.Nascimento);
            cmd.Parameters.AddWithValue("id", id);
            resultado=await cmd.ExecuteNonQueryAsync();
        }
         return  resultado;

    }
    public  async Task<int> DeleteFuncionario(int id)
    {
        int resultado;
       
        await using NpgsqlConnection connect=host.Connect();

        await connect.OpenAsync();
     
       await using (var  cmd = new NpgsqlCommand("DELETE FROM funcionario WHERE id = @id ", connect))
        {
            cmd.Parameters.AddWithValue("id",id);
            resultado=await cmd.ExecuteNonQueryAsync();
        }
        
        return resultado ;
        
        
    }
    public  async Task<FuncionarioDto> GetById(int id)
    {
        await using NpgsqlConnection connect=host.Connect(); 
        
        await connect.OpenAsync();

        await using var cmd = new NpgsqlCommand("SELECT * FROM funcionario WHERE id = @id", connect);
        cmd.Parameters.AddWithValue("id", id);
        
        await using var reader = await cmd.ExecuteReaderAsync();
        if (!await reader.ReadAsync()) { return null;}
        
        FuncionarioDto campos=new();
        campos.Nome=(string)reader["nome"];
        campos.Cpf=(string)reader["cpf"];
        campos.Isadmin=(bool)reader["Isadmin"];
        campos.Nascimento=(int)reader["nascimento"];
        campos.QuantidadeAtestado = (int)reader["quantidade_atestado"];
           
        
         
        return campos;
    }


}
}