﻿using Pan.Affiliation.Infrastructure.Data.Entities.Shared;
using System.ComponentModel.DataAnnotations;

namespace Pan.Affiliation.Infrastructure.Data.Entities
{
    public class Customer : BaseEntity
    {
        [MaxLength(255), Required]
        public string? Name { get; set; }
        
        [MaxLength(14), Required]
        public string? DocumentNumber { get; set; }

        public IEnumerable<Address>? Addresses { get; set; }
    }
}