using AttendanceStudent.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AttendanceStudent.Database.Configurations
{
    public class RollCallConfiguration : IEntityTypeConfiguration<Models.RollCall>
    {
        public void Configure(EntityTypeBuilder<Models.RollCall> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).ValueGeneratedOnAdd();
            builder.Property(u => u.StartTime).IsUnicode().HasMaxLength(50);
            builder.Property(u => u.FinishTime).IsUnicode().HasMaxLength(50);
            builder.ToTable("RollCalls");
            builder.HasOne(r => r.Subject).WithMany(r => r.RollCalls).HasForeignKey(r => r.SubjectId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(u => u.Class).WithMany(u => u.RollCalls).HasForeignKey(u => u.ClassId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}