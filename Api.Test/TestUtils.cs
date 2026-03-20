using Api.Test;
using Xunit;

using Utils;

public class TestUtils
{
    private static Validation validation=new ();
    
    [Fact]
    public  void TestCpfValido()
    {
        string cpf = ReturnDados.ReturnCpf();
        cpf= validation.IsValidDigit(cpf);
        Assert.Equal(11,cpf.Length);
        Assert.Equal(cpf,cpf);
        
       
    }
    [Fact]
    public  void TestCpfInvalido()
    {
        string cpf = "7854624550";
        Assert.Throws<InvalidCpfException>(() =>  validation.IsValidDigit(cpf));
        

    }
    
    [Theory]
    [InlineData(2200)]
    [InlineData(1500)]
    public  void TestNascimentoInvalido(int nascimento)
    {
        try
        {
            bool resultado= validation.ValidateBirthYear(nascimento);
            Assert.True(resultado,"o teste falhou");
        }
        catch ( InvalidNascimentoException)
        {
            Assert.True(true,"o teste foi um sucesso");
        }
    }
    [Theory]
    [InlineData(2005)]
    [InlineData(1980)]
    public void TestNascimentoValido(int nascimento)
    {
        try
        {
            bool resultado= validation.ValidateBirthYear(nascimento);
            Assert.True(resultado,"o teste passou");
        }
        catch (InvalidNascimentoException)
        {
            Assert.Fail("o teste falhou");
        }
    }
    
    
}