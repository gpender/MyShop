using System;

namespace MyShop.Core.Models
{
    public abstract class BaseEntity
    {
        public string Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public BaseEntity()
        {
            Id = Guid.NewGuid().ToString();
            CreatedAt = DateTime.Now;
        }
    }
}
