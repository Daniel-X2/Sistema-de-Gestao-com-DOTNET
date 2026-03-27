

using Api.Core.Application.utils;


namespace Api

{
    class Program
    {
        
        
        public static async Task Main()
        {
            
            WebApplicationBuilder builder = WebApplication.CreateBuilder();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            AddScope scope = new();
            Load.LoadEnv();
            
            scope.AddScopeFuncion(builder);
           
            var app = builder.Build();
            
            
            app.MapSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Minha API v1");
                c.RoutePrefix = string.Empty; // Isso faz o swagger abrir direto na raiz (http://localhost:5152)
            });
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            
            await new Routers.Routers().Teste(app);
            app.UseHttpsRedirection();
            
            app.Run();
        }
    }
}

