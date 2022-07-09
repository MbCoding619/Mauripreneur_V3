using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.DTOs.AutoDTO;
using API.DTOs.UpdateDTO;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class BidController : BaseApiController
    {
        private readonly IBidRepository _bidRepository;
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IUserRepository _userRepository;
        public BidController(DataContext context, IMapper mapper, IBidRepository bidRepository, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _context = context;
            _mapper = mapper;
            _bidRepository = bidRepository;

        }


        [HttpPost("showInterest")]

        public async Task<ActionResult<BidReponseDTO>> addBid(BidAddDTO bidAddDTO)
        {
            var user = await _context.Users.SingleOrDefaultAsync(b => b.UserName == bidAddDTO.username.ToLower());
            var bid = new Bid();

            if (user != null)
            {
                var prof = await _context.Professionals.SingleOrDefaultAsync(p => p.AppUserId == user.AppUserId);


                var bidCheck = await _context.Bid.FirstOrDefaultAsync(b => b.JobId == bidAddDTO.JobId && b.ProfessionalId == prof.Id);

                if (bidCheck == null)
                {
                    bid = new Bid
                    {

                        JobId = bidAddDTO.JobId,
                        ProfessionalId = prof.Id,
                        BidResponse = "INTERESTED",

                    };

                    _context.Bid.Add(bid);

                }
                else
                {
                    return BadRequest("Already Bid for the job");
                }


            }
            else
            {

                return BadRequest("Error Happened");
            }

            await _context.SaveChangesAsync();

            return new BidReponseDTO
            {
                Status = "Added"
            };

        }

        [HttpPut("addBidDetails")]
        public async Task<ActionResult<BidReponseDTO>> addBidDetails(BidAddDTO bidAddDTO)
        {
            var bid = await _bidRepository.GetBidByIdAsync(bidAddDTO.BidId);
            if (bid != null)
            {
                bid.BidAmount = bidAddDTO.BidAmount;
                bid.Description = bidAddDTO.Description;
                bid.OtherDetails = bidAddDTO.OtherDetails;
                _bidRepository.Update(bid);
                return new BidReponseDTO
                {
                    Status = "Details Added"
                };
            }
            else
            {
                return BadRequest();
            }


        }

        [HttpGet("bidCheck/{jobId}/{username}")]
        public async Task<ActionResult<ActionStatusDTO>> bidCheck(int jobId, string username)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.UserName == username.ToLower());
            var prof = await _context.Professionals.SingleOrDefaultAsync(p => p.AppUserId == user.AppUserId);
            var bidCheck = await _context.Bid.FirstOrDefaultAsync(b => b.JobId == jobId && b.ProfessionalId == prof.Id);
            if (bidCheck == null)
            {
                return new ActionStatusDTO
                {
                    status = "Not"
                };
            }
            else
            {
                return new ActionStatusDTO
                {
                    status = "Bidded"
                };
            }
        }

        [HttpGet("byJidPid/{bidId}/{ProfId}")]

        public async Task<ActionResult<Bid>> getByJidPid(int bidId, int ProfId)
        {

            return await _bidRepository.GetBidByJobIdProfId(bidId, ProfId);
        }

        [HttpDelete("deleteBid/{bidId}")]

        public async Task<ActionResult> deleteBidById(int bidId)
        {
            var bid = await _bidRepository.GetBidByIdAsync(bidId);

            if (bid != null)
            {
                _bidRepository.Delete(bid);
                return Ok();
            }
            else
            {
                return BadRequest("Something Went Wrong");
            }
        }





        [HttpPut("acceptBid")]

        public async Task<ActionResult<ActionStatusDTO>> acceptBid(BidAcceptDTO bidAcceptDTO)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.UserName == bidAcceptDTO.username.ToLower());
            var bid = await _bidRepository.GetBidByIdAsync(bidAcceptDTO.bidId);
            var sme = await _context.Sme.SingleOrDefaultAsync(s => s.AppUserId == user.AppUserId);
            if (user != null)
            {
                if (bid.BidResponse != "INTERESTED")
                {
                    return new ActionStatusDTO
                    {
                        status = "Already Accepted/Declined bid"
                    };
                }
                else
                {

                    bid.BidResponse = "Accepted";
                    bid.SmeId = sme.Id;
                    _bidRepository.Update(bid);

                    var bidlist = await _context.Bid.Where(bd => bd.JobId == bid.JobId && bd.BidResponse != "Accepted").ToListAsync();
                    foreach (Bid bids in bidlist)
                    {
                        bids.BidResponse = "DECLINED";
                        _bidRepository.Update(bids);
                    }

                    return new ActionStatusDTO
                    {
                        status = "Bid Accepted"
                    };
                }

            }
            else
            {

                return BadRequest("Something Went Wrong");
            }
        }

        [HttpPut("declineBid")]

        public async Task<ActionResult> declineBid(BidAcceptDTO bidAcceptDTO)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.UserName == bidAcceptDTO.username.ToLower());
            var bid = await _bidRepository.GetBidByIdAsync(bidAcceptDTO.bidId);
            var sme = await _context.Sme.SingleOrDefaultAsync(s => s.AppUserId == user.AppUserId);
            if (user != null)
            {
                if (bid.BidResponse != "INTERESTED")
                {
                    return Ok("Already Approve/Decline bid");
                }
                else
                {

                    bid.BidResponse = "DECLINED";
                    bid.SmeId = sme.Id;
                    _bidRepository.Update(bid);


                    return Ok("Bid Accepted");
                }

            }
            else
            {

                return BadRequest("Something Went Wrong");
            }
        }

        [HttpGet("{username}")]

        public async Task<ActionResult> queryBid(string username)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.UserName == username);
            var sme = await _context.Sme.SingleOrDefaultAsync(s => s.AppUserId == user.AppUserId);

            var bidToReturn = (from bd in _context.Bid
                               join jb in _context.Job on bd.JobId equals jb.Id
                               join pr in _context.Professionals on bd.ProfessionalId equals pr.Id
                               join sm in _context.Sme on jb.SmeId equals sm.Id
                               where sm.Id == sme.Id
                               select new
                               {
                                   BidId = bd.Id,
                                   Description = bd.Description,
                                   JobTitle = jb.JobTitle,
                                   Budget = jb.Budget,
                                   BidAmount = bd.BidAmount,
                                   Name = pr.FName,
                                   Response = bd.BidResponse
                               });


            return Ok(bidToReturn);

        }

        [HttpGet("checkBidStatus/{jobId}")]

        public async Task<ActionResult<IEnumerable<ATBidDTO>>> checkBidStatus(int jobId)
        {
            var bid = await _context.Bid.Where(bd => bd.JobId == jobId).Where(bd => bd.BidResponse == "DECLINED" || bd.BidResponse == "Accepted").ToListAsync();

            if (bid != null)
            {
                var bids = _mapper.Map<IEnumerable<ATBidDTO>>(bid);
                return Ok(bids);
            }
            else
            {
                return BadRequest("No Bid Found");
            }
        }

        [HttpGet("getBidAccepted/{username}")]
        public async Task<ActionResult> queryBidAccepted(string username)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.UserName == username);
            var sme = await _context.Sme.SingleOrDefaultAsync(s => s.AppUserId == user.AppUserId);

            var bidToReturn = (from bd in _context.Bid
                               join jb in _context.Job on bd.JobId equals jb.Id
                               join pr in _context.Professionals on bd.ProfessionalId equals pr.Id
                               join sm in _context.Sme on jb.SmeId equals sm.Id
                               where (sm.Id == sme.Id && bd.BidResponse == "Accepted")
                               select new
                               {
                                   BidId = bd.Id,
                                   Description = bd.Description,
                                   JobTitle = jb.JobTitle,
                                   Budget = jb.Budget,
                                   BidAmount = bd.BidAmount,
                                   Name = pr.FName,
                                   Response = bd.BidResponse,
                                   ProfId = pr.Id
                               });


            return Ok(bidToReturn);

        }

        [HttpGet("getBidSent/{username}")]
        public async Task<ActionResult> queryBidSent(string username)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.UserName == username);
            var prof = await _context.Professionals.SingleOrDefaultAsync(s => s.AppUserId == user.AppUserId);

            var bidToReturn = (from bd in _context.Bid
                               join jb in _context.Job on bd.JobId equals jb.Id
                               join pr in _context.Professionals on bd.ProfessionalId equals pr.Id
                               join sm in _context.Sme on jb.SmeId equals sm.Id
                               join us in _context.Users on sm.AppUserId equals us.AppUserId
                               where (pr.Id == prof.Id)
                               select new
                               {
                                   BidId = bd.Id,
                                   BidAmount = bd.BidAmount,
                                   description = bd.Description,
                                   otherDetails = bd.OtherDetails,
                                   JobId = jb.Id,
                                   JobTitle = jb.JobTitle,
                                   JobDescription = jb.Desc,
                                   Budget = jb.Budget,
                                   Name = sm.RepresentName,
                                   lName = sm.RepresentLName,
                                   phone = sm.RepresentPhone,
                                   compName = sm.CompName,
                                   social = sm.SocialLink,
                                   imagePath = us.imagePath

                               });


            return Ok(bidToReturn);

        }

        [HttpGet("getBidSent/{username}/{bidStatus}")]
        public async Task<ActionResult> queryBidSentByStatus(string username, string bidStatus)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.UserName == username);
            var prof = await _context.Professionals.SingleOrDefaultAsync(s => s.AppUserId == user.AppUserId);

            var bidToReturn = (from bd in _context.Bid
                               join jb in _context.Job on bd.JobId equals jb.Id
                               join pr in _context.Professionals on bd.ProfessionalId equals pr.Id
                               join sm in _context.Sme on jb.SmeId equals sm.Id
                               join us in _context.Users on sm.AppUserId equals us.AppUserId
                               where (pr.Id == prof.Id && bd.BidResponse == bidStatus)
                               select new
                               {
                                   BidId = bd.Id,
                                   BidAmount = bd.BidAmount,
                                   description = bd.Description,
                                   otherDetails = bd.OtherDetails,
                                   JobId = jb.Id,
                                   JobTitle = jb.JobTitle,
                                   JobDescription = jb.Desc,
                                   Budget = jb.Budget,
                                   Name = sm.RepresentName,
                                   lName = sm.RepresentLName,
                                   phone = sm.RepresentPhone,
                                   compName = sm.CompName,
                                   social = sm.SocialLink,
                                   imagePath = us.imagePath

                               });


            return Ok(bidToReturn);

        }


        [HttpPut("addBidNotes")]

        public async Task<ActionResult<ActionStatusDTO>> addBidNotes(AddNotesDTO addNotesDTO)
        {
            var bid = await _bidRepository.GetBidByIdAsync(addNotesDTO.Id);

            if (bid != null)
            {

                _mapper.Map(addNotesDTO, bid);
                _bidRepository.Update(bid);

                return new ActionStatusDTO
                {
                    status = "bidNotesAdded"
                };
            }
            else
            {
                return BadRequest("Something went wrong");
            }
        }



        [HttpPut("addBidScore")]

        public async Task<ActionResult<ActionStatusDTO>> addBidScore(BidScoreDTO bidScoreDTO)
        {
            var bid = await _bidRepository.GetBidByIdAsync(bidScoreDTO.bidId);
            if (bid != null)
            {
                bid.bidScore = bidScoreDTO.bidScore;
                _bidRepository.Update(bid);

                return new ActionStatusDTO
                {
                    status = "bidScoreAdded"
                };
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("addTimelines")]

        public async Task<ActionResult<ReponseDTO>> addTimeline(TimelineAddDTO timelineAddDTO)
        {
            var bid = await _bidRepository.GetBidByIdAsync(timelineAddDTO.BidId);

            if (bid != null)
            {
                var timeline = new Timeline
                {
                    Title = timelineAddDTO.Title,
                    Description = timelineAddDTO.Description,
                    Date = timelineAddDTO.Date,
                    BidId = timelineAddDTO.BidId
                };
                _context.Timeline.Add(timeline);
                await _context.SaveChangesAsync();

                return new ReponseDTO
                {
                    Response = "Timeline Added"
                };
            }
            else
            {
                return BadRequest();
            }
        }


        [HttpGet("getTimelines/{bidId}")]

        public async Task<ActionResult<IEnumerable<ATTimelineDTO>>> getTimeline(int bidId)
        {
            var bid = await _bidRepository.GetBidByIdAsync(bidId);
            if (bid != null)
            {
                var timelines = await _context.Timeline.Where(t => t.BidId == bidId).ToListAsync();

                var timelineToReturn = _mapper.Map<IEnumerable<ATTimelineDTO>>(timelines);

                return Ok(timelineToReturn);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("getTimelineById/{timelineId}")]

        public async Task<ActionResult<ATTimelineDTO>> getTimelineById(int timelineId)
        {
            var timeline = await _context.Timeline.FindAsync(timelineId);
            if (timeline != null)
            {
                var timelineToReturn = _mapper.Map<ATTimelineDTO>(timeline);
                return Ok(timelineToReturn);
            }
            else
            {
                return BadRequest();
            }
        }


        [HttpPut("updateTimeline")]

        public async Task<ActionResult> updateTimeline(TimelineUpdateDTO timelineUpdateDTO)
        {
            var timeline = await _context.Timeline.FindAsync(timelineUpdateDTO.TimelineId);
            if (timeline != null)
            {
                _mapper.Map(timelineUpdateDTO, timeline);
                _context.Entry(timeline).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete("deleteTimeline/{timelineId}")]

        public async Task<ActionResult> deleteTimeline(int timelineId)
        {
            var timeline = await _context.Timeline.FindAsync(timelineId);
            if (timeline != null)
            {
                _context.Timeline.Remove(timeline);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut("setTimelineStatus")]

        public async Task<ActionResult<ActionStatusDTO>> setTimelineStatus(TimelineStatusUpdateDTO timelineStatusUpdateDTO)
        {
            var timeline = await _context.Timeline.FindAsync(timelineStatusUpdateDTO.timelineId);
            if (timeline != null)
            {
                _mapper.Map(timelineStatusUpdateDTO, timeline);
                _context.Entry(timeline).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return new ActionStatusDTO
                {
                    status = "Timeline Status Changed"
                };
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("addTimelineNotes")]

        public async Task<ActionResult<ActionStatusDTO>> addTimelineNotes(TimelineNotesDTO timelineNotesDTO)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            var timelineNote = new TimelineNotes();
            if (user != null)
            {
                if (user.AppUserRole == "SME")
                {
                    var sme = await _context.Sme.SingleOrDefaultAsync(sm => sm.AppUserId == user.AppUserId);
                    timelineNote = new TimelineNotes
                    {

                        Notes = timelineNotesDTO.Notes,
                        TimelineId = timelineNotesDTO.TimelineId,
                        SmeId = sme.Id
                    };

                    _context.TimelineNotes.Add(timelineNote);
                }
                else if (user.AppUserRole == "PROFESSIONAL")
                {
                    var prof = await _context.Professionals.SingleOrDefaultAsync(prof => prof.AppUserId == user.AppUserId);
                    timelineNote = new TimelineNotes
                    {

                        Notes = timelineNotesDTO.Notes,
                        TimelineId = timelineNotesDTO.TimelineId,
                        ProfessionalId = prof.Id
                    };

                    _context.TimelineNotes.Add(timelineNote);
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return BadRequest();
            }
            await _context.SaveChangesAsync();
            return new ActionStatusDTO
            {
                status = "Notes Added"
            };
        }

        [HttpPut("editTimelineNotes")]

        public async Task<ActionResult<ActionStatusDTO>> editTimelineNotes(TimelineNotesUpdateDTO timelineNotesUpdateDTO)
        {
            var timelineNote = await _context.TimelineNotes.FindAsync(timelineNotesUpdateDTO.timelineNotesId);
            if (timelineNote != null)
            {
                timelineNote.Notes = timelineNotesUpdateDTO.timelineNotes;
                _context.Entry(timelineNote).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return new ActionStatusDTO
                {
                    status = "Note edited"
                };
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete("deleteTimelineNotes/{timelineNoteId}")]

        public async Task<ActionResult<ActionStatusDTO>> deleteTimelineNotes(int timelineNoteId)
        {
            var timelineNote = await _context.TimelineNotes.FindAsync(timelineNoteId);
            if (timelineNote != null)
            {
                _context.TimelineNotes.Remove(timelineNote);
                await _context.SaveChangesAsync();
                return new ActionStatusDTO
                {
                    status = "Note deleted"
                };
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("getTimelineNotes/{timelineId}")]
        public IActionResult getTimelineNotes(int timelineId)
        {

            var query = _context.TimelineNotes
                        .Join(
                            _context.Timeline,
                            tmnLN => tmnLN.TimelineId,
                            tmnL => tmnL.TimelineId,
                            (tmnLN, tmnL) => new
                            {
                                timelineNotesId = tmnLN.TimelineNotesId,
                                timelineId = tmnL.TimelineId,
                                Notes = tmnLN.Notes,
                                imgPathS = tmnLN.Sme.User.imagePath,
                                imgPathP = tmnLN.Professional.User.imagePath

                            }
                        ).Where(tm => tm.timelineId == timelineId).ToList();


            return Ok(query);
        }


        [HttpGet("checkTimelineDoneForPay/{bidId}")]
        public IActionResult checkTimelineDoneForPay(int bidId)
        {

            var count = _context.Timeline.Where(tm => tm.BidId == bidId).GroupBy(tm => tm.timelineStatus).Select
            (
                tm => new
                {
                    tmStatus = tm.Key,
                    count = tm.Count()
                }
            );

            return Ok(count);
        }



        [HttpGet("getBidProfByJobId/{jobId}/{bidResponse}")]

        public IActionResult getBidProfBySmeId(int jobId, string bidResponse)
        {
            var query = _context.Bid
            .Join(
                _context.Job,
                bid => bid.JobId,
                job => job.Id,
                (bid, job) => new
                {
                    BidId = bid.Id,
                    BidAmount = bid.BidAmount,
                    BidDesc = bid.Description,
                    BidResponse = bid.BidResponse,
                    BidDate = bid.BidDate,
                    BidOtherDetails = bid.OtherDetails,
                    BidNotes = bid.bidNotes,
                    BidScore = bid.bidScore,
                    ProfId = bid.ProfessionalId,
                    JobId = job.Id,
                    JobTitle = job.JobTitle,
                    JobDescription = job.Desc,
                    SmeId = job.SmeId,
                    timeline = bid.Timeline,

                }
            ).Join(
                _context.Professionals,
                bid => bid.ProfId,
                prof => prof.Id,
                (bid, prof) => new
                {

                    BidId = bid.BidId,
                    BidAmount = bid.BidAmount,
                    BidDesc = bid.BidDesc,
                    BidResponse = bid.BidResponse,
                    BidDate = bid.BidDate,
                    BidOtherDetails = bid.BidOtherDetails,
                    BidNotes = bid.BidNotes,
                    BidScore = bid.BidScore,
                    ProfId = bid.ProfId,
                    JobId = bid.JobId,
                    JobTitle = bid.JobTitle,
                    JobDescription = bid.JobDescription,
                    SmeId = bid.SmeId,
                    timeline = bid.timeline,
                    ProfName = prof.FName,
                    ProfDesc = prof.BriefDesc,
                    ProfAppId = prof.AppUserId

                }
            ).Join(
                _context.Users,
                bid => bid.ProfAppId,
                appUser => appUser.AppUserId,
                (bid, appUser) => new
                {

                    BidId = bid.BidId,
                    BidAmount = bid.BidAmount,
                    BidDesc = bid.BidDesc,
                    BidResponse = bid.BidResponse,
                    BidDate = bid.BidDate,
                    BidOtherDetails = bid.BidOtherDetails,
                    BidNotes = bid.BidNotes,
                    BidScore = bid.BidScore,
                    ProfId = bid.ProfId,
                    JobId = bid.JobId,
                    JobTitle = bid.JobTitle,
                    JobDescription = bid.JobDescription,
                    SmeId = bid.SmeId,
                    timeline = bid.timeline,
                    ProfName = bid.ProfName,
                    ProfDesc = bid.ProfDesc,
                    ProfAppId = bid.ProfAppId,
                    //Below is implemented to get the ImagePath
                    //This Branch does not have the image Functionality
                    //implemented.
                    ProfPic = appUser.imagePath

                }
            )
            .Where(jb => jb.JobId == jobId)
            .Where(bd => bd.BidResponse == bidResponse)
            .ToList();

            return Ok(query);
        }

        [HttpGet("getBidProfByJobId/{jobId}")]

        public IActionResult getBidProfByJobId(int jobId)
        {
            var query = _context.Bid
            .Join(
                _context.Job,
                bid => bid.JobId,
                job => job.Id,
                (bid, job) => new
                {
                    BidId = bid.Id,
                    BidAmount = bid.BidAmount,
                    BidDesc = bid.Description,
                    BidResponse = bid.BidResponse,
                    BidDate = bid.BidDate,
                    BidOtherDetails = bid.OtherDetails,
                    BidNotes = bid.bidNotes,
                    BidScore = bid.bidScore,
                    ProfId = bid.ProfessionalId,
                    JobId = job.Id,
                    JobTitle = job.JobTitle,
                    JobDescription = job.Desc,
                    SmeId = job.SmeId,
                    timeline = bid.Timeline,

                }
            ).Join(
                _context.Professionals,
                bid => bid.ProfId,
                prof => prof.Id,
                (bid, prof) => new
                {

                    BidId = bid.BidId,
                    BidAmount = bid.BidAmount,
                    BidDesc = bid.BidDesc,
                    BidResponse = bid.BidResponse,
                    BidDate = bid.BidDate,
                    BidOtherDetails = bid.BidOtherDetails,
                    BidNotes = bid.BidNotes,
                    BidScore = bid.BidScore,
                    ProfId = bid.ProfId,
                    JobId = bid.JobId,
                    JobTitle = bid.JobTitle,
                    JobDescription = bid.JobDescription,
                    SmeId = bid.SmeId,
                    timeline = bid.timeline,
                    ProfName = prof.FName,
                    ProfDesc = prof.BriefDesc,
                    ProfAppId = prof.AppUserId

                }
            ).Join(
                _context.Users,
                bid => bid.ProfAppId,
                appUser => appUser.AppUserId,
                (bid, appUser) => new
                {

                    BidId = bid.BidId,
                    BidAmount = bid.BidAmount,
                    BidDesc = bid.BidDesc,
                    BidResponse = bid.BidResponse,
                    BidDate = bid.BidDate,
                    BidOtherDetails = bid.BidOtherDetails,
                    BidNotes = bid.BidNotes,
                    BidScore = bid.BidScore,
                    ProfId = bid.ProfId,
                    JobId = bid.JobId,
                    JobTitle = bid.JobTitle,
                    JobDescription = bid.JobDescription,
                    SmeId = bid.SmeId,
                    timeline = bid.timeline,
                    ProfName = bid.ProfName,
                    ProfDesc = bid.ProfDesc,
                    ProfAppId = bid.ProfAppId,
                    //Below is implemented to get the ImagePath
                    //This Branch does not have the image Functionality
                    //implemented.
                    ProfPic = appUser.imagePath

                }
            )
            .Where(jb => jb.JobId == jobId)
            .ToList();

            return Ok(query);
        }

        [HttpGet("getJobBidProfByBidResponse/{bidResponse}/{username}")]

        public IActionResult getJobBidProfMeetBySmeId(string bidResponse, string username)
        {

            // var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
            var query = _context.Job
                        .Join(
                            _context.Bid,
                            job => job.Id,
                            bid => bid.JobId,
                            (job, bid) => new
                            {
                                JobId = job.Id,
                                JobTitle = job.JobTitle,
                                JobDescription = job.Desc,
                                BidId = bid.Id,
                                BidAmount = bid.BidAmount,
                                BidDesc = bid.Description,
                                BidResponse = bid.BidResponse,
                                BidDate = bid.BidDate,
                                BidOtherDetails = bid.OtherDetails,
                                ProfId = bid.ProfessionalId,
                                SmeId = job.SmeId,
                                user = job.Sme.User,
                                timeline = bid.Timeline
                            }
                        ).Join(
                            _context.Professionals,
                            bid => bid.ProfId,
                            prof => prof.Id,
                            (bid, prof) => new
                            {
                                JobId = bid.JobId,
                                JobTitle = bid.JobTitle,
                                JobDescription = bid.JobDescription,
                                BidId = bid.BidId,
                                BidAmount = bid.BidAmount,
                                BidDesc = bid.BidDesc,
                                BidResponse = bid.BidResponse,
                                BidDate = bid.BidDate,
                                BidOtherDetails = bid.BidOtherDetails,
                                ProfId = bid.ProfId,
                                SmeId = bid.SmeId,
                                user = bid.user,
                                timeline = bid.timeline,
                                ProfFName = prof.FName,
                                ProfLName = prof.LName,
                                ProfSocials = prof.LinkedInLink,
                                ProfAppId = prof.AppUserId,
                                field = prof.Field
                            }
                        ).Join(
                            _context.Users,
                            prof => prof.ProfAppId,
                            appUser => appUser.AppUserId,
                            (prof, appUser) => new
                            {
                                JobId = prof.JobId,
                                JobTitle = prof.JobTitle,
                                JobDescription = prof.JobDescription,
                                BidId = prof.BidId,
                                BidAmount = prof.BidAmount,
                                BidDesc = prof.BidDesc,
                                BidResponse = prof.BidResponse,
                                BidDate = prof.BidDate,
                                BidOtherDetails = prof.BidOtherDetails,
                                ProfId = prof.ProfId,
                                SmeId = prof.SmeId,
                                user = prof.user,
                                timeline = prof.timeline,
                                ProfFName = prof.ProfFName,
                                ProfLName = prof.ProfLName,
                                ProfSocials = prof.ProfSocials,
                                ProfPic = appUser.imagePath,
                                Field = prof.field
                            }
                        ).Where(bd => bd.user.UserName == username.ToLower())
                        .Where(bd => bd.BidResponse == bidResponse).ToList();


            return Ok(query);
        }





    }
}