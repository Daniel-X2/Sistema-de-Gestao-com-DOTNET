
namespace Dto
{
    public class ListaClient
    {
        public List<ClientDto> Clients {get;set;}= new List<ClientDto>();
    }
    
    
    
public class ListaFuncionario
{
    public List<FuncionarioDto> Funcionarios {get;set;}= new ();
}
public class ListaProduct
{
     public List<ProdutoDto> Product {get;set;}=new List<ProdutoDto>();
}
public class ClientDto
{
    public int Id { get; set; }
    public string Nome{get;set;}
    public string Empresa { get; set;}
    public string Cpf{get;set;}
    public int Conta{get;set;}
    public bool Isvip{get;set;}
    public int Data { get; set; }
}


public class FuncionarioDto
{
    public int Id { get; set; }
    public string Nome{get;set;}
    public string Senha { get; set; }
    public string Cpf{get;set;}
    public bool Isadmin{get;set;}
    public int QuantidadeAtestado{get;set;}
    public int Nascimento{get;set;}
    public int Data { get; set; }
}

public class FuncionarioLoginDto
{
    public bool isadmin { get; set; }
    public string cpf { get; set; }
    public string SenhaHash { get; set; }
}

public class FuncionarioJsonBody
{
    public string cpf { get; set; }
    public string Senha { get; set; }
    
}
public class ProdutoDto
{
    public int Id { get; set; }
    public string Nome{get;set;}
    public int Codigo{get;set;}
    public int Quantidade{get;set;}
    public decimal ValorRevenda{get;set;}
    public int Lote{get;set;}
}

public class DashboardStatsDto
{
    public long TotalClientes { get; set; }
    public long TotalFuncionarios { get; set; }
    public long TotalProdutos { get; set; }
    public decimal PatrimonioEstoque { get; set; }
    public double TaxaVip { get; set; }
    public List<ClientDto> LatestClients { get; set; } = new();
    public List<NewsItemDto> Activities { get; set; } = new();
}

public class NewsItemDto
{
    public string Source { get; set; }
    public string Title { get; set; }
    public string Time { get; set; }
}
}
