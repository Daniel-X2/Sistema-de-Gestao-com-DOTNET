
namespace Api
{
    

    
    class Exec
    {
        
        
        public static async Task Main()
        {
            
            WebApplicationBuilder builder = WebApplication.CreateBuilder();
            AddScope scope = new();
            
           
            scope.AddScopeFuncion(builder);
            var app = builder.Build();
            app.UseMiddleware<ExceptionHandlingMiddleware>();
           // builder.Services.AddScoped<ILoggerFactory>();
            await new Routers.Routers().Teste(app);
            app.UseHttpsRedirection();
            
            
           
             app.Run();
            
        }
    }
}

