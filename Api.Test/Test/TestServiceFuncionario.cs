
using Api.Core.Application.repository;
using Api.Core.Application.service;
using Dto;
using Moq;
using Xunit;

namespace Api.Test;

public class TestServiceFuncionario
{
    Mock<IRepositoryFuncionario> moq = new();
    [Fact]
    public async Task TestGetFuncionarioInvalido()
    {
        var campos = new ListaFuncionario();
        moq.Setup(repo => repo.GetFuncionario(TODO, TODO)).ReturnsAsync(campos);
        var n1 = new ServiceFuncionario(moq.Object);
        await Assert.ThrowsAsync<ReturnDataIsEmpty>(async () => await n1.GetAll(TODO, TODO));
    }
    [Fact]
    public async Task TestGetClientvalido()
    {
        var campos = new ListaFuncionario();
        campos.Funcionarios.Add(ReturnDados.ReturnFuncionario());
        moq.Setup(repo => repo.GetFuncionario(TODO, TODO)).ReturnsAsync(campos);
        var n1 = new ServiceFuncionario(moq.Object);
        await n1.GetAll(TODO, TODO);
        
    }
    [Fact]
    public async Task TestAddFuncionario()
    {
        var funcionario = ReturnDados.ReturnFuncionario();
     
        moq.Setup(repo=> repo.AddFuncionario(funcionario) ).ReturnsAsync(1);
        var n1 = new ServiceFuncionario(moq.Object);
        
        await n1.AddService(funcionario);
    }
    [Fact]
    public async Task TestAddFuncionarioCpfInvalido()
    {
        var funcionario = ReturnDados.ReturnFuncionario();
        funcionario.Cpf = "12345678901";
        moq.Setup(repo=> repo.AddFuncionario(funcionario) ).ReturnsAsync(1);
        var n1 = new ServiceFuncionario(moq.Object);
        
        await Assert.ThrowsAsync<InvalidCpfException>(async ()=> await n1.AddService(funcionario));
    }
    [Fact]
    public async Task TestAddFuncionarioInvalidoNascimento()
    {
        var funcionario = ReturnDados.ReturnFuncionario();
        funcionario.Nascimento = 1900; 
        moq.Setup(repo=> repo.AddFuncionario(funcionario) ).ReturnsAsync(1);
        var n1 = new ServiceFuncionario(moq.Object);
        await Assert.ThrowsAsync<InvalidNascimentoException>(async () => await n1.AddService(funcionario));
        
    }
    [Fact]
    public async Task TestAddFuncionarioInvalidName()
    {
        var funcionario = ReturnDados.ReturnFuncionario();
        funcionario.Nome="dan"; 
        moq.Setup(repo=> repo.AddFuncionario(funcionario) ).ReturnsAsync(1);
        var n1 = new ServiceFuncionario(moq.Object);
        await Assert.ThrowsAsync<InvalidNameException>(async () => await n1.AddService(funcionario));
    }
    [Fact]
    public async Task TestUpdateClientValido()
    {
        var funcionario =  ReturnDados.ReturnFuncionario() ;
       
        moq.Setup(repo => repo.GetById(5)).ReturnsAsync(funcionario);
        moq.Setup(repo=> repo.UpdateFuncionario(funcionario,5) ).ReturnsAsync(1);
        var n1 = new ServiceFuncionario(moq.Object);
        
        await n1.UpdateFuncionarioService(funcionario,5);
        
    }
}