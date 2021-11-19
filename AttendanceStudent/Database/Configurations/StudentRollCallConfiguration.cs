using System;
using System.Collections.Generic;
using AttendanceStudent.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AttendanceStudent.Database.Configurations
{
    public class StudentRollCallConfiguration : IEntityTypeConfiguration<StudentRollCall>
    {
        public void Configure(EntityTypeBuilder<StudentRollCall> builder)
        {
            builder.HasKey(u => new {u.StudentId, u.RollCallId});
            builder.HasIndex(u => new {u.StudentId, u.RollCallId});
            builder.ToTable("StudentRollCalls");
            builder.HasOne(r => r.Student).WithMany(r => r.StudentRollCalls).HasForeignKey(r => r.StudentId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(u => u.RollCall).WithMany(u => u.StudentRollCalls).HasForeignKey(u => u.RollCallId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}