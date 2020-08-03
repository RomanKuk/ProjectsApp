import { Component, OnInit, OnDestroy } from '@angular/core';
import { TaskModel } from 'src/app/models/task/task.model';
import { TaskService } from '../../../../services/task.service';
import { Subject } from 'rxjs';
import { Router, ActivatedRoute } from '@angular/router';
import { takeUntil } from 'rxjs/operators';
import { SnackBarService } from 'src/app/services/snack-bar.service';

@Component({
  selector: 'app-task-list',
  templateUrl: './task-list.component.html',
  styleUrls: ['./task-list.component.css']
})
export class TaskListComponent implements OnInit, OnDestroy {
  tasks: TaskModel[] = [];
  private unsubscribe$ = new Subject<void>();

  constructor(
    private taskService: TaskService,
    private snackbarService: SnackBarService,
    private router: Router,
    private route: ActivatedRoute
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
            },
            (error) => {this.snackbarService.showErrorMessage(error.message); }
        );
}

onNewTask(): void {
  this.router.navigate(['new'], {relativeTo: this.route});
}

}
