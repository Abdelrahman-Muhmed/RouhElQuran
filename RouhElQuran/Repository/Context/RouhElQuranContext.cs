using System;
using System.Collections.Generic;
using Core;
using Core.HelperModel;
using Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Repository.Models;

public partial class RouhElQuranContext : IdentityDbContext<AppUser, IdentityRole<int>, int>
{
    public RouhElQuranContext(DbContextOptions<RouhElQuranContext> options) : base(options)
    { }

    public virtual DbSet<Attendence> Attendences { get; set; }
    public virtual DbSet<Booking> Bookings { get; set; }
    public virtual DbSet<Course> Courses { get; set; }
    public virtual DbSet<Employee> Employees { get; set; }
    public virtual DbSet<Exam> Exams { get; set; }
    public virtual DbSet<Instructor> Instructors { get; set; }
    public virtual DbSet<Student> Students { get; set; }
    public virtual DbSet<Student_Course> Student_Crs { get; set; }
    public virtual DbSet<Ins_Course> Ins_Crs { get; set; }
    public virtual DbSet<freeClass> FreeClasses { get; set; }
    public virtual DbSet<CoursePlan> CoursePlans { get; set; }
    public virtual DbSet<UserPayments> UserPayments { get; set; }
	public virtual DbSet<Files> Files { get; set; }
	public virtual DbSet<InstructorSpecialty> InstructorSpecialty { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //modelBuilder.Entity<IdentityRole<int>>().HasData(
        //    new IdentityRole<int> { Id = 1, Name = RolesNames.Admin, NormalizedName = RolesNames.Admin.ToUpper() },
        //    new IdentityRole<int> { Id = 2, Name = RolesNames.Student, NormalizedName = RolesNames.Student.ToUpper() }
        //);


        modelBuilder.Entity<Attendence>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Attenden__3214EC073E165244");

            entity.ToTable("Attendence");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Duration).HasMaxLength(50);
            entity.Property(e => e.InsId).HasColumnName("Ins_Id");
            entity.Property(e => e.StdId).HasColumnName("Std_Id");

            entity.HasOne(d => d.Ins).WithMany(p => p.Attendences)
                .HasForeignKey(d => d.InsId)
                .HasConstraintName("fk_Attend_Ins_Id");

            entity.HasOne(d => d.Std).WithMany(p => p.Attendences)
                .HasForeignKey(d => d.StdId)
                .HasConstraintName("fk_Attend_Std_Id");
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Booking__3214EC0726CBF4B2");

            entity.ToTable("Booking");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.CountDaysInWeek).HasColumnName("Count_Days_In_Week");
            entity.Property(e => e.CourseId).HasColumnName("Course_id");
            entity.Property(e => e.DaysLearning)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Days_Learning");
            entity.Property(e => e.StdId).HasColumnName("Std_id");
            entity.Property(e => e.TimeLearning).HasColumnName("Time_Learning");

            entity.HasOne(d => d.Course).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK__Booking__Course___47DBAE45");

            entity.HasOne(d => d.Std).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.StdId)
                .HasConstraintName("FK__Booking__Std_id__46E78A0C");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Courses__3214EC07B6BA946D");

            entity.HasMany(e => e.Ins_Courses).WithOne();
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.CoursesTime).HasColumnName("Courses_Time");
            entity.Property(e => e.CrsName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Crs_Name");
            entity.Property(e => e.SessionTime).HasColumnName("Session_Time");
            entity.Property(e => e.Specialty)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC079D52C16B");
            entity.HasOne(d => d.User_Id).WithOne(p => p.Emp)
            .HasForeignKey<Employee>(d => d.EmpUser_Id);

            entity.ToTable("Employee");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.HireDate).HasColumnName("Hire_Date");
            entity.Property(e => e.Salary).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<Exam>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Exam__3214EC07753C783F");

            entity.ToTable("Exam");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.CourseId).HasColumnName("Course_id");
            entity.Property(e => e.ExamResult)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("Exam_Result");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.StdId).HasColumnName("Std_id");

            entity.HasOne(d => d.Course).WithMany(p => p.Exams)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK__Exam__Course_id__440B1D61");

            entity.HasOne(d => d.Std).WithMany(p => p.Exams)
                .HasForeignKey(d => d.StdId)
                .HasConstraintName("FK__Exam__Std_id__4316F928");
        });

        modelBuilder.Entity<Instructor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Instruct__DD4A9EC21D0C8E56");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("Instructor_id");
            entity.Property(e => e.Certificate)
                .HasMaxLength(100)
                .IsUnicode(false);
           
            entity.Property(e => e.DaysWork)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("Days_Work");
            entity.Property(e => e.Salary).HasColumnType("decimal(10, 2)");

            entity.Property(e => e.TimeFrom)
                .HasColumnName("Time_From");

            entity.Property(e => e.TimeTo)
             .HasColumnName("Time_To");

            entity.Property(e => e.Description)
            .HasMaxLength(800)
            .HasColumnName("Description");

            entity.Property(e => e.WorkExperienceFrom)
            .IsRequired();
            entity.Property(e => e.WorkExperienceTo)
           .IsRequired();

            entity.Property(e => e.YearsOfExperience)
            .HasMaxLength(100)
            .IsUnicode(false);

            entity.HasOne(d => d.User_id).WithOne(p => p.Ins)
            .HasForeignKey<Instructor>(d => d.InsUser_Id);
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Student__3214EC07E08C3C9A");

            entity.HasOne(d => d.User_id).WithOne(p => p.Std)
           .HasForeignKey<Student>(d => d.StdUser_Id);

            entity.ToTable("Student");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.CountHours).HasColumnName("Count_Hours");
        });

        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC07C842DC7C");

            //entity.ToTable("User");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Country).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.Language).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
        });

        modelBuilder.Entity<Ins_Course>(entity =>
        {
            entity.ToTable("Ins_Courses");

            // Configure foreign keys
            entity.HasOne(ic => ic.Instructor)
                .WithMany(i => i.Ins_Courses)
                .HasForeignKey(ic => ic.Ins_Id)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(ic => ic.Course)
                .WithMany(c => c.Ins_Courses)
                .HasForeignKey(ic => ic.Course_Id)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Student_Course>(entity =>
        {
            // Configure foreign keys
            entity.HasOne(ic => ic.students)
                .WithMany(i => i.Std_Courses)
                .HasForeignKey(ic => ic.Std_Id)
                .OnDelete(DeleteBehavior.Cascade); // Adjust deletion behavior as needed

            entity.HasOne(ic => ic.Course)
                .WithMany(c => c.Std_Course)
                .HasForeignKey(ic => ic.Course_Id)
                .OnDelete(DeleteBehavior.Cascade); // Adjust deletion behavior as needed
        });

		modelBuilder.Entity<Files>(entity =>
		{
			entity.HasOne(f => f.AppUser)
				.WithMany(u => u.files)  
				.HasForeignKey(f => f.AppUserId) 
				.OnDelete(DeleteBehavior.Cascade); 
		});
		modelBuilder.Entity<Files>(entity =>
		{
			entity.HasOne(f => f.Course) 
				.WithMany(c => c.files)  
				.HasForeignKey(f => f.CourseId) 
				.OnDelete(DeleteBehavior.Cascade); 
		});

		modelBuilder.Entity<InstructorSpecialty>(entity =>
		{
			entity.HasOne(f => f.Instructor)
				.WithMany(c => c.InstructorSpecialty)
				.HasForeignKey(f => f.InstructorId)
				.OnDelete(DeleteBehavior.Cascade);
		});

	}
}
