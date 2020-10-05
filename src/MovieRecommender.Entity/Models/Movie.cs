using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieRecommender.Entity.Models
{
    [Table("Movies")]
    public class Movie : IBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        
        [Required]
        public string Title { get; set; }
        
        [Required]
        public string OriginalTitle { get; set; }
        
        [Required]
        public DateTime ReleaseDate { get; set; }
    }
}