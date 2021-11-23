using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AttendanceStudent.Database.Configurations
{
    public class SubjectConfiguration : IEntityTypeConfiguration<Models.Subject>
    {
        public void Configure(EntityTypeBuilder<Models.Subject> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).ValueGeneratedOnAdd();
            builder.HasIndex(u => u.Code).IsUnique();
            builder.Property(u => u.Name).IsRequired().IsUnicode().HasMaxLength(255);
            builder.ToTable("Subjects");
        }
    }
}