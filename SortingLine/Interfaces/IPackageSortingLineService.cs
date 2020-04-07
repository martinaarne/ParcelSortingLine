using SortingLine.Models;

namespace SortingLine.Interfaces
{
    public interface IPackageSortingLineService
    {
        PackageSortingLineResult CanPackagePassThrough(Package package, PackageSortingLine sortingLine);
    }
}
