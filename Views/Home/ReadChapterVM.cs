using MangakaApp.Models;

namespace MangakaApp.Models
{
    public class ReadChapterVM
    {
        // Thông tin ảnh của chapter đang đọc
        public ChapterDetailData CurrentChapter { get; set; }

        // Danh sách toàn bộ chapter (để làm Dropdown và tính Next/Prev)
        public List<ChapterItem> ChapterList { get; set; }

        // Thông tin để điều hướng
        public string MangaSlug { get; set; }
        public string CurrentChapterId { get; set; }
        public string CurrentChapterName { get; set; }

        // ID (hoặc api_data) của chap trước và sau
        public string NextChapterId { get; set; }
        public string PrevChapterId { get; set; }
    }
}