import { Component, OnInit, Input } from '@angular/core';
import { Team } from '../../../models/team/team.model';

@Component({
  selector: 'app-team-detail',
  templateUrl: './team-detail.component.html',
  styleUrls: ['./team-detail.component.css']
})
export class TeamDetailComponent implements OnInit {
  @Input() team: Team;

  constructor() { }

  ngOnInit(): void {
  }

}
