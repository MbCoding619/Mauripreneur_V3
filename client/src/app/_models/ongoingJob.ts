import { field } from "./field";
import { timeline } from "./timeline";

export interface ongoingJob{
    jobId : string;
    jobTitle : string;
    jobDescription: string;
    bidId : string;
    bidDesc : string;
    bidResponse : string;
    bidDate : Date;
    bidOtherDetails :string;
    bidStatus : string;
    bidAmount : number;
    profId : string;
    smeId : string;
    timeline : timeline[];
    profFname : string;
    profLName : string;
    profSocials : string;
    profPic : string;
    field : field;
    
}