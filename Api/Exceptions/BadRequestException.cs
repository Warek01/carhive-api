namespace Api.Exceptions;

public class BadRequestException(string message) : HttpException(message);