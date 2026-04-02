using Dto;
using Xunit;
using Api.Core.Application.repository;
using Api.Core.Application.service;
using Moq;
namespace Api.Test;

public class TestServiceProduct
{
    Mock<IRepositoryProduct> moq = new();

    [Fact]
    public async Task TestGetProductValido()
    {
        
        var product = new ListaProduct();
        product.Product.Add(ReturnDados.ReturnProduct());
        moq.Setup(repo => repo.GetAllProduct(TODO, TODO)).ReturnsAsync(product);
        
        var n1 =new  ServiceProduct(moq.Object);
        await n1.GetAllProduct(TODO, TODO);
    }
    [Fact]
    public async Task TestGetProductInvalido()
    {
        
        var product = new ListaProduct();
       
        moq.Setup(repo => repo.GetAllProduct(TODO, TODO)).ReturnsAsync(product);
        
        var n1 =new  ServiceProduct(moq.Object);
        await  Assert.ThrowsAsync<ReturnDataIsEmpty>(async () => await n1.GetAllProduct(TODO, TODO));
        
    }
   [Fact]
    public async Task TestAddProduct()
    {
        var product = ReturnDados.ReturnProduct();
        moq.Setup(repo => repo.AddProduct(product)).ReturnsAsync(1);
        
        var n1 =new  ServiceProduct(moq.Object);
        await  n1.AddProduct(product);
    }
    [Fact]
    public async Task TestAddInvalidCode()
    {
        var product = ReturnDados.ReturnProduct();
        product.Codigo = 0;
        moq.Setup(repo => repo.AddProduct(product)).ReturnsAsync(1);
        var n1 =new  ServiceProduct(moq.Object);

        await Assert.ThrowsAsync<InvalidCodeException>(async () => await n1.AddProduct(product));
    }
    [Fact]
    public async Task TestAddInvalidlote()
    {
        var product = ReturnDados.ReturnProduct();
        product.Lote = 0;
        moq.Setup(repo => repo.AddProduct(product)).ReturnsAsync(1);
        var n1 =new  ServiceProduct(moq.Object);

        await Assert.ThrowsAsync<InvalidLoteException>(async () => await n1.AddProduct(product));
    }
    [Fact]
    public async Task TestAddInvalidValor()
    {
        var product = ReturnDados.ReturnProduct();
        product.ValorRevenda = 0;
        
        moq.Setup(repo => repo.AddProduct(product)).ReturnsAsync(1);
        var n1 =new  ServiceProduct(moq.Object);

        await Assert.ThrowsAsync<NegativeNumericException>(async () => await n1.AddProduct(product));
        
    }
    [Fact]
    public async Task TestAddInvalidQuantidade()
    {
        var product = ReturnDados.ReturnProduct();
        product.Quantidade = 0;
        moq.Setup(repo => repo.AddProduct(product)).ReturnsAsync(1);
        var n1 =new  ServiceProduct(moq.Object);

        await Assert.ThrowsAsync<NegativeNumericException>(async () => await n1.AddProduct(product));
        
    }
    
    [Fact]
    public async Task TestUpdateProduct()
    {
        int id = 2;
        var product = ReturnDados.ReturnProduct();
        moq.Setup(repo => repo.GetProductById(id)).ReturnsAsync(product);
        moq.Setup(repo => repo.UpdateProduct(product,id)).ReturnsAsync(1);
        var n1 =new  ServiceProduct(moq.Object);

        Assert.True(await n1.UpdateProduct(product,id));
    }
     
    
}
