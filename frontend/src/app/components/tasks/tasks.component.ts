import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { TaskModel } from 'src/app/models/task/task.model';
import { TaskState } from 'src/app/models/task-state/task-state.model';
import { TaskService } from 'src/app/services/task.service';

@Component({
  selector: 'app-tasks',
  templateUrl: './tasks.component.html',
  styleUrls: ['./tasks.component.css'],
  providers: [TaskService]
})
export class TasksComponent implements OnInit {
  selectedTask: TaskModel;

  constructor(private taskService: TaskService) { }

  ngOnInit(): void {
    this.taskService.taskSelected
      .subscribe(
        (task: TaskModel) => {
          this.selectedTask = task;
        }
      );
  }

}
