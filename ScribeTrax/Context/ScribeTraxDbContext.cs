using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ScribeTrax.Models;

namespace ScribeTrax.Context;

public partial class ScribeTraxDbContext : DbContext
{
    public ScribeTraxDbContext()
    {
    }

    public ScribeTraxDbContext(DbContextOptions<ScribeTraxDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Byline> Bylines { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Market> Markets { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<PaymentType> PaymentTypes { get; set; }

    public virtual DbSet<Submission> Submissions { get; set; }

    public virtual DbSet<Work> Works { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=localhost;Database=ScribeTraxDb;Trusted_Connection=True;Encrypt=False;MultipleActiveResultSets=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Byline>(entity =>
        {
            entity.HasKey(e => e.BylineId).HasName("PK__Bylines__852A4D0CD47EF5F1");

            entity.HasIndex(e => e.Name, "UQ_Bylines_Name").IsUnique();

            entity.Property(e => e.Inactive).HasDefaultValue(false);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Type).HasMaxLength(50);
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.GenreId).HasName("PK__Genres__0385057EE1E1A1E8");

            entity.HasIndex(e => e.Name, "UQ_Genres_Name").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Market>(entity =>
        {
            entity.HasKey(e => e.MarketId).HasName("PK__Markets__74B186AF60244A64");

            entity.HasIndex(e => e.Name, "UQ_Markets_Name").IsUnique();

            entity.HasIndex(e => e.Url, "UQ_Markets_Url").IsUnique();

            entity.Property(e => e.Editor).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Postal).HasMaxLength(200);
            entity.Property(e => e.Type).HasMaxLength(50);
            entity.Property(e => e.Url).HasMaxLength(200);
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payments__9B556A38C92E13DC");
        });

        modelBuilder.Entity<PaymentType>(entity =>
        {
            entity.HasKey(e => e.PaymentTypeId).HasName("PK__PaymentT__BA430B354EC87EDD");

            entity.HasIndex(e => e.Name, "UQ_PaymentTypes_Name").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Submission>(entity =>
        {
            entity.HasKey(e => e.SubmissionId).HasName("PK__Submissi__449EE1252922D03A");

            entity.HasIndex(e => e.BylineId, "IX_Submissions_BylineId");

            entity.HasIndex(e => e.MarketId, "IX_Submissions_MarketId");

            entity.HasIndex(e => e.WorkId, "IX_Submissions_WorkId");

            entity.Property(e => e.Fee).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Status).HasMaxLength(50);
        });

        modelBuilder.Entity<Work>(entity =>
        {
            entity.HasKey(e => e.WorkId).HasName("PK__Works__2DE6D5F59CBD18EE");

            entity.Property(e => e.Title).HasMaxLength(200);
            entity.Property(e => e.Type).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
