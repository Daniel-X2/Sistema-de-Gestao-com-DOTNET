using Utils;
using Dto;
using Api.Core.Application.repository;

namespace Api.Core.Application.service
{
    public interface IServiceCLient
    {
        Task<ListaClient> GetAllService();

        Task<bool> UpdateService(int id, ClientDto campos);
        Task<bool> AddService(ClientDto campos);
        Task<bool> DeleteService(int id);
    }
    class ClientService(IRepositoryClient repo):IServiceCLient
{
    public async Task<ClientDto> GetByIdService(int id)
    {
        try

        {
            ClientDto resultado = await repo.GetById(id);
            if (string.IsNullOrWhiteSpace(resultado.Nome))
            {
                throw new ReturnDataIsEmpty();
            }

            return resultado;
        }
        catch (InvalidOperationException)
        {
            throw new InvalidIdException(id);
        }
     
        
    }
    public async Task<ListaClient> GetAllService()//
    {
    
        
        ListaClient valores= await repo.GetAllClient();
        Console.WriteLine(valores.Clients.Count);
        switch (valores.Clients.Count)
        {
            case 0:
            {
                throw new ReturnDataIsEmpty();
            }
            case >= 1:
            {
                return valores;
            }
            default:
            {
                throw new ReturnDataIsEmpty();
            }
        }
   
    }
    public async Task<bool> AddService(ClientDto campos)//
    {
       
        Validation verificador = new();

        
       
        campos.Cpf = verificador.IsValidDigit(campos.Cpf);
        await IsValidAccount(campos.Conta);
        if (await repo.IsExistsCpf(campos.Cpf))
        {
            throw new InvalidCpfException(campos.Cpf);
        }
        if (!verificador.VerificarNome(campos.Nome))
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

    public async Task<bool> UpdateService(int id,
        ClientDto campos)
    {
      
        Validation verificar = new();
        var valores =await  repo.GetById(id);
        if (string.IsNullOrWhiteSpace(valores.Nome))
        {
            throw new InvalidIdException(id);
        }

        try
        {
            campos.Cpf= verificar.IsValidDigit(campos.Cpf);
            if (await repo.IsExistsCpf(campos.Cpf))
            {
                campos.Cpf = valores.Cpf;
            }
        }
        catch (InvalidCpfException)
        {
            campos.Cpf = valores.Cpf;
        }
      
        
        if(!verificar.VerificarNome(campos.Nome)){
  
            campos.Nome = valores.Nome;
        }

        if (!await IsValidAccount(campos.Conta))
        {
            campos.Conta =valores.Conta;
        }

        switch (await repo.UpdateClient(campos, id))
        {
            case 0:
            {
                throw new ErroUpdateToDatabaseException();
            }
            case >= 1:
            {
                return true;
            }
            default:
            {
                throw new ErroUpdateToDatabaseException();
            }
        }
      
        
       
        
        
    }

    public async Task<bool> DeleteService(int id)//
    {
      if ( await repo.DeleteClient(id)==0)
      {
          throw new InvalidIdException(id);
      }
      return true;
    }

    public async Task<bool> IsValidAccount(int account)
    {
        if (int.IsNegative(account) || account==0)
        {
            throw new InvalidAccount();
        }
        if(await repo.ContaExiste(account))
        {
            throw new InvalidAccount();
        }
        return true;
    }

    public async Task IsValidCpf(string cpf)
    {
        Validation verificador = new();
        verificador.IsValidDigit(cpf);
        if (await repo.IsExistsCpf(cpf) )
        {
            throw new InvalidCpfException(cpf);
        }
        
    }
    public async Task<int> GetIdService(string cpf)
    {
       int id= await repo.GetIdByCpf(cpf);
       return id;
    }
}
}