import { Component, OnInit } from '@angular/core';
import { Team } from 'src/app/models/team/team.model';

@Component({
  selector: 'app-teams',
  templateUrl: './teams.component.html',
  styleUrls: ['./teams.component.css']
})
export class TeamsComponent implements OnInit {
  selectedTeam: Team;

  constructor() { }

  ngOnInit(): void {
  }

}
