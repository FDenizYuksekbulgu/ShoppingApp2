using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using ShoppingApp2.Data.Context;

namespace ShoppingApp2.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ShoppingApp2DbContext   _db;
        private IDbContextTransaction _transaction;

        public UnitOfWork(ShoppingApp2DbContext db)
        {
            _db = db;
        }

        public async Task BeginTransaction()
        {
            _transaction = await _db.Database.BeginTransactionAsync();
        }

        public async Task CommitTransaction()
        {
            await _transaction.CommitAsync();
        }

        public void Dispose()
        {
            _db.Dispose(); // Garbage Collector'a sen bunu temizleyebilirsin izni verdiğimiz yer. O an silmiyoruz - silinebilir yapıyoruz.

           
        }

        public async Task RollbackTransaction()
        {
            await _transaction.RollbackAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _db.SaveChangesAsync();
        }
    }
}
