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
       Task<ListaFuncionario> GetAll(int limit, int page);
       Task<FuncionarioDto> GetByIdService(int id);
       Task<FuncionarioLoginDto> Admin(string cpf);
       Task<long> QuantidadeFuncionario();
   }
    /// <summary>
    /// Gerencia a lógica de negócio referente aos funcionários (autenticação e CRUD).
    /// </summary>
    public class  ServiceFuncionario(IRepositoryFuncionario repo):IServiceFuncionario
    {
        /// <summary>
        /// Retorna todos os funcionários cadastrados.
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <returns>ListaFuncionario com todos os registros.</returns>
        public async Task<ListaFuncionario> GetAll(int limit, int page)
       {
           ListaFuncionario lista=new();
           lista =await repo.GetFuncionario(limit,page);
           if (lista.Funcionarios.Count==0)
           {
               throw new ReturnDataIsEmpty();
           }

           return lista;
       }

        /// <summary>
        /// Busca as credenciais de um funcionário pelo CPF para validação de login.
        /// </summary>
        /// <param name="cpf">CPF do funcionário.</param>
        /// <returns>Dados para login (hash da senha e status de admin).</returns>
        public async Task<FuncionarioLoginDto> Admin(string cpf)
        {
            Validation valid = new Validation();
            FuncionarioLoginDto funcionario=await  repo.GetAdmin(valid.IsValidDigit(cpf));
            if (funcionario==null)
            {
                throw new InvalidCpfException(cpf);
            }

            return funcionario;
        }
        /// <summary>
        /// Cadastra um novo funcionário após validar CPF, nome e ano de nascimento.
        /// </summary>
        /// <param name="campos">DTO com dados do funcionário.</param>
        /// <returns>True se cadastrado.</returns>
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

        /// <summary>
        /// Atualiza os dados de um funcionário, mantendo valores originais se as novas entradas forem inválidas.
        /// </summary>
        /// <param name="campos">DTO com novos dados.</param>
        /// <param name="id">ID do funcionário.</param>
        /// <returns>True se atualizado.</returns>
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
    
        /// <summary>
        /// Exclui um funcionário pelo ID.
        /// </summary>
        /// <param name="id">ID do funcionário.</param>
        /// <returns>True se excluído.</returns>
        public async Task<bool> DeleteFuncionarioService(int id)
        {
            if(await repo.DeleteFuncionario(id)>=1)
            {
                return true;
            }

            throw new InvalidIdException(id);
        }
        /// <summary>
        /// Obtém os detalhes de um funcionário pelo ID.
        /// </summary>
        /// <param name="id">ID do funcionário.</param>
        /// <returns>DTO com dados do funcionário.</returns>
        public async Task<FuncionarioDto> GetByIdService(int id)
        {
            
                FuncionarioDto resultado= await repo.GetById(id);
                if (resultado==null)
                {
                    throw new InvalidIdException(id);
                }
                return resultado;
            
        }
        
        public async Task<long> QuantidadeFuncionario()
        {
            return await repo.QuantidadeFuncionario();
        }
    }
}

