using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.Service.Entities;

namespace ZSZ.DAL.ModelConfig
{
    class HouseConfig:EntityTypeConfiguration<HouseEntity>
    {
        public HouseConfig()
        {
            ToTable("T_Houses");
            HasRequired(h => h.Community).WithMany().HasForeignKey(h => h.CommunityId).WillCascadeOnDelete(false);
            HasRequired(h => h.RoomType).WithMany().HasForeignKey(h => h.RoomTypeId).WillCascadeOnDelete(false);
            HasRequired(h => h.Status).WithMany().HasForeignKey(h => h.StatusId).WillCascadeOnDelete(false);
            HasRequired(h => h.DecorateStatus).WithMany().HasForeignKey(h => h.DecorateStatusId).WillCascadeOnDelete(false);
            HasRequired(h => h.Type).WithMany().HasForeignKey(h => h.TypeId).WillCascadeOnDelete(false);
            Property(h => h.Address).IsRequired().HasMaxLength(128);
            Property(h => h.Description).IsOptional();
            Property(h => h.Direction).IsRequired().HasMaxLength(20);
            Property(h => h.OwnerName).IsRequired().HasMaxLength(20);
            Property(h => h.OwnerPhoneNum).IsRequired().HasMaxLength(20).IsUnicode(false);
        }
    }
}
