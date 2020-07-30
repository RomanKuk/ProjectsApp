using DAL.Context;
using DAL.Entities.Abstract;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        protected readonly ModelsContext context;

        public UnitOfWork(ModelsContext context)
        {
            this.context = context;
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }

        public IRepository<TEntity> Set<TEntity>() where TEntity : BaseEntity
        {
            return new Repository<TEntity>(context);
        }
    }
}
