namespace FafCarsApi.Exceptions;

public class BadRequestException(string message) : HttpException(message);