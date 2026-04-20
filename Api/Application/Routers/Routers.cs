namespace Api.Routers;

public class Routers
{
    public async  Task InitRouters(WebApplication app)
    {
       await new ClientRouter().Routers(app);
       await new FuncionarioRouters().Router(app);
       await new ProductRouters().Routers(app);
       await new DashboardRouter().Routers(app);
    }
}