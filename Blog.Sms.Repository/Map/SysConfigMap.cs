using Blog.Sms.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Sms.Repository.Map
{
  public  class SysConfigMap: IEntityTypeConfiguration<SysConfig>
    {
        public void Configure(EntityTypeBuilder<SysConfig> builder)
        {
            builder.ToTable("SYS_Config");
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id).HasColumnName("config_id");
            builder.Property(s => s.Key).HasColumnName("config_key");
            builder.Property(s => s.Value).HasColumnName("config_value");
            builder.Property(s => s.CreateTime).HasColumnName("config_createtime");
            builder.Property(s => s.UpdateTime).HasColumnName("config_updatetime");

        }
    }
}
