using System.Runtime.CompilerServices;
using Api.Core.Application.repository;
using Api.Core.Application.service;
using Dto;
using Microsoft.Win32.SafeHandles;
using Moq;
using Xunit;

namespace Api.Test;

public class TestServiceClient
{
    Mock<IRepositoryClient> moq = new();
    private ClientDto campos = new();
    
    [Theory]
    [InlineData("Daniel","156585658",875,true)]
    public async Task TestAddClientValido(string nome,string cpf,int conta, bool isvip)
    {
        campos.Nome = nome;
        campos.Conta = conta;
        campos.Cpf = cpf;
        campos.Isvip = isvip;
        
        moq.Setup(repo=> repo.AddClient(campos) ).ReturnsAsync(1);
        var n1 = new ClientService(moq.Object);

        await n1.AddService(campos);
        
    }
}