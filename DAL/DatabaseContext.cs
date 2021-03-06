﻿using BAL.Entities;
using DAL.ModelBuilders;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace DAL
{
    public class DatabaseContext : DbContext
    {
        #region [ Create Common DB Entities ]
        /// <summary>
        /// Create Common DB Entities
        /// </summary>
        public DbSet<Currency> Currencies { get; set; }        
        #endregion

        #region [ Create User Info DB Entities ]
        /// <summary>
        /// Create User Info DB Entities
        /// </summary>
        public DbSet<User> Users { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<UserRoles> UserRoles { get; set; }
        #endregion

        #region [ Creating Product DB Entities ]
        /// <summary>
        /// Creating Product DB Entities
        /// </summary>
        public DbSet<Section> Sections { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<SubCatGallery> SubCatGalleries { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductDesign> ProductDesigns { get; set; }
        public DbSet<ProductGrade> ProductGrades { get; set; }
        public DbSet<DoorType> DoorType { get; set; }
        public DbSet<ProductHeight> ProductHeights { get; set; }
        public DbSet<ProductWidth> ProductWidths { get; set; }
        public DbSet<ProductThickness> ProductThicknesses { get; set; }
        public DbSet<ProductAttributeThickness> ProductAttributeThickness { get; set; }
        public DbSet<ProductAttribute> ProductAttributes { get; set; }
        public DbSet<ProductSizeAndPrice> ProductSizeAndPrices { get; set; }        
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<ProductSupplier> ProductSuppliers { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
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

        //// In Startup.cs in ConfigureServices(IServiceCollection services)
        /*
             services.AddDbContext<DatabaseContext>(options =>
             {
                options.UseSqlServer(Configuration.GetConnectionString("EcoTechCon"));
             });          
        */

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

            // Build Website Info Model
            UserInfoBuilder.UserInfoModel(modelBuilder);
        }
        #endregion
    }
}