using Api.Core.Application.repository;
using Api.Core.Application.service;
using Api.Core.Application.utils;


public class AddScope
{
    public void AddScopeFuncion(WebApplicationBuilder builder)
    {
        
        builder.Services.AddScoped<IConnect, ConnectHost>();
        builder.Services.AddScoped<IRepositoryFuncionario,RepositoryFuncionario>();
        builder.Services.AddScoped<IServiceFuncionario,ServiceFuncionario>();
        
        
        builder.Services.AddScoped<IRepositoryClient,RepositoryClient>();
        builder.Services.AddScoped<IServiceClient, ServiceClient>();
        
        builder.Services.AddScoped<IServiceProduct, ServiceProduct>();
        builder.Services.AddScoped<IRepositoryProduct, RepositoryProduct>();

        builder.Services.AddScoped<IDashboardService, DashboardService>();
    }
}
