import { User } from "../user/user.model";
import { Team } from "../team/team.model";
import { TaskModel } from "../task/task.model";

export interface Project {
    id: number,
    name: string,
    description: string,
    createdAt: Date,
    deadline: Date,
    author: User,
    team: Team,
    tasks: TaskModel[] 
}