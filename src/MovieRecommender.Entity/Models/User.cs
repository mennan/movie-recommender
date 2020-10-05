using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieRecommender.Entity.Models
{
    [Table("Users")]
    public class User : IBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}