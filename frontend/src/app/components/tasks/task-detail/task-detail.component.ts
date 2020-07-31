import { Component, OnInit, Input } from '@angular/core';
import { TaskModel } from '../../../models/task/task.model';

@Component({
  selector: 'app-task-detail',
  templateUrl: './task-detail.component.html',
  styleUrls: ['./task-detail.component.css']
})
export class TaskDetailComponent implements OnInit {
  @Input() task: TaskModel;

  constructor() { }

  ngOnInit(): void {
  }

}
