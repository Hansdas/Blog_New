using Blog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Repository.Map
{
    public class CommentMap : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("T_Comment");
            builder.HasKey(s => s.Guid);
            builder.Property(s => s.Guid).HasColumnName("comment_guid");
            builder.Property(s => s.Content).HasColumnName("comment_content");
            builder.Property(s => s.CommentType).HasColumnName("comment_type");
            builder.Property(s => s.PostUser).HasColumnName("comment_postuser");
            builder.Property(s => s.RevicerUser).HasColumnName("comment_revicer");
            builder.Property(s => s.AdditionalData).HasColumnName("comment_additional");
            builder.Property(s => s.PostDate).HasColumnName("comment_postdate");
        }
    }
}
