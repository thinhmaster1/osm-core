﻿using OSM.Infrastructure.SharedKernel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OSM.Data.Entities
{
    [Table("Colors")]
    public class Color : DomainEntity<int>
    {
        [StringLength(250)]
        public string Name
        {
            get; set;
        }
    }
}