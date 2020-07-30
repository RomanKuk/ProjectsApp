import { User } from "../user/user";
import { Team } from "../team/team";
import { TaskModel } from "../task/task";

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