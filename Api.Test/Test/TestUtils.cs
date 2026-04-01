using Api.Test;
using Xunit;

using Utils;

/// <summary>
/// Conjunto de testes para as utilidades de validação (CPF, Datas, Nomes).
/// </summary>
public class TestUtils
{
    private static Validation validation=new ();
    
    /// <summary>
    /// Verifica se o validador aceita um CPF matematicamente correto.
    /// </summary>
    [Fact]
    public  void TestCpfValido()
    {
        string cpf = ReturnDados.ReturnCpf();
     
        cpf= validation.IsValidDigit(cpf);
        Assert.Equal(11,cpf.Length);
        Assert.Equal(cpf,cpf);
        
       
    }
    /// <summary>
    /// Verifica se o validador rejeita CPFs com dígitos incorretos.
    /// </summary>
    [Fact]
    public  void TestCpfInvalido()
    {
        string cpf = "78546245501";
        Assert.Throws<InvalidCpfException>(() =>  validation.IsValidDigit(cpf));
        

    }
    
    /// <summary>
    /// Testa se datas de nascimento futuras ou fora da faixa permitida são bloqueadas.
    /// </summary>
    [Theory]
    [InlineData(2200)]
    [InlineData(1500)]
    public  void TestNascimentoInvalido(int nascimento)
    {
        Assert.Throws<InvalidNascimentoException>(() => validation.ValidateBirthYear(nascimento));
        
    }
    /// <summary>
    /// Testa se anos de nascimento válidos são aceitos.
    /// </summary>
    [Fact]
    public void TestNascimentoValido()
    {
           bool resultado= validation.ValidateBirthYear(ReturnDados.ReturnBirthYear());
           Assert.True(resultado);
    }
    
    
}