
using Api.Core.Application.repository;
using Api.Core.Application.service;
using Dto;

using Moq;
using Xunit;

namespace Api.Test;

/// <summary>
/// Testes unitários para a camada de serviço de clientes.
/// </summary>
public class TestServiceClient
{
    Mock<IRepositoryClient> moq = new();

    /// <summary>
    /// Verifica se o serviço adiciona corretamente um cliente com dados válidos.
    /// </summary>
    [Fact]
    public async Task TestAddClientValido()
    {
        var client = ReturnDados.ReturnCLient();
     
        moq.Setup(repo=> repo.AddClient(client) ).ReturnsAsync(1);
        var n1 = new ServiceClient(moq.Object);
        
        await n1.AddService(client);
        
    }

    [Fact]
    public async Task TestGetClientInvalido()
    {
        var campos = new ListaClient();
        moq.Setup(repo => repo.GetAllClient(1, 10)).ReturnsAsync(campos);
        var n1 = new ServiceClient(moq.Object);
        await Assert.ThrowsAsync<ReturnDataIsEmpty>(async () => await n1.GetAllService(1, 10));
    }
    [Fact]
    public async Task TestGetClientvalido()
    {
        var campos = new ListaClient();
        campos.Clients.Add(ReturnDados.ReturnCLient());
        moq.Setup(repo => repo.GetAllClient(1, 10)).ReturnsAsync(campos);
        var n1 = new ServiceClient(moq.Object);
        await n1.GetAllService(1, 10);
        
    }
    [Fact]
    public async Task TestAddClientInvalidoCpf()
    {
        var client = ReturnDados.ReturnCLient() ;
        client.Cpf = "47854664505";
        moq.Setup(repo=> repo.AddClient(client) ).ReturnsAsync(1);
        var n1 = new ServiceClient(moq.Object);
        
        
        await Assert.ThrowsAsync<InvalidCpfException>(async ( )=> await n1.AddService(client));
        
    }
    [Fact]
    public async Task TestAddClientInvalidoConta()
    {
        var client = ReturnDados.ReturnCLient() ;
        client.Conta = 0;
        
        moq.Setup(repo=> repo.AddClient(client) ).ReturnsAsync(1);
        var n1 = new ServiceClient(moq.Object);
        
        
        await Assert.ThrowsAsync<InvalidAccountException>(async ( )=> await n1.AddService(client));
        
    }
    [Fact]
    public async Task TestAddClientInvalidoNome()
    {
        var client =ReturnDados.ReturnCLient() ;
        client.Nome = "dan";
        moq.Setup(repo=> repo.AddClient(client) ).ReturnsAsync(1);
        var n1 = new ServiceClient(moq.Object);
        
        await Assert.ThrowsAsync<InvalidNameException>(async ( )=> await n1.AddService(client));
        
    }
    [Fact]
    public async Task TestAddClientInvalidoRepo()
    {
        var client =ReturnDados.ReturnCLient() ;
       
        moq.Setup(repo=> repo.AddClient(client) ).ReturnsAsync(0);
        var n1 = new ServiceClient(moq.Object);
        
        await Assert.ThrowsAsync<ErroAddToDatabaseException>(async ( )=> await n1.AddService(client));
        
    }
    [Fact]
    public async Task TestUpdateClientValido()
    {
        var client =  ReturnDados.ReturnCLient() ;
        //eu pego outro que ele pega pelo id e faço o retorno ser igual zero
        moq.Setup(repo => repo.GetById(5)).ReturnsAsync(client);
        moq.Setup(repo=> repo.UpdateClient(client,5) ).ReturnsAsync(1);
        var n1 = new ServiceClient(moq.Object);
        
        await n1.UpdateService(5,client);
        Assert.True(true,"o teste passou");
    }
    [Theory]
    [InlineData(-1)]
    public async Task TestDeleteClientInValido(int id)
    {
        moq.Setup(repo => repo.DeleteClient(id)).ReturnsAsync(0);
        var n1 = new ServiceClient(moq.Object);
        
        await Assert.ThrowsAsync<InvalidIdException>(async () => await n1.DeleteService(id));

    }

    public async Task TestUpdateInvalidoRepo(int id)
    {
        var client = ReturnDados.ReturnCLient();
        
        moq.Setup(repo => repo.GetById(5)).ReturnsAsync(client);
        moq.Setup(repo => repo.UpdateClient(client, 5)).ReturnsAsync(0);
        var n1 = new ServiceClient(moq.Object);

        
        await Assert.ThrowsAsync<ErroUpdateToDatabaseException>(async () => await n1.UpdateService(5, client));
    }

    [Theory]
    [InlineData(5)]
    public async Task TestDeleteClientValido(int id)
    {
        moq.Setup(repo => repo.DeleteClient(id)).ReturnsAsync(1);
        var n1 = new ServiceClient(moq.Object);
        bool resultado =await n1.DeleteService(id);
        Assert.True(resultado);
    }
    
}