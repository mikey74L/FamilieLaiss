namespace FamilieLaissInterfaces.Models.Data;

public interface IBlogMinMaxDateModel
{
    public DateTimeOffset? MinCreateDate { get; set; }
    public DateTimeOffset? MaxCreateDate { get; set; }
}
