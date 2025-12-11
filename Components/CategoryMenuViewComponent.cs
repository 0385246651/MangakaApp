using Microsoft.AspNetCore.Mvc;
using MangakaApp.Services;
using System.Threading.Tasks;
using System.Collections.Generic; 
using MangakaApp.Models;       

namespace MangakaApp.Components
{
    public class CategoryMenuViewComponent : ViewComponent
    {
        private readonly OtruyenService _otruyenService;

        public CategoryMenuViewComponent(OtruyenService otruyenService)
        {
            _otruyenService = otruyenService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // 1. Gọi Service lấy object CategoryListData
            var data = await _otruyenService.GetCategoriesAsync();

            // 2. Kiểm tra null để tránh lỗi
            var items = data?.Items ?? new List<Category>();

            // 3. Truyền List<Category> sang View Default.cshtml
            return View(items);


        }
    }
}