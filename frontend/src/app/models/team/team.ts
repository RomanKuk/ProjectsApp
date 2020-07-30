import { User } from "../user/user";

export interface Team {
    id: number,
    name: string,
    createdAt: Date,
    participants: User[]
}