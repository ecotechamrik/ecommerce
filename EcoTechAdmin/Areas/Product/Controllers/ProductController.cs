﻿#region [ Namespace References ]
using System.Collections.Generic;
using BAL.ViewModels.Product;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using BAL;
#endregion

namespace EcoTechAdmin.Areas.Product.Controllers
{
    public class ProductController : ProductBaseController
    {
        #region [ Local Variables ]
        // Generate API Response Variable through Dependency Injection
        IUnitOfWork generateAPIResponse;
        #endregion

        #region [ Default Constructor - Used to call Inject Dependency Injection Method for API Calls ]
        public ProductController(IConfiguration _config, IUnitOfWork _generateAPIResponse)
        {
            generateAPIResponse = _generateAPIResponse;
        }
        #endregion

        #region [ Index - Load the List of Products ]
        /// <summary>
        /// Index - Load the List of Products
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }
        #endregion

        #region [ Load Index With Products Listing based on given Response from API ]
        /// <summary>
        /// Load Index With Products Listing based on given Response from API
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private async Task<IActionResult> RedirectToIndex()
        {
            IEnumerable<ProductViewModel> _products = await generateAPIResponse.ProductViewRepo.GetAll("product");
            return View("Index", _products);
        }
        #endregion

        #region [ Search the particular product ]
        /// <summary>
        /// Search the selected product
        /// </summary>
        /// <param name="currentFilter"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public async Task<IActionResult> Search(String currentFilter, String search)
        {
            if (search == null && !string.IsNullOrEmpty(currentFilter))
            {
                search = currentFilter;
            }
            ViewData["CurrentFilter"] = search;
            return PartialView("_ProductsList", await SearchResult(search));
        }

        /// <summary>
        /// Search Product Results
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        private async Task<IEnumerable<ProductViewModel>> SearchResult(string search)
        {
            var searchResults = await GetAllProductDataAsync(search);
            if (searchResults.Count() > 0)
                ViewBag.HaveRecords = true;
            else
                ViewBag.HaveRecords = false;
            return searchResults;
        }

        /// <summary>
        /// Get All Products List from All Properties of the product
        /// </summary>
        /// <returns></returns>
        private async Task<IEnumerable<ProductViewModel>> GetAllProductDataAsync(String _search)
        {
            IEnumerable<ProductViewModel> _products = await generateAPIResponse.ProductViewRepo.GetAll("product");

            // Search product details into all properties
            if (!String.IsNullOrEmpty(_search))
            {
                _products = _products.Where(p =>
                                          (p.ProductName != null ? p.ProductName.ToLower().Contains(_search.ToLower()) : false)
                                       || (p.CategoryName != null ? p.CategoryName.ToLower().Contains(_search.ToLower()) : false)
                                       || (p.ProductDesc != null ? p.ProductDesc.ToLower().Contains(_search.ToLower()) : false)
                                       || (p.ProductDesignName != null ? p.ProductDesignName.ToLower().Contains(_search.ToLower()) : false)
                                       || (p.ProductGradeName != null ? p.ProductGradeName.ToLower().Contains(_search.ToLower()) : false)
                                       || (p.SubCategoryName != null ? p.SubCategoryName.ToLower().Contains(_search.ToLower()) : false)
                                       ).ToList();
            }
            return _products;
        }
        #endregion

        #region [ Search the particular product by ProductCode ]
        /// <summary>
        /// Search the selected product
        /// </summary>
        /// <param name="currentFilter"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public async Task<IActionResult> SearchByCode(String currentFilter, String search)
        {
            if (search == null && !string.IsNullOrEmpty(currentFilter))
            {
                search = currentFilter;
            }
            ViewData["CurrentFilter"] = search;
            return PartialView("_ProductsList", await SearchResultByCode(search));
        }

        /// <summary>
        /// Search Product Results
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        private async Task<IEnumerable<ProductViewModel>> SearchResultByCode(string search)
        {
            var searchResults = await GetAllProductByCodeDataAsync(search);
            if (searchResults.Count() > 0)
                ViewBag.HaveRecords = true;
            else
                ViewBag.HaveRecords = false;
            return searchResults;
        }

        /// <summary>
        /// Get All Products List from All Properties of the product
        /// </summary>
        /// <returns></returns>
        private async Task<IEnumerable<ProductViewModel>> GetAllProductByCodeDataAsync(String _search)
        {
            IEnumerable<ProductViewModel> _products = await generateAPIResponse.ProductViewRepo.GetAll("product/SearchProducts/" + _search);
            return _products;
        }
        #endregion

