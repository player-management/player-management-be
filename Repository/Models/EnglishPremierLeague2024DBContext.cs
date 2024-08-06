using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace Repository.Models
{
    public partial class EnglishPremierLeague2024DBContext : DbContext
    {
        public EnglishPremierLeague2024DBContext()
        {
        }

        public EnglishPremierLeague2024DBContext(DbContextOptions<EnglishPremierLeague2024DBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<FootballClub> FootballClubs { get; set; } = null!;
        public virtual DbSet<FootballPlayer> FootballPlayers { get; set; } = null!;
        public virtual DbSet<PremierLeagueAccount> PremierLeagueAccounts { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(GetConnectionString());
            }
        }

        private string GetConnectionString()
        {
            IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();
            return configuration.GetConnectionString("ConnectionStrings");
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FootballClub>(entity =>
            {
                entity.ToTable("FootballClub");

                entity.Property(e => e.FootballClubId)
                    .HasMaxLength(30)
                    .HasColumnName("FootballClubID");

                entity.Property(e => e.ClubName).HasMaxLength(100);

                entity.Property(e => e.ClubShortDescription).HasMaxLength(400);

                entity.Property(e => e.Mascos).HasMaxLength(100);

                entity.Property(e => e.SoccerPracticeField).HasMaxLength(250);
            });

            modelBuilder.Entity<FootballPlayer>(entity =>
            {
                entity.ToTable("FootballPlayer");

                entity.Property(e => e.FootballPlayerId)
                    .HasMaxLength(30)
                    .HasColumnName("FootballPlayerID");

                entity.Property(e => e.Achievements).HasMaxLength(400);

                entity.Property(e => e.Birthday).HasColumnType("datetime");

                entity.Property(e => e.FootballClubId)
                    .HasMaxLength(30)
                    .HasColumnName("FootballClubID");

                entity.Property(e => e.FullName).HasMaxLength(100);

                entity.Property(e => e.Nomination).HasMaxLength(400);

                entity.Property(e => e.PlayerExperiences).HasMaxLength(400);

                entity.HasOne(d => d.FootballClub)
                    .WithMany(p => p.FootballPlayers)
                    .HasForeignKey(d => d.FootballClubId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__FootballP__Footb__4E88ABD4");
            });

            modelBuilder.Entity<PremierLeagueAccount>(entity =>
            {
                entity.HasKey(e => e.AccId)
                    .HasName("PK__PremierL__91CBC3984B1264B1");

                entity.ToTable("PremierLeagueAccount");

                entity.HasIndex(e => e.EmailAddress, "UQ__PremierL__49A147401B2C848F")
                    .IsUnique();

                entity.Property(e => e.AccId)
                    .ValueGeneratedNever()
                    .HasColumnName("AccID");

                entity.Property(e => e.Description).HasMaxLength(140);

                entity.Property(e => e.EmailAddress).HasMaxLength(90);

                entity.Property(e => e.Password).HasMaxLength(90);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
