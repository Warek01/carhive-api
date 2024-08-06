namespace Api.Exceptions;

public abstract class HttpException(string message) : Exception(message);