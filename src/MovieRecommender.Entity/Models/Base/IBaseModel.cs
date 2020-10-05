using System;

namespace MovieRecommender.Entity.Models
{
    public interface IBaseModel
    {
        public Guid Id { get; set; }
    }
}