
public class ExceptionHandlingMiddleware
{
    private  RequestDelegate _next;
    private ILogger<ExceptionHandlingMiddleware> _logger;
    public  ExceptionHandlingMiddleware(RequestDelegate next,ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ReturnDataIsEmpty ex)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new { erro = "nao teve correspondecia aos dados solicitados" });
            _logger.LogError($"Sem dados Correspondentes {ex.Message}");
        }
        catch (InvalidCpfException ex)
        {
            context.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
            await context.Response.WriteAsJsonAsync(new
                { erro = "O Cpf inserido e invalido", details = "Verifique o cpf e tente novamente" });
            _logger.LogError($"Cpf Invalido {ex.Message}");
        }
        catch (InvalidNameException ex)
        {
            context.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
            await context.Response.WriteAsJsonAsync(new
                { erro = "O nome inserido e invalido", details = "nomes com menos de 4 caracteres nao e aceito" });
            _logger.LogError($"Name Invalido: {ex.Message}");
        }
        catch (ErroAddToDatabaseException ex)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new
                { erro = "aconteceu um erro ao adicionar os  dados", details = ex.Message });
            _logger.LogError($"Add Invalido: {ex.Message}");
        }
        catch (InvalidAccountException ex )
        {
            context.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
            await context.Response.WriteAsJsonAsync(new
            {
                erro = "A conta inserida e invalida", details = "Verifique se a conta ja existe ou o numero e negativo"
            });
            _logger.LogError($"Account Invalido {ex.Message}");
        }
        catch (InvalidNascimentoException ex)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new { erro = "O ano de nascimento nao e valido" });
            _logger.LogError($"Nascimento Invalido {ex.Message}");
        }
        catch (InvalidIdException ex)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new { erro = "o Id inserido e invalido" });
            _logger.LogError($"Id Invalido {ex.Message}");
        }
        catch (InvalidConnection ex)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(new { erro = "aconteceu um erro de conexao", details = ex.Message });
            _logger.LogError($"Conexao Invalida: {ex.Message}");
        }
        catch (InvalidCodeException ex)
        {
            context.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
            await context.Response.WriteAsJsonAsync(new
                { erro = "O codigo inserido e invalido", details = "o codigo ja existe ou o numero e negativo" });
            _logger.LogError($"Codigo Invalido: {ex.Message}");
        }
        catch (NegativeNumericException ex)
        {
            context.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
            await context.Response.WriteAsJsonAsync(new { erro = "O numero inserido e negativo" });
            _logger.LogError($"Numero Invalido: {ex.Message}");
        }
        catch (InvalidLoteException ex)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new { erro = "O numero de lote e invalido" });
            _logger.LogError($"Lote Invalido: {ex.Message}");
        }
        catch (InvalidPassword ex)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(new { erro = "A senha inserida esta errada" });
            _logger.LogError($"Senha Invalida: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro inesperado: {ex.Message}");
        }
    }
}