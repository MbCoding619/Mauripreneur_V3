import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ToastrService } from 'ngx-toastr';
import { DialogTimelineNotesComponent } from 'src/app/dialog/dialog-timeline-notes/dialog-timeline-notes.component';
import { ongoingJob } from 'src/app/_models/ongoingJob';
import { timeline } from 'src/app/_models/timeline';
import { timelineNotesQuery } from 'src/app/_models/timelineNotesQuery';
import { AccountsService } from 'src/app/_services/accounts.service';
import { BidService } from 'src/app/_services/bid.service';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-ongoing-job',
  templateUrl: './ongoing-job.component.html',
  styleUrls: ['./ongoing-job.component.css']
})
export class OngoingJobComponent implements OnInit {
  public myAngularxQrCode: string = null;

  displayedColumns : string[] =['profPic','profFName','jobTitle','jobDescription','bidResponse','action'];
  dataSource : MatTableDataSource<ongoingJob>;

  viewJob = false;
  jobData : ongoingJob;
  timelineData : timeline;
  timelineNotesData : timelineNotesQuery;
  modelT : any;
  modetlTimelineNotes:any;
  timlineNoteId : any;
  p =1;
  showTimelineNotes =false;
  updateNote =false;
  notesForm : FormGroup;
  username :any;
  testQ:any;
  testQL:any;
  showPayment = false;
  

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatPaginator) sort: MatSort;


  constructor(private bidService : BidService,
              private toastr : ToastrService,
              private dialog : MatDialog,
              private fb : FormBuilder,
              private accountService : AccountsService
) { 

    
  }

  ngOnInit(): void {
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => {
      this.username = user.username;

    })
    this.getOnGoingJob();  
     
  }



  getOnGoingJob(){
    this.bidService.getOngoingJob("Accepted","ONGOING",this.username).subscribe(response =>{
      this.dataSource =  new MatTableDataSource(response);
      this.dataSource.paginator = this.paginator;      
    },error =>{
      this.toastr.error("Something went wrong");
    })
  }

  showJobDetails(row :any){
    this.viewJob = true;
    this.jobData = row;
    this.myAngularxQrCode = this.jobData?.profSocials;
    this.getTimeline(this.jobData?.bidId);   
    console.log(this.jobData);
  }

  getTimeline(id : any){
    this.bidService.getTimeline(id).subscribe(response =>{
      this.timelineData = response;
      console.log(this.timelineData);
      this.checkAllTimelineDone(this.timelineData[0].bidId);
      
    })
  }

  
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  back(){
    this.viewJob = false;
    this.getOnGoingJob();
  }

  back2(){
    this.showTimelineNotes =false;
  }

  createImgPath(serverPath : string){
    return `https://localhost:5001/${serverPath}`;
  }
  
  setTimelineStatus(status : any,timelineId :any){
    this.modelT = {
      "timelineId" : timelineId,
      "timelineStatus": status
    }
    //console.log("Wa");

    this.bidService.setTimelineStatus(this.modelT).subscribe(
      response =>{
        if(response){
          this.toastr.success(response.status);
          this.getTimeline(this.jobData?.bidId);
        }
      },error=>{
        this.toastr.error("Something went wrong");
      }
    )
  }

  getTimelineNotes(timelineId : any){
    this.bidService.getTimelineNotes(timelineId).subscribe(
      response => {
        this.timelineNotesData = response;
        this.showTimelineNotes = true;
        this.initializeNoteForm();
        console.log(this.timelineNotesData);
      }
    )
  }

  addTimelineNotes(){
    this.modetlTimelineNotes ={
      "timelineId": this.timelineNotesData[0].timelineId,
      "notes" : this.notesForm.controls["notes"]?.value
    };
    this.bidService.insertTimelineNotes(this.modetlTimelineNotes).subscribe(response =>{
      if(response){
        this.toastr.success(response.status);
        this.getTimelineNotes(this.timelineNotesData[0].timelineId);
      }
    },error=>{
      this.toastr.error("Somethin Went Wrong");
    })
  }

  initializeNoteForm(){
    this.notesForm = this.fb.group({
      notes : new FormControl('',Validators.required)
    })
  }
  showEditTimelineNotes(timelineNote : timelineNotesQuery){
    this.updateNote = true;
    this.timlineNoteId = timelineNote.timelineNotesId;
    console.log(this.timlineNoteId);
    this.notesForm.patchValue({
      notes : timelineNote.notes
    })
  }

  editTimelineNotes(){
    this.modetlTimelineNotes ={
      "timelineNotesId" : this.timlineNoteId,
      "timelineNotes" : this.notesForm.controls["notes"]?.value
    }
    this.bidService.editTimelineNotes(this.modetlTimelineNotes).subscribe(
      response =>{
        if(response){
          this.toastr.success(response.status);
          this.getTimelineNotes(this.timelineNotesData[0].timelineId);
        }
      },error =>{
        this.toastr.error("Something went wrong");
      }
    )
  }

  deleteTimelineNote(){
    this.bidService.deleteTimelineNotes(this.timlineNoteId).subscribe(
      response =>{
        if(response){
          this.toastr.success(response.status);
          this.getTimelineNotes(this.timelineNotesData[0].timelineId);
        }
      },error =>{
        this.toastr.error("Something went wrong");
      }
    )
  }

  test(){
    console.log("wa");
  }

  openDialog(row: any) {
    const dialogRef = this.dialog.open(DialogTimelineNotesComponent, {
      data: row,

    });
  }

  checkAllTimelineDone(bidId:any){
    this.bidService.checkTimelineDone(bidId).subscribe(response =>{
      this.testQ = response;
      this.testQL = Object.keys(this.testQ).length;
      if(this.testQ[0].tmStatus ==="DONE" && this.testQL ===1){
        this.showPayment = true;
        console.log(this.showPayment);
      }
      
     // console.log(this.testQ[0].tmStatus);
      //console.log(this.testQL);
    })
  }

  markAsDone(){
    let model ={
      "bidId" : this.jobData?.bidId
    }
    this.bidService.markProjectAsDone(model).subscribe(
      response =>{
        if(response){
          this.toastr.success(response.status);
        }
      },error=>{
        this.toastr.error("Something went wrong");
      }
    )
    //console.log(model);
  }

}
