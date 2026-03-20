
using System.Text;


namespace Utils
{ 
    class Validation
    {
   
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
    public bool ValidateName(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome) || nome.Length<4)
        {
            return false;
        }

        return true;
    }
    
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
