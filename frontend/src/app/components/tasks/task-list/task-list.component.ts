import { Component, OnInit, EventEmitter, Output } from '@angular/core';
import { takeUntil } from 'rxjs/operators';
import { TaskModel } from 'src/app/models/task/task.model';
import { TaskService } from 'src/app/services/task.service';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-task-list',
  templateUrl: './task-list.component.html',
  styleUrls: ['./task-list.component.css']
})
export class TaskListComponent implements OnInit {
  @Output() taskWasSelected = new EventEmitter<TaskModel>();
  tasks: TaskModel[] = [];
  firstTask: TaskModel;
  private unsubscribe$ = new Subject<void>();

  constructor(
    private taskService: TaskService
  ) { }

  ngOnInit() {
    this.getTasks();
  }

  public ngOnDestroy() {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
}

  onTaskSelected(task: TaskModel) {
   this.taskWasSelected.emit(task);
  }

  public getTasks() {
    this.taskService
        .getTasks()
        .pipe(takeUntil(this.unsubscribe$))
        .subscribe(
            (resp) => {
                this.tasks = resp.body;
                if(this.tasks.length !== 0)
                {
                  this.firstTask = this.tasks[0];
                  this.onTaskSelected(this.firstTask);
                }
            },
            (error) => {console.error()}
        );
}
}
