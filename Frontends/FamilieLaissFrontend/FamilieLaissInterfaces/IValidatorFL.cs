using FluentValidation;

namespace FamilieLaissInterfaces
{
    public interface IValidatorFl<in T>: IValidator<T>
    {
        Func<object, string, Task<IEnumerable<string>>> ValidateValue {get; }
    }
}
