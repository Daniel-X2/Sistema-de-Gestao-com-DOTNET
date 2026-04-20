using Api.Core.Application.service;
using Dto;

namespace Api.Routers
{
    public class DashboardRouter
    {
        public async Task Routers(WebApplication app)
        {
            app.MapGet("/dashboard/stats", async (IDashboardService service) =>
            {
                DashboardStatsDto stats = await service.GetStats();
                return Results.Ok(stats);
            })
            .WithTags("Dashboard")
            .WithSummary("Obtém as estatísticas gerais para o dashboard")
            .RequireAuthorization();
        }
    }
}
