namespace WaterShop.Exceptions;

public class ValidationDataException : Exception
{
	public ValidationDataException() : base()
	{
	}

	public ValidationDataException(string? message) : base(message)
	{
	}

	public ValidationDataException(string? message, Exception? innerException) : base(message, innerException)
	{
	}
}
