using Npgsql;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Dto;
using Microsoft.Extensions.Options;
using models;
namespace Api.Core.Application.utils
{
  
internal interface IConnect
{
    internal NpgsqlConnection Connect();
    
}

   class  ConnectHost(DbContextOptions<ConnectHost> options) : DbContext(options)
  {
      public DbSet<cliente> cliente{get;set;}
      public DbSet<Funcionario> funcionario{get;set;}
      public DbSet<Produto> produto{get;set;}
      
      public NpgsqlConnection Connect()
      {
          //return new NpgsqlConnection (EnvConnection()); 
        return new NpgsqlConnection();
      }
      
      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
          modelBuilder.Entity<cliente>().ToTable("cliente");
          modelBuilder.Entity<Funcionario>().ToTable("funcionario");
          modelBuilder.Entity<Produto>().ToTable("produto");
      }
  }

public class Load
{
    public static void LoadEnv()
    {
        Env.Load();
    }
    public static string EnvConnection()
    {
        string  connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION");
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidConnection();
        }
        return connectionString;
    }
    public NpgsqlConnection Connect()
    {
        return new NpgsqlConnection();
    }
    
}

}









