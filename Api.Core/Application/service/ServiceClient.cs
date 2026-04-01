using System.Text;
using Utils;
using Dto;
using Api.Core.Application.repository;

namespace Api.Core.Application.service
{
    public interface IServiceClient
    {
        Task<ListaClient> GetAllService();

        Task<StringBuilder> UpdateService(int id, ClientDto campos);
        Task<bool> AddService(ClientDto campos);
        Task<bool> DeleteService(int id);
    }
    /// <summary>
    /// Implementação da lógica de negócio para gestão de clientes.
    /// </summary>
    class ServiceClient(IRepositoryClient repo):IServiceClient
{
    /// <summary>
    /// Recupera os dados de um cliente pelo seu ID.
    /// </summary>
    /// <param name="id">O ID do cliente no banco.</param>
    /// <returns>Retorna um DTO com os dados do cliente.</returns>
    public async Task<ClientDto> GetByIdService(int id)
    {
        //falta adicionar esse aqui na rota
            ClientDto resultado = await repo.GetById(id);
            if (resultado==null)
            {
                throw new InvalidIdException(id);
            }
            return resultado;
 
    }
    /// <summary>
    /// Obtém uma lista contendo todos os clientes cadastrados.
    /// </summary>
    /// <returns>Objeto ListaClient com a lista de clientes.</returns>
    public async Task<ListaClient> GetAllService()//
    {
    
        
        ListaClient valores= await repo.GetAllClient();
       
        if(valores.Clients.Count>=1)
        {
            return valores;
        }

        throw new ReturnDataIsEmpty();

    }
    /// <summary>
    /// Valida os dados e adiciona um novo cliente ao sistema.
    /// </summary>
    /// <param name="campos">DTO contendo os campos do novo cliente.</param>
    /// <returns>True se adicionado com sucesso.</returns>
    public async Task<bool> AddService(ClientDto campos)//
    {
       
        Validation verificador = new();

        campos.Cpf = verificador.IsValidDigit(campos.Cpf);
        await IsValidAccount(campos.Conta);
        if (await repo.ExistsCpf(campos.Cpf))
        {
            throw new InvalidCpfException(campos.Cpf);
        }
        if (!verificador.ValidateName(campos.Nome))
        {
            throw new InvalidNameException();
        }
        int resultant= await repo.AddClient(campos);
        if (resultant ==0)
        {
             throw new ErroAddToDatabaseException("AddService");
        }
        return true;
    }

    /// <summary>
    /// Atualiza as informações de um cliente existente.
    /// </summary>
    /// <param name="id">ID do cliente a ser atualizado.</param>
    /// <param name="campos">Novos dados do cliente.</param>
    /// <returns>StringBuilder contendo os campos que NÃO foram atualizados por falha na validação.</returns>
    public async Task<StringBuilder> UpdateService(int id,
        ClientDto campos)
    {
        StringBuilder camposNotUpdate=new ();
        Validation verificar = new();
        var valores =await  repo.GetById(id);
        
        if (valores==null)
        {
            throw new InvalidIdException(id);
        }

        try
        {
            campos.Cpf= verificar.IsValidDigit(campos.Cpf);
            if (await repo.ExistsCpf(campos.Cpf))
            {
                campos.Cpf = valores.Cpf;
                camposNotUpdate.Append(" CPF") ;
            }
        }
        catch (InvalidCpfException)
        {
            campos.Cpf = valores.Cpf;
            camposNotUpdate.Append(" CPF");
        }
      
        
        if(!verificar.ValidateName(campos.Nome) || campos.Nome==valores.Nome){
  
            campos.Nome = valores.Nome;
            camposNotUpdate.Append(" NOME");
        }
        
        try
        {
            await IsValidAccount(campos.Conta);

        }
        catch (InvalidAccountException)
        {
            campos.Conta =valores.Conta;
            camposNotUpdate.Append(" CONTA");
        }
        

        switch (await repo.UpdateClient(campos, id))
        {
            case 0:
            {
                throw new ErroUpdateToDatabaseException();
            }
            
        }

        return camposNotUpdate;





    }

    /// <summary>
    /// Remove permanentemente um cliente pelo ID.
    /// </summary>
    /// <param name="id">ID do cliente.</param>
    /// <returns>True se deletado com sucesso.</returns>
    public async Task<bool> DeleteService(int id)//
    {
      if ( await repo.DeleteClient(id)==0)
      {
          throw new InvalidIdException(id);
      }
      return true;
    }

    /// <summary>
    /// Valida se uma conta informada é válida e se já não existe no banco.
    /// </summary>
    /// <param name="account">Número da conta.</param>
    /// <returns>True se for uma conta nova e válida.</returns>
    public async Task<bool> IsValidAccount(int account)
    {
        if (int.IsNegative(account) || account==0)
        {
            throw new InvalidAccountException();
        }
        if(await repo.ExistsAccount(account))
        {
            throw new InvalidAccountException();
        }
        return true;
    }
    
    
}
}