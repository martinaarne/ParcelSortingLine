using System.Collections.Generic;
using System.Linq;
using SortingLine.BusinessRules;

namespace SortingLine.Models.Base
{
    public class BaseResult
    {
        public BaseResult()
        {
            BrokenRules = new List<BusinessRule>();
        }

        public void AddBrokenRule(BusinessRule error)
        {
            BrokenRules.Add(error);
        }

        public bool Success => !BrokenRules.Any();
        public List<BusinessRule> BrokenRules { get; set; }
    }
}
