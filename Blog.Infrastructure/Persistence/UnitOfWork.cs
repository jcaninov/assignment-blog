using System.Data;
using Blog.Domain.Repositories;

namespace Blog.Infrastructure.Persistence;

public sealed class UnitOfWork : IUnitOfWork
{
    public IDbConnection Connection { get; }
    public IDbTransaction? Transaction { get; private set; }


    public UnitOfWork(IDbConnection db) => Connection = db;

    
    public void Begin()
    {
        if (Connection.State == ConnectionState.Closed)
             Connection.Open();
        
        Transaction = Connection.BeginTransaction();
    }

    public void Commit()
    {
        Transaction?.Commit();
        DisposeTransaction();
    }

    public void Rollback()
    {
        Transaction?.Rollback();
        DisposeTransaction();
    }

    private void DisposeTransaction()
    {
        Transaction?.Dispose();
        Transaction = null;
    }

    public void Dispose()
    {
        DisposeTransaction();
        Connection.Dispose();
    }
}
