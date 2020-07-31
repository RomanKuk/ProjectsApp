export interface TaskModelCreate{
    name: string,
    description: string,
    finishedAt: string,
    projectId: number,
    performerId: number
}