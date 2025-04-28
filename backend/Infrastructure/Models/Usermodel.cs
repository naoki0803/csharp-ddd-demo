using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace TodoApi.Infrastructure.Models
{
    [Table("users")]
    public class UserModel : BaseModel
    {
        [PrimaryKey("id")]
        public string? Id { get; set; }

        [Column("name")]
        public string? Name { get; set; }
    }
}