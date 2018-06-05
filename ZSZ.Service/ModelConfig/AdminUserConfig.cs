using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.Service.Entities;

namespace ZSZ.DAL.ModelConfig
{
    class AdminUserConfig : EntityTypeConfiguration<AdminUserEntity>
    {
        public AdminUserConfig()
        {
            ToTable("T_AdminUsers");

            //一般配置到“多”端，因为“一端”可能根本不知道“多端”的存在
            HasOptional(u => u.City).WithMany().HasForeignKey(u => u.CityId)
                .WillCascadeOnDelete(false);

            HasMany(u => u.Roles).WithMany(r => r.AdminUsers).Map(m=>m.ToTable("T_AdminUserRoles")
                .MapLeftKey("AdminUserId").MapRightKey("RoleId"));
            Property(e => e.Name).HasMaxLength(50).IsRequired();
            Property(e => e.Email).HasMaxLength(30).IsRequired().IsUnicode(false);//varchar(30)
            Property(e => e.PhoneNum).HasMaxLength(20).IsRequired().IsUnicode(false);
            Property(e => e.PasswordSalt).HasMaxLength(20).IsRequired().IsUnicode(false);
            Property(e => e.PasswordHash).HasMaxLength(100).IsRequired().IsUnicode(false);
        }
    }
}
