using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.DTOs.AutoDTO;
using API.DTOs.UpdateDTO;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class JobController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly IJobRepository _jobRepository;
        
        private readonly IMapper _mapper;
        public JobController(DataContext context,IJobRepository jobRepository,IMapper mapper)
        {
            _jobRepository = jobRepository;
            _mapper = mapper;            
            _context = context;
        }



   [HttpPost("addJob")]

    public async Task<ActionResult<JobDTO>> addJob([FromForm] JobAddDTO jobAddDTO){

        var user = await _context.Users.SingleOrDefaultAsync(b => b.UserName == jobAddDTO.username.ToLower());
        
            if( user != null){
                var sme = await _context.Sme.SingleOrDefaultAsync( s => s.AppUserId == user.AppUserId);
            try
            {
               var file = Request.Form.Files[0];
               var folderName = Path.Combine("Resources","Documents");
               var pathToSave = Path.Combine(Directory.GetCurrentDirectory(),folderName);
               if(file.Length > 0){
                   var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                   var fullPath = Path.Combine(pathToSave, fileName);
                   var dbPath = Path.Combine(folderName, fileName);
                     using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

             var job = new Job {

              JobTitle = jobAddDTO.JobTitle,
              Desc = jobAddDTO.Desc,
              Requirements = jobAddDTO.Requirements,
              FieldId = jobAddDTO.FieldId,
              Timeframe = jobAddDTO.Timeframe,
              Budget = jobAddDTO.Budget,
              SmeId = sme.Id,
              filePath = dbPath
              

          } ;

        _context.Job.Add(job); 

               }else{
                   return BadRequest("File Missing");
               }     

            }catch(Exception ex){
                return StatusCode(500,$"Internal Server error :{ex}");
            }

            

               
        }
        else {

            return BadRequest("Please Relog");

        }
         
        await _context.SaveChangesAsync();

        return new JobDTO{

            Status = "Job Added"
        };

    

    }

    [HttpGet("allJob")]

    public async Task<ActionResult<IEnumerable<ATJobDTO>>> GetJob(){

        var jobs = await _jobRepository.GetJobMAsync();
        
        var jobToReturn = _mapper.Map<IEnumerable<ATJobDTO>>(jobs);

        return Ok(jobToReturn);
    }

    [HttpGet("{username}")]

    public async  Task<ActionResult<IEnumerable<ATJobDTO>>> GetJobBySmeId(string username){
        var user = await _context.Users.SingleOrDefaultAsync(b => b.UserName == username);

        if(user !=null){

            var sme = await _context.Sme.SingleOrDefaultAsync(s => s.AppUserId == user.AppUserId);

        var jobs = await _jobRepository.GetJobBySmeAsync(sme.Id);

         var jobToReturn = _mapper.Map<IEnumerable<ATJobDTO>>(jobs);

        return Ok(jobToReturn);
        }else
        {
            return BadRequest("Something Went Wrong!");
        }

         
    }


    [HttpPut("editJob")]

    public async Task<ActionResult> UpdateJob (JobUpdateDTO jobUpdateDTO)
    {   
        var job = await _jobRepository.GetJobByIdAsync(jobUpdateDTO.Id);
        if(job !=null){
            _mapper.Map(jobUpdateDTO, job); 

            _jobRepository.Update(job);
            return Ok("Wa DreC");
        }else{

            return BadRequest("Pa DreC");
        }
        
               
    }

    [HttpGet("jobById/{id}")]

    public async Task<ActionResult<ATJobDTO>> getJobById(int id)
    {
        var job = await _jobRepository.GetJobByIdAsync(id);

        var jobToReturn = _mapper.Map<ATJobDTO>(job);

        return jobToReturn;
    }



    }
}