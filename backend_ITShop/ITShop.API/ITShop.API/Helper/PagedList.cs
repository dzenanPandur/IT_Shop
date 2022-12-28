using Microsoft.EntityFrameworkCore;

namespace ITShop.API.Helper
{
    public class PagedList<T>
    {
        public List<T> DataItems { get; set; } = new List<T>();
        private PagedList(List<T> items, int totalCount, int pageNumber, int pageSize)
        {
            TotalCount = totalCount;
            CurrentPage = pageNumber;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            this.DataItems.AddRange(items);
        }

        public int CurrentPage { get; }

        public int TotalPages { get; }

        public int PageSize { get; }

        public int TotalCount { get; }

        public bool HasPrevios => CurrentPage > 1;

        public bool HasNext => CurrentPage < TotalPages;

        public static async Task<PagedList<T>> Create(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var totalCount = await source.CountAsync();
            var items = await source
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return new PagedList<T>(items, totalCount, pageNumber, pageSize);
        }
    }
}
