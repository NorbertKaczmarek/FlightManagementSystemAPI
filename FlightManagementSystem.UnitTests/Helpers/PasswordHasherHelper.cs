namespace FlightManagementSystem.UnitTests.Helpers;

public static class PasswordHasherHelper
{
    public static string HashedPassword(this object obj, User newUser)
    {
        var _passwordHasher = new PasswordHasher<User>();
        var hashedPasword = _passwordHasher.HashPassword(newUser, (string)obj);

        return hashedPasword;
    }
}
