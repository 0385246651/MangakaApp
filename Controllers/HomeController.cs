using MangakaApp.Models;
using MangakaApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MangakaApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly OtruyenService _otruyenService;

        public HomeController(OtruyenService otruyenService)
        {
            _otruyenService = otruyenService;
        }

        // --- ACTION 1: TRANG CHỦ ---
        public async Task<IActionResult> Index(int page = 1)
        {
            MangaListData data = await _otruyenService.GetNewMangaAsync(page);

            // Gọi hàm phụ trợ để tính toán phân trang
            SetupPagination(data);

            // Đặt tiêu đề cho View
            ViewData["Title"] = "Trang chủ - Truyện mới cập nhật";
            ViewData["ActionName"] = "Index"; // Để View biết đang ở trang nào

            return View(data);
        }

        // --- ACTION 2: TRANG THỂ LOẠI (MỚI) ---
        public async Task<IActionResult> Category(string slug, int page = 1)
        {
            // 1. Lấy dữ liệu
            MangaListData data = await _otruyenService.GetMangaByCategoryAsync(slug, page);

            // 2. Tính toán phân trang (Hàm SetupPagination bạn đã viết ở bước trước)
            SetupPagination(data);

            // 3. Truyền thêm thông tin để View biết đường tạo Link
            ViewData["Title"] = $"Thể loại: {data.seoOnPage.titleHead}";

            ViewData["ActionName"] = "Category"; // <--- Báo hiệu đang ở Action Category
            ViewData["CurrentSlug"] = slug;      // <--- Truyền Slug để tạo link

            // 4. TRẢ VỀ VIEW INDEX thay vì View mặc định
            return View("Index", data);
        }

        // --- ACTION 3: TRUYỆN HOT (MỚI) ---
        public async Task<IActionResult> HotManga(int page = 1)
        {
            // 1. Gọi Service lấy dữ liệu Truyện Hot
            MangaListData data = await _otruyenService.GetHotMangaAsync(page);

            // 2. Tính toán phân trang (Dùng lại hàm SetupPagination có sẵn)
            SetupPagination(data);

            // 3. Thiết lập thông tin cho View
            ViewData["Title"] = "Truyện Hot - Được xem nhiều nhất";
            ViewData["ActionName"] = "HotManga"; // <--- Đặt tên Action để View nhận diện

            // 4. Trả về View Index
            return View("Index", data);
        }

      
        // --- ACTION 4: TRUYỆN Đang phát hành (MỚI) ---
        public async Task<IActionResult> ReleasingManga(int page = 1)
        {
            // 1. Gọi Service lấy dữ liệu Truyện Hot
            MangaListData data = await _otruyenService.GetReleasingMangaAsync(page);

            // 2. Tính toán phân trang (Dùng lại hàm SetupPagination có sẵn)
            SetupPagination(data);

            // 3. Thiết lập thông tin cho View
            ViewData["Title"] = "Truyện đang phát hành";
            ViewData["ActionName"] = "ReleasingManga"; // <--- Đặt tên Action để View nhận diện

            // 4. Trả về View Index
            return View("Index", data);
        }

        // --- ACTION 5: TRUYỆN Hoàn thành (MỚI) ---
        public async Task<IActionResult> DoneManga(int page = 1)
        {
            // 1. Gọi Service lấy dữ liệu Truyện Hot
            MangaListData data = await _otruyenService.GetDoneMangaAsync(page);

            // 2. Tính toán phân trang (Dùng lại hàm SetupPagination có sẵn)
            SetupPagination(data);

            // 3. Thiết lập thông tin cho View
            ViewData["Title"] = "Truyện Hoàn thành";
            ViewData["ActionName"] = "DoneManga"; // <--- Đặt tên Action để View nhận diện

            // 4. Trả về View Index
            return View("Index", data);
        }

        // --- ACTION 5: TRUYỆN sắp ra mắt (MỚI) ---
        public async Task<IActionResult> CommingManga(int page = 1)
        {
            // 1. Gọi Service lấy dữ liệu Truyện Hot
            MangaListData data = await _otruyenService.GetCommingMangaAsync(page);

            // 2. Tính toán phân trang (Dùng lại hàm SetupPagination có sẵn)
            SetupPagination(data);

            // 3. Thiết lập thông tin cho View
            ViewData["Title"] = "Truyện sắp ra mắt";
            ViewData["ActionName"] = "CommingManga"; // <--- Đặt tên Action để View nhận diện

            // 4. Trả về View Index
            return View("Index", data);
        }

        // --- ACTION 6: TÌM KIẾM (MỚI) ---
        public async Task<IActionResult> Search(string keyword, int page = 1)
        {
            // Kiểm tra nếu từ khóa rỗng thì quay về trang chủ
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return RedirectToAction("Index");
            }

            // 1. Gọi Service tìm kiếm
            MangaListData data = await _otruyenService.SearchMangaAsync(keyword, page);

            // 2. Tính toán phân trang
            SetupPagination(data);

            // 3. Thiết lập thông tin cho View
            ViewData["Title"] = $"Kết quả tìm kiếm: {keyword}";
            ViewData["ActionName"] = "Search";      // <--- Đánh dấu Action là Search
            ViewData["CurrentKeyword"] = keyword;   // <--- Lưu từ khóa để dùng cho phân trang

            // 4. Trả về View Index để hiển thị danh sách
            return View("Index", data);
        }

        // --- ACTION 7: LẤY SỐ LƯỢNG TRUYỆN CẬP NHẬT TRONG NGÀY (MỚI) ---
        public async Task<IActionResult> GetDailyUpdateCount()
        {
            // Gọi Service lấy dữ liệu (Hàm bạn cung cấp)
            var data = await _otruyenService.GetHomeMangaAsync();

            // Lấy số lượng, mặc định là 0 nếu null
            int count = data.Params?.ItemsUpdateInDay ?? 0;
            //data?.itemsUpdateInDay ?? 
            //0;

            // Trả về JSON
            return Json(new { count = count });
        }

        // --- ACTION 7: Detail truyện (MỚI) ---
        public async Task<IActionResult> Detail(string slug)
        {
            if (string.IsNullOrEmpty(slug))
            {
                return RedirectToAction("Index");
            }

            MangaDetailData data = await _otruyenService.GetMangaDetailAsync(slug);

            if (data == null)
            {
                return NotFound(); 
            }

           string thumbUrl =  _otruyenService.GetThumbUrl(data.Item.ThumbUrl);

            ViewData["Title"] = data.Item.Name  ?? "Chi tiết truyện";
            ViewData["ActionName"] = "Detail";
            ViewData["CurrentSlug"] = slug;
            ViewData["ThumbUrl"] = thumbUrl ?? "";

            // 3. Trả về View Detail.cshtml
            return View(data);
        }

        // --- HÀM PHỤ TRỢ (PRIVATE METHOD) ---
        // Giúp code gọn hơn, không phải viết lại công thức tính toán nhiều lần
        private void SetupPagination(MangaListData data)
        {
            if (data?.Params?.Pagination != null)
            {
                var pagination = data.Params.Pagination;

                int currentPage = pagination.CurrentPage;
                int totalItems = pagination.TotalItems;
                int itemsPerPage = pagination.TotalItemsPerPage > 0 ? pagination.TotalItemsPerPage : 1;
                int totalPages = (int)Math.Ceiling((double)totalItems / itemsPerPage);

                // Logic hiển thị 2 trang trước và sau
                int startPage = Math.Max(1, currentPage - 2);
                int endPage = Math.Min(totalPages, currentPage + 2);

                var pager = new PaginationVMModel // Đảm bảo bạn dùng đúng tên class ViewModel của bạn
                {
                    CurrentPage = currentPage,
                    TotalPages = totalPages,
                    TotalItems = totalItems,
                    StartPage = startPage,
                    EndPage = endPage
                };

                ViewBag.Pager = pager;
            }
        }

        // ... Các Action khác (Privacy, Error) giữ nguyên ...
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}