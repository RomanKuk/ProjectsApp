using DAL.Entities.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IRepository<TEntity> where TEntity: BaseEntity
    {
        Task Create(TEntity entity, string createdBy = null);
        Task Update(TEntity entity, string updatedBy = null);
        Task DeleteById(int id);
        Task Delete(TEntity entity);
        Task<DbSet<TEntity>> GetAll();
        Task<TEntity> GetById(int id);
    }
}
