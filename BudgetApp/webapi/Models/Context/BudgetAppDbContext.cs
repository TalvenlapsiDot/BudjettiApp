using System;
using System.Collections.Generic;
using Back_End.Models;
using Microsoft.EntityFrameworkCore;

namespace Back_End.Models.Context;

public partial class BudgetAppDbContext : DbContext
{
    public BudgetAppDbContext()
    {
    }

    public BudgetAppDbContext(DbContextOptions<BudgetAppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Budget> Budgets { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Expenditure> Expenditures { get; set; }

    public virtual DbSet<Income> Incomes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:AzureDB");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Budget>(entity =>
        {
            entity.HasKey(e => e.BudgetId).HasName("PK__Budget__E38E79C4737B7A66");

            entity.ToTable("Budget");

            entity.Property(e => e.BudgetId).HasColumnName("BudgetID");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            //entity.HasOne(d => d.User).WithMany(p => p.Budgets)
            //    .HasForeignKey(d => d.UserId)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("FK__Budget__UserID__628FA481");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Categori__19093A2BED73AE56");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CategoryDescription).HasMaxLength(100);
            entity.Property(e => e.CategoryName).HasMaxLength(25);
        });

        modelBuilder.Entity<Expenditure>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Expenditure");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.ExpenditureDate).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Category).WithMany()
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Expenditu__Categ__6383C8BA");

            entity.HasOne(d => d.User).WithMany()
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Expenditu__UserI__6477ECF3");
        });

        modelBuilder.Entity<Income>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Income");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.IncomeDate).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            //entity.HasOne(d => d.Category).WithMany()
            //    .HasForeignKey(d => d.CategoryId)
            //    .HasConstraintName("FK__Income__Category__656C112C");

            //entity.HasOne(d => d.User).WithMany()
            //    .HasForeignKey(d => d.UserId)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("FK__Income__UserID__66603565");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCACA90CF35F");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.Username).HasMaxLength(25);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
