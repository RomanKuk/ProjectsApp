export interface UserCreate {
    firstName: string,
    lastName: string,
    email: string,
    birthday: Date,
    teamId: number | undefined
}