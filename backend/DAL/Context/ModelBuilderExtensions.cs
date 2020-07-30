using Bogus;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace DAL.Context
{
    public static class ModelBuilderExtensions
    {
        private const int TEAMS_COUNT = 10;
        private const int USERS_COUNT = 100;
        private const int PROJECT_COUNT = 50;
        private const int TASKS_COUNT = 200;
        private const int MAX_PROJECT_TEAM_NAME = 25;
        private const int MAX_TASK_NAME = 100;
        private const int MAX_FIRST_LAST_NAME = 25;
        private const int MAX_EMAIL = 60;

        public static void Configure(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>()
                .HasMany(p => p.Tasks)
                .WithOne(t => t.Project)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Team>()
                .HasMany(t => t.Participants)
                .WithOne(u => u.Team)
                .HasForeignKey(u => u.TeamId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Project>()
                .HasOne(t => t.Team)
                .WithMany(p => p.Projects)
                .HasForeignKey(p => p.TeamId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Tasks)
                .WithOne(t => t.Performer)
                .HasForeignKey(t => t.PerformerId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Project>()
                .HasOne(p => p.Author)
                .WithMany(t => t.Projects)
                .HasForeignKey(t => t.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Project>()
                .Property("Name").HasMaxLength(MAX_PROJECT_TEAM_NAME);

            modelBuilder.Entity<TaskModel>()
                .Property("Name").HasMaxLength(MAX_TASK_NAME);

            modelBuilder.Entity<Team>()
                .Property("Name").HasMaxLength(MAX_PROJECT_TEAM_NAME);

            modelBuilder.Entity<User>()
                .Property("FirstName").HasMaxLength(MAX_FIRST_LAST_NAME);

            modelBuilder.Entity<User>()
                .Property("LastName").HasMaxLength(MAX_FIRST_LAST_NAME);

            modelBuilder.Entity<User>()
                .Property("Email").HasMaxLength(MAX_EMAIL);
        }

        public static void Seed(this ModelBuilder modelBuilder)
        {
            var teams = GenerateRandomTeams();
            var users = GenerateRandomUsers(teams);
            var projects = GenerateRandomProjects(users, teams);
            var tasks = GenerateRandomTasks(users, projects);

            modelBuilder.Entity<Team>().HasData(teams);
            modelBuilder.Entity<User>().HasData(users);
            modelBuilder.Entity<Project>().HasData(projects);
            modelBuilder.Entity<TaskModel>().HasData(tasks);
        }

        public static ICollection<Team> GenerateRandomTeams()
        {
            int teamId = 1;

            var teamsFake = new Faker<Team>()
                .RuleFor(p => p.Id, _ => teamId++)
                .RuleFor(p => p.Name, f => f.Company.CompanyName(0))
                .RuleFor(pi => pi.CreatedAt, f => f.Date.Between(new DateTime(2000, 1, 1), DateTime.Now));

            return teamsFake.Generate(TEAMS_COUNT);
        }

        public static ICollection<User> GenerateRandomUsers(this ICollection<Team> teams)
        {
            int userId = 1;

            var usersFake = new Faker<User>()
                .RuleFor(u => u.Id, _ => userId++)
                .RuleFor(u => u.FirstName, f => f.Person.FirstName)
                .RuleFor(u => u.LastName, f => f.Person.LastName)
                .RuleFor(u => u.Email, f => f.Person.Email)
                .RuleFor(pi => pi.Birthday, f => f.Date.Between(new DateTime(1990, 1, 1), new DateTime(2010, 1, 1)))
                .RuleFor(pi => pi.RegisteredAt, _ => DateTime.Now)
                .RuleFor(pi => pi.TeamId, f => f.PickRandom(teams).Id);

            return usersFake.Generate(USERS_COUNT);
        }

        public static ICollection<Project> GenerateRandomProjects(this ICollection<User> users, ICollection<Team> teams)
        {
            int projectId = 1;

            var projectsFake = new Faker<Project>()
                .RuleFor(c => c.Id, _ => projectId++)
                .RuleFor(c => c.Name, f => f.Commerce.Product())
                .RuleFor(c => c.Description, f => f.Lorem.Sentences())
                .RuleFor(c => c.AuthorId, f => f.PickRandom(users).Id)
                .RuleFor(c => c.TeamId, f => f.PickRandom(teams).Id)
                .RuleFor(pi => pi.CreatedAt, f => f.Date.Between(new DateTime(2015, 1, 1), new DateTime(2019, 1, 1)))
                .RuleFor(pi => pi.Deadline, f => f.Date.Future(5, new DateTime(2019, 1, 1)));

            return projectsFake.Generate(PROJECT_COUNT);
        }

        public static ICollection<TaskModel> GenerateRandomTasks(this ICollection<User> users, ICollection<Project> projects)
        {
            int taskId = 1;

            var tasksFake = new Faker<TaskModel>()
                .RuleFor(c => c.Id, _ => taskId++)
                .RuleFor(c => c.Name, f => f.Lorem.Sentence())
                .RuleFor(c => c.Description, f => f.Lorem.Sentences())
                .RuleFor(c => c.ProjectId, f => f.PickRandom(projects).Id)
                .RuleFor(c => c.PerformerId, f => f.PickRandom(users).Id)
                .RuleFor(c => c.State, f => f.Random.Enum<TaskState>())
                .RuleFor(pi => pi.CreatedAt, f => f.Date.Between(new DateTime(2015, 1, 1), new DateTime(2019, 1, 1)))
                .RuleFor(pi => pi.FinishedAt, f => f.Date.Future(5, new DateTime(2019, 1, 1)));

            return tasksFake.Generate(TASKS_COUNT);
        }
    }
}
