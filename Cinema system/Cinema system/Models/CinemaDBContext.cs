using System;
using Microsoft.EntityFrameworkCore;

namespace CinemaSystem.Models
{

    public partial class CinemaDBContext : DbContext
    {

        public CinemaDBContext(DbContextOptions<CinemaDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cinema> Cinemas { get; set; }
        public virtual DbSet<Hall> Halls { get; set; }
        public virtual DbSet<Movie> Movies { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Seance> Seances { get; set; }
        public virtual DbSet<SeanceSeat> SeanceSeats { get; set; }
        public virtual DbSet<Seat> Seats { get; set; }
        public virtual DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cinema>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Hall>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Cinema)
                    .WithMany(p => p.Halls)
                    .HasForeignKey(d => d.CinemaId)
                    .HasConstraintName("FK_Hall_Cinema");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Movie>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever();

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.EndingDate)
                    .HasColumnType("datetime");

                entity.Property(e => e.ReleaseDate)
                    .HasColumnType("datetime");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever();

                entity.HasOne(d => d.Seance)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.SeanceId)
                    .HasConstraintName("FK_Order_Seance");

                entity.HasOne(d => d.Seat)
                    .WithMany()
                    .HasForeignKey(d => d.SeatId)
                    .HasConstraintName("FK_Order_Seat");
            });
                

            modelBuilder.Entity<Seance>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever();

                entity.Property(e => e.ShowDate)
                    .HasColumnType("datetime");

                entity.HasOne(d => d.Movie)
                    .WithMany(p => p.Seances)
                    .HasForeignKey(d => d.MovieId)
                    .HasConstraintName("FK_Seance_Movie");

                entity.HasOne(d => d.Hall)
                    .WithMany(p => p.Seances)
                    .HasForeignKey(d => d.HallId)
                    .HasConstraintName("FK_Seances_Hall");
            });

            modelBuilder.Entity<SeanceSeat>(entity =>
            {
                entity.HasNoKey();

                entity.HasOne(d => d.Seance)
                    .WithMany()
                    .HasForeignKey(d => d.SeanceId)
                    .HasConstraintName("FK_SeanceSeat_Seance");

                entity.HasOne(d => d.Seat)
                    .WithMany()
                    .HasForeignKey(d => d.SeatId)
                    .HasConstraintName("FK_SeanceSeat_Seat");
            });

            modelBuilder.Entity<Seat>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.TypeOfSeat)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasConversion(x => x.ToString(),
                         x => (SeatType)Enum.Parse(typeof(SeatType), x));

                entity.HasOne(d => d.Hall)
                    .WithMany(p => p.Seats)
                    .HasForeignKey(d => d.HallId)
                    .HasConstraintName("FK_Seat_Hall");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
