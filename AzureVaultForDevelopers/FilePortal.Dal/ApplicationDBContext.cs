using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FilePortal.Dal.Model;

namespace FilePortal.Dal
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public virtual DbSet<ExternalFileSource> ExternalFileSource { get; set; } = null!;
        public virtual DbSet<PortalFile> PortalFiles { get; set; } = null!;
      

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {


            builder.Entity<PortalFile>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PortalFiles)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_PortalFile_AspNetUsers");

                entity.HasOne(d => d.ExternalFileSource)
                  .WithMany(p => p.PortalFiles)
                  .HasForeignKey(d => d.ExternalSourceId)
                  .HasConstraintName("FK_PortalFile_ExternalSources");
            });
            builder.Entity<ExternalFileSource>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ExternalFileSources)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_ExternalSources_AspNetUsers");
            });

           

           

            base.OnModelCreating(builder);

        }
    }
}
