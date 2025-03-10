using DomainHelper.Exceptions;
using InfrastructureHelper.Exceptions;

namespace Catalog.API.GraphQL.Filter;

public class GraphQlErrorFilter : IErrorFilter
{
    #region Implementation of IErrorFilter

    /// <inheritdoc />
    public IError OnError(IError error)
    {
        if (error.Exception is DomainException { ExceptionType: DomainExceptionType.NoDataFound })
        {
            return error.WithCode("NoDataFound");
        }
        else if (error.Exception is DomainException { ExceptionType: DomainExceptionType.WrongParameter })
        {
            return error.WithCode("WrongParameter");
        }
        else if (error.Exception is DataDuplicatedValueException)
        {
            return error.WithCode("DuplicatedValue");
        }
        else if (error.Code == "AUTH_NOT_AUTHORIZED")
        {
            return error.WithCode("NotAuthorized");
        }

        return error.WithCode("ServerError");
    }

    #endregion
}