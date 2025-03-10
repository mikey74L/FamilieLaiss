using DomainHelper.AbstractClasses;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Upload.Domain.Entities;

public class UploadIdentifier : EntityCreation<long>
{
    [Required][MaxLength(30)] public string PseudoText { get; init; } = string.Empty;

    #region Called from Change-Tracker

    public override Task EntityAddedAsync(DbContext dbContext, IDictionary<string, object> dictContextParams)
    {
        return Task.CompletedTask;
    }

    public override Task EntityDeletedAsync(DbContext dbContext, IDictionary<string, object> dictContextParams)
    {
        return Task.CompletedTask;
    }

    #endregion
}