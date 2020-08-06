﻿using System;
using System.Collections.Generic;

namespace BAL.Entities
{
    public class Product
    {
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        //public string ProductCode { get; set; }
        public string ProductDesc { get; set; }
        public string PriorityNumber { get; set; }
        public string Notes { get; set; }
        public string GroupNumber { get; set; }
        public string BuildingCode { get; set; }
        public string LocationCode { get; set; }
        public string RetailBin { get; set; }
        public string WholeSaleBin { get; set; }
        public string IndexNumber { get; set; }
        public int? CategoryID { get; set; }
        public int? SubCategoryID { get; set; }
        public int? ProductDesignID { get; set; }
        public int? ProductGradeID { get; set; }
        public int BestQuantity { get; set; }
        public int CurrentQuantity { get; set; }
        public string IPAddress { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }

        // Navigation Property
        //[ForeignKey("CategoryID")] //-- CategoryID
        public Category Category { get; set; }
        public SubCategory SubCategory { get; set; }
        public ProductDesign ProductDesign { get; set; }
        public ProductGrade ProductGrade { get; set; }
        public ICollection<ProductAttribute> ProductAttributes { get; set; }
        public ICollection<ProductImages> ProductImages { get; set; }
    }
}