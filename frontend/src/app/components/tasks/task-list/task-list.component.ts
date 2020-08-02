import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { TaskModel } from 'src/app/models/task/task.model';
import { TaskService } from 'src/app/services/task.service';

@Component({
  selector: 'app-task-list',
  templateUrl: './task-list.component.html',
  styleUrls: ['./task-list.component.css']
})
export class TaskListComponent implements OnInit, OnDestroy {
  tasks: TaskModel[] = [];
  private unsubscribe$ = new Subject<void>();

  constructor(
    private taskService: TaskService
  ) { }

  ngOnInit(): void {
    this.getTasks();
  }

  public ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
}

  public getTasks(): void {
    this.taskService
        .getTasks()
        .pipe(takeUntil(this.unsubscribe$))
        .subscribe(
            (resp) => {
                this.tasks = resp.body;
                if (this.tasks.length !== 0)
                {
                  this.taskService.taskSelected.emit(this.tasks[0]);
                }
            },
            (error) => {console.error(); }
        );
}
}
