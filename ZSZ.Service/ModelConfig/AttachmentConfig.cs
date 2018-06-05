using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.Service.Entities;

namespace ZSZ.DAL.ModelConfig
{
    class AttachmentConfig:EntityTypeConfiguration<AttachmentEntity>
    {
        public AttachmentConfig()
        {
            ToTable("T_Attachments");
            //多对多WithMany不能空
            HasMany(a => a.Houses).WithMany(a=>a.Attachments).Map(m=>m.ToTable("T_HouseAttachments")
                .MapLeftKey("AttachmentId").MapRightKey("HouseId"));
            Property(e => e.IconName).IsRequired().HasMaxLength(50).IsUnicode(false);
            Property(e => e.Name).IsRequired().HasMaxLength(50);
        }
    }
}
