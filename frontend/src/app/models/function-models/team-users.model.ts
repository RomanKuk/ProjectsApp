import { User } from "../user/user.model";

export interface TeamUsers {
    id: number,
    name: string,
    users: User[]
}