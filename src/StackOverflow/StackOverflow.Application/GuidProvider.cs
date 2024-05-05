using StackOverflow.Application.Contracts.Utilities;

namespace StackOverflow.Application;

public class GuidProvider : IGuidProvider
{
    public Guid GetGuid() => Guid.NewGuid();
}