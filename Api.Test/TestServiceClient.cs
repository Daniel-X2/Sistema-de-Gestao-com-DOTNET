
using Api.Core.Application.repository;
using Api.Core.Application.service;
using Dto;

using Moq;
using Xunit;

namespace Api.Test;

public class TestServiceClient
{
    Mock<IRepositoryClient> moq = new();

    [Fact]
    public async Task TestAddClientValido()
    {
        var client =await ReturnDados.ReturnCLient();
     
        moq.Setup(repo=> repo.AddClient(client) ).ReturnsAsync(1);
        var n1 = new ClientService(moq.Object);
        
        await n1.AddService(client);
        
    }
    [Fact]
    public async Task TestAddClientInvalidoCpf()
    {
        var client =await ReturnDados.ReturnCLient() ;
        client.Cpf = "47854664505";
        moq.Setup(repo=> repo.AddClient(client) ).ReturnsAsync(1);
        var n1 = new ClientService(moq.Object);
        
        
        await Assert.ThrowsAsync<InvalidCpfException>(async ( )=> await n1.AddService(client));
        
    }
    [Fact]
    public async Task TestAddClientInvalidoConta()
    {
        var client =await ReturnDados.ReturnCLient() ;
        client.Conta = 0;
        
        moq.Setup(repo=> repo.AddClient(client) ).ReturnsAsync(1);
        var n1 = new ClientService(moq.Object);
        
        
        await Assert.ThrowsAsync<InvalidAccountException>(async ( )=> await n1.AddService(client));
        
    }
    [Fact]
    public async Task TestAddClientInvalidoNome()
    {
        var client =await ReturnDados.ReturnCLient() ;
        client.Nome = "dan";
        moq.Setup(repo=> repo.AddClient(client) ).ReturnsAsync(1);
        var n1 = new ClientService(moq.Object);
        
        await Assert.ThrowsAsync<InvalidNameException>(async ( )=> await n1.AddService(client));
        
    }
    [Fact]
    public async Task TestUpdateClientValido()
    {
        var client = await ReturnDados.ReturnCLient() ;
        //eu pego outro que ele pega pelo id e faço o retorno ser igual zero
        moq.Setup(repo => repo.GetById(5)).ReturnsAsync(client);
        moq.Setup(repo=> repo.UpdateClient(client,5) ).ReturnsAsync(1);
        var n1 = new ClientService(moq.Object);
        
        await n1.UpdateService(5,client);
        Assert.True(true,"o teste passou");
    }
    [Theory]
    [InlineData(-1)]
    public async Task TestDeleteClientInValido(int id)
    {
        moq.Setup(repo => repo.DeleteClient(id)).ReturnsAsync(0);
        var n1 = new ClientService(moq.Object);
        
        await Assert.ThrowsAsync<InvalidIdException>(async () => await n1.DeleteService(id));

    }

    [Theory]
    [InlineData(5)]
    public async Task TestDeleteClientValido(int id)
    {
        moq.Setup(repo => repo.DeleteClient(id)).ReturnsAsync(1);
        var n1 = new ClientService(moq.Object);
        bool resultado =await n1.DeleteService(id);
        Assert.True(resultado);
    }
    
}