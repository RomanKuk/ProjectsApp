import { Component, OnInit, Input } from '@angular/core';
import { TaskModel } from '../../../../models/task/task.model';
import { TaskService } from 'src/app/services/task.service';

@Component({
  selector: 'app-task-item',
  templateUrl: './task-item.component.html',
  styleUrls: ['./task-item.component.css']
})
export class TaskItemComponent implements OnInit {
  @Input() task: TaskModel;

  constructor(private taskService: TaskService) { }

  ngOnInit() {
  }

  onSelected() {
    this.taskService.taskSelected.emit(this.task);
  }
}
