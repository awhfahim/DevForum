using StackOverflow.Application.Contracts.Utilities;

namespace StackOverflow.Application;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime GetUtcNow() => DateTime.UtcNow;
}