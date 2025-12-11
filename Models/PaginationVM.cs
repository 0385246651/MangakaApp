namespace MangakaApp.Models
{
    public class PaginationVM
    {
        public PaginationVMModel Pager { get; set; }
        public string ActionName { get; set; }
        public string CurrentSlug { get; set; }
        public string CurrentKeyword { get; set; }
    }
    public class PaginationVMModel
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public int StartPage { get; set; }
        public int EndPage { get; set; }
    }
}
