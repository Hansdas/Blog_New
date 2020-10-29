using Blog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Repository.Map
{
  public  class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("T_User");
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id).HasColumnName("user_id");
            builder.Property(s => s.LoginType).HasColumnName("user_logintype");
            builder.Property(s => s.Username).HasColumnName("user_username");
            builder.Property(s => s.Account).HasColumnName("user_account");
            builder.Property(s => s.Password).HasColumnName("user_password");
            builder.Property(s => s.Sex).HasColumnName("user_sex");
            builder.Property(s => s.Phone).HasColumnName("user_phone");
            builder.Property(s => s.Email).HasColumnName("user_email");
            builder.Property(s => s.BirthDate).HasColumnName("user_birthdate");
            builder.Property(s => s.Sign).HasColumnName("user_sign");
            builder.Property(s => s.HeadPhoto).HasColumnName("user_headphoto");
            builder.Property(s => s.IsValid).HasColumnName("user_isvalid");
            builder.Property(s => s.CreateTime).HasColumnName("user_createtime");
            builder.Property(s => s.UpdateTime).HasColumnName("user_updatetime");
        }
    }
}
