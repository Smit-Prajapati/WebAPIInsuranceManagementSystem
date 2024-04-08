using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebAPIInsuranceManagementSystem.DataAccess.Models;

public partial class InsuranceAndClaimManagementDbContext : DbContext
{
    public InsuranceAndClaimManagementDbContext()
    {
    }

    public InsuranceAndClaimManagementDbContext(DbContextOptions<InsuranceAndClaimManagementDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AuditLog> AuditLogs { get; set; }

    public virtual DbSet<Claim> Claims { get; set; }

    public virtual DbSet<ClaimDocument> ClaimDocuments { get; set; }

    public virtual DbSet<DocumentList> DocumentLists { get; set; }

    public virtual DbSet<Policy> Policies { get; set; }

    public virtual DbSet<PolicyType> PolicyTypes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserPolicy> UserPolicies { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AuditLog__3214EC27A1E57198");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Action)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Category)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Details).IsUnicode(false);
            entity.Property(e => e.Timestamp).HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.AuditLogs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AuditLogs__UserI__5070F446");
        });

        modelBuilder.Entity<Claim>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Claims__3214EC27C146C4A8");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.IncidentLocation)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Policy).WithMany(p => p.Claims)
                .HasForeignKey(d => d.PolicyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Claims__PolicyId__49C3F6B7");

            entity.HasOne(d => d.User).WithMany(p => p.Claims)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Claims__UserId__4AB81AF0");
        });

        modelBuilder.Entity<ClaimDocument>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ClaimDoc__3214EC274A5A6FED");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DocumentPath)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Claim).WithMany(p => p.ClaimDocuments)
                .HasForeignKey(d => d.ClaimId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ClaimDocu__Claim__4D94879B");
        });

        modelBuilder.Entity<DocumentList>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Document__3214EC273850AEA1");

            entity.ToTable("DocumentList");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DocumentType)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Policy).WithMany(p => p.DocumentLists)
                .HasForeignKey(d => d.PolicyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DocumentL__Polic__4222D4EF");
        });

        modelBuilder.Entity<Policy>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Policies__3214EC27B188D18A");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.Installment).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PolicyName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PolicyNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PolicyTypeId).HasColumnName("PolicyTypeID");
            entity.Property(e => e.PremiumAmount).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.PolicyType).WithMany(p => p.Policies)
                .HasForeignKey(d => d.PolicyTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Policies__Policy__3F466844");
        });

        modelBuilder.Entity<PolicyType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PolicyTy__3214EC272B5B98E6");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.PolicyTypeName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Roles__3214EC273471BA00");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC27748BDBC8");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534B8942B74").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Users__RoleId__3A81B327");
        });

        modelBuilder.Entity<UserPolicy>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserPoli__3214EC27BCCA8FE8");

            entity.ToTable("UserPolicy");

            entity.Property(e => e.Id).HasColumnName("ID");

            entity.HasOne(d => d.Agent).WithMany(p => p.UserPolicyAgents)
                .HasForeignKey(d => d.AgentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserPolic__Agent__46E78A0C");

            entity.HasOne(d => d.Policy).WithMany(p => p.UserPolicies)
                .HasForeignKey(d => d.PolicyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserPolic__Polic__45F365D3");

            entity.HasOne(d => d.User).WithMany(p => p.UserPolicyUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserPolic__UserI__44FF419A");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
