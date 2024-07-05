using EventAggregator.Blazor;
using FamilieLaissInterfaces.Models.Data;
using FamilieLaissInterfaces.Services;
using System;
using System.Collections.Generic;

namespace FamilieLaissServices;

public class GraphQlSortAndFilterServiceFactory(IEventAggregator eventAggregator) : IGraphQlSortAndFilterServiceFactory
{
    public IGraphQlSortAndFilterService<TModel, TSortInput, TFilterInput> GetService<TModel, TSortInput, TFilterInput>(string identifier,
        Func<string, Dictionary<int, string>> provideNumberList)
        where TModel : IBaseModel
        where TSortInput : class
        where TFilterInput : class
    {
        return new GraphQlSortAndFilterService<TModel, TSortInput, TFilterInput>(identifier, eventAggregator, provideNumberList);
    }
}
