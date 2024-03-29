using AttendanceStudent.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AttendanceStudent.Database.Configurations
{
    public class ImageConfiguration : IEntityTypeConfiguration<StudentImage>
    {
        public void Configure(EntityTypeBuilder<StudentImage> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).ValueGeneratedOnAdd();
            builder.HasIndex(u => u.Name);

            builder.Property(u => u.Name).IsRequired().IsUnicode().HasMaxLength(255);
            builder.Property(u => u.OriginalName).IsRequired().IsUnicode().HasMaxLength(255);
            builder.Property(t => t.Size).IsRequired();
            builder.Property(t => t.ContentType).IsRequired().IsUnicode().HasMaxLength(255);
            builder.HasOne(r => r.Student).WithMany(r => r.Images).HasForeignKey(r => r.StudentId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.ToTable("Student_Images");
        }
    }
}