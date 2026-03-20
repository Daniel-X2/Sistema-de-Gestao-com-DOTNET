using Xunit;
using Bogus;
using Bogus.Extensions.Brazil;
using Dto;
namespace Api.Test;

public class ReturnDados
{
    
    public static async Task<ClientDto> ReturnCLient()
    {
        var n1 = new Faker("pt_BR");
        Dto.ClientDto client = new Dto.ClientDto();
        //Console.WriteLine(n1.Person.Cpf());
        client.Cpf= n1.Person.Cpf();
        client.Conta = n1.Random.Int(min:10,max:2000);
        client.Isvip = n1.Random.Bool();
        client.Nome = n1.Person.FullName;
        Task.Delay(1000);
        return client;
    }

    public static ProdutoDto ReturnProduct()
    {
        
        var n1 = new Faker("pt_BR");
        ProdutoDto product = new();
        product.Nome = n1.Commerce.ProductName();
        product.Codigo = n1.Random.Int(min: 50, max: 2000);
        product.Lote = n1.Random.Int(min: 20, max: 2000);
        product.Quantidade = n1.Random.Int(min: 20, max: 2000);
        product.ValorRevenda = n1.Finance.Random.Float();
        return product;
    }

    public static String ReturnCpf()
    {
        var n1 = new Faker("pt_BR");
        return n1.Person.Cpf();
    }
}