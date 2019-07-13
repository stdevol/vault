using System;
using JetBrains.Annotations;

namespace Vault.Helpers
{
    public static class Guard
    {
        [AssertionMethod]
        public static T NotNull<T>(
            [NotNull, NoEnumeration, AssertionCondition(AssertionConditionType.IS_NOT_NULL)] T obj,
            [InvokerParameterName] string argumentName = null)
            where T : class
        {

            if (obj == null)
                throw new ArgumentNullException(argumentName ?? string.Empty);
            return obj;
        }

        [AssertionMethod]
        public static T NotDefault<T>(T obj, [InvokerParameterName] string argumentName = null)
            where T : struct
        {
            if (Equals(obj, default(T)))
                throw new ArgumentOutOfRangeException(argumentName ?? string.Empty);
            return obj;
        }

        [AssertionMethod]
        public static string NotNullOrEmptyOrWhitespace([NotNull] string str, [InvokerParameterName] string argumentName = null)
        {
            if (string.IsNullOrWhiteSpace(str))
                throw new ArgumentOutOfRangeException(argumentName ?? string.Empty, "Cannot be empty string");
            return str;
        }

        [AssertionMethod]
        public static void Check(bool value, [InvokerParameterName] string argumentName = null, string message = null)
        {
            if (!value)
                throw new ArgumentException(message ?? "Bad argument value", argumentName ?? string.Empty);
        }

        [AssertionMethod]
        public static int Positive(int value, [InvokerParameterName] string argumentName = null)
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(argumentName ?? string.Empty, "Should be positive integer");

            return value;
        }
    }
}
