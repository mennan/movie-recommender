using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieRecommender.Entity.Models
{
    [Table("UserMovieRates")]
    public class UserMovieRate : IBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        
        [Required]
        public Guid UserId { get; set; }
        
        [Required]
        public Guid MovieId { get; set; }
        
        [Required]
        public short Rating { get; set; }
        
        [Required]
        public string Note { get; set; }
    }
}