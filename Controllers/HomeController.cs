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

        // --- ACTION 8: ĐỌC TRUYỆN (MỚI) ---
        public async Task<IActionResult> ReadChapter(string slug, string chapterId)
        {
            if (string.IsNullOrEmpty(slug) || string.IsNullOrEmpty(chapterId))
            {
                return RedirectToAction("Index");
            }

            // 1. Gọi song song 2 API để tối ưu tốc độ
            var taskMangaDetail = _otruyenService.GetMangaDetailAsync(slug);
            var taskChapterDetail = _otruyenService.GetChapterDetailAsync(chapterId);

            await Task.WhenAll(taskMangaDetail, taskChapterDetail);

            var mangaData = taskMangaDetail.Result;
            var chapterData = taskChapterDetail.Result;

            // Kiểm tra dữ liệu
            if (mangaData == null || chapterData == null)
            {
                return NotFound();
            }

            // 2. Xử lý danh sách Chapter
            // Trong Model Otruyen, Chapter nằm trong mảng ServerVolume -> ServerData
            // Chúng ta sẽ lấy danh sách từ Server đầu tiên (thường là server chính)
            List<ChapterItem> allChapters = new List<ChapterItem>();

            if (mangaData.Item.Chapters != null && mangaData.Item.Chapters.Count > 0)
            {
                // Lấy dữ liệu từ server đầu tiên
                allChapters = mangaData.Item.Chapters[0].ServerData;
            }

            // 3. Tìm vị trí chapter hiện tại để tính Next/Prev
            // Lưu ý: api_data chính là chapterId
            // SỬA 1: Dùng .EndsWith() thay vì == để tìm ID trong đường dẫn dài
            var currentIndex = allChapters.FindIndex(c => c.ChapterApiData.EndsWith(chapterId));

            string nextChapId = null;
            string prevChapId = null;

            if (currentIndex != -1)
            {
                // Logic: Danh sách thường sắp xếp từ Mới nhất (index 0) -> Cũ nhất (index cuối)

                // Chap CŨ HƠN (Prev) -> Là phần tử có index lớn hơn (về phía cuối mảng)
                if (currentIndex > 0)
                {
                    // SỬA 2: Phải CẮT CHUỖI ngay ở đây để lấy ID sạch cho nút bấm
                    prevChapId = allChapters[currentIndex - 1].ChapterApiData.Split('/').Last();
                }

               
                // Chap MỚI HƠN (Next) -> Là phần tử có index nhỏ hơn (về phía 0)
                if (currentIndex < allChapters.Count - 1)
                {
                    // SỬA 2: Phải CẮT CHUỖI ngay ở đây
                    nextChapId = allChapters[currentIndex + 1].ChapterApiData.Split('/').Last();
                }
            }

            // 4. Tạo ViewModel
            var viewModel = new ReadChapterVM
            {
                CurrentChapter = new ChapterDetailData { 
                     DomainCdn = chapterData.DomainCdn,
                     Item = chapterData.Item
                } , // Dữ liệu ảnh
                ChapterList = allChapters,         // Danh sách chap
                MangaSlug = slug,
                CurrentChapterId = chapterId,
                CurrentChapterName = chapterData.Item.ChapterName,
                NextChapterId = nextChapId,
                PrevChapterId = prevChapId
            };

            // Đường dẫn ảnh CDN cần xử lý domain
            ViewData["DomainCdn"] = chapterData.DomainCdn;
            ViewData["Title"] = $"{chapterData.Item.ComicName} - Chapter {chapterData.Item.ChapterName}";

            return View(viewModel);
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