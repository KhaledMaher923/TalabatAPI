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
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        #endregion

        #region With Specifications
        Task<IEnumerable<T>> GetAllWithSpecAsync(ISpecifications<T> Spec);
        Task<T> GetByIdWithSpecAsync(ISpecifications<T> Spec);
        


        #endregion

    }
}
