namespace FamilieLaissServices.DataServices;

//public class BlogService : BaseDataService, IBlogService
//{
//    #region C'tor
//    public BlogService(IHttpClientFactory clientFactory, IAppSettingsService appSettingsService) :
//        base(clientFactory, appSettingsService)
//    {
//    }
//    #endregion

//    #region Query
//    public async Task<IEnumerable<IBlogItemModel>> GetAllBlogEntriesAsync(DateTimeOffset? filterMinDate, DateTimeOffset? filterMaxDate)
//    {
//        List<IBlogItemModel> result = new();

//        if (filterMinDate is null && filterMaxDate is null)
//        {
//            var graphQLResponseNormal = await ExecuteQueryAsync<GetAllBlogEntriesResponce>(BlogQueries.GetAllBlogEntriesQuery());

//            if (!graphQLResponseNormal.Errors.HasErrors() && graphQLResponseNormal.Data is not null)
//            {
//                foreach (var item in graphQLResponseNormal.Data.BlogEntries)
//                {
//                    result.Add(item);
//                }
//            }
//            else
//            {
//                throw new Exception("Error while querying");
//            }
//        }

//        if (filterMinDate is not null && filterMaxDate is not null)
//        {
//            var maxDateUse = new DateTimeOffset(
//                new DateTime(
//                    filterMaxDate.Value.Year,
//                    filterMaxDate.Value.Month,
//                    filterMaxDate.Value.Day,
//                    23, 59, 59));

//            var graphQLResponseFilter = await ExecuteQueryAsync<GetAllBlogEntriesFilterResponce>(
//                BlogQueries.GetAllBlogEntriesFilterQuery(filterMinDate.Value, maxDateUse));

//            if (!graphQLResponseFilter.Errors.HasErrors() && graphQLResponseFilter.Data is not null)
//            {
//                foreach (var item in graphQLResponseFilter.Data.BlogEntries)
//                {
//                    result.Add(item);
//                }
//            }
//            else
//            {
//                throw new Exception("Error while querying");
//            }

//        }

//        return result;
//    }

//    public async Task<IBlogItemModel> GetBlogEntryAsync(long id)
//    {
//        IBlogItemModel result;

//        var graphQLResponse = await ExecuteQueryAsync<GetBlogEntryResponce>(BlogQueries.GetBlogEntryQuery(id));

//        if (!graphQLResponse.Errors.HasErrors() && graphQLResponse.Data is not null)
//        {
//            if (graphQLResponse.Data.BlogEntries.Count == 1)
//            {
//                result = graphQLResponse.Data.BlogEntries.First();
//            }
//            else
//            {
//                throw new Exception("No single blog entry found with this id");
//            }
//        }
//        else
//        {
//            throw new Exception("Error while querying");
//        }

//        return result;
//    }

//    public async Task<IBlogMinMaxDateModel> GetBlogMinMaxDates()
//    {
//        IBlogMinMaxDateModel result;

//        var graphQLResponse = await ExecuteQueryAsync<BlogGetMinMaxDateResponce>(BlogQueries.GetBlogGetMinMaxDateQuery());

//        if (!graphQLResponse.Errors.HasErrors() && graphQLResponse.Data is not null)
//        {
//            result = graphQLResponse.Data.BlogGetMinAndMaxDate;
//        }
//        else
//        {
//            throw new Exception("Error while querying");
//        }

//        return result;
//    }
//    #endregion

//    #region Mutation
//    public async Task<IBlogItemModel> AddBlogEntryAsync(IBlogItemModel model)
//    {
//        if (model.HeaderGerman is null) throw new ArgumentNullException(nameof(model.HeaderGerman));
//        if (model.HeaderEnglish is null) throw new ArgumentNullException(nameof(model.HeaderEnglish));
//        if (model.TextGerman is null) throw new ArgumentNullException(nameof(model.TextGerman));
//        if (model.TextEnglish is null) throw new ArgumentNullException(nameof(model.TextEnglish));

//        var mutationResult = await ExecuteMutationAsync<AddBlogEntryResponce>(
//            BlogMutations.GetAddBlogEntryMutation(model.HeaderGerman, model.HeaderEnglish, model.TextGerman,
//            model.TextEnglish));

//        if (!mutationResult.Errors.HasErrors() && mutationResult.Data is not null)
//        {
//            model.CreateDate = mutationResult.Data.AddBlogEntry.BlogEntry.CreateDate;
//            model.Id = mutationResult.Data.AddBlogEntry.BlogEntry.Id;
//        }
//        else
//        {
//            throw new Exception("Error while mutating");
//        }

//        return model;
//    }

//    public async Task<IBlogItemModel> UpdateBlogEntryAsync(IBlogItemModel model)
//    {
//        if (model.HeaderGerman is null) throw new ArgumentNullException(nameof(model.HeaderGerman));
//        if (model.HeaderEnglish is null) throw new ArgumentNullException(nameof(model.HeaderEnglish));
//        if (model.TextGerman is null) throw new ArgumentNullException(nameof(model.TextGerman));
//        if (model.TextEnglish is null) throw new ArgumentNullException(nameof(model.TextEnglish));

//        var mutationResult = await ExecuteMutationAsync<UpdateBlogEntryResponce>(
//            BlogMutations.GetUpdateBlogEntryMutation(model.Id, model.HeaderGerman, model.HeaderEnglish,
//            model.TextGerman, model.TextEnglish));

//        if (!mutationResult.Errors.HasErrors() && mutationResult.Data is not null)
//        {
//            model.ChangeDate = mutationResult.Data.UpdateBlogEntry.BlogEntry.ChangeDate;
//        }
//        else
//        {
//            throw new Exception("Error while mutating");
//        }

//        return model;
//    }

//    public async Task<IBlogItemModel> DeleteBlogEntryAsync(IBlogItemModel model)
//    {
//        var mutationResult = await ExecuteMutationAsync<DeleteBlogEntryResponce>(
//            BlogMutations.GetDeleteBlogEntryMutation(model.Id));

//        if (!mutationResult.Errors.HasErrors() && mutationResult.Data is not null)
//        {
//            model.Id = mutationResult.Data.DeleteBlogEntry.BlogEntry.Id;
//        }
//        else
//        {
//            throw new Exception("Error while mutating");
//        }

//        return model;
//    }
//    #endregion
//}
