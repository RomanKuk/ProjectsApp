import { Pipe, PipeTransform } from '@angular/core';
import { TaskState } from 'src/app/models/task-state/task-state.model';

@Pipe({
  name: 'taskStatusString'
})
export class TaskStatusStringPipe implements PipeTransform {

  transform(value: TaskState): string {
    return TaskState[value];
  }

}
