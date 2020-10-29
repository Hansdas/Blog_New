using Blog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Repository.Map
{
    public class WhisperMap : IEntityTypeConfiguration<Whisper>
    {
        public void Configure(EntityTypeBuilder<Whisper> builder)
        {
            builder.ToTable("T_Whisper");
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id).HasColumnName("whisper_id");
            builder.Property(s => s.Account).HasColumnName("whisper_account");
            builder.Property(s => s.CommentGuids).HasColumnName("whisper_commentguids");
            builder.Property(s => s.Content).HasColumnName("whisper_content");
            builder.Property(s => s.IsPassing).HasColumnName("whisper_ispassing");
            builder.Property(s => s.CreateTime).HasColumnName("whisper_createtime");
            builder.Property(s => s.UpdateTime).HasColumnName("whisper_updatetime");
        }
    }
}
