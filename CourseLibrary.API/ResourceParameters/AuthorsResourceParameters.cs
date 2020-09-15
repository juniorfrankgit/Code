using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseLibrary.API.ResourceParameters
{
    public class AuthorsResourceParameters
    {
        const int maxPageSize = 20; // The maximum page size
        public string MainCategory { get; set; }
        public string SearchQuery { get; set; }
        public int pageNumber { get; set; } = 1; // The default value of the page number we want to get

        private int pageSize = 10; // The default value of the  page size

        public int PageSize
        {
            get => pageSize;
            set => pageSize = (value > maxPageSize) ? maxPageSize : value;
        }

    }
}
