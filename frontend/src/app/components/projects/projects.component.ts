import { Component, OnInit, Input } from '@angular/core';
import { Project } from '../../models/project/project.model';
import { ProjectService } from 'src/app/services/project.service';
import { RecursiveTemplateAstVisitor } from '@angular/compiler';


@Component({
  selector: 'app-projects',
  templateUrl: './projects.component.html',
  styleUrls: ['./projects.component.css'],
  providers: [ProjectService]
})
export class ProjectsComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

}
