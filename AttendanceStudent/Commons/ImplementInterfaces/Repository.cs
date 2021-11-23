using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AttendanceStudent.Commons.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AttendanceStudent.Commons.ImplementInterfaces
{
    [ExcludeFromCodeCoverage]
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly DbSet<TEntity> _dbSet;

        public Repository(IApplicationDbContext applicationDbContext)
        {
            // Init TEntity DbSet
            _dbSet = applicationDbContext.Set<TEntity>();
        }


        public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            await _dbSet.AddAsync(entity, cancellationToken);
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            await _dbSet.AddRangeAsync(entities, cancellationToken);
        }

        public void Remove(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }

        public void UpdateRange(IEnumerable<TEntity> entities)
        {
            _dbSet.UpdateRange(entities);
        }

        public async Task<TEntity?> FindByIdAsync(Guid id,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _dbSet.FindAsync(new object[] {id}, cancellationToken);
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> expression)
        {
            return _dbSet.Where(expression);
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _dbSet.Where(expression).ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _dbSet.ToListAsync(cancellationToken);
        }

        public async Task<bool> AllAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _dbSet.AllAsync(predicate, cancellationToken);
        }

        public bool All(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.All(predicate);
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _dbSet.AnyAsync(predicate, cancellationToken);
        }

        public bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.Any(predicate);
        }
    }
}