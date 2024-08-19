
using Microsoft.EntityFrameworkCore;

namespace webapi.Models { 
    public partial class SampleDBContext : DbContext
    {
        public SampleDBContext(DbContextOptions
        <SampleDBContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Hasta> Hasta { get; set; }
        public virtual DbSet<Doctor> Doctor { get; set; }
        public virtual DbSet<Admin> Admin { get; set; }
        public virtual DbSet<Diagnosis> Diagnosis { get; set; }
        public virtual DbSet<Prescription> Prescription { get; set; }
        public virtual DbSet<Examination> Examination { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Hasta>(entity => {
                entity.HasKey(k => k.HastaId);
            });
           
            modelBuilder.Entity<Admin>(entity => {
                entity.HasKey(k => k.AdminId);
            });
            modelBuilder.Entity<Doctor>(entity => {
                entity.HasKey(k => k.DoktorId);
            });
            modelBuilder.Entity<Diagnosis>(entity => {
                entity.HasKey(k => k.Id);
            });
            modelBuilder.Entity<Prescription>(entity => {
                entity.HasKey(k => k.Id);
            });

            modelBuilder.Entity<Examination>(entity => {
                entity.HasKey(k => k.Id);
            });


            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);


    }
}