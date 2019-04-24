using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace t_rext
{
    public static class Ensure
    {
        public static void ArgumentNotNull(object value, string name)
        {
            if (value != null)
            {
                return;
            }

            throw new ArgumentNullException(name);
        }

        public static void ArgumentNotNullOrEmptyString(string value, string name)
        {
            ArgumentNotNull(value, name);

            if (!string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            throw new ArgumentException("String cannot be empty", name);
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static void GreaterThanZero(TimeSpan value, string name)
        {
            ArgumentNotNull(value, name);

            if (value.TotalMilliseconds > 0)
            {
                return;
            }

            throw new ArgumentException("Timespan must be greater than zero", name);
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static void GreaterThanZero(int value, string name)
        {
            if (value > 0)
            {
                return;
            }

            throw new ArgumentException("int must be greater than zero", name);
        }

        public static void ArgumentNotNullOrEmptyEnumerable<T>(IEnumerable<T> value, string name)
        {
            ArgumentNotNull(value, name);

            if (value.Any())
            {
                return;
            }

            throw new ArgumentException("List cannot be empty", name);
        }
    }
}