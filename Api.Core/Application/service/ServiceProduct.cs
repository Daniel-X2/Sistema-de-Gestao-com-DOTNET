using Api.Core.Application.repository;
using Dto;
using Utils;

namespace Api.Core.Application.service
{
    public interface IServiceProduct
{
    Task<ListaProduct> GetAllProduct();
   Task<bool> AddProduct(ProdutoDto campos);
   Task<bool> DeleteProduct(int id);
   Task<ListaProduct>  GetEstoque();
   Task<List<decimal>> GetValorBruto();
   Task<ProdutoDto> GetProdutId(int id);
   Task<bool> UpdateProduct(ProdutoDto campos, int id);
}
class ServiceProduct(IRepositoryProduct repo):IServiceProduct
{
    public async Task<ListaProduct> GetAllProduct()
    {
       
       ListaProduct  lista =await repo.GetAllProduct();
       if(lista.Product.Count>=1)
       {
               return lista;
       }

       throw new ReturnDataIsEmpty();
    }

    public async Task<bool> AddProduct(ProdutoDto campos)
    {
        
            var validation = new Validation();
            if (!validation.ValidateName(campos.Nome))
            {
                throw new InvalidNameException();
            }
            if (await repo.IsExistCode(campos.Codigo) || int.IsNegative(campos.Codigo) || campos.Codigo==0)
            {
                throw new InvalidCodeException(campos.Codigo);
            }
            if (campos.Quantidade<=0)
            {
                throw new NegativeNumericException();
            }

            if (campos.ValorRevenda<=0)
            {
                throw new NegativeNumericException();
            }

            if (await repo.IsExistLote(campos.Lote) || campos.Lote<=0)
            {
                throw new InvalidLoteException(campos.Lote);
            }


            int resultado = await repo.AddProduct(campos);
            if (resultado>=1)
            {
                return true;
            }
            throw new ErroAddToDatabaseException();
            
    }

    public async Task<bool> DeleteProduct(int id)
    {
        if (await repo.DeleteProduct(id)==0)
        {
            throw new InvalidIdException(id);
        }
        
        return true;
    }

    public async Task<ListaProduct> GetEstoque()
    {
        ListaProduct lista =await repo.GetEstoque();
        if (lista.Product.Count <= 0)
        {
            throw new ReturnDataIsEmpty();
        }

        return lista;
    }

    public async Task<List<decimal>> GetValorBruto()
    {
        List<decimal> lista =await repo.GetValorBruto();

        if (lista.Count<=0)
        {
            throw new ReturnDataIsEmpty();
        }

        return lista;
    }

    public async Task<ProdutoDto> GetProdutId(int id)
    {
            ProdutoDto product= await  repo.GetProductById(id);
            if (product==null)
            {
                throw new InvalidIdException(id);
            }
            return product;
    }

    public async Task<bool> UpdateProduct(ProdutoDto campos,int id)
    {
        Validation validation = new();
        var valores =await repo.GetProductById(id);
        
        if (valores==null)
        {
            throw new ReturnDataIsEmpty();
        }
        if (await repo.IsExistCode(campos.Codigo) || campos.Codigo<=0)
        {
            campos.Codigo = valores.Codigo;
            //aqui coloca o negocio pra listar os que nao foram atualizados
        }
            
        if (await repo.IsExistLote(campos.Lote) || campos.Lote<=0)
        {
            campos.Lote = valores.Lote;
        }
        
        if (!validation.ValidateName(campos.Nome))
        {
            campos.Nome = valores.Nome;
        }
        
        if (campos.Quantidade<=0)
        {
            campos.Quantidade = valores.Quantidade;
        }

        if (campos.ValorRevenda<=0)
        {
            campos.ValorRevenda = valores.ValorRevenda;
        }

       int resultado= await repo.UpdateProduct(campos, id);
       if (resultado>=1)
       {
           return true;
       }
       throw new ErroUpdateToDatabaseException();
    }
}
}
