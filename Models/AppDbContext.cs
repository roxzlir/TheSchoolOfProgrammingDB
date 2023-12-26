using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TheSchoolOfProgrammingDB.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Class> Classes { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EnrollmentList> EnrollmentLists { get; set; }

    public virtual DbSet<Profession> Professions { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source = (localdb)\\MSSQLLocalDB;Initial Catalog=TheSchoolOfProgramming;Integrated Security=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Class>(entity =>
        {
            entity.Property(e => e.ClassId)
                .ValueGeneratedNever()
                .HasColumnName("ClassID");
            entity.Property(e => e.ClassName)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.Property(e => e.CourseId)
                .ValueGeneratedNever()
                .HasColumnName("CourseID");
            entity.Property(e => e.CourseStatus)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.SubjectName).HasMaxLength(30);
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
            entity.Property(e => e.DepartmentName)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.EmpFirstName).HasMaxLength(50);
            entity.Property(e => e.EmpLastName).HasMaxLength(50);
            entity.Property(e => e.FkProfessionId).HasColumnName("FK_ProfessionID");
            entity.Property(e => e.HiredDate).HasColumnType("datetime");
            entity.Property(e => e.Salery).HasColumnType("decimal(8, 2)");

            entity.HasOne(d => d.FkProfession).WithMany(p => p.Employees)
                .HasForeignKey(d => d.FkProfessionId)
                .HasConstraintName("FK_Employees_Professions");
        });

        modelBuilder.Entity<EnrollmentList>(entity =>
        {
            entity.HasKey(e => e.EnrollmentId);

            entity.ToTable("EnrollmentList", tb => tb.HasTrigger("trg_SetEmployeeID"));

            entity.Property(e => e.EnrollmentId).HasColumnName("EnrollmentID");
            entity.Property(e => e.FkClassId).HasColumnName("FK_ClassID");
            entity.Property(e => e.FkCourseId).HasColumnName("FK_CourseID");
            entity.Property(e => e.FkEmployeeId).HasColumnName("FK_EmployeeID");
            entity.Property(e => e.FkStudentId).HasColumnName("FK_StudentID");
            entity.Property(e => e.Grade)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.GradeDate).HasColumnType("datetime");

            entity.HasOne(d => d.FkClass).WithMany(p => p.EnrollmentLists)
                .HasForeignKey(d => d.FkClassId)
                .HasConstraintName("FK_EnrollmentList_Classes");

            entity.HasOne(d => d.FkCourse).WithMany(p => p.EnrollmentLists)
                .HasForeignKey(d => d.FkCourseId)
                .HasConstraintName("FK_EnrollmentList_Courses");

            entity.HasOne(d => d.FkEmployee).WithMany(p => p.EnrollmentLists)
                .HasForeignKey(d => d.FkEmployeeId)
                .HasConstraintName("FK_EnrollmentList_Employees");

            entity.HasOne(d => d.FkStudent).WithMany(p => p.EnrollmentLists)
                .HasForeignKey(d => d.FkStudentId)
                .HasConstraintName("FK_EnrollmentList_Students");
        });

        modelBuilder.Entity<Profession>(entity =>
        {
            entity.Property(e => e.ProfessionId)
                .ValueGeneratedNever()
                .HasColumnName("ProfessionID");
            entity.Property(e => e.FkDepartmentId).HasColumnName("FK_DepartmentID");
            entity.Property(e => e.ProTitle).HasMaxLength(20);

            entity.HasOne(d => d.FkDepartment).WithMany(p => p.Professions)
                .HasForeignKey(d => d.FkDepartmentId)
                .HasConstraintName("FK_Professions_Departments");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.Property(e => e.StudentId).HasColumnName("StudentID");
            entity.Property(e => e.StuFirstName).HasMaxLength(50);
            entity.Property(e => e.StuLastName).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
