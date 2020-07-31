import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { TaskModel } from '../../../../models/task/task.model';

@Component({
  selector: 'app-task-item',
  templateUrl: './task-item.component.html',
  styleUrls: ['./task-item.component.css']
})
export class TaskItemComponent implements OnInit {
  @Input() task: TaskModel;
  @Output() taskSelected = new EventEmitter<void>();

  constructor() { }

  ngOnInit() {
  }

  onSelected() {
    this.taskSelected.emit();
  }
}
