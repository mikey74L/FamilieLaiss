using System.ComponentModel.DataAnnotations;
using DomainHelper.AbstractClasses;
using System.Threading.Tasks;

namespace Upload.Domain.Entities;

public class UploadIdentifier : EntityCreation<long>
{
    [Required]
    [MaxLength(30)]
    public string PseudoText { get; set; } = string.Empty;

    #region Called from Change-Tracker
    public override Task EntityAddedAsync()
    {
        return Task.CompletedTask;
    }

    public override Task EntityDeletedAsync()
    {
        return Task.CompletedTask;
    }
    #endregion
}
