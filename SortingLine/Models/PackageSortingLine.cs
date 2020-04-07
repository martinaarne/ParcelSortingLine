using System.Collections.Generic;

namespace SortingLine.Models
{
    public class PackageSortingLine
    {
        public PackageSortingLine()
        {
            Segments = new List<PackageSortingLineSegment>();
        }
        public List<PackageSortingLineSegment> Segments { get; set; }
    }
}
