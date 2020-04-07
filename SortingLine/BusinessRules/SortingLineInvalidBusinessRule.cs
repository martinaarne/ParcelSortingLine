namespace SortingLine.BusinessRules
{
    public class SortingLineInvalidBusinessRule : BusinessRule
    {
        private readonly string _errorDescription;

        public SortingLineInvalidBusinessRule(string errorDescription)
        {
            _errorDescription = errorDescription;
        }
        public override string Message => $"Sorting Line is invalid! {_errorDescription}";
    }
}
