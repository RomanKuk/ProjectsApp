import { Component, OnInit, Input } from '@angular/core';
import { Project } from '../../models/project/project.model';


@Component({
  selector: 'app-projects',
  templateUrl: './projects.component.html',
  styleUrls: ['./projects.component.css']
})
export class ProjectsComponent implements OnInit {
  selectedProject: Project;

  constructor() { }

  ngOnInit(): void {
  }

}
