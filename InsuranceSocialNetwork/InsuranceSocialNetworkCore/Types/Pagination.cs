using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTypes.Types
{
    public class Pagination
    {
        public int NumberOfPages { get; set; }

        public int NumberOfResults { get; set; }

        public int CurrentPage { get; set; }

        public int ResultsPerPage { get; set; }

        public string FormId { get; set; }
    }
}
