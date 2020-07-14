﻿using Microsoft.EntityFrameworkCore;
using BAL.Entities;

namespace DAL.ModelBuilders
{
    public class ProductBuilder
    {
        #region [ Product Model Builder ]
        /// <summary>
        /// Product Model Builder
        /// </summary>
        /// <param name="modelBuilder"></param>
        public static void ProductModel(ModelBuilder modelBuilder)
        {
            // Setting Product Table Name and Primary Key
            modelBuilder.Entity<Product>(p => { p.ToTable("Products").HasKey(p => p.ProductID); p.Property(p => p.ProductID).ValueGeneratedOnAdd(); });

            // Setting Product Name Required, MaxLengthand and Typename to nvarchar
            modelBuilder.Entity<Product>().Property(p => p.ProductName).IsRequired(true).HasMaxLength(500).IsUnicode(true);

            // 1:m
            //modelBuilder.Entity<Product>().HasOne(p => p.Category).WithMany(c => c.Products).HasForeignKey(p => p.CategoryID);

            // OR Alternate Way

            // Category To Products [1 - M Relationship]
            modelBuilder.Entity<Category>().HasMany(c => c.Products).WithOne(c => c.Category).HasForeignKey(c => c.CategoryID).OnDelete(DeleteBehavior.SetNull);

            // Category To Products [1 - M Relationship]
            modelBuilder.Entity<SubCategory>().HasMany(c => c.Products).WithOne(c => c.SubCategory).HasForeignKey(c => c.SubCategoryID).OnDelete(DeleteBehavior.SetNull);

            // Category To SubCategories [1 - M Relationship]
            modelBuilder.Entity<Category>().HasMany(s => s.SubCategories).WithOne(c => c.Category).HasForeignKey(c => c.CategoryID).OnDelete(DeleteBehavior.SetNull);

            // Product To ProductAttributes [1 - M Relationship]
            modelBuilder.Entity<Product>().HasMany(p => p.ProductAttributes).WithOne(p => p.Product).HasForeignKey(p => p.ProductID).OnDelete(DeleteBehavior.SetNull);
        }
        #endregion
    }
}