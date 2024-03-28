namespace LegacyApp.Exceptions;

public class UserValidationException(string message) : UserException(message)
{
}