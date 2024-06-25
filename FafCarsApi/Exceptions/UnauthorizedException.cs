namespace FafCarsApi.Exceptions;

public class UnauthorizedException(string message) : HttpException(message);