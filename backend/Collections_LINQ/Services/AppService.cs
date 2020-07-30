using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Http;
using System.Linq;
using System.Threading.Tasks;
using Collections_LINQ.Interfaces;
using Common.DTOs.Project;
using Common.DTOs.TaskModel;
using Common.DTOs.Team;
using Common.DTOs.User;
using System.Timers;

namespace Collections_LINQ.Services
{
    internal class AppService
    {
        private readonly IProjectService _projectService;
        private readonly ITaskService _taskService;
        private readonly ITeamService _teamService;
        private readonly IUserService _userService;

        public AppService(string baseUrl, HttpClient client)
        {
            _projectService = new ProjectService(baseUrl, client);
            _taskService = new TaskService(baseUrl, client);
            _teamService = new TeamService(baseUrl, client);
            _userService = new UserService(baseUrl, client);
        }

        public async Task StartMarkingTasks()
        {
            try
            {
                var markedTaskId = await _taskService.MarkRandomTaskWithDelay(1000);
                Console.WriteLine($"Task with id - {markedTaskId} is finished");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task GetAllProjectsAsync()
        {
            foreach (var project in await _projectService.GetProjects())
            {
                Console.WriteLine(project);
                Console.WriteLine("###########################################################\n");
            }
        }

        public async Task GetAllTasksAsync()
        {
            foreach (var task in await _taskService.GetTasks())
            {
                Console.WriteLine(task);
                Console.WriteLine("###########################################################\n");
            }
        }

        public async Task GetAllTeamsAsync()
        {
            foreach (var team in await _teamService.GetTeams())
            {
                Console.WriteLine(team);
                Console.WriteLine("###########################################################\n");
            }
        }

        public async Task GetAllUsersAsync()
        {
            foreach (var user in await _userService.GetUsers())
            {
                Console.WriteLine(user);
                Console.WriteLine("###########################################################\n");
            }
        }

        public async Task GetProjectById(int id)
        {
            if (await IsProjectExistAsync(id))
                Console.WriteLine(await _projectService.GetProjectById(id));
        }
        public async Task GetTaskById(int id)
        {
            if (await IsTaskExistAsync(id))
                Console.WriteLine(await _taskService.GetTaskById(id));
        }
        public async Task GetTeamById(int id)
        {
            if (await IsTeamExistAsync(id))
                Console.WriteLine(await _teamService.GetTeamById(id));
        }
        public async Task GetUserById(int id)
        {
            if (await IsUserExistAsync(id))
                Console.WriteLine(await _userService.GetUserById(id));
        }

        public async Task PrintUserProjectTasksCountByUserIdAsync(int userId)
        {
            var tasks = (await _projectService.GetUserProjectTasksCountByUserId(userId)).ToList();
            if(tasks.Count == 0)
            {
                Console.WriteLine("User is not an author of any project");
                return;
            }
            foreach (var t in tasks)
            {
                Console.WriteLine($"Project:\n[\n{t.Key}\n]\n\nTaskCount:\t{t.Value}\n" +
                "#########################################\n");
            }
        }

        public async Task PrintTasksByUserIdWithNameConditionAsync(int userId)
        {
            var tasks = (await _taskService.GetTasksByUserIdWithNameCondition(userId)).ToList();

            if(tasks.Count == 0)
            {
                Console.WriteLine("User is not a performer of any task");
                return;
            }
            Console.WriteLine("Tasks:\n[");
            foreach (var t in tasks)
            {
                Console.WriteLine(t.ToString());
                Console.WriteLine("#################################\n");
            }
            Console.WriteLine("]\n");
        }

        public async Task PrintTasksFinishedIn2020ByUserIdAsync(int userId)
        {
            var tasks = (await _taskService.GetTasksByUserFinishedThisYear(userId)).ToList();

            if (tasks.Count == 0)
            {
                Console.WriteLine("No tasks of this user has been finished in 2020\n");
                return;
            }
            foreach (var t in tasks)
            {
                Console.WriteLine($"id:\t{t.Id}\nname:\t{t.Name}\n");
            }
        }

        public async Task PrintGetUnfinishedTasksByUser(int userId)
        {
            var tasks = (await _taskService.GetUnfinishedTasksByUser(userId));

            if (!tasks.Any())
            {
                Console.WriteLine("All tasks of this user are finished\n");
                return;
            }
            foreach (var t in tasks)
            {
                Console.WriteLine(t);
            }
        }

        public async Task PrintTeamsUsersSortedByRegisteredDateAsync()
        {
            var teams = (await _teamService.GetTeamsUsersSortedByRegisteredDate()).ToList();

            foreach(var t in teams)
            {
                Console.WriteLine($"id:\t{t.Id}\nname:\t{t.Name}\nusers:\n[");
                if (t.Users.Count == 0)
                {
                    Console.WriteLine("\tNo Users satisfying the condition");
                }
                else
                {
                    foreach (var user in t.Users)
                    {
                        Console.WriteLine("\t" + user.ToString() + "\n");
                    }
                }
                Console.Write("]\t\n\n");
            }
        }

        public async Task PrintUsersSortedByFirstNameAndSortedTasksAsync()
        {
            var users = (await _taskService.GetUsersSortedByFirstNameAndSortedTasks()).ToList();

            foreach (var u in users)
            {
                Console.WriteLine($"user:\n[\n\t{u.User}\n]\ntasks:\n[");
                if (u.Tasks.Count == 0)
                {
                    Console.WriteLine("\tThis user has no tasks\n");
                }
                else {
                    foreach (var task in u.Tasks)
                    {
                        Console.WriteLine(task.ToString());
                    }
                }

                Console.Write("]\t\n##############################\n");
            }
        }

        public async Task PrintNewUserStructureAsync(int userId)
        {
            var structure = (await _taskService.GetNewUserStructure(userId)).ToList();

            foreach (var s in structure)
            {
                Console.WriteLine($"user:\n[\n\t{s.User}\n]\n\n" +
                    $"last project:\n[\n\t{(s.LastProject == null ? "This user has no project" : s.LastProject.ToString())}\n]\n\n" +
                    "total tasks count of last project:\t" +
                    $"{(s.LastProjectTasksCount.HasValue ? s.LastProjectTasksCount.ToString() : "As this user has no project, the user has no task of it")}\n\n" +
                    $"undone or canceled tasks count:\t{s.UndoneOrCanceledTasksCount}\n\n" +
                    $"longest task:\n[\n{s.LongestTask}\n]\n##############################\n");
            }
        }

        public async Task PrintNewProjectStructureAsync()
        {
            var structure = (await _projectService.GetNewProjectStructure()).ToList();

            foreach (var s in structure)
            {
                Console.WriteLine($"project:\n[\n\t{s.Project}\n]\n\n" +
                    $"the longest project task by descriprion:\n[\n{s.LongestTaskByDescription}\n]\n\n" +
                    $"the shortest project task by name:\n[\n{s.ShortestTaskByName}\n]\n\n" +
                    $"total users count in project team:\t{s.TotalUsersCount}\n" +
                    "##############################\n");
            }
        }

        public async Task<bool> IsUserExistAsync(int userId)
        {
            if (!(await _userService.GetUsers()).Any(user => user.Id == userId))
            {
                Console.WriteLine("User with such id doesn't exist!");
                return false;
            }
            return true;
        }

        public async Task<bool> IsProjectExistAsync(int projectId)
        {
            if (!(await _projectService.GetProjects()).Any(project => project.Id == projectId))
            {
                Console.WriteLine("Project with such id doesn't exist!");
                return false;
            }
            return true;
        }

        public async Task<bool> IsTaskExistAsync(int taskId)
        {
            if (!(await _taskService.GetTasks()).Any(task => task.Id == taskId))
            {
                Console.WriteLine("Task with such id doesn't exist!");
                return false;
            }
            return true;
        }

        public async Task<bool> IsTeamExistAsync(int teamId)
        {
            if (!(await _teamService.GetTeams()).Any(team => team.Id == teamId))
            {
                Console.WriteLine("Team with such id doesn't exist!");
                return false;
            }
            return true;
        }

        public async Task DeleteProjectAsync()
        {
            if ((await _projectService.GetProjects()).Count == 0)
            {
                Console.WriteLine("There is no project!");
                return;
            }

            Console.WriteLine("Enter id of the project you want to remove:");
            var id = Console.ReadLine();
            try
            {
                var idNum = int.Parse(id);
                await _projectService.DeleteProject(idNum);
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async Task DeleteTaskAsync()
        {
            if ((await _taskService.GetTasks()).Count == 0)
            {
                Console.WriteLine("There is no task!");
                return;
            }

            Console.WriteLine("Enter id of the task you want to remove:");
            var id = Console.ReadLine();
            try
            {
                var idNum = int.Parse(id);
                await _taskService.DeleteTask(idNum);
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async Task DeleteTeamAsync()
        {
            if ((await _teamService.GetTeams()).Count == 0)
            {
                Console.WriteLine("There is no team!");
                return;
            }

            Console.WriteLine("Enter id of the team you want to remove:");
            var id = Console.ReadLine();
            try
            {
                var idNum = int.Parse(id);
                await _teamService.DeleteTeam(idNum);
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async Task DeleteUserAsync()
        {
            if ((await _userService.GetUsers()).Count == 0)
            {
                Console.WriteLine("There is no users!");
                return;
            }

            Console.WriteLine("Enter id of the user you want to remove:");
            var id = Console.ReadLine();
            try
            {
                var idNum = int.Parse(id);
                await _userService.DeleteUser(idNum);
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async Task CreateProjectAsync()
        {
            Console.WriteLine("Input name of the project (or type ? to skip):");
            var projectNameString = Console.ReadLine();

            Console.WriteLine("Input decription of the project (or type ? to skip):");
            var projectDescriptionString = Console.ReadLine();

            Console.WriteLine("Input deadline of the project (dd-mm-yyy):");
            var projectDeadlineString = Console.ReadLine();

            Console.WriteLine("Choose user to be an author of the project:");
            foreach (var user in (await _userService.GetUsers()))
                Console.WriteLine($"\t{user.Id} - {user.FirstName} {user.LastName}");

            var projectAuthorId = Console.ReadLine();

            Console.WriteLine("Choose team of the project:");
            foreach (var team in (await _teamService.GetTeams()))
                Console.WriteLine($"\t{team.Id} - {team.Name}");

            var projectTeamId = Console.ReadLine();

            try
            {
                var projectName = projectNameString == "?" ? null : projectNameString;
                var projectDescription = projectDescriptionString == "?" ? null : projectDescriptionString;
                var projectDeadline = DateTime.Parse(projectDeadlineString);
                int authorId = int.Parse(projectAuthorId);
                int teamId = int.Parse(projectTeamId);

                await _projectService.CreateProject(new ProjectCreateDTO()
                {
                    Name = projectName,
                    Description = projectDescription,
                    Deadline = projectDeadline,
                    AuthorId = authorId,
                    TeamId = teamId
                });
            }
            catch (FormatException e)
            {
                Console.WriteLine($"Unable to parse '{e.Data}'");
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async Task CreateTaskAsync()
        {
            Console.WriteLine("Input name of the task (or type ? to skip):");
            var taskNameString = Console.ReadLine();

            Console.WriteLine("Input decription of the task (or type ? to skip):");
            var taskDescriptionString = Console.ReadLine();

            Console.WriteLine("Input when task will be finished (dd-mm-yyy):");
            var taskDeadlineString = Console.ReadLine();

            Console.WriteLine("Choose user to be a performer of the task:");
            foreach (var user in (await _userService.GetUsers()))
                Console.WriteLine($"\t{user.Id} - {user.FirstName} {user.LastName}");

            var taskPerformerId = Console.ReadLine();

            Console.WriteLine("Choose project for the task:");
            foreach (var project in (await _projectService.GetProjects()))
                Console.WriteLine($"\t{project.Id} - {project.Name}");

            var taskProjectId = Console.ReadLine();

            try
            {
                var taskName = taskNameString == "?" ? null : taskNameString;
                var taskDescription = taskDescriptionString == "?" ? null : taskDescriptionString;
                var taskDeadline = DateTime.Parse(taskDeadlineString);
                int performerId = int.Parse(taskPerformerId);
                int projectId = int.Parse(taskProjectId);

                await _taskService.CreateTask(new TaskModelCreateDTO()
                {
                    Name = taskName,
                    Description = taskDescription,
                    FinishedAt = taskDeadline,
                    PerformerId = performerId,
                    ProjectId = projectId
                });
            }
            catch (FormatException e)
            {
                Console.WriteLine($"Unable to parse '{e.Data}'");
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async Task CreateTeamAsync()
        {
            Console.WriteLine("Input name of the team (or type ? to skip):");
            var teamNameString = Console.ReadLine();

            try
            {
                var teamName = teamNameString == "?" ? null : teamNameString;

                await _teamService.CreateTeam(new TeamCreateDTO()
                {
                    Name = teamName
                });
            }
            catch (FormatException e)
            {
                Console.WriteLine($"Unable to parse '{e.Data}'");
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async Task CreateUserAsync()
        {
            Console.WriteLine("Input first name of the user (or type ? to skip):");
            var userFirstNameString = Console.ReadLine();

            Console.WriteLine("Input last name of the user (or type ? to skip):");
            var userLastNameString = Console.ReadLine();

            Console.WriteLine("Input email of the user (or type ? to skip):");
            var userEmailString = Console.ReadLine();

            Console.WriteLine("Input birthday date (dd-mm-yyy):");
            var userBirthdayString = Console.ReadLine();

            Console.WriteLine("Choose team of the user (or 0 for skip):");
            foreach (var team in (await _teamService.GetTeams()))
                Console.WriteLine($"\t{team.Id} - {team.Name}");

            var userTeamId = Console.ReadLine();

            try
            {
                var firstName = userFirstNameString == "?" ? null : userFirstNameString;
                var lastName = userLastNameString == "?" ? null : userLastNameString;
                var email = userEmailString == "?" ? null : userEmailString;
                var birthday = DateTime.Parse(userBirthdayString);
                int teamId = int.Parse(userTeamId);

                await _userService.CreateUser(new UserCreateDTO()
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    Birthday = birthday,
                    TeamId = teamId
                });
            }
            catch (FormatException e)
            {
                Console.WriteLine($"Unable to parse '{e.Data}'");
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async Task UpdateTaskAsync()
        {
            TaskModelDTO task = null;

            if ((await _taskService.GetTasks()).Count == 0)
            {
                Console.WriteLine("There is no task!");
                return;
            }

            Console.WriteLine("Enter id of the task you want to update:");
            var id = Console.ReadLine();
            try
            {
                int idNum = int.Parse(id);
                task = await _taskService.GetTaskById(idNum);
                if (task == null)
                    throw new NullReferenceException();
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (FormatException e)
            {
                Console.WriteLine($"Unable to parse '{e.Data}'");
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine("Input new name of the task (or type ? to skip):");
            var taskNameString = Console.ReadLine();

            Console.WriteLine("Input new decription of the task (or type ? to skip):");
            var taskDescriptionString = Console.ReadLine();

            Console.WriteLine("Input new data when task will be finished (dd-mm-yyy) (or type ? to skip):");
            var taskDeadlineString = Console.ReadLine();

            Console.WriteLine("Choose new user to be a performer of the task (or type ? to skip):");
            foreach (var user in (await _userService.GetUsers()))
                Console.WriteLine($"\t{user.Id} - {user.FirstName} {user.LastName}");

            var taskPerformerId = Console.ReadLine();

            Console.WriteLine("Choose project for the task (or type ? to skip):");
            foreach (var project in await _projectService.GetProjects())
                Console.WriteLine($"\t{project.Id} - {project.Name}");

            var taskProjectId = Console.ReadLine();

            try
            {
                var taskName = taskNameString == "?" ? task.Name : taskNameString;
                var taskDescription = taskDescriptionString == "?" ? task.Description : taskDescriptionString;
                var taskDeadline = taskDeadlineString == "?" ? task.FinishedAt : DateTime.Parse(taskDeadlineString);
                var performerId = taskPerformerId == "?" ? task.Performer.Id : int.Parse(taskPerformerId);
                var projectId = taskProjectId == "?" ? task.ProjectId : int.Parse(taskProjectId);

                await _taskService.UpdateTask(new TaskModelUpdateDTO()
                {
                    Id = task.Id,
                    Name = taskName,
                    Description = taskDescription,
                    FinishedAt = taskDeadline,
                    State = task.State,
                    PerformerId = performerId,
                    ProjectId = projectId
                });
            }
            catch (FormatException e)
            {
                Console.WriteLine($"Unable to parse '{e.Data}'");
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async Task UpdateProjectAsync()
        {
            ProjectDTO project = null;

            if ((await _projectService.GetProjects()).Count == 0)
            {
                Console.WriteLine("There is no project!");
                return;
            }

            Console.WriteLine("Enter id of the project you want to update:");
            var id = Console.ReadLine();
            try
            {
                int idNum = int.Parse(id);
                project = await _projectService.GetProjectById(idNum);
                if (project == null)
                    throw new NullReferenceException();
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (FormatException e)
            {
                Console.WriteLine($"Unable to parse '{e.Data}'");
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine("Input new name of the project (or type ? to skip):");
            var projectNameString = Console.ReadLine();

            Console.WriteLine("Input new decription of the project (or type ? to skip):");
            var projectDescriptionString = Console.ReadLine();

            Console.WriteLine("Input new deadline of the project (dd-mm-yyy) (or type ? to skip):");
            var projectDeadlineString = Console.ReadLine();

            Console.WriteLine("Choose new user to be an author of the project (or type ? to skip):");
            foreach (var user in (await _userService.GetUsers()))
                Console.WriteLine($"\t{user.Id} - {user.FirstName} {user.LastName}");

            var projectAuthorId = Console.ReadLine();

            Console.WriteLine("Choose new team of the project (or type ? to skip):");
            foreach (var team in (await _teamService.GetTeams()))
                Console.WriteLine($"\t{team.Id} - {team.Name}");

            var projectTeamId = Console.ReadLine();

            try
            {
                var projectName = projectNameString == "?" ? null : projectNameString;
                var projectDescription = projectDescriptionString == "?" ? null : projectDescriptionString;
                var projectDeadline = projectDeadlineString == "?" ? project.Deadline : DateTime.Parse(projectDeadlineString);
                int authorId = projectAuthorId == "?" ? project.Author.Id : int.Parse(projectAuthorId);
                int teamId = projectTeamId == "?" ? project.Team.Id : int.Parse(projectTeamId);

                await _projectService.UpdateProject(new ProjectUpdateDTO()
                {
                    Id = project.Id,
                    Name = projectName,
                    Description = projectDescription,
                    Deadline = projectDeadline,
                    AuthorId = authorId,
                    TeamId = teamId
                });
            }
            catch (FormatException e)
            {
                Console.WriteLine($"Unable to parse '{e.Data}'");
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async Task UpdateTeamAsync()
        {
            TeamDTO team = null;

            if ((await _projectService.GetProjects()).Count == 0)
            {
                Console.WriteLine("There is no team!");
                return;
            }

            Console.WriteLine("Enter id of the team you want to update:");
            var id = Console.ReadLine();
            try
            {
                int idNum = int.Parse(id);
                team = await _teamService.GetTeamById(idNum);
                if (team == null)
                    throw new NullReferenceException();
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (FormatException e)
            {
                Console.WriteLine($"Unable to parse '{e.Data}'");
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine("Input new name of the team (or type ? to skip):");
            var teamNameString = Console.ReadLine();

            try
            {
                var teamName = teamNameString == "?" ? null : teamNameString;

                await _teamService.UpdateTeam(new TeamUpdateDTO()
                {
                    Id = team.Id,
                    Name = teamName
                });
            }
            catch (FormatException e)
            {
                Console.WriteLine($"Unable to parse '{e.Data}'");
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async Task UpdateUserAsync()
        {
            UserDTO user = null;

            if ((await _userService.GetUsers()).Count == 0)
            {
                Console.WriteLine("There is no user!");
                return;
            }

            Console.WriteLine("Enter id of the user you want to update:");
            var id = Console.ReadLine();
            try
            {
                int idNum = int.Parse(id);
                user = await _userService.GetUserById(idNum);
                if (user == null)
                    throw new NullReferenceException();
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (FormatException e)
            {
                Console.WriteLine($"Unable to parse '{e.Data}'");
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine("Input new first name of the user (or type ? to skip):");
            var userFirstNameString = Console.ReadLine();

            Console.WriteLine("Input new last name of the user (or type ? to skip):");
            var userLastNameString = Console.ReadLine();

            Console.WriteLine("Input new email of the user (or type ? to skip):");
            var userEmailString = Console.ReadLine();

            Console.WriteLine("Input new birthday date (dd-mm-yyy) (or type ? to skip):");
            var userBirthdayString = Console.ReadLine();

            Console.WriteLine("Choose new team of the user (or type ? to skip):");
            foreach (var team in (await _teamService.GetTeams()))
                Console.WriteLine($"\t{team.Id} - {team.Name}");

            var userTeamId = Console.ReadLine();

            try
            {
                var firstName = userFirstNameString == "?" ? null : userFirstNameString;
                var lastName = userLastNameString == "?" ? null : userLastNameString;
                var email = userEmailString == "?" ? null : userEmailString;
                var birthday = userBirthdayString == "?" ? user.Birthday : DateTime.Parse(userBirthdayString);
                var teamId = userTeamId == "?" ? user.TeamId : int.Parse(userTeamId);

                await _userService.UpdateUser(new UserUpdateDTO()
                {
                    Id = user.Id,
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    Birthday = birthday,
                    TeamId = teamId
                });
            }
            catch (FormatException e)
            {
                Console.WriteLine($"Unable to parse '{e.Data}'");
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
