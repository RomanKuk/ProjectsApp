import { Component, OnInit } from '@angular/core';
import { Team } from 'src/app/models/team/team.model';
import { TeamService } from 'src/app/services/team.service';

@Component({
  selector: 'app-teams',
  templateUrl: './teams.component.html',
  styleUrls: ['./teams.component.css'],
  providers: [TeamService]
})
export class TeamsComponent implements OnInit {
  selectedTeam: Team;

  constructor(private teamService: TeamService) { }

  ngOnInit(): void {
    this.teamService.teamSelected
      .subscribe(
        (team: Team) => {
          this.selectedTeam = team;
        }
      );
  }
}
