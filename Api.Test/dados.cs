
using Bogus;
using Bogus.Extensions.Brazil;
using Dto;
namespace Api.Test;

public class ReturnDados
{
    
    public static  ClientDto ReturnCLient()
    {
        
        var n1 = new Faker("pt_BR");
        ClientDto client = new ClientDto();
        //Console.WriteLine(n1.Person.Cpf());
        client.Cpf= n1.Person.Cpf();
        client.Conta = n1.Random.Int(min:10,max:2000);
        client.Isvip = n1.Random.Bool();
        client.Nome = n1.Person.FullName;
        client.Empresa = n1.Company.CompanyName();
        client.Data = n1.Random.Int(min:2000, max:2026);
        
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
        product.ValorRevenda = n1.Finance.Random.Decimal();
        return product;
    }

    public static FuncionarioDto ReturnFuncionario()
    {
        var n1 = new Faker("pt_BR");
        FuncionarioDto funcionario = new();
        funcionario.Nome = n1.Name.FullName();
        funcionario.Cpf = n1.Person.Cpf();
        funcionario.Isadmin = n1.Random.Bool();
        funcionario.QuantidadeAtestado = n1.Random.Int(min: 0, max: 50);
        funcionario.Nascimento = n1.Person.DateOfBirth.Year;
       
        
       
        return funcionario;
    }
    public static String ReturnCpf()
    {
        var n1 = new Faker("pt_BR");
        return n1.Person.Cpf();
    }
    public static int ReturnBirthYear()
    {
        var n1 = new Faker("pt_BR");
        return n1.Person.DateOfBirth.Year;
    }
}