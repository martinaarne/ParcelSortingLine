namespace SortingLine.BusinessRules
{
    public class PackageInvalidBusinessRule : BusinessRule
    {
        private readonly string _errorDescription;

        public PackageInvalidBusinessRule(string errorDescription)
        {
            _errorDescription = errorDescription;
        }
        public override string Message => $"Package is invalid! {_errorDescription}";
    }
}
