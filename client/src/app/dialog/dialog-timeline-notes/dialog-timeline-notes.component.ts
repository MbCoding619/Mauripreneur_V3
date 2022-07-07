import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-dialog-timeline-notes',
  templateUrl: './dialog-timeline-notes.component.html',
  styleUrls: ['./dialog-timeline-notes.component.css']
})
export class DialogTimelineNotesComponent implements OnInit {

  constructor(
    private dialogRef : MatDialogRef<DialogTimelineNotesComponent>,@Inject(MAT_DIALOG_DATA) public timelineData:any) { }

  ngOnInit(): void {
  }

}
