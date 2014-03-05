using System;

namespace NinjaNye.SearchExtensions.Validation
{
    internal static class Ensure
    {
        /// <summary>
        /// Ensures an argument is not null
        /// </summary>
        /// <param name="value">Argument value to check</param>
        /// <param name="paramName">Argument name</param>
        public static void ArgumentNotNull(object value, string paramName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }
    }
}