


public class Crypto
{
    public static bool VerificarHash(string text,string hash_banco)
    {
       
        if (BCrypt.Net.BCrypt.Verify(text, hash_banco))
        {
            return true;
        }

        return false;
    }

    public static  string RetornHash(string text)
    {
        string hashPassword=  BCrypt.Net.BCrypt.HashPassword(text);
        
        return hashPassword;
    }
}