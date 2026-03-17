using Xunit;
using Api.Core.Application.utils;
using Utils;

public class TestUtils
{
    private static Validation validation=new ();
    
    [Theory]
    [InlineData("665.940.427-93")]
    [InlineData("348.026.691-60")]
    public  void TestCpfValido(string cpf)
    {
        //Validation _validation = new();
        try
        {
            cpf= validation.IsValidDigit(cpf);
            Assert.Equal(11,cpf.Length);
            Assert.Equal(cpf,cpf);
        }
        catch (InvalidCpfException)
        {
            Assert.Fail("O IsvalidDigit nao passou no test");
        }
       
    }
    [Theory]
    [InlineData("248.526.791-60")]
    [InlineData("123456789")]
    public  void TestCpfInvalido(string cpf)
    {
       // Validation validation = new();
        
        try
        {
            cpf = validation.IsValidDigit(cpf);
            Assert.Fail("O IsvalidDigit nao passou no test");
        }
        catch (InvalidCpfException)
        {
            Assert.True(true,"o IsvalidDigit passou no test");
        }
        
    }
    
    [Theory]
    [InlineData(2200)]
    [InlineData(1500)]
    public  void TestInvalidNascimento(int nascimento)
    {
        try
        {
            bool resultado= validation.IsValidNascimento(nascimento);
            Assert.True(resultado,"o teste falhou");
        }
        catch ( InvalidNascimentoException)
        {
            Assert.False(false,"o teste foi um sucesso");
        }
    }
    
    
    
}