using Blog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Repository.Map
{
    public class LeaveMessageMap : IEntityTypeConfiguration<LeaveMessage>
    {
        public void Configure(EntityTypeBuilder<LeaveMessage> builder)
        {
            builder.ToTable("T_LeaveMessage");
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id).HasColumnName("lm_id");
            builder.Property(s => s.IsFriendLink).HasColumnName("lm_is_friendlink");
            builder.Property(s => s.Content).HasColumnName("lm_content");
            builder.Property(s => s.ContractEmail).HasColumnName("lm_contract_email");
            builder.Property(s => s.CreateTime).HasColumnName("lm_createtime");
            builder.Property(s => s.IsAction).HasColumnName("lm_is_action");
            builder.Property(s => s.UpdateTime).HasColumnName("lm_updatetime");
        }
    }
}
