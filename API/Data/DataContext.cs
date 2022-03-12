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

        public DbSet<Sme> Sme { get; set; }

        public DbSet<Professional> Professionals { get; set;}

        public DbSet<Field> Fields{ get; set;}

        public DbSet<Student> Students{get; set;}

        public DbSet<Organization> Organizations { get; set; }
        

        public DbSet<Job> Jobs { get; set; }


    }
}