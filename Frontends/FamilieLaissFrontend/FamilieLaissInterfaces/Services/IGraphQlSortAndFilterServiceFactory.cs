using FamilieLaissInterfaces.Models.Data;

namespace FamilieLaissInterfaces.Services;

public interface IGraphQlSortAndFilterServiceFactory
{
    IGraphQlSortAndFilterService<TModel, TSortInput, TFilterInput> GetService<TModel, TSortInput, TFilterInput>(string identifier,
        Func<string, Dictionary<int, string>> provideNumberList)
        where TModel : IBaseModel
        where TSortInput : class
        where TFilterInput : class;
}
