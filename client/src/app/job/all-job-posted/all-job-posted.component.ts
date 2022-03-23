import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import {MatPaginator} from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import {MatTableDataSource} from '@angular/material/table';
import { ToastrService } from 'ngx-toastr';
import { AccountsService } from 'src/app/_services/accounts.service';
import { SharedService } from 'src/app/_services/shared.service';
import {MatDialog} from '@angular/material/dialog';
import { DialogJobEditComponent } from 'src/app/dialog/dialog-job-edit/dialog-job-edit.component';
import { DialogBidComponent } from 'src/app/dialog/dialog-bid/dialog-bid.component';




@Component({
  selector: 'app-all-job-posted',
  templateUrl: './all-job-posted.component.html',
  styleUrls: ['./all-job-posted.component.css']
})
export class AllJobPostedComponent implements OnInit {
 displayedColumns : string[] =['jobTitle','desc','timeframe','budget' , 'Action'];
 dataSource : MatTableDataSource<any>;


  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatPaginator) sort: MatSort;

  constructor(private sharedService : SharedService,
    public accountService : AccountsService,
    private toastr : ToastrService, 
    private dialog : MatDialog
    ) { }

  ngOnInit(): void {
    this.getAllJob();
    
  
   
  }

  getAllJob(){

    this.sharedService.getAllJob().subscribe(
      job => {

        this.dataSource = new MatTableDataSource(job);
        //this.dataSource.sort = this.sort;
        this.dataSource.paginator = this.paginator;       
        console.log(job);
      },error =>{

        this.toastr.error(error.error);
      }
    )

  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  openDialog(row : any) {
    const dialogRef = this.dialog.open(DialogBidComponent,{
        data: row
    });
  }

  


}