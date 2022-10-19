using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiBase.DatabaseAccessor;

namespace WebApiBase.Models
{
    [Table("user_info")]
    //[Serializable]
    public class UserInfo : IEntity
    {
        [Key]
        [Comment("ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Comment("用户账号")]
        [Column(TypeName = "varchar(30)")]
        public string? UserName { get; set; }

        [Comment("密码")]
        [Column(TypeName = "varchar(30)")]
        public string? Pwd { get; set; }

        [Comment("角色")]
        [Column(TypeName = "varchar(5)")]
        public string? Role { get; set; }

        [Comment("创建时间")]
        [Column(TypeName = "datetime(0)")]
        public DateTime CreationTime { get; set; }

        [Comment("头像")]
        [Column(TypeName = "varchar(50)")]
        public string HeadSculptrue { get; set; }

        [Comment("用户名")]
        [Column(TypeName = "varchar(30)")]
        public string Name { get; set; }

        [Comment("备注")]
        [Column(TypeName = "varchar(200)")]
        public string Remark { get; set; }
    }
}
