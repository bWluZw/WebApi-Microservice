using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiBase.Services.UserManager
{
    public class InputUserModel
    {
        [Comment("用户账号")]
        [Column(TypeName = "varchar(30)")]
        public string? UserName { get; set; }

        [Comment("密码")]
        [Column(TypeName = "varchar(30)")]
        public string? Pwd { get; set; }
    }
}
