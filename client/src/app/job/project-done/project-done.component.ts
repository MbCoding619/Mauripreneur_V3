import { AfterViewInit, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { ongoingJob } from 'src/app/_models/ongoingJob';
import { AccountsService } from 'src/app/_services/accounts.service';
import { BidService } from 'src/app/_services/bid.service';
import { take } from 'rxjs/operators';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { ToastrService } from 'ngx-toastr';
import { timelineNotesQuery } from 'src/app/_models/timelineNotesQuery';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
declare var Stripe;
@Component({
  selector: 'app-project-done',
  templateUrl: './project-done.component.html',
  styleUrls: ['./project-done.component.css']
})
export class ProjectDoneComponent implements OnInit  {
  displayedColumns: string[] = ['profPic', 'profFName', 'jobDescription', 'bidStatus', 'action'];
  dataSource: MatTableDataSource<ongoingJob>;
  username: any;
  showTab = false;
  showNotes = false;
  timelineNotesData: timelineNotesQuery;
  jobData: ongoingJob;
  p=1;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatPaginator) sort: MatSort;
  @ViewChild('cardNumber', { static: true }) cardNumberElement: ElementRef;
  @ViewChild('cardExpiry', { static: true }) cardExpiryElement: ElementRef;
  @ViewChild('cardCvc', { static: true }) cardCvcElement: ElementRef;
  stripe:any;
  cardNumber:any;
  cardExpiry:any;
  cardCvc:any;
  paymentForm :FormGroup;
 

  public myAngularxQrCode: string = null;




  constructor(private bidService: BidService,
    private accountService: AccountsService,
    private toastr: ToastrService,
    private fb : FormBuilder) {
    this.myAngularxQrCode = 'tutsmake.com';
  }
  

  ngOnInit(): void {
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => {
      this.username = user.username;

    })

    this.getProjectDone();
  
  }

 initialisePaymentForm(){
  this.paymentForm = this.fb.group({
    nameOnCard :new FormControl('',Validators.required)
  })
 }

  

  getProjectDone() {
    this.bidService.getOngoingJob("Accepted", "DONE", this.username).subscribe(response => {
      this.dataSource = new MatTableDataSource(response);
      this.dataSource.paginator = this.paginator;
      this.initialisePaymentForm();
    }, error => {
      this.toastr.error("Something went wrong");
    })

   
  }


  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  createImgPath(serverPath: string) {
    return `https://localhost:5001/${serverPath}`;
  }

  showPaymentTab(row: ongoingJob) {
    this.jobData = row;
    this.showTab = true;
    console.log(this.jobData);
  }

  back() {
    this.showTab = false;
    this.getProjectDone();
  }

  getTimelineNotes(timelineId: any) {
    this.bidService.getTimelineNotes(timelineId).subscribe(
      response => {
        this.timelineNotesData = response;
        this.showNotes =true;
        console.log(this.timelineNotesData);
      }
    )
  }

  addPayment(){
    let model ={
      "bidId" : this.jobData?.bidId,
      "bidAmount" : this.jobData?.bidAmount,
      "cardOnName" : this.paymentForm.controls['nameOnCard']?.value
    }
    this.bidService.addPayment(model).subscribe(
      response =>{
        if(response){
          this.toastr.success(response.status);
        }
      },erorr =>{
        this.toastr.error("Something Wrong happened");
      }
    )
  }

}
