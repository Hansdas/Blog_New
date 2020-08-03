using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Blog.Domain.Article;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Repoistory.DB
{
    public class ArticleMap : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {
            builder.ToTable("T_Article");
            builder.HasKey(s=>s.Id);
            builder.Property(s => s.Id).HasColumnName("article_id");
            builder.Property(s => s.Author).HasColumnName("article_author");
            builder.Property(s => s.Title).HasColumnName("article_title");
            builder.Property(s => s.TextSection).HasColumnName("article_textsection");
            builder.Property(s => s.Content).HasColumnName("article_content");
            builder.Property(s => s.ArticleType).HasColumnName("article_articletype");
            builder.Property(s => s.PraiseCount).HasColumnName("article_praisecount");
            builder.Property(s => s.BrowserCount).HasColumnName("article_browsercount");
            builder.Property(s => s.CommentIds).HasColumnName("article_comments");
            builder.Property(s => s.IsDraft).HasColumnName("article_isdraft");
            builder.Property(s => s.IsSendEmail).HasColumnName("article_sendemail");
            builder.Property(s => s.CreateTime).HasColumnName("article_createtime");
            builder.Property(s => s.UpdateTime).HasColumnName("article_updatetime");
        }
    }
}
