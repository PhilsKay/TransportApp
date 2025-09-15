namespace TransportApp.Application.Helper;

public class Util
{
    public static string GenerateRandomString(int length, bool specialChar = false)
    {
        Random random = new();

        string characters = specialChar ? "@$ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz@_!$" : "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        string result = new(Enumerable.Repeat(characters, length).Select(s => s[random.Next(s.Length)]).ToArray());
        return result.ToUpper();
    }

}
