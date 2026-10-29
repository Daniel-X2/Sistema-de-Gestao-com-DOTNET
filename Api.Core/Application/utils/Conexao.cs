using Npgsql;
using DotNetEnv;



namespace Api.Core.Application.utils
{
  
internal interface IConnect
{
    internal NpgsqlConnection Connect();
}

   class  ConnectHost:IConnect
  {
      
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
          
          return new NpgsqlConnection (EnvConnection());        
      }
  }

public class Load
{
    public static void LoadEnv()
    {
        Env.Load();
    }
}

}









