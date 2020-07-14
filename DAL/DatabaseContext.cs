﻿using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using BAL.Entities;
using DAL.ModelBuilders;

namespace DAL
{
    public class DatabaseContext : DbContext
    {
        #region [ Creating Product DB Entities ]
        /// <summary>
        /// Creating Product DB Entities
        /// </summary>
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<ProductAttributes> ProductAttributes { get; set; }
        #endregion

        #region [ Create Website Info DB Entities ]
        /// <summary>
        /// Create Website Info DB Entities
        /// </summary>
        public DbSet<WebsiteInfo> WebsiteInfos { get; set; }
        #endregion

        #region [ Read Connection String from appsettings.json and Create DB Connection ]
        /// <summary>
        /// Read Connection String from appsettings.json and Create DB Connection
        /// </summary>
        //public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        //{

        //}

        // Establish DB Connection with OnConfiguring Override
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            //optionsBuilder.UseSqlServer(@"data source=208.91.198.196; initial catalog=webasqvy_ecotechdoors;user id=ecotechdoors;password=EcoTech1@3#;MultipleActiveResultSets=true");
            optionsBuilder.UseSqlServer(new SqlConnection(configuration.GetConnectionString("EcoTechCon")));
        }
        #endregion

        #region [ Setting Table Properties with Fluent API through OnModelCreating override ]
        /// <summary>
        /// Setting Table Properties with Fluent API through OnModelCreating override
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Build Product Model
            ProductBuilder.ProductModel(modelBuilder);

            // Build Website Info Model
            WebsiteInfoBuilder.WebsiteInfoModel(modelBuilder);
        }        
        #endregion
    }
}