using System;
using System.Collections.Generic;
using System.Text;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;
 
namespace Talabat.Core.Repositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        #region Without Specifications
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        #endregion

        #region With Specifications
        Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> Spec);
        Task<T> GetByIdWithSpecAsync(ISpecifications<T> Spec);
        Task<int> GetCountWithSpecAsync (ISpecifications<T> Spec); 
        Task AddAsync(T item);
        void Update(T item);
        void Delete(T item);


        #endregion

    }
}
