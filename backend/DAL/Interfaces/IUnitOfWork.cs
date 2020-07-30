using DAL.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IUnitOfWork: IDisposable
    {
        IRepository<TEntity> Set<TEntity>() where TEntity : BaseEntity;
        void SaveChanges();
        Task SaveChangesAsync();
    }
}
