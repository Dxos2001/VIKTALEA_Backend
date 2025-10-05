using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace VIKTALEA_Backend.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    public virtual DbSet<Clientes> Clientes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseOracle("User Id=VIKTALEA;Password=Viktalea_12345!;Data Source=localhost:1521/XEPDB1");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema("VIKTALEA")
            .UseCollation("USING_NLS_COMP")
            .Entity<Clientes>(e =>
            {
                e.ToTable("Clientes");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).ValueGeneratedOnAdd();
                e.HasIndex(x => x.Ruc).IsUnique();
                e.Property(x => x.Ruc).HasMaxLength(11).IsRequired();
                e.Property(x => x.RazonSocial).HasMaxLength(255).IsRequired();
                e.Property(x => x.TelefonoContacto).HasMaxLength(50);
                e.Property(x => x.CorreoContacto).HasMaxLength(255);
                e.Property(x => x.Direccion).HasMaxLength(500);
                e.Property(x => x.activate)
                 .HasColumnType("CHAR(1)")
                 .HasDefaultValueSql("1")                    
                 .IsRequired();
                e.Property(x => x.createdAt).HasDefaultValueSql("SYS_EXTRACT_UTC(SYSTIMESTAMP)");
                e.Property(x => x.updatedAt);
            });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
