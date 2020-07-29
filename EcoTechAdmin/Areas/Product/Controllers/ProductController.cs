﻿#region [ Namespace References ]
using System.Collections.Generic;
using BAL.ViewModels.Product;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Text;
#endregion

namespace EcoTechAdmin.Areas.Product.Controllers
{
    public class ProductController : ProductBaseController
    {
        #region [ Local Variables ]
        // Get API URL from appsettings.json
        IConfiguration config;

        // HttpClient Variable to access the Web APIs
        HttpClient client;
        #endregion

        #region [ Default Constructor  ]
        public ProductController(IConfiguration _config)
        {
            client = new HttpClient();
            config = _config;
            client.BaseAddress = new Uri(config["URL:API"]);
        }
        #endregion

        #region [ Index - Load the List of Products ]
        /// <summary>
        /// Index - Load the List of Products
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            return await RedirectToIndex();
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
            var response = await client.GetAsync(client.BaseAddress + "product");
            IEnumerable<ProductViewModel> _products = new List<ProductViewModel>();
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                _products = JsonConvert.DeserializeObject<IEnumerable<ProductViewModel>>(data);
            }
            return View("Index", _products);
        }
        #endregion

        #region [ Create New Product ]
        /// <summary>
        /// Create New Product
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            GetCategories();
            return View();
        }
        #endregion

        #region [ Get All Categories - Bind Categories Drop Down List ]
        /// <summary>
        /// Get All Sub Categories - Bind Categories Drop Down List
        /// </summary>
        private void GetCategories()
        {
            var response = client.GetAsync(client.BaseAddress + "category").Result;
            IEnumerable<CategoryViewModel> _category = new List<CategoryViewModel>();
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                _category = JsonConvert.DeserializeObject<IEnumerable<CategoryViewModel>>(data);
                ViewBag.Categories = _category;
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
            IEnumerable<SubCategoryViewModel> _subcategory = new List<SubCategoryViewModel>();

            var response = await client.GetAsync(client.BaseAddress + "subcategory/getbycategoryid/" + id);

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                _subcategory = JsonConvert.DeserializeObject<IEnumerable<SubCategoryViewModel>>(data);
            }
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
        public IActionResult Edit(int id)
        {
            var response = client.GetAsync(client.BaseAddress + "product" + "/" + id).Result;
            ProductViewModel model = new ProductViewModel();

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;

                if (data != "" && data.StartsWith('['))
                    data = data.Substring(1, data.Length - 2);
                model = JsonConvert.DeserializeObject<ProductViewModel>(data);

                if (model != null)
                {
                    GetCategories();
                    return View("Create", model);
                }
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
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

                var response = new HttpResponseMessage();

                // Call Post Method to Create New Product Details
                if (action.ToLower() == "create")
                {
                    response = await client.PostAsync(client.BaseAddress + "product", content);
                    ViewBag.Message = "Product record has been created successfully.";
                }
                // Call Put Method to Update Existing Product Details
                else
                {
                    response = await client.PutAsync(client.BaseAddress + "product/" + model.ProductID, content);
                    ViewBag.Message = "Product record has been updated successfully.";
                }

                if (response.IsSuccessStatusCode)
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
            GetCategories();
            return View("Create", model);
        }
        #endregion

        #region [ Show Product Details ]
        /// <summary>
        /// Show Product Details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Details(int id)
        {
            var response = client.GetAsync(client.BaseAddress + "product" + "/" + id).Result;
            ProductViewModel model = new ProductViewModel();

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;

                if (data != "" && data.StartsWith('['))
                    data = data.Substring(1, data.Length - 2);
                model = JsonConvert.DeserializeObject<ProductViewModel>(data);

                if (model != null)
                    return View(model);
            }

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
                var response = await client.DeleteAsync(client.BaseAddress + "product/" + id);

                if (response.IsSuccessStatusCode)
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