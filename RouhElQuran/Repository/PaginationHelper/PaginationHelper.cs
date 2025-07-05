using Core.HelperModel.PaginationModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.PaginationHelper
{
    public class PaginationHelper
    {
        // Get page sizes for dropdown
        public static List<int> GetPageSizes()
        {
            return new List<int> { 5, 10, 15, 25, 50, 100 };
        }

        // Calculate pagination info for display
        public static string GetPaginationInfo(int currentPage, int pageSize, int totalCount)
        {
            if (totalCount == 0)
                return "No entries found";
            var start = (currentPage - 1) * pageSize + 1;
            var end = Math.Min(currentPage * pageSize, totalCount);
            return $"Showing {start} to {end} of {totalCount} entries";
        }

        // Generate pagination numbers for display
        public static List<int> GetPaginationNumbers(int currentPage, int totalPages, int maxDisplayPages = 5)
        {
            var pages = new List<int>();
            if (totalPages <= maxDisplayPages)
            {
                for (int i = 1; i <= totalPages; i++)
                    pages.Add(i);
            }
            else
            {
                var half = maxDisplayPages / 2;
                var start = Math.Max(1, currentPage - half);
                var end = Math.Min(totalPages, start + maxDisplayPages - 1);

                // Adjust if we're near the end
                if (end - start + 1 < maxDisplayPages)
                    start = Math.Max(1, end - maxDisplayPages + 1);

                for (int i = start; i <= end; i++)
                    pages.Add(i);
            }
            return pages;
        }

        // Check if we should show ellipsis
        public static bool ShouldShowStartEllipsis(int currentPage, int totalPages)
        {
            return currentPage > 3 && totalPages > 5;
        }

        public static bool ShouldShowEndEllipsis(int currentPage, int totalPages)
        {
            return currentPage < totalPages - 2 && totalPages > 5;
        }

        // Generic method to create paginated result
        public static PaginationRequest<T> CreatePaginatedResult<T>(
            IEnumerable<T> items,
            int currentPage,
            int pageSize,
            int totalCount,
            string sortBy,
            bool IsDec)
        {
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            var paginatedItems = items.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            return new PaginationRequest<T>
            {
                Items = paginatedItems,
                TotalCount = totalCount,
                CurrentPage = currentPage,
                PageSize = pageSize,
                PageNumbers = GetPaginationNumbers(currentPage, totalPages),
                ShowStartEllipsis = ShouldShowStartEllipsis(currentPage, totalPages),
                ShowEndEllipsis = ShouldShowEndEllipsis(currentPage, totalPages),
                PaginationInfo = GetPaginationInfo(currentPage, pageSize, totalCount),
                SortBy = sortBy,
                IsDesc = IsDec
            };
        }
    }
}
