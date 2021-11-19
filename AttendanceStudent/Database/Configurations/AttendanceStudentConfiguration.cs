using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AttendanceStudent.Database.Configurations
{
    public class AttendanceStudentConfiguration : IEntityTypeConfiguration<Models.AttendanceStudent>
    {
        public void Configure(EntityTypeBuilder<Models.AttendanceStudent> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).ValueGeneratedOnAdd();
            builder.Property(u => u.Note).IsRequired().IsUnicode().HasMaxLength(255);
            builder.ToTable("AttendanceStudents");
            builder.HasOne(r => r.Student).WithMany(r => r.AttendanceStudents).HasForeignKey(r => r.StudentId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(u => u.AttendanceLog).WithMany(u => u.AttendanceStudents).HasForeignKey(u => u.AttendanceLogId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}