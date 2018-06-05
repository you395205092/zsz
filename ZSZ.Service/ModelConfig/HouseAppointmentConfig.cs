using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.Service.Entities;

namespace ZSZ.DAL.ModelConfig
{
    class HouseAppointmentConfig:EntityTypeConfiguration<HouseAppointmentEntity>
    {
        public HouseAppointmentConfig()
        {
            ToTable("T_HouseAppointments");
            HasOptional(h => h.User).WithMany().HasForeignKey(h=>h.UserId).WillCascadeOnDelete(false);
            HasRequired(h => h.House).WithMany().HasForeignKey(h=>h.HouseId).WillCascadeOnDelete(false);
            HasOptional(h => h.FollowAdminUser).WithMany().HasForeignKey(h=>h.FollowAdminUserId).WillCascadeOnDelete(false);
            Property(h => h.Name).IsRequired().HasMaxLength(20);
            Property(h => h.PhoneNum).IsRequired().HasMaxLength(20).IsUnicode(false);
            Property(h => h.Status).IsRequired().HasMaxLength(20);
            Property(h => h.RowVersion).IsRequired().IsRowVersion();
        }
    }
}
