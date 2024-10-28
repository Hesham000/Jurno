using System.Collections.Generic;
using Travel.Models;

namespace Travel.ViewModel
{
    public class TourPaginationViewModel
    {
        public List<Tour> Tours { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
    }
}
