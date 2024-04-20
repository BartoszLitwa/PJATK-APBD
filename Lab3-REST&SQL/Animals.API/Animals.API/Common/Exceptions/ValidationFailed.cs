namespace Animals.API.Common.Exceptions;

public class ValidationFailed : Exception
{
    public ValidationFailed(string message) : base(message) { }
}