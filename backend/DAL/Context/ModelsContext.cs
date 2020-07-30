using Bogus;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Context
{
    public class ModelsContext : DbContext
    {
        public ModelsContext(DbContextOptions<ModelsContext> options) : base(options) { }

        public DbSet<Project> Projects { get; }
        public DbSet<TaskModel> Tasks { get; }
        public DbSet<User> Users { get; }
        public DbSet<Team> Teams { get; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Configure();

            modelBuilder.Seed();
        }
    }
}
