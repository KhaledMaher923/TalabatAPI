using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext _dbContext;
        private Hashtable _repository;
        public UnitOfWork(StoreContext dbContext)
        {
            _repository = new Hashtable();
            _dbContext = dbContext;
        }

        public async Task<int> CompleteAsync()
        => await _dbContext.SaveChangesAsync();

        public ValueTask DisposeAsync()
        => _dbContext.DisposeAsync();

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            var type = typeof(TEntity).Name;

            if (!_repository.ContainsKey(type))
            {
                var Repository = new GenericRepository<TEntity>(_dbContext);
                _repository.Add(type, Repository);
            }
            return _repository[type] as IGenericRepository<TEntity>;

        }
    }
}
