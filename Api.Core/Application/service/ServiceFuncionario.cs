using Dto;
using Utils;
using Api.Core.Application.repository;
namespace Api.Core.Application.service
{
   public interface IServiceFuncionario
   {
       Task<bool> AddService(FuncionarioDto campos);

       Task<bool> UpdateFuncionarioService(FuncionarioDto campos, int id);
       Task<bool> DeleteFuncionarioService(int id); 
       Task<ListaFuncionario> GetAll();
       Task<FuncionarioDto> GetByIdService(int id);
   }
    public class  ServiceFuncionario(IRepositoryFuncionario repo):IServiceFuncionario
    {
       
       public async Task<ListaFuncionario> GetAll()
       {
           ListaFuncionario lista=new();
           lista =await repo.GetFuncionario();
           if (lista.Funcionarios.Count==0)
           {
               throw new ReturnDataIsEmpty();
           }

           return lista;
       }

        public async Task<bool> AddService(FuncionarioDto campos)
       {
           Validation verificador = new();
           
           verificador.ValidateBirthYear(campos.Nascimento);//aqui retorna bool
           campos.Cpf = verificador.IsValidDigit(campos.Cpf);
           if (!verificador.ValidateName(campos.Nome))
           {
               throw new InvalidNameException(campos.Nome);
           }
           if (await repo.ExistsCpf(campos.Cpf))
           {
               throw new InvalidCpfException(campos.Cpf);
           }

           int resultado = await repo.AddFuncionario(campos);
           if(resultado>=1)
           {
               return true;
           }
           throw new ErroAddToDatabaseException();
       }

        public async Task<bool> UpdateFuncionarioService(FuncionarioDto campos,int id)
        {
            
            Validation verificar = new();
           
            var valores =await  repo.GetById(id);
            if (valores==null)
            {
                throw new InvalidIdException(id);
            }

            try
            {
                campos.Cpf= verificar.IsValidDigit(campos.Cpf);
                if ( await repo.ExistsCpf(campos.Cpf))
                {
                    campos.Cpf = valores.Cpf;
                }
            }
            catch (InvalidCpfException)
            {
                campos.Cpf = valores.Cpf;

            }

            if (!verificar.ValidateName(campos.Nome))
            {
                 campos.Nome = valores.Nome;
            }

            try
            {
                if (!verificar.ValidateBirthYear(campos.Nascimento))
                {
                    campos.Nascimento = valores.Nascimento;
                }
            }
            catch (InvalidNascimentoException)
            {
                campos.Nascimento = valores.Nascimento;
            } 
            campos.Isadmin = valores.Isadmin;
       
            if (await repo.UpdateFuncionario(campos, id)==0)
            {
                throw new ErroUpdateToDatabaseException();
            }
            return true;
        }
    
        public async Task<bool> DeleteFuncionarioService(int id)
        {
            if(await repo.DeleteFuncionario(id)>=1)
            {
                return true;
            }

            throw new InvalidIdException(id);
        }
        public async Task<FuncionarioDto> GetByIdService(int id)
        {
            
                FuncionarioDto resultado= await repo.GetById(id);
                if (resultado==null)
                {
                    throw new InvalidIdException(id);
                }
                return resultado;
            
        }
        
    }
}

