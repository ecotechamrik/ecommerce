﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BAL.Entities
{
    public class Supplier
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SupplierID { get; set; }
        public string Name { get; set; }
        public string ModelCode { get; set; }
        public double InboundCost { get; set; }
        public double LandedCost { get; set; }
        public double SupplierCost { get; set; }


        /* -- Navigation Properties -- */
        public DateTime CreatedDateTime { get; set; }
        public DateTime UpdatedDateTime { get; set; }
        public ICollection<ProductSizeAndPrice> ProductSizeAndPrices { get; set; }
    }
}