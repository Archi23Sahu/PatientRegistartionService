using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace PatientRegistartionService.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Patients> Patients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patients>().HasKey(p => p.PatientId);
            modelBuilder.Entity<Patients>().HasIndex(p => p.MedicalRecordNumber).IsUnique();
    
            base.OnModelCreating(modelBuilder);
        }
    }
}
