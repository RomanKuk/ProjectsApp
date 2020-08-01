import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/models/user/user.model';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css'],
  providers: [UserService]
})
export class UsersComponent implements OnInit {
  selectedUser: User;

  constructor(private userService: UserService) { }

  ngOnInit(): void {
    this.userService.userSelected
      .subscribe(
        (user: User) => {
          this.selectedUser = user;
        }
      );
  }

}
