
using System.Text;


namespace Utils
{ 
    /// <summary>
    /// Fornece métodos para validação de dados sensíveis como CPF, nomes e datas.
    /// </summary>
    class Validation
    {
   
    /// <summary>
    /// Valida se um CPF é autêntico através do cálculo dos dígitos verificadores.
    /// </summary>
    /// <param name="cpf">String do CPF (com ou sem pontuação).</param>
    /// <returns>O CPF limpo (apenas números) se for válido.</returns>
    /// <exception cref="InvalidCpfException">Lançada se o CPF for nulo, incompleto ou tiver dígitos verificadores incorretos.</exception>
    public string IsValidDigit(string cpf)
    {
        
        if (string.IsNullOrWhiteSpace(cpf))
        {
            throw new InvalidCpfException(cpf);
        }

        StringBuilder _cpf = new StringBuilder(string.Empty,12);
        
            
        for (int c=0;c<cpf.Length;c++ )
        {
            if (char.IsNumber(cpf[c]))
            {
                _cpf.Append(cpf[c]);
            }
        }
        cpf = _cpf.ToString();
        _cpf.Clear();
        
        if (cpf.Length != 11)
        {
            throw new InvalidCpfException(cpf);
        }
        int.TryParse(cpf[9].ToString(),out int digito1);
        int.TryParse(cpf[10].ToString(),out int digito2);
            
        cpf=cpf.Substring(0,9);
        int resultado1;
        if (digito1 == 0)
        {
            resultado1 = digito1;
        }
        else
        {
            resultado1 = 0+CpfEtapa1(cpf);
        }
      
            
        int resultado2 = CpfEtapa2(cpf, resultado1);
        if (resultado1 != digito1 || resultado2 != digito2)
        {
            throw new InvalidCpfException(cpf);
        }

        _cpf.Append(cpf);
        _cpf.Append(digito1.ToString()+digito2.ToString());
        
        return _cpf.ToString();
        
        
    }
    /// <summary>
    /// Valida se um nome é válido (não nulo e com comprimento mínimo).
    /// </summary>
    /// <param name="nome">Nome a verificar.</param>
    /// <returns>True se for válido.</returns>
    public bool ValidateName(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome) || nome.Length<4)
        {
            return false;
        }

        return true;
    }
    
    /// <summary>
    /// Executa a primeira etapa do cálculo do dígito verificador do CPF.
    /// </summary>
    /// <param name="cpf">Os 9 primeiros dígitos do CPF.</param>
    /// <returns>O primeiro dígito verificador calculado.</returns>
    public int CpfEtapa1(string cpf)
    {
        if (cpf.Length == 9)
        {
            int contador = 10;
            int soma=0;
            
            foreach (char c in cpf )
            {
                if (char.IsNumber(c))
                {
                    int.TryParse(c.ToString(), out int saida);
                    soma = (saida * contador)+soma;
                    
                    contador--;
                    if (contador == 1) 
                    {
                        soma = soma % 11;
                        soma = soma - 11;
                        break;
                    }
                }
            }
            
            return 0-soma;
        }

        return 0;
    }

    /// <summary>
    /// Executa a segunda etapa do cálculo do dígito verificador do CPF.
    /// </summary>
    /// <param name="cpf">Os 9 primeiros dígitos do CPF.</param>
    /// <param name="digito">O primeiro dígito verificador já calculado.</param>
    /// <returns>O segundo dígito verificador calculado.</returns>
    public int CpfEtapa2(string cpf,int digito)
    {
        if (cpf.Length == 9)
        {
            int soma = 0;
            int contador = 11;
            string cpfCompleto = cpf + digito; 

            foreach (char c in cpfCompleto)
            {
                if (char.IsNumber(c))
                {
                    
                    int valor = c - '0'; 
                    soma += valor * contador;
                    contador--;
                }
            }

            int resto = soma % 11;
            return (resto < 2) ? 0 : 11 - resto;
        }
        
        return 00;
    }

    /// <summary>
    /// Valida o ano de nascimento para garantir uma idade entre 18 e 85 anos.
    /// </summary>
    /// <param name="ano">Ano de nascimento.</param>
    /// <returns>True se a idade estiver na faixa permitida.</returns>
    /// <exception cref="InvalidNascimentoException">Lançada se o ano for futuro, negativo ou fora da faixa de idade.</exception>
    public bool ValidateBirthYear(int ano)
    {
        int anoAtual = DateTime.Now.Year;
        int idadeMaxima = 85;
        int idadeMinima = 18;
        
        if (ano > anoAtual || int.IsNegative(ano))
        {
            throw new InvalidNascimentoException(ano);
        }
        if (anoAtual - ano > idadeMaxima || anoAtual - ano <idadeMinima )
        {
            throw new InvalidNascimentoException(ano);
        }

        return true;
    }
    
    }
}
