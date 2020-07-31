import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/models/user/user.model';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit {
  selectedUser: User;

  constructor() { }

  ngOnInit(): void {
  }

}
