import { User } from "../user/user.model";
import { TaskModel } from "../task/task.model";

export interface UserTasks {
    user: User,
    tasks: TaskModel[]
}