using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerShopApi.Models
{
    public class Computer
    {
        [Key]
        [MaxLength(36)]

        public Guid Id { get; set; }

        [Column("varchar(37)")]
        public string? Brand { get; set; }

        [Column("(30)")]
        public string? Type { get; set; }

        [Column("double")]
        public float Display { get; set; }

        public int Memory { get; set; }

        public DateTime CreatedTime { get; set; }
    }
}
