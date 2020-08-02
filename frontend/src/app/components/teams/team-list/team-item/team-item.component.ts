import { Component, OnInit, Input, Output, EventEmitter, OnDestroy } from '@angular/core';
import { Team } from '../../../../models/team/team.model';
import { TeamService } from 'src/app/services/team.service';

@Component({
  selector: 'app-team-item',
  templateUrl: './team-item.component.html',
  styleUrls: ['./team-item.component.css']
})
export class TeamItemComponent implements OnInit {
  @Input() team: Team;

  constructor(private teamService: TeamService) { }

  ngOnInit(): void {
  }

  onSelected(): void {
    this.teamService.teamSelected.emit(this.team);
  }

}
