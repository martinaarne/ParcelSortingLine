using System;
using System.Linq;
using SortingLine.BusinessRules;
using SortingLine.Interfaces;
using SortingLine.Models;

namespace SortingLine.Services
{
    public class PackageSortingLineService : IPackageSortingLineService
    {
        public PackageSortingLineResult CanPackagePassThrough(Package package, PackageSortingLine sortingLine)
        {
            var result = new PackageSortingLineResult();

            ValidatePackage(package, result);
            ValidateSortingLine(sortingLine, result);

            if (!result.Success)
            {
                return result;
            }

            PackageSortingLineSegment currentSegment = null;

            foreach (var segment in sortingLine.Segments)
            {
                bool canPackageFit = CanPackageFit(package, currentSegment, segment);
                if (!canPackageFit)
                {
                    var rotatedPackage = RotatePackage(package, currentSegment);
                    if (rotatedPackage != null)
                    {
                        package = rotatedPackage;
                        canPackageFit = CanPackageFit(package, currentSegment, segment);
                    }
                }

                if (!canPackageFit)
                {
                    var message = $"width {(currentSegment == null ? "" : $"{currentSegment.Width} and")} {segment.Width}";
                    result.AddBrokenRule(new PackageCannotFitThroughSortingLineBusinessRule(message));
                    break;
                }

                currentSegment = segment;
            }

            return result;
        }

        private Package RotatePackage(Package package, PackageSortingLineSegment currentSegment)
        {
            if (currentSegment != null && currentSegment.Width < Math.Sqrt(Math.Pow(package.Width, 2) + Math.Pow(package.Length, 2)))
            {
                return null;
            }
            return new Package{Width = package.Length, Length = package.Width};
        }

        private bool CanPackageFit(Package package, PackageSortingLineSegment currentSegment, PackageSortingLineSegment nextSegment)
        {
            if (currentSegment == null)
            {
                // edge case - should a package with equal width to pipe fit or not?
                return package.Width < nextSegment.Width;
            }

            var cornerDiagonal = Math.Sqrt(Math.Pow(currentSegment.Width, 2) + Math.Pow(nextSegment.Width, 2));
            
            // edge case - should a package with equal width to pipe fit or not?
            return (cornerDiagonal / 2) > package.Width && nextSegment.Width > package.Width;
        }

        private void ValidateSortingLine(PackageSortingLine sortingLine, PackageSortingLineResult result)
        {
            if (sortingLine == null)
            {
                result.AddBrokenRule(new SortingLineInvalidBusinessRule("No Sorting Line provided"));
                return;
            }

            if (sortingLine.Segments == null || !sortingLine.Segments.Any())
            {
                result.AddBrokenRule(new SortingLineInvalidBusinessRule("No Sorting Line Segments provided!"));
                return;
            }

            for (var i = 0; i < sortingLine.Segments.Count; i++)
            {
                var segment = sortingLine.Segments[i];
                if (segment.Width <= 0)
                {
                    result.AddBrokenRule(new SortingLineInvalidBusinessRule($"Sorting Line Segment {i + 1} Width is {segment.Width}. It must be greater than 0!"));
                }
            }
        }

        private void ValidatePackage(Package package, PackageSortingLineResult result)
        {
            if (package == null)
            {
                result.AddBrokenRule(new PackageInvalidBusinessRule("No Package provided"));
                return;
            }

            if (package.Width <= 0)
            {
                result.AddBrokenRule(new PackageInvalidBusinessRule($"Package Width is {package.Width}. It must be greater than 0!"));
            }

            if (package.Length <= 0)
            {
                result.AddBrokenRule(new PackageInvalidBusinessRule($"Package Length is {package.Length}. It must be greater than 0!"));
            }
        }
    }
}
