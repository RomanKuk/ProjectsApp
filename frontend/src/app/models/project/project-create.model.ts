export interface ProjectCreate {
    name: string,
    description: string,
    deadline: Date,
    authorId: number,
    teamId: number,
}