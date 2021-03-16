using Blog.Domain.Gallery;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Repository.Map
{
    public class GalleryMap : IEntityTypeConfiguration<Gallery>
    {
        public void Configure(EntityTypeBuilder<Gallery> builder)
        {
            builder.ToTable("T_Gallery");
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id).HasColumnName("id");
            builder.Property(s => s.Account).HasColumnName("account");
            builder.Property(s => s.Url).HasColumnName("url");
            builder.Property(s => s.Lable).HasColumnName("lable");
            builder.Property(s => s.Description).HasColumnName("description");
            builder.Property(s => s.CreateTime).HasColumnName("createtime");
            builder.Property(s => s.UpdateTime).HasColumnName("updatetime");
        }
    }
}
