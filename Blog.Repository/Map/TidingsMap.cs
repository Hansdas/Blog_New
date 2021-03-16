using Blog.Domain.Tidings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Repository.Map
{
    public class TidingsMap : IEntityTypeConfiguration<Tidings>
    {
        public void Configure(EntityTypeBuilder<Tidings> builder)
        {
            builder.ToTable("T_Tidings");
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id).HasColumnName("tidings_id");
            builder.Property(s => s.CommentId).HasColumnName("tidings_commentid");
            builder.Property(s => s.PostUser).HasColumnName("tidings_postuser");
            builder.Property(s => s.ReviceUser).HasColumnName("tidings_reviceuser");
            builder.Property(s => s.PostContent).HasColumnName("tidings_postcontent");
            builder.Property(s => s.IsRead).HasColumnName("tidings_isread");
            builder.Property(s => s.CreateTime).HasColumnName("tidings_createtime");
            builder.Property(s => s.Url).HasColumnName("tidings_url");
            builder.Property(s => s.AdditionalData).HasColumnName("tidings_additionaldata");
        }
    }
}
