import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Team } from '../../../../models/team/team.model';

@Component({
  selector: 'app-team-item',
  templateUrl: './team-item.component.html',
  styleUrls: ['./team-item.component.css']
})
export class TeamItemComponent implements OnInit {
  @Input() team: Team;
  @Output() teamSelected = new EventEmitter<void>();

  constructor() { }

  ngOnInit() {
  }

  onSelected() {
    this.teamSelected.emit();
  }

}
