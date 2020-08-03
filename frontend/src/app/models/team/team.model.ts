import { User } from '../user/user.model';

export interface Team {
    id: number;
    name: string;
    createdAt: Date;
    participants: User[];
}
