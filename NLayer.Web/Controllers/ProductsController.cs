using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NLayer.Core.DTOs;
using NLayer.Core.Entities;
using NLayer.Core.Services;
using NLayer.Web.Services;

namespace NLayer.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProductApiService _productApiService;
        private readonly CategoryApiService _categoryApiService;

        public ProductsController(ProductApiService productApiService, CategoryApiService categoryApiService)
        {
            _productApiService = productApiService;
            _categoryApiService = categoryApiService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _productApiService.GetProductsWithCategoryAsync());
        }

        public async Task<IActionResult> Save()
        {

            ViewBag.categories = Task.Run(async () => await AddCategoryListAsync()).Result;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Save(ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                await _productApiService.SaveAsync(productDto);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.categories = Task.Run(async () => await AddCategoryListAsync()).Result;
            return View();

        }
        [ServiceFilter(typeof(NotFoundFilter<Product>))]
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {

            var productDto = await _productApiService.GetByIdAsync(id);
            ViewBag.categories = Task.Run(async () => await AddCategoryListAsync(productDto.CategoryId)).Result;
            return View(productDto);

        }

        [HttpPost]
        public async Task<IActionResult> Update(ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                await _productApiService.UpdateAsync(productDto);
                return RedirectToAction("Index");
            }
            ViewBag.categories = Task.Run(async () => await AddCategoryListAsync(productDto.CategoryId)).Result;

            return View(productDto);
        }

        public async Task<IActionResult> Remove(int id)
        {

            await _productApiService.RemoveAsync(id);
            return RedirectToAction("Index");

        }


        public async Task<List<SelectListItem>> AddCategoryListAsync()
        {
            var categories = await _categoryApiService.GetAllAsync();
            List<SelectListItem> items = new SelectList(categories, "Id", "Name").ToList();
            items.Insert(0, (new SelectListItem { Text = "Select one", Value = "0" }));
            return items;
        }
        public async Task<List<SelectListItem>> AddCategoryListAsync(int categoryID)
        {
            var categories = await _categoryApiService.GetAllAsync();
            List<SelectListItem> items = new SelectList(categories, "Id", "Name", categoryID).ToList();
            items.Insert(0, (new SelectListItem { Text = "Select one", Value = "0" }));
            return items;
        }
    }
}
