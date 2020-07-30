import { User } from "../user/user";
import { TaskState } from "../task-state/task-state";

export interface TaskModel{
    id: number,
    name: string,
    description: string,
    createdAt: string,
    finishedAt: string,
    state: TaskState,
    projectId: number,
    performer: User
}