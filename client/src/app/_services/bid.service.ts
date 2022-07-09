import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { ReplaySubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { ActionStatus } from '../_models/ActionStatus';
import { addBid } from '../_models/addBid';
import { Bid } from '../_models/Bid';
import { bidProfCard } from '../_models/bidProfCard';
import { jobDetails } from '../_models/jobDetails';
import { ongoingJob } from '../_models/ongoingJob';
import { timeline } from '../_models/timeline';
import { timelineNotesQuery } from '../_models/timelineNotesQuery';

@Injectable({
  providedIn: 'root'
})
export class BidService {
  baseUrl = environment.apiUrl;

  public jobToBid = new ReplaySubject<jobDetails>(1);
  currentJobToBid$ = this.jobToBid.asObservable();

  constructor(private http : HttpClient, private toastr: ToastrService) { }

  clickToBid(job : jobDetails){
   this.jobToBid.next(job);
  }

  cleanBid(){
    this.jobToBid.next(null);
  }

  placeBid(model : any){

    return this.http.post(this.baseUrl+'bid/showInterest',model).pipe(

      map((bid : addBid)=>{

        if(bid){
         this.toastr.show(bid.status)
          return(bid);                  
        }
      })
    )
  }
  placeBid2(model : any){

    return this.http.put(this.baseUrl+'bid/addBidDetails',model).pipe(

      map((bid : addBid)=>{

        if(bid){
         this.toastr.success(bid.status)
          return(bid);                  
        }
      })
    )
  }
  showInterest(model : any){

    return this.http.post(this.baseUrl+'bid/showInterest',model).pipe(

      map((bid : addBid)=>{

        if(bid){
         this.toastr.show(bid.status)
          return(bid);                  
        }
      })
    )
  }

  checkBid(jobId : any, username : any){
    return this.http.get<ActionStatus>(`${this.baseUrl}bid/bidCheck/${jobId}/${username}`);
  }

  addBidScore(model: any){
    return this.http.put(this.baseUrl+'bid/addBidScore',model).pipe(
      map((response : ActionStatus)=>{
        if(response){
          return response;
        }
      },error =>{
        this.toastr.error(error.error);
      })
    )
  }

  addTimeline(model:any){
    return this.http.post(this.baseUrl+'bid/addTimelines',model).pipe(
      map((status : ActionStatus)=>{
        if(status){
          return(status);
        }
      },error =>{
        this.toastr.error(error.error);
      })
    )
  }

  getTimeline(id:any){
    return this.http.get<timeline>(`${this.baseUrl}bid/getTimelines/${id}`);
  }

  getTimelineById(id:number){
    return this.http.get<timeline>(`${this.baseUrl}bid/getTimelineById/${id}`);
  }

  updateTimeline(model : any){
    return this.http.put(this.baseUrl+'bid/updateTimeline',model).pipe(
      map((response : ActionStatus)=>{
        if(response){
          this.toastr.success();
        }
      },error =>{
        this.toastr.error(error.error);
      })
    )
  }

  deleteTimelineById(id:number){
    return this.http.delete(`${this.baseUrl}bid/deleteTimeline/${id}`);
  }

  getBidProfByJobId(id : number, bidResponse : string){
    return this.http.get<bidProfCard>(`${this.baseUrl}bid/getBidProfByJobId/${id}/${bidResponse}`);
  }

  getBidProfByJobId2(id:any){
    return this.http.get<bidProfCard[]>(`${this.baseUrl}bid/getBidProfByJobId/${id}`);
  }

  insertBidNotes(model: any){
    return this.http.put(this.baseUrl+'bid/addBidNotes',model).pipe(
      map((response : ActionStatus)=>{
        if(response){
          this.toastr.success("Note added");
        }
      },error =>{
        this.toastr.error(error.error);
      })
    )
  }

  acceptBid(model :any){
    return this.http.put(this.baseUrl+'bid/acceptBid',model).pipe(
      map((response : ActionStatus)=>{
        if(response){
          return response;
        }
      },error =>{
        this.toastr.error(error.error);
      })
    )
  }

  declineBid(model :any){
    return this.http.put(this.baseUrl+'bid/declineBid',model).pipe(
      map((response : ActionStatus)=>{
        if(response){
          return response;
        }
      },error =>{
        this.toastr.error(error.error);
      })
    )
  }

  checkBidStatus(jobId : any){
    return this.http.get<Bid>(`${this.baseUrl}bid/checkBidStatus/${jobId}`);
  }

  getOngoingJob(bidResponse : any,bidStatus:any,username :any){
    return this.http.get<ongoingJob[]>(`${this.baseUrl}bid/getJobBidProfByBidResponse/${bidResponse}/${bidStatus}/${username}`);
  }

  setTimelineStatus(model : any){
    return this.http.put(this.baseUrl+'bid/setTimelineStatus',model).pipe(
      map((response : ActionStatus)=>{
        if(response){
          return response;
        }
      },error =>{
        this.toastr.error(error.error);
      })
    )
  }

  insertTimelineNotes(model : any){
    return this.http.post(this.baseUrl+'bid/addTimelineNotes',model).pipe(
      map((status : ActionStatus)=>{
        if(status){
          return(status);
        }
      },error =>{
        this.toastr.error(error.error);
      })
    )
  }

  getTimelineNotes(timelineId :any){
    return this.http.get<timelineNotesQuery>(`${this.baseUrl}bid/getTimelineNotes/${timelineId}`);
  }

  editTimelineNotes(model :any){
    return this.http.put(this.baseUrl+'bid/editTimelineNotes',model).pipe(
      map((response : ActionStatus)=>{
        if(response){
          return response;
        }
      },error =>{
        this.toastr.error(error.error);
      })
    )
  }

  deleteTimelineNotes(timelineId){
    return this.http.delete(this.baseUrl+`bid/deleteTimelineNotes/${timelineId}`).pipe(
      map((response : ActionStatus)=>{
        if(response){
          return response;
        }
      },error =>{
        this.toastr.error(error.error);
      })
    )
  }

  checkTimelineDone(bidId :any){
    return this.http.get<any>(`${this.baseUrl}bid/checkTimelineDoneForPay/${bidId}`);
  }

  markProjectAsDone(model : any){
    return this.http.put(this.baseUrl+'bid/markProjectDone',model).pipe(
      map((response : ActionStatus)=>{
        if(response){
          return response;
        }
      },error =>{
        this.toastr.error(error.error);
      })
    )
  }


  addPayment(model :any){
    return this.http.post(this.baseUrl+'bid/addPayment',model).pipe(
      map((status : ActionStatus)=>{
        if(status){
          return(status);
        }
      },error =>{
        this.toastr.error(error.error);
      })
    )
  }

}
