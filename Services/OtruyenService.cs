using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MangakaApp.Models;
using System.Collections.Generic;

namespace MangakaApp.Services
{
    public class OtruyenService
    {
        private readonly HttpClient _httpClient;
        private const string BASE_URL = "https://otruyenapi.com/v1/api";
        private const string CHAPTER_BASE_URL = "https://sv1.otruyencdn.com/v1/api";
        public const string IMAGE_CDN_URL = "https://otruyenapi.com/uploads/comics/";

        public OtruyenService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<MangaListData> GetHomeMangaAsync()
        {
            string url = $"{BASE_URL}/home";
            var response = await _httpClient.GetStringAsync(url);
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<MangaListData>>(response);
            return apiResponse?.Data;
        }

        public async Task<MangaListData> GetNewMangaAsync(int page)
        {
            string url = $"{BASE_URL}/danh-sach/truyen-moi?page={page}";
            var response = await _httpClient.GetStringAsync(url);
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<MangaListData>>(response);
            return apiResponse?.Data;
        }

        public async Task<MangaListData> GetHotMangaAsync(int page = 1)
        {
            string url = $"{BASE_URL}/danh-sach/truyen-hot?page={page}";
            var response = await _httpClient.GetStringAsync(url);
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<MangaListData>>(response);
            return apiResponse?.Data;
        }

        public async Task<MangaListData> GetReleasingMangaAsync(int page = 1)
        {
            string url = $"{BASE_URL}/danh-sach/dang-phat-hanh?page={page}";
            var response = await _httpClient.GetStringAsync(url);
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<MangaListData>>(response);
            return apiResponse?.Data;
        }

        public async Task<MangaListData> GetCommingMangaAsync(int page = 1)
        {
            string url = $"{BASE_URL}/danh-sach/sap-ra-mat?page={page}";
            var response = await _httpClient.GetStringAsync(url);
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<MangaListData>>(response);
            return apiResponse?.Data;
        }

        public async Task<MangaListData> GetDoneMangaAsync(int page = 1)
        {
            string url = $"{BASE_URL}/danh-sach/hoan-thanh?page={page}";
            var response = await _httpClient.GetStringAsync(url);
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<MangaListData>>(response);
            return apiResponse?.Data;
        }

        public async Task<MangaListData> SearchMangaAsync(string keyword, int page = 1)
        {
            string url = $"{BASE_URL}/tim-kiem?keyword={Uri.EscapeDataString(keyword)}&page={page}";
            var response = await _httpClient.GetStringAsync(url);
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<MangaListData>>(response);
            return apiResponse?.Data;
        }

        public async Task<MangaDetailData> GetMangaDetailAsync(string slug)
        {
            string url = $"{BASE_URL}/truyen-tranh/{slug}";
            var response = await _httpClient.GetStringAsync(url);
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<MangaDetailData>>(response);
            return apiResponse?.Data;
        }

        public async Task<ChapterDetailData> GetChapterDetailAsync(string chapterId)
        {
            string url = $"{CHAPTER_BASE_URL}/chapter/{chapterId}";
            var response = await _httpClient.GetStringAsync(url);
            var apiResponse = JsonConvert.DeserializeObject<ChapterDetailResponse>(response);
            return apiResponse?.Data;
        }
       
        public async Task<CategoryListData> GetCategoriesAsync()
        {
            string url = $"{BASE_URL}/the-loai";

            try
            {
                // Gọi API và map vào ApiResponse<CategoryListData>
                var response = await _httpClient.GetFromJsonAsync<ApiResponse<CategoryListData>>(url);

                // Trả về phần Data (chính là CategoryListData)
                return response?.Data;
            }
            catch (Exception ex)
            {
                // Xử lý lỗi hoặc log lỗi nếu cần
                Console.WriteLine("Lỗi lấy thể loại: " + ex.Message);
                return new CategoryListData { Items = new List<Category>() };
            }
        }

        public async Task<MangaListData> GetMangaByCategoryAsync(string slug, int page = 1)
        {
            string url = $"{BASE_URL}/the-loai/{slug}?page={page}";
            var response = await _httpClient.GetStringAsync(url);
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<MangaListData>>(response);
            return apiResponse?.Data;
        }

        public string GetThumbUrl(string thumbPath)
        {
             if (string.IsNullOrEmpty(thumbPath)) return "";
             if (thumbPath.StartsWith("http")) return thumbPath;
             return $"{IMAGE_CDN_URL}{thumbPath}";
        }
    }
}
