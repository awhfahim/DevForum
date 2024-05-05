namespace StackOverflow.Infrastructure.Utilities;

public interface IAdoNetUtility
{
    Task<(IList<TReturn> result, IDictionary<string, object> outValues)>
        QueryWithStoredProcedureAsync<TReturn>(string storedProcedureName,
            IDictionary<string, object>? parameters = null, IDictionary<string, Type>? outParameters = null) where TReturn : class, new();
}