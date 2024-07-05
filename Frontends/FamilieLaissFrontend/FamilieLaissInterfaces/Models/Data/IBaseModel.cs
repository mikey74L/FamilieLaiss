namespace FamilieLaissInterfaces.Models.Data;

public interface IBaseModel
{

}

public interface IBaseModel<T> : IBaseModel
{
    public T Clone();

    public void TakeOverValues(T sourceModel);
}