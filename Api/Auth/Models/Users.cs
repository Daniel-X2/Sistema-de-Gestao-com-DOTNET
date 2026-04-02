namespace auth.Models;


public record Users
(
        string cpf ,
        string Senha ,
        string[] roles
);