        #region [ Create New Product ]
        /// <summary>
        /// Create New Product
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            BindDropDownLists();            
            return View();
        }
        #endregion

        #region [ Bind Drop Down Lists ]
        private void BindDropDownLists()
        {
            GetCategories();
            GetProductDesigns();
            GetProductGrades();
        }
        #endregion

        #region [ Get All Categories - Bind Categories Drop Down List ]
        /// <summary>
        /// Get All Categories - Bind Categories Drop Down List
        /// </summary>
        private void GetCategories()
        {
            IEnumerable<CategoryViewModel> _category =  generateAPIResponse.CategoryViewRepo.GetAll("category").Result;
            if (_category!=null)
            {
                ViewBag.Categories = _category;
            }
        }
        #endregion

        #region [ Get All Product Designs - Bind Product Designs Drop Down List ]
        /// <summary>
        /// Get All Product Designs - Bind Product Designs Drop Down List
        /// </summary>
        private void GetProductDesigns()
        {
            IEnumerable<ProductDesignViewModel> _productDesigns = generateAPIResponse.ProductDesignViewRepo.GetAll("productdesign").Result;
            if (_productDesigns != null)
            {
                ViewBag.ProductDesigns = _productDesigns;
            }
        }
        #endregion

        #region [ Get All Product Grades - Bind Product Grades Drop Down List ]
        /// <summary>
        /// Get All Product Grades - Bind Product Grades Drop Down List
        /// </summary>
        private void GetProductGrades()
        {
            IEnumerable<ProductGradeViewModel> _productGrades = generateAPIResponse.ProductGradeViewRepo.GetAll("productgrade").Result;
            if (_productGrades != null)
            {
                ViewBag.ProductGrades = _productGrades;
            }
        }
        #endregion

        #region [ Bind Sub Categories ]
        /// <summary>
        /// Bind Sub Categories
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> bindsubcategories(int? id)
        {
            IEnumerable<SubCategoryViewModel> _subcategory = await generateAPIResponse.SubCategoryViewRepo.GetAll("subcategory/getbycategoryid/" + id);
            return PartialView(@"~\Views\SubCatGallery\_SubCategory.cshtml", _subcategory);
        }
        #endregion

        #region [ Save New Product ]
        /// <summary>
        /// Save New Product
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            return await SaveProductDetails(model, "Create");
        }
        #endregion

        #region [ Edit Product ]
        /// <summary>
        /// Edit Product
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Edit(int id)
        {
            ProductViewModel model = await generateAPIResponse.ProductViewRepo.GetByID("product", id);
            if (model != null)
            {
                BindDropDownLists();
                return View("Create", model);
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region [ Update Product ]
        /// <summary>
        /// Update Product
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Edit(ProductViewModel model)
        {
            return await SaveProductDetails(model, "Edit");
        }
        #endregion

        #region [ Save & Update Product Details with Post & Put Methods of the Web APIs ]
        /// <summary>
        /// Save & Update Product Details with Post & Put Methods of the Web APIs.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        private async Task<IActionResult> SaveProductDetails(ProductViewModel model, String action)
        {
            try
            {
                dynamic response;   

                // Call Post Method to Create New Product Details
                if (action.ToLower() == "create")
                {
                    response = await generateAPIResponse.ProductViewRepo.Save("product", model);
                    ViewBag.Message = "Product record has been created successfully.";
                }
                // Call Put Method to Update Existing Product Details
                else
                {
                    response = await generateAPIResponse.ProductViewRepo.Update("product/" + model.ProductID, model);
                    ViewBag.Message = "Product record has been updated successfully.";
                }

                if (response)
                {
                    ViewBag.Class = "text-success";
                    return await RedirectToIndex();
                }
                else
                {
                    ViewBag.Message = null;
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Something went wrong: " + ex.Message;
            }
            BindDropDownLists();
            return View("Create", model);
        }
        #endregion

        #region [ Show Product Details ]
        /// <summary>
        /// Show Product Details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Details(int id)
        {
            ProductViewModel model = await generateAPIResponse.ProductViewRepo.GetByID("product", id);
            if (model != null)
                return View(model);
            return RedirectToAction("Index");
        }
        #endregion

        #region [ Delete Product Record form DB. ]
        /// <summary>
        /// Delete Product Record form DB.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await generateAPIResponse.ProductViewRepo.Delete("product/" + id);

                if (response)
                {
                    ViewBag.Message = "Product record has been deleted successfully.";
                    return await RedirectToIndex();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Something went wrong: " + ex.Message;
            }
            return RedirectToAction("Index");
        }
        #endregion
    }
}