using System.Linq;
using Shouldly;
using SortingLine.BusinessRules;
using SortingLine.Models.Base;

namespace SortingLine.Tests.Utils
{
    public static class BusinessRuleUtils
    {
        public static void ShouldBreakRule<T>(this BaseResult result) where T : BusinessRule
        {
            if (result.BrokenRules.All(r => r.GetType() != typeof(T)))
            {
                throw new ShouldAssertException($"Result should break rule {typeof(T)}, but it is not breaking it!");
            }
        }

        public static void ShouldBeValid(this BaseResult result)
        {
            result.Success.ShouldBeTrue();
            result.BrokenRules.ShouldBeEmpty();
        }

        public static void ShouldNotBeValid(this BaseResult result)
        {
            result.Success.ShouldBeFalse();
            result.BrokenRules.Count.ShouldBeGreaterThan(0);
        }
    }
}
