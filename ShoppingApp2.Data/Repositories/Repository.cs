using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShoppingApp2.Data.Context;
using ShoppingApp2.Data.Entities;

namespace ShoppingApp2.Data.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : BaseEntity
    {
        private readonly ShoppingApp2DbContext _db;

        private readonly DbSet<TEntity> _dbSet;

        public Repository(ShoppingApp2DbContext db)
        {
            _db = db;
            _dbSet = _db.Set<TEntity>();
        }

        public void Add(TEntity entity)
        {
            entity.CreatedDate = DateTime.Now;
            _dbSet.Add(entity);
            //_db.SaveChanges();
            // _db.SaveChanges()'lar transaction(iki kaydın aynı anda tamamlandıktan sonra kaydedilmesi durumu- banka örneği-) durumları göz önüne alınarak UnitOfWork adı verdiğimiz başka bir pattern içerisinde yönetilecek
        }

        public void Delete(TEntity entity, bool softDelete = true)
        {
            if (softDelete)
            {
                entity.ModifiedDate = DateTime.Now;
                entity.IsDeleted = true;
                _dbSet.Update(entity);
            }
            else
            {
                _dbSet.Remove(entity);
            }

            //_db.SaveChanges();
            // _db.SaveChanges()'lar transaction(iki kaydın aynı anda tamamlandıktan sonra kaydedilmesi durumu- banka örneği-) durumları göz önüne alınarak UnitOfWork adı verdiğimiz başka bir pattern içerisinde yönetilecek

        }

        public void Delete(int id)
        {
            var entity = _dbSet.Find(id);
            Delete(entity);
        }

        public TEntity Get(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.FirstOrDefault(predicate);
        }

        public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate = null)
        {
            return predicate is null ? _dbSet : _dbSet.Where(predicate);
        }

        public TEntity GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public void Update(TEntity entity)
        {
            entity.ModifiedDate = DateTime.Now;
            _dbSet.Update(entity);
            //_db.SaveChanges();
            // _db.SaveChanges()'lar transaction(iki kaydın aynı anda tamamlandıktan sonra kaydedilmesi durumu- banka örneği-) durumları göz önüne alınarak UnitOfWork adı verdiğimiz başka bir pattern içerisinde yönetilecek.
        }
    }
}
