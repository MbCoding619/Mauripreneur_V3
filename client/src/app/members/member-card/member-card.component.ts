import { Component, OnInit, Input } from '@angular/core';
import { User } from 'src/app/_models/user';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css']
})
export class MemberCardComponent implements OnInit {

  @Input() member : User;
  constructor() { }

  ngOnInit(): void {
  }

  createImgPath(serverPath : string){
    return `https://localhost:5001/${serverPath}`;
  }

}
