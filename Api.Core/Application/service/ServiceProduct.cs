using Api.Core.Application.repository;
using Dto;
using Utils;

namespace Api.Core.Application.service
{
    public interface IServiceProduct
{
    Task<ListaProduct> GetAllProduct(int limit, int page);
   Task<bool> AddProduct(ProdutoDto campos);
   Task<bool> DeleteProduct(int id);
   Task<ListaProduct> GetEstoque(int limit, int page);
   Task<List<decimal>> GetValorBruto();
   Task<ProdutoDto> GetProdutId(int id);
   Task<bool> UpdateProduct(ProdutoDto campos, int id);
}
/// <summary>
/// Lógica de negócio para a gestão de produtos e estoque.
/// </summary>
class ServiceProduct(IRepositoryProduct repo):IServiceProduct
{
    /// <summary>
    /// Lista todos os produtos com informações completas.
    /// </summary>
    /// <param name="limit"></param>
    /// <param name="page"></param>
    /// <returns>Objeto ListaProduct.</returns>
    public async Task<ListaProduct> GetAllProduct(int limit, int page)
    {
       
       ListaProduct  lista =await repo.GetAllProduct(limit, page);
       if(lista.Product.Count>=1)
       {
               return lista;
       }

       throw new ReturnDataIsEmpty();
    }

    /// <summary>
    /// Adiciona um novo produto validando nome, código, quantidade, valor e lote.
    /// </summary>
    /// <param name="campos">DTO com dados do produto.</param>
    /// <returns>True se adicionado com sucesso.</returns>
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

    /// <summary>
    /// Remove um produto do sistema pelo ID.
    /// </summary>
    /// <param name="id">ID do produto.</param>
    /// <returns>True se removido.</returns>
    public async Task<bool> DeleteProduct(int id)
    {
        if (await repo.DeleteProduct(id)==0)
        {
            throw new InvalidIdException(id);
        }
        
        return true;
    }

    /// <summary>
    /// Obtém uma lista simplificada do estoque (nome e quantidade).
    /// </summary>
    /// <param name="limit"></param>
    /// <param name="page"></param>
    /// <returns>Objeto ListaProduct com dados de estoque.</returns>
    public async Task<ListaProduct> GetEstoque(int limit, int page)
    {
        ListaProduct lista =await repo.GetEstoque(limit, page);
        if (lista.Product.Count <= 0)
        {
            throw new ReturnDataIsEmpty();
        }

        return lista;
    }

    /// <summary>
    /// Obtém a lista de valores de revenda de todos os produtos.
    /// </summary>
    /// <returns>Lista de valores decimais.</returns>
    public async Task<List<decimal>> GetValorBruto()
    {
        List<decimal> lista =await repo.GetValorBruto();

        if (lista.Count<=0)
        {
            throw new ReturnDataIsEmpty();
        }

        return lista;
    }

    /// <summary>
    /// Busca um produto específico pelo ID.
    /// </summary>
    /// <param name="id">ID do produto.</param>
    /// <returns>DTO do produto encontrado.</returns>
    public async Task<ProdutoDto> GetProdutId(int id)
    {
            ProdutoDto product= await  repo.GetProductById(id);
            if (product==null)
            {
                throw new InvalidIdException(id);
            }
            return product;
    }

    /// <summary>
    /// Atualiza os dados de um produto existente, validando as novas entradas.
    /// </summary>
    /// <param name="campos">Novos dados.</param>
    /// <param name="id">ID do produto.</param>
    /// <returns>True se atualizado.</returns>
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
