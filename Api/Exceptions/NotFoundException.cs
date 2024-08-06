namespace Api.Exceptions;

public class NotFoundException(string message) : HttpException(message);