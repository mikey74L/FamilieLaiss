using DomainHelper.Exceptions;
using HotChocolate;
using InfrastructureHelper.Exceptions;

namespace Upload.API.GraphQL.Filter;

public class GraphQlErrorFilter : IErrorFilter
{
    #region Implementation of IErrorFilter

    /// <inheritdoc />
    public IError OnError(IError error)
    {
        switch (error.Exception)
        {
            case DomainException { ExceptionType: DomainExceptionType.NoDataFound }:
                return error.WithCode("NoDataFound");
            case DomainException { ExceptionType: DomainExceptionType.WrongParameter }:
                return error.WithCode("WrongParameter");
            case DataDuplicatedValueException:
                return error.WithCode("DuplicatedValue");
            default:
                {
                    if (error.Code == "AUTH_NOT_AUTHORIZED")
                    {
                        return error.WithCode("NotAuthorized");
                    }

                    break;
                }
        }

        return error.WithCode("ServerError");
    }

    #endregion
}