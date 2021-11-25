using AttendanceStudent.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AttendanceStudent.Database.Configurations
{
    public class StudentConfiguration: IEntityTypeConfiguration<Models.Student>
    {
        public void Configure(EntityTypeBuilder<Models.Student> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).ValueGeneratedOnAdd();
            builder.HasIndex(u => u.StudentCode).IsUnique();
            builder.HasIndex(u => u.Email);
            builder.Property(u => u.FullName).IsRequired().IsUnicode().HasMaxLength(255);
            builder.Property(u => u.Email).IsRequired().IsUnicode().HasMaxLength(255);
            builder.Property(u => u.PhoneNumber).IsUnicode().HasMaxLength(50);
            builder.ToTable("Students");
        }
    }
}