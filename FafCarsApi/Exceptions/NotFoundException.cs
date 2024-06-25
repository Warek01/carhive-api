namespace FafCarsApi.Exceptions;

public class NotFoundException(string message) : HttpException(message);