using Collections_LINQ.Services;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Collections_LINQ
{
    internal static class Program
    {
        private static readonly string _baseUrl = Resource.Uri;

        private static async Task Main()
        {
            var httpClient = new HttpClient();
            var appClient = new AppService(_baseUrl, httpClient);

            while (true)
            {
                Console.WriteLine("Select a command:");
                Console.WriteLine("1\t-\tPrint task count per project of certain user");
                Console.WriteLine("2\t-\tPrint tasks of certain user (length of task name < 45)");
                Console.WriteLine("3\t-\tPrint tasks (id, name) of certain user which have been finished in the current (2020) year");
                Console.WriteLine("4\t-\tPrint list of teams (id, name, users), users age > 10, " +
                                "sorted in descending order by registered date, grouped by teams");
                Console.WriteLine("5\t-\tPrint users sorted by first name whose tasks are sorted in descending order by length of name");
                Console.WriteLine("6\t-\tPrint structure:\n\t\t\tUser\n\t\t\tLast Project\n\t\t\tOverall tasks count of last project\n\t\t\t" +
                                "Overall count of undone or canceled tasks\n\t\t\tThe longest task");
                Console.WriteLine("7\t-\tPrint structure:\n\t\t\tProject\n\t\t\tThe longest project task by description\n\t\t\t" +
                                "The shortest project task by name\n\t\t\tOverall users count in project team");
                Console.WriteLine("8\t-\tCreate a new project");
                Console.WriteLine("9\t-\tUpdate a project");
                Console.WriteLine("10\t-\tDelete a project");
                Console.WriteLine("11\t-\tCreate a new task");
                Console.WriteLine("12\t-\tUpdate a task");
                Console.WriteLine("13\t-\tDelete a task");
                Console.WriteLine("14\t-\tCreate a new team");
                Console.WriteLine("15\t-\tUpdate a team");
                Console.WriteLine("16\t-\tDelete a team");
                Console.WriteLine("17\t-\tCreate a new user");
                Console.WriteLine("18\t-\tUpdate a user");
                Console.WriteLine("19\t-\tDelete a user");
                Console.WriteLine("20\t-\tPrint all projects");
                Console.WriteLine("21\t-\tPrint all tasks");
                Console.WriteLine("22\t-\tPrint all teams");
                Console.WriteLine("23\t-\tPrint all users");
                Console.WriteLine("24\t-\tPrint project by id");
                Console.WriteLine("25\t-\tPrint task by id");
                Console.WriteLine("26\t-\tPrint team by id");
                Console.WriteLine("27\t-\tPrint user by id");
                Console.WriteLine("28\t-\tPrint unfinished tasks of user by id");
                Console.WriteLine("29\t-\tMark random task as finished");
                Console.WriteLine("0\t-\tExit");
                Console.WriteLine();

                var commandString = Console.ReadLine();

                Console.WriteLine();

                try
                {
                    int command = int.Parse(commandString);
                    int id;
                    switch (command)
                    {
                        case 0:
                            return;
                        case 1:
                            Console.WriteLine("Input id of a user:\t");
                            id = int.Parse(Console.ReadLine());
                            Console.WriteLine();
                            if (await appClient.IsUserExistAsync(id))
                                await appClient.PrintUserProjectTasksCountByUserIdAsync(id);
                            break;
                        case 2:
                            Console.WriteLine("Input id of a user:\t");
                            id = int.Parse(Console.ReadLine());
                            Console.WriteLine();
                            if (await appClient.IsUserExistAsync(id))
                            {
                                await appClient.PrintTasksByUserIdWithNameConditionAsync(id);
                            }

                            break;
                        case 3:
                            Console.WriteLine("Input id of a user:\t");
                            id = int.Parse(Console.ReadLine());
                            Console.WriteLine();
                            if (await appClient.IsUserExistAsync(id))
                                await appClient.PrintTasksFinishedIn2020ByUserIdAsync(id);
                            break;
                        case 4:
                            await appClient.PrintTeamsUsersSortedByRegisteredDateAsync();
                            break;
                        case 5:
                            await appClient.PrintUsersSortedByFirstNameAndSortedTasksAsync();
                            break;
                        case 6:
                            Console.WriteLine("Input id of a user:\t");
                            id = int.Parse(Console.ReadLine());
                            Console.WriteLine();
                            if (await appClient.IsUserExistAsync(id))
                                await appClient.PrintNewUserStructureAsync(id);
                            break;
                        case 7:
                            await appClient.PrintNewProjectStructureAsync();
                            break;
                        case 8:
                            await appClient.CreateProjectAsync();
                            break;
                        case 9:
                            await appClient.UpdateProjectAsync();
                            break;
                        case 10:
                            await appClient.DeleteProjectAsync();
                            break;
                        case 11:
                            await appClient.CreateTaskAsync();
                            break;
                        case 12:
                            await appClient.UpdateTaskAsync();
                            break;
                        case 13:
                            await appClient.DeleteTaskAsync();
                            break;
                        case 14:
                            await appClient.CreateTeamAsync();
                            break;
                        case 15:
                            await appClient.UpdateTeamAsync();
                            break;
                        case 16:
                            await appClient.DeleteTeamAsync();
                            break;
                        case 17:
                            await appClient.CreateUserAsync();
                            break;
                        case 18:
                            await appClient.UpdateUserAsync();
                            break;
                        case 19:
                            await appClient.DeleteUserAsync();
                            break;
                        case 20:
                            await appClient.GetAllProjectsAsync();
                            break;
                        case 21:
                            await appClient.GetAllTasksAsync();
                            break;
                        case 22:
                            await appClient.GetAllTeamsAsync();
                            break;
                        case 23:
                            await appClient.GetAllUsersAsync();
                            break;
                        case 24:
                            Console.WriteLine("Input id of a project:\t");
                            id = int.Parse(Console.ReadLine());
                            Console.WriteLine();
                            await appClient.GetProjectById(id);
                            break;
                        case 25:
                            Console.WriteLine("Input id of a task:\t");
                            id = int.Parse(Console.ReadLine());
                            Console.WriteLine();
                            await appClient.GetTaskById(id);
                            break;
                        case 26:
                            Console.WriteLine("Input id of a team:\t");
                            id = int.Parse(Console.ReadLine());
                            Console.WriteLine();
                            await appClient.GetTeamById(id);
                            break;
                        case 27:
                            Console.WriteLine("Input id of a user:\t");
                            id = int.Parse(Console.ReadLine());
                            Console.WriteLine();
                            await appClient.GetUserById(id);
                            break;
                        case 28:
                            Console.WriteLine("Input id of a user:\t");
                            id = int.Parse(Console.ReadLine());
                            Console.WriteLine();
                            if (await appClient.IsUserExistAsync(id))
                                await appClient.PrintGetUnfinishedTasksByUser(id);
                            break;
                        case 29:
                            appClient.StartMarkingTasks();
                            break;
                        default:
                            throw new FormatException();
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Incorrect command");
                }
                Console.WriteLine();
            }
        }
    }
}
