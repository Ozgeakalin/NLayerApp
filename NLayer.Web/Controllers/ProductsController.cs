using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NLayer.Core.DTOs;
using NLayer.Core.Entities;
using NLayer.Core.Services;

namespace NLayer.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _services;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public ProductsController(IProductService services, ICategoryService categoryService, IMapper mapper)
        {
            _services = services;
            _categoryService = categoryService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _services.GetProductWithCategory());
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
                var product = _mapper.Map<Product>(productDto);
                await _services.AddAsync(product);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.categories = Task.Run(async () => await AddCategoryListAsync()).Result;
            return View();

        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            if (await _services.AnyAsync(x => x.Id == id))
            {
                var product = await _services.GetByIdAsync(id);
                var productDto = _mapper.Map<ProductDto>(product);
                ViewBag.categories = Task.Run(async () => await AddCategoryListAsync(productDto.CategoryId)).Result;
                return View(productDto);
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Update(ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                await _services.UpdateAsync(_mapper.Map<Product>(productDto));
                return RedirectToAction("Index");
            }
            ViewBag.categories = Task.Run(async () => await AddCategoryListAsync(productDto.CategoryId)).Result;

            return View(productDto);
        }

        public  async Task< List<SelectListItem>> AddCategoryListAsync()
        {
            var categories = await _categoryService.GetAllAsync();
            var categoriesDto = _mapper.Map<List<CategoryDto>>(categories.ToList());
            List<SelectListItem> items = new SelectList(categoriesDto, "Id", "Name").ToList();
            items.Insert(0, (new SelectListItem { Text = "Select one", Value = "0" }));
            return items;
        }
        public async Task<List<SelectListItem>> AddCategoryListAsync(int categoryID)
        {
            var categories = await _categoryService.GetAllAsync();
            var categoriesDto = _mapper.Map<List<CategoryDto>>(categories.ToList());
            List<SelectListItem> items = new SelectList(categoriesDto, "Id", "Name",categoryID).ToList();
            items.Insert(0, (new SelectListItem { Text = "Select one", Value = "0" }));
            return items;
        }
    }
}
