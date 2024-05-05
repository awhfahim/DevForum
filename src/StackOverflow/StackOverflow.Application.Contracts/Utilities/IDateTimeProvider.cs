namespace StackOverflow.Application.Contracts.Utilities;

public interface IDateTimeProvider
{
    DateTime GetUtcNow();
}