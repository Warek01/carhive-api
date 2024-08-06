namespace Api.Exceptions;

public class UnauthorizedException(string message) : HttpException(message);