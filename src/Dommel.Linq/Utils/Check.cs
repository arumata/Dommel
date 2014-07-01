using System;

namespace Dommel.Linq.Utils
{
    internal class Check
    {
        public static T NotNull<T>(T value, string parameterName)
        {
            NotEmpty(parameterName, "parameterName");

            if (ReferenceEquals(value, null))
            {
                throw new ArgumentNullException(parameterName);
            }

            return value;
        }

        public static string NotEmpty(string value, string parameterName)
        {
            if (ReferenceEquals(parameterName, null))
            {
                throw new ArgumentNullException("parameterName");
            }

            if (parameterName.Length == 0)
            {
                throw new ArgumentException("Parameter cannot be null or empty.", parameterName);
            }

            if (ReferenceEquals(value, null))
            {
                throw new ArgumentNullException(parameterName);
            }

            if (value.Length == 0)
            {
                throw new ArgumentException("Parameter cannot be null or empty.", parameterName);
            }

            return value;
        }
    }
}
