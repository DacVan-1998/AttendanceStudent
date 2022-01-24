using AttendanceStudent.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AttendanceStudent.Database.Configurations
{
    public class AttendanceLogConfiguration : IEntityTypeConfiguration<AttendanceLog>
    {
        public void Configure(EntityTypeBuilder<AttendanceLog> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).ValueGeneratedOnAdd();
            builder.HasOne(r => r.RollCall).WithMany(r => r.AttendanceLogs).HasForeignKey(r => r.RollCallId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.ToTable("AttendanceLogs");
        }
    }
}