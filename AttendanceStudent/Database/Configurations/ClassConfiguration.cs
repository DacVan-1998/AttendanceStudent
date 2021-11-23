using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AttendanceStudent.Database.Configurations
{
    public class ClassConfiguration : IEntityTypeConfiguration<Models.Class>
    {
        public void Configure(EntityTypeBuilder<Models.Class> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).ValueGeneratedOnAdd();
            builder.HasIndex(u => u.Code).IsUnique();
            builder.Property(u => u.Name).IsRequired().IsUnicode().HasMaxLength(255);
            builder.ToTable("Classes");
        }
    }
}