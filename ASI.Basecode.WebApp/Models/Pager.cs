using System;
using System.Collections.Generic;
using System.Linq;

namespace ASI.Basecode.WebApp.Models
{
    public class Pager
    {
        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int StartPage { get; set; }
        public int EndPage { get; set; }

        public Pager() 
        {
        
        }
            

        public Pager(int totalItems, int page, int pageSize = 10)
        {
            // Calculate Total Pages using Ceiling for division
            TotalPages = (int)Math.Ceiling((decimal)totalItems / pageSize);

            // Calculate the start and end pages for pagination
            int currentPage = page;
            int startPage = currentPage - 5;
            int endPage = currentPage + 4;

            if (startPage <= 0)
            {
                endPage = endPage - (startPage - 1);
                startPage = 1;
            }

            if (endPage > TotalPages)
            {
                endPage = TotalPages;
                if (endPage > 10)
                {
                    startPage = endPage - 9;
                }
            }

            CurrentPage = currentPage;
            PageSize = pageSize;
            StartPage = startPage;
            EndPage = endPage;
        }
    }
}