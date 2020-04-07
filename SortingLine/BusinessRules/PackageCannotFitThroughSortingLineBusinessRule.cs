namespace SortingLine.BusinessRules
{
    public class PackageCannotFitThroughSortingLineBusinessRule : BusinessRule
    {
        private readonly string _errorDescription;

        public PackageCannotFitThroughSortingLineBusinessRule(string errorDescription)
        {
            _errorDescription = errorDescription;
        }

        public override string Message => $"Package cannot fit through sorting line segment(s) {_errorDescription}";
    }
}
