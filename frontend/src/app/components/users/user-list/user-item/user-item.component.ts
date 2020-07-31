import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { User } from '../../../../models/user/user.model';

@Component({
  selector: 'app-user-item',
  templateUrl: './user-item.component.html',
  styleUrls: ['./user-item.component.css']
})
export class UserItemComponent implements OnInit {
  @Input() user: User;
  @Output() userSelected = new EventEmitter<void>();

  constructor() { }

  ngOnInit() {
  }

  onSelected() {
    this.userSelected.emit();
  }

}
