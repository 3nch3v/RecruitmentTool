namespace RecruitmentTool.Data
{
    using Microsoft.EntityFrameworkCore;

    using RecruitmentTool.Data.Models;

    public class RecruitmentToolDbContext : DbContext
    {
        public RecruitmentToolDbContext(DbContextOptions<RecruitmentToolDbContext> options)
            : base(options) 
        {
        }

        public DbSet<Candidate> Candidates { get; set; }

        public DbSet<Recruiter> Recruiters { get; set; }

        public DbSet<Job> Jobs { get; set; }

        public DbSet<Skill> Skills { get; set; }

        public DbSet<Interview> Interviews { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Interview>(interview =>
            {
                interview.HasKey(i => new { i.CandidateId, i.RecruiterId, i.JobId });

                interview.HasOne(i => i.Recruiter)
                  .WithMany(r => r.Interviews)
                  .HasForeignKey(i => i.RecruiterId)
                  .OnDelete(DeleteBehavior.Restrict);

                interview.HasOne(i => i.Candidate)
                    .WithMany(c => c.Interviews)
                    .HasForeignKey(a => a.CandidateId)
                    .OnDelete(DeleteBehavior.Restrict);

                interview.HasOne(i => i.Job)
                    .WithMany(c => c.Interviews)
                    .HasForeignKey(a => a.JobId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}