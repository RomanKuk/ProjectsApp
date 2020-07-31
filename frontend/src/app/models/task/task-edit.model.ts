import { TaskState } from "../task-state/task-state.model";

export interface TaskModelEdit {
    id: number
    name: string,
    description: string,
    state: TaskState,
    finishedAt: string,
    projectId: number,
    performerId: number
}