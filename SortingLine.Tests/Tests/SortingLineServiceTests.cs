using NUnit.Framework;
using SortingLine.BusinessRules;
using SortingLine.Models;
using SortingLine.Services;
using SortingLine.Tests.Utils;

namespace SortingLine.Tests.Tests
{
    [TestFixture]
    public class SortingLineServiceTests
    {

        PackageSortingLineService _service;
        private PackageSortingLine _sortingLine;


        [SetUp]
        public void Setup()
        {
            _service = new PackageSortingLineService();
            _sortingLine = GetSortingLine(10, 10);
        }

        [TestCase(0)]
        [TestCase(-5)]
        public void Package_should_not_fit_through_invalid_width_sorting_line(int sortingLineWidth)
        {
            var package = new Package { Length = 10, Width = 10 };
            _sortingLine = GetSortingLine(sortingLineWidth);

            var sortingLineResult = _service.CanPackagePassThrough(package, _sortingLine);

            sortingLineResult.ShouldNotBeValid();
            sortingLineResult.ShouldBreakRule<SortingLineInvalidBusinessRule>();
        }

        [TestCase(0, 5)]
        [TestCase(-5, 5)]
        [TestCase(5, 0)]
        [TestCase(5, -5)]
        public void Invalid_dimensions_Package_should_not_be_valid(int packageWidth, int packageLength)
        {
            var package = new Package { Length = packageLength, Width = packageWidth };

            var sortingLineResult = _service.CanPackagePassThrough(package, _sortingLine);

            sortingLineResult.ShouldNotBeValid();
            sortingLineResult.ShouldBreakRule<PackageInvalidBusinessRule>();
        }

        [Test]
        public void Missing_Package_should_not_be_valid()
        {
            var sortingLineResult = _service.CanPackagePassThrough(null, _sortingLine);

            sortingLineResult.ShouldNotBeValid();
            sortingLineResult.ShouldBreakRule<PackageInvalidBusinessRule>();
        }

        [Test]
        public void Missing_SortingLine_should_not_be_valid()
        {
            var package = new Package{Length = 5, Width = 5};
            var sortingLineResult = _service.CanPackagePassThrough(package, null);

            sortingLineResult.ShouldNotBeValid();
            sortingLineResult.ShouldBreakRule<SortingLineInvalidBusinessRule>();
        }

        [Test]
        public void SortingLine_without_parts_should_not_be_valid()
        {
            var package = new Package{Length = 5, Width = 5};
            _sortingLine = GetSortingLine();
            var sortingLineResult = _service.CanPackagePassThrough(package, _sortingLine);

            sortingLineResult.ShouldNotBeValid();
            sortingLineResult.ShouldBreakRule<SortingLineInvalidBusinessRule>();
        }

        [TestCase(60, 120, 100, 75)]
        [TestCase(100, 35, 75, 50, 80, 100, 37)]
        public void Valid_Package_should_fit_through_sorting_line(int packageWidth, int packageLength, params int[] sortingLineWidths)
        {
            var package = new Package{Length = packageLength, Width = packageWidth};
            var sortingLine = GetSortingLine(sortingLineWidths);

            var sortingLineResult = _service.CanPackagePassThrough(package, sortingLine);

            sortingLineResult.ShouldBeValid();
        }

        [TestCase(70, 50, 60, 60, 55, 90)]
        [TestCase(100, 100, 110, 200, 90)]
        public void Oversized_Package_should_not_fit_through_sorting_line(int packageWidth, int packageLength, params int[] sortingLineWidths)
        {
            var package = new Package { Length = packageLength, Width = packageWidth };
            var sortingLine = GetSortingLine(sortingLineWidths);

            var sortingLineResult = _service.CanPackagePassThrough(package, sortingLine);

            sortingLineResult.ShouldBreakRule<PackageCannotFitThroughSortingLineBusinessRule>();
            sortingLineResult.ShouldNotBeValid();
        }

        private static PackageSortingLine GetSortingLine(params int[] sortingLineWidths)
        {
            var sortingLine = new PackageSortingLine();

            foreach (var sortingLineWidth in sortingLineWidths)
            {
                sortingLine.Segments.Add(new PackageSortingLineSegment {Width = sortingLineWidth});
            }

            return sortingLine;
        }
    }
}
