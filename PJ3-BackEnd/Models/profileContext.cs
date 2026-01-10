using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PJ3_BackEnd.Models;

public partial class profileContext : DbContext
{
    public profileContext(DbContextOptions<profileContext> options)
        : base(options)
    {
    }

    public virtual DbSet<aboutme> aboutme { get; set; }

    public virtual DbSet<auth> auth { get; set; }

    public virtual DbSet<certificate> certificate { get; set; }

    public virtual DbSet<education> education { get; set; }

    public virtual DbSet<workexperience> workexperience { get; set; }

    public virtual DbSet<works> works { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<aboutme>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.Property(e => e.LINE).HasMaxLength(255);
            entity.Property(e => e.address).HasMaxLength(255);
            entity.Property(e => e.birth).HasMaxLength(255);
            entity.Property(e => e.email).HasMaxLength(255);
            entity.Property(e => e.englishName).HasMaxLength(255);
            entity.Property(e => e.fb).HasMaxLength(255);
            entity.Property(e => e.gender).HasMaxLength(255);
            entity.Property(e => e.ig).HasMaxLength(255);
            entity.Property(e => e.intro).HasColumnType("text");
            entity.Property(e => e.introEng).HasColumnType("text");
            entity.Property(e => e.linkedin).HasMaxLength(255);
            entity.Property(e => e.name).HasMaxLength(255);
            entity.Property(e => e.phone).HasMaxLength(255);
            entity.Property(e => e.photo).HasMaxLength(255);
        });

        modelBuilder.Entity<auth>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.HasIndex(e => e.email, "email").IsUnique();

            entity.Property(e => e.name).HasMaxLength(255);
            entity.Property(e => e.password).HasMaxLength(255);
            entity.Property(e => e.role)
                .HasDefaultValueSql("'viewer'")
                .HasColumnType("enum('admin','viewer')");
        });

        modelBuilder.Entity<certificate>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.Property(e => e.issuing_authority).HasMaxLength(255);
            entity.Property(e => e.name).HasMaxLength(255);
            entity.Property(e => e.photo).HasMaxLength(255);
        });

        modelBuilder.Entity<education>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.Property(e => e.degree).HasMaxLength(255);
            entity.Property(e => e.degreeEng).HasMaxLength(255);
            entity.Property(e => e.department).HasMaxLength(255);
            entity.Property(e => e.departmentEng).HasMaxLength(255);
            entity.Property(e => e.periodOfStudytime).HasMaxLength(255);
            entity.Property(e => e.school).HasMaxLength(255);
            entity.Property(e => e.schoolEng).HasMaxLength(255);
        });

        modelBuilder.Entity<workexperience>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.Property(e => e.company).HasMaxLength(255);
            entity.Property(e => e.companyEng).HasMaxLength(255);
            entity.Property(e => e.companyType).HasMaxLength(255);
            entity.Property(e => e.descript).HasColumnType("text");
            entity.Property(e => e.descriptEng).HasColumnType("text");
            entity.Property(e => e.jobPosition).HasMaxLength(255);
            entity.Property(e => e.jobPositionEng).HasMaxLength(255);
            entity.Property(e => e.location).HasMaxLength(255);
            entity.Property(e => e.tenure).HasMaxLength(255);
            entity.Property(e => e.yearInService).HasMaxLength(255);
        });

        modelBuilder.Entity<works>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.Property(e => e.backEnd).HasColumnType("json");
            entity.Property(e => e.database_name).HasMaxLength(255);
            entity.Property(e => e.descript).HasColumnType("text");
            entity.Property(e => e.frontEnd).HasColumnType("json");
            entity.Property(e => e.function_name).HasColumnType("json");
            entity.Property(e => e.gitHub_link).HasColumnType("json");
            entity.Property(e => e.image).HasMaxLength(255);
            entity.Property(e => e.item_label).HasMaxLength(255);
            entity.Property(e => e.name).HasMaxLength(255);
            entity.Property(e => e.page_link).HasMaxLength(255);
            entity.Property(e => e.tool).HasColumnType("json");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
