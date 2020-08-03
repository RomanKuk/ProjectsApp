import { TaskModel } from "../task/task.model";
import { Project } from "../project/project.model";

export interface ProjectTasks {
    project: Project,
    longestTaskByDescription: TaskModel,
    shortestTaskByName: TaskModel,
    totalUsersCount: number
}