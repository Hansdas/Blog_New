using Blog.Quartz.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Quartz.Repository.Map
{
    public class QuartzOptionMap : IEntityTypeConfiguration<QuartzOption>
    {
        public void Configure(EntityTypeBuilder<QuartzOption> builder)
        {
            builder.ToTable("SYS_Quartz");
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id).HasColumnName("id");
            builder.Property(s => s.JobName).HasColumnName("job_name");
            builder.Property(s => s.GroupName).HasColumnName("group_name");
            builder.Property(s => s.Cron).HasColumnName("cron");
            builder.Property(s => s.Api).HasColumnName("api");
            builder.Property(s => s.LastActionTime).HasColumnName("last_action_time");
            builder.Property(s => s.Description).HasColumnName("description");
            builder.Property(s => s.RequestType).HasColumnName("resuest_type");
            builder.Property(s => s.ParameterValue).HasColumnName("parameter_value");
            builder.Property(s => s.TaskStatus).HasColumnName("task_status");
            builder.Property(s => s.CreateTime).HasColumnName("create_time");
            builder.Property(s => s.UpdateTime).HasColumnName("update_time");
        }
    }
}
