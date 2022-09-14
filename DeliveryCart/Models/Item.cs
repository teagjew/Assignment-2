using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Assignment_2.Models
{
    public class Item
    {
        public int ItemID {get; set;} //Primary Key

        [StringLength(60,MinimumLength = 3)]
        [Required]
        public string Name {get; set;}

        [DataType(DataType.Currency)]
        [Required]
        public double Price {get; set;}

        public string Status { get; set; }

        public List<OrderedItem> OrderedItems { get; set; }
    }
}