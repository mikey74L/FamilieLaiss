using InfrastructureHelper.Converter;
using Microsoft.EntityFrameworkCore;
using System;

namespace InfrastructureHelper.Context;

public abstract class BaseContextFamilieLaiss<TContext>(DbContextOptions<TContext> options) : DbContext(options) where TContext : DbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingInternal(modelBuilder);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(DateTimeOffset))
                {
                    property.SetValueConverter(new InfrastructureHelper.Converter.DateTimeOffsetConverter());
                }
                else if (property.ClrType == typeof(DateTimeOffset?))
                {
                    property.SetValueConverter(new NullableDateTimeOffsetConverter());
                }
            }
        }
    }

    /// <summary>
    /// Would be called when the model is creating to define special behaviour
    /// </summary>
    /// <param name="modelBuilder">The builder being used to construct the model for this context</param>
    protected abstract void OnModelCreatingInternal(ModelBuilder modelBuilder);
}