using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.HelperModel.PaginationModel
{
    public class PaginationRequest<T>
    {
        public List<T> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;
        public int StartRecord => (CurrentPage - 1) * PageSize + 1;
        public int EndRecord => Math.Min(CurrentPage * PageSize, TotalCount);

        // Additional properties for UI
        public List<int> PageNumbers { get; set; } = new();
        public bool ShowStartEllipsis { get; set; }
        public bool ShowEndEllipsis { get; set; }
        public string PaginationInfo { get; set; } = "";

        public string SortBy { get; set; }
        public bool IsDesc { get; set; }


    }
}
