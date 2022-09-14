using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Assignment_2.Models
{
    public class Order
    {
        public int OrderID {get; set;} //Primary Key

        [StringLength(60,MinimumLength = 3)]
        [Required]
        public string CustomerName {get; set;}

        [StringLength(64,MinimumLength = 3)]
        [Required]
        public string CustomerAddress {get; set;}

        [DataType(DataType.Currency)]
        [Required]
        public double OrderTotal {get; set;}

        public string Status {get; set;}

        public List<OrderedItem> OrderedItems {get; set;}
    }
}