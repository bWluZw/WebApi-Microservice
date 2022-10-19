using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiBase.Models
{
    [NotMapped]
    public class TokenModel 
    {
        public UserInfo UserInfo { get; set; }
        public string nbf { get; set; }
        public string exp { get; set; }
        public string iss { get; set; }
        public string aud { get; set; }
    }
}
