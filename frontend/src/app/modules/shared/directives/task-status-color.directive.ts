import { Directive, ElementRef, HostListener, Input } from '@angular/core';
import { TaskState } from 'src/app/models/task-state/task-state.model';

@Directive({
  selector: '[appTaskStatusColor]'
})
export class TaskStatusColorDirective {
  @Input('appTaskStatusColor') taskStatus: TaskState;

  constructor(private el: ElementRef) { }

  private setColor(color: string): void {
    this.el.nativeElement.style.color = color;
  }

  @HostListener('DOMSubtreeModified')
  onTaskStatusChange(): void
  {
    switch (this.taskStatus)
    {
      case TaskState.Created: this.setColor('grey'); break;
      case TaskState.Started: this.setColor('blue'); break;
      case TaskState.Canceled: this.setColor('red'); break;
      case TaskState.Finished: this.setColor('green'); break;
    }
  }
}
