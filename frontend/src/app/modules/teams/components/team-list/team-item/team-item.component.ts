import { Component, OnInit, Input } from '@angular/core';
import { Team } from 'src/app/models/team/team.model';

@Component({
  selector: 'app-team-item',
  templateUrl: './team-item.component.html',
  styleUrls: ['./team-item.component.css']
})
export class TeamItemComponent implements OnInit {
  @Input() team: Team;
  constructor() { }

  ngOnInit(): void {
  }

}
