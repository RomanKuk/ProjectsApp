import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { Project } from 'src/app/models/project/project.model';

@Component({
  selector: 'app-project-item',
  templateUrl: './project-item.component.html',
  styleUrls: ['./project-item.component.css']
})
export class ProjectItemComponent implements OnInit {
  @Input() project: Project;
  @Output() projectSelected = new EventEmitter<void>();
  
  constructor() { }

  ngOnInit() {
  }

  onSelected() {
    this.projectSelected.emit();
  }

}
