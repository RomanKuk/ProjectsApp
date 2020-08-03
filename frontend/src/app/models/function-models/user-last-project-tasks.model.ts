import { User } from "../user/user.model";
import { Project } from "../project/project.model";
import { TaskModel } from "../task/task.model";

export interface UserLastProjectTasks {
    user: User,
    lastProject: Project,
    lastProjectTasksCount: number | undefined,
    undoneOrCanceledTasksCount: number,
    longestTask: TaskModel
}