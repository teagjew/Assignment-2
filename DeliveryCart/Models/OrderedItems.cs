using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Assignment_2.Models
{
    public class OrderedItem
    {
        public int ItemID { get; set; }
        public Item Item { get; set; }
        public int OrderID { get; set; }
        public Order Order { get; set; }
    }
}