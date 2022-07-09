using CalixtoStore.Core.DomainObjects;
using System;

namespace CalixtoStore.Core.Data
{
    public interface IRepository<T> : IDisposable where T : IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
    }
}