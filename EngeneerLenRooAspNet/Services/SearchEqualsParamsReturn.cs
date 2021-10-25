using System;

namespace EngeneerLenRooAspNet.Services
{
    public static class SearchEqualsParamsReturn
    {
        public static TypeSearchEqualsParams IsContains(object firstParams, object secondParams)
        {
            if (secondParams is null || firstParams is null || String.IsNullOrWhiteSpace(secondParams.ToString() ?? string.Empty))
                return TypeSearchEqualsParams.IsNull;

            return firstParams.ToString().Contains(secondParams.ToString(), StringComparison.OrdinalIgnoreCase) 
                ? TypeSearchEqualsParams.Found : TypeSearchEqualsParams.NotFound;
        }

        public static TypeSearchEqualsParams IsEqualsNumbers(long? firstParams, long? secondParams)
        {
            if (firstParams is null || secondParams is null || secondParams == 0 || firstParams == 0)
                return TypeSearchEqualsParams.IsNull;
            
            return firstParams == secondParams ? TypeSearchEqualsParams.Found : TypeSearchEqualsParams.NotFound;
        }
    }
}