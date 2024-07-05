using DomainHelper.AbstractClasses;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace FLBackEnd.Domain.Entities;

public class UploadIdentifier : EntityCreation<long>
{
    [Required][MaxLength(30)] public string PseudoText { get; set; } = string.Empty;

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