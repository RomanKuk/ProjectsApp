import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { TaskModel } from 'src/app/models/task/task.model';

@Component({
  selector: 'app-tasks',
  templateUrl: './tasks.component.html',
  styleUrls: ['./tasks.component.css']
})
export class TasksComponent implements OnInit {
  selectedTask: TaskModel;

  constructor() { }

  ngOnInit(): void {
  }

}
