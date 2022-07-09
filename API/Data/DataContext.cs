using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : DbContext
    {

        //Generate Contructor(Options)
        public DataContext( DbContextOptions options) : base(options)
        {

        }

        public DbSet<AppUser> Users { get; set; }

        public DbSet<Admin> Admin { get; set; }

        public DbSet<Sme> Sme { get; set; }

        public DbSet<Professional> Professionals { get; set;}

        public DbSet<Field> Fields{ get; set;}

        public DbSet<Student> Students{get; set;}

        public DbSet<Organization> Organizations { get; set; }
        

        public DbSet<Job> Job { get; set; }

        public DbSet<Bid> Bid { get; set; }

        public DbSet<Meeting> Meeting { get; set; }       

        public DbSet<Vacancy> Vacancy { get ; set; }

        public DbSet<Application> Application { get; set; }

        public DbSet<Timeline> Timeline {get; set;}

        public DbSet<SubField> SubField {get; set;}

        public DbSet<Qualification> Qualification {get; set;}

        public DbSet<Experience> Experience {get; set;}
        
        public DbSet<Skills> Skills {get; set;}   

        public DbSet<TimelineNotes> TimelineNotes { get; set; }  

        public DbSet<Payment> Payment{ get; set; }   



        


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Application>()
                .HasKey(ap => new {ap.StudId,ap.VacId});
                
            builder.Entity<Application>()
                .HasOne(ap => ap.Student)
                .WithMany(ap => ap.Vacancy)
                .HasForeignKey(ap => ap.StudId);

            builder.Entity<Application>()
                .HasOne(ap => ap.Vacancy)
                .WithMany(ap => ap.Students)
                .HasForeignKey(ap => ap.VacId);

            builder.Entity<Skills>()
                    .HasKey(sk => new {sk.ProfId,sk.SubFieldId});

            builder.Entity<Skills>()
                    .HasOne(sk => sk.Professional)
                    .WithMany(sk => sk.SubField)
                    .HasForeignKey(sk => sk.ProfId);

            builder.Entity<Skills>()
                    .HasOne(sk => sk.SubField)
                    .WithMany(sk => sk.Professional)
                    .HasForeignKey(sk => sk.SubFieldId);          

            builder.Entity<Meeting>()
                .HasKey(me => me.MeetId);

            builder.Entity<Meeting>()
                .HasOne(me => me.Sme)
                .WithMany(me => me.Meeting)
                .HasForeignKey(me => me.SmeId); 

            builder.Entity<Meeting>()
                .HasOne(me => me.Professional)
                .WithMany(me => me.Meeting)
                .HasForeignKey(me => me.ProfId); 

            builder.Entity<Meeting>()
                .HasOne(me => me.Student)
                .WithMany(me => me.Meeting)
                .HasForeignKey(me => me.StudId); 


            builder.Entity<Meeting>()
                .HasOne(me => me.Bid)
                .WithMany(me => me.Meeting)
                .HasForeignKey(me => me.BidId);

             builder.Entity<Meeting>()
                .HasOne(me => me.Vacancy)
                .WithMany(me => me.Meeting)
                .HasForeignKey(me => me.VacId);
                

            builder.Entity<Vacancy>()
                .HasKey( vc => vc.VacId);

            builder.Entity<Job>()
                   .HasKey(jb => jb.Id);

            builder.Entity<Job>()
                   .HasOne(jb => jb.Field)
                   .WithMany(jb => jb.Job)
                   .HasForeignKey(jb => jb.FieldId);

            builder.Entity<Job>()
                   .HasOne(jb => jb.Sme)
                   .WithMany(jb => jb.Job)
                   .HasForeignKey(jb => jb.SmeId);


            builder.Entity<Bid>()
                   .HasKey(bd =>  bd.Id);

            builder.Entity<Bid>()
                   .HasOne(bd => bd.Job)
                   .WithMany(bd => bd.Bid)
                   .HasForeignKey(bd => bd.JobId);

            builder.Entity<Bid>()
                   .HasOne(bd => bd.Professional)
                   .WithMany(bd => bd.Bid)
                   .HasForeignKey(bd => bd.ProfessionalId);


            builder.Entity<Experience>()
                    .HasOne(exp => exp.Professional)
                    .WithMany(exp => exp.Experience)
                    .HasForeignKey(exp => exp.ProfId);
                             
            builder.Entity<Qualification>()
                    .HasOne(qual => qual.Professional)
                    .WithMany(qual => qual.Qualification)
                    .HasForeignKey(qual => qual.ProfId);

            builder.Entity<TimelineNotes>()
                    .HasOne(timeNote => timeNote.Professional)
                    .WithMany(timeNote => timeNote.TimelineNotes )
                    .HasForeignKey(timeNote => timeNote.ProfessionalId);
                             


     


            
        }
        

    }
}