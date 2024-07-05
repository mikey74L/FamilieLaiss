using DomainHelper.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfrastructureHelper.UnitOfWork
{
    public interface iUnitOfWork<TContext> : iUnitOfWork where TContext : DbContext
    {
        TContext Context { get; }
    }
}
