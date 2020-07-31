export interface UserEdit {
    id: number,
    firstName: string,
    lastName: string,
    email: string,
    birthday: Date,
    teamId: number | undefined
}