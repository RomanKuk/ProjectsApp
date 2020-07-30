using DAL.Context;
using DAL.Entities.Abstract;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Services
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly ModelsContext context;

        public Repository(ModelsContext context)
        {
            this.context = context;
        }
        
        public virtual async Task Create(TEntity entity, string createdBy = null)
        {
            await context.Set<TEntity>().AddAsync(entity);
        }

        public virtual async Task DeleteById(int id)
        {
            TEntity entity = await context.Set<TEntity>().FindAsync(id);
            await Delete(entity);
        }

        public virtual async Task Delete(TEntity entity)
        {
            await Task.Run(() =>
            {
                var dbSet = context.Set<TEntity>();
                if (context.Entry(entity).State == EntityState.Detached)
                {
                    dbSet.Attach(entity);
                }
                dbSet.Remove(entity);
            });
        }

        public virtual async Task Update(TEntity entity, string updatedBy = null)
        {
            await Task.Run(() =>
            {
                context.Set<TEntity>().Attach(entity);
                context.Entry(entity).State = EntityState.Modified;
            });
        }

        public virtual async Task<DbSet<TEntity>> GetAll()
        {
            return await Task.Run(() => context.Set<TEntity>());
        }

        public virtual async Task<TEntity> GetById(int id)
        {
            return await context.Set<TEntity>().FindAsync(id);
        }
    }
}
