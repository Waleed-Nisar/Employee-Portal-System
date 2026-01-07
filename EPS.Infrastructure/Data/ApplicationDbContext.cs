using EPS.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace EPS.Infrastructure.Data;

/// <summary>
/// Main database context for the Employee Portal System
/// </summary>
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // DbSets
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Designation> Designations { get; set; }
    public DbSet<Leave> Leaves { get; set; }
    public DbSet<Attendance> Attendances { get; set; }
    public DbSet<Document> Documents { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Employee Configuration
        builder.Entity<Employee>(entity =>
        {
            entity.HasIndex(e => e.EmployeeId).IsUnique();
            entity.HasIndex(e => e.Email).IsUnique();

            // Self-referencing relationship for Manager
            entity.HasOne(e => e.Manager)
                .WithMany(e => e.Subordinates)
                .HasForeignKey(e => e.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Department relationship
            entity.HasOne(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Designation relationship
            entity.HasOne(e => e.Designation)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DesignationId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Department Configuration
        builder.Entity<Department>(entity =>
        {
            entity.HasIndex(d => d.Code).IsUnique();

            // Department head relationship
            entity.HasOne(d => d.HeadEmployee)
                .WithMany()
                .HasForeignKey(d => d.HeadEmployeeId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Designation Configuration
        builder.Entity<Designation>(entity =>
        {
            entity.HasIndex(d => d.Code).IsUnique();
        });

        // Leave Configuration
        builder.Entity<Leave>(entity =>
        {
            entity.HasOne(l => l.Employee)
                .WithMany(e => e.Leaves)
                .HasForeignKey(l => l.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(l => l.Approver)
                .WithMany()
                .HasForeignKey(l => l.ApprovedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(l => new { l.EmployeeId, l.StartDate, l.EndDate });
        });

        // Attendance Configuration
        builder.Entity<Attendance>(entity =>
        {
            entity.HasOne(a => a.Employee)
                .WithMany(e => e.Attendances)
                .HasForeignKey(a => a.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(a => new { a.EmployeeId, a.Date }).IsUnique();
        });

        // Document Configuration
        builder.Entity<Document>(entity =>
        {
            entity.HasOne(d => d.Employee)
                .WithMany(e => e.Documents)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(d => new { d.EmployeeId, d.DocumentType });
        });
    }

    /// <summary>
    /// Override SaveChanges to automatically update audit fields
    /// </summary>
    public override int SaveChanges()
    {
        UpdateAuditFields();
        return base.SaveChanges();
    }

    /// <summary>
    /// Override SaveChangesAsync to automatically update audit fields
    /// </summary>
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateAuditFields();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateAuditFields()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is Employee || e.Entity is Department || e.Entity is Designation);

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Modified)
            {
                if (entry.Entity is Employee employee)
                {
                    employee.UpdatedAt = DateTime.UtcNow;
                }
                else if (entry.Entity is Department department)
                {
                    department.UpdatedAt = DateTime.UtcNow;
                }
                else if (entry.Entity is Designation designation)
                {
                    designation.UpdatedAt = DateTime.UtcNow;
                }
            }
        }
    }
}