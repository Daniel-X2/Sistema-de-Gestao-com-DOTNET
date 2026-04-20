using Dto;

namespace Api.Core.Application.service
{
    public interface IDashboardService
    {
        Task<DashboardStatsDto> GetStats();
    }

    public class DashboardService(
        IServiceClient clientService,
        IServiceFuncionario funcionarioService,
        IServiceProduct productService) : IDashboardService
    {
        public async Task<DashboardStatsDto> GetStats()
        {
            var stats = new DashboardStatsDto();

            // Total Clientes
            stats.TotalClientes = await clientService.QuantidadeClient();

            // Total Funcionários
            stats.TotalFuncionarios = await funcionarioService.QuantidadeFuncionario();

            // Total Produtos
            stats.TotalProdutos = await productService.QuantidadeProduct();

            // Patrimônio em Estoque & Taxa VIP (Cálculos simples baseados nos dados existentes)
            var allProducts = await productService.GetAllProduct(1000, 1);
            stats.PatrimonioEstoque = allProducts.Product.Sum(p => p.ValorRevenda * p.Quantidade);

            var allClients = await clientService.GetAllService(1, 1000);
            int totalClients = allClients.Clients.Count;
            int vips = allClients.Clients.Count(c => c.Isvip);
            stats.TaxaVip = totalClients > 0 ? (double)vips / totalClients * 100 : 0;

            // Latest Clients
            stats.LatestClients = allClients.Clients.OrderByDescending(c => c.Id).Take(5).ToList();

            // Mock Activities (using recent data)
            stats.Activities = new List<NewsItemDto>
            {
                new NewsItemDto { Source = "Sistema", Title = $"Dashboard atualizado com {stats.TotalClientes} clientes.", Time = DateTime.Now.ToString("HH:mm") },
                new NewsItemDto { Source = "Estoque", Title = $"Patrimônio total calculado: {stats.PatrimonioEstoque:C}", Time = DateTime.Now.AddMinutes(-10).ToString("HH:mm") }
            };

            return stats;
        }
    }
}
