using System;

// ReSharper disable InconsistentNaming

namespace App.Commons.Helpers
{
    public static class ExceptionsHelper
    {
        public static Exception ArgumentNotNull(string argName, string? customErrorText = null)
        {
            var newMessage = customErrorText ?? Requires.Strings.ArgumentMustNotBeNull;

            return new ArgumentNullException(argName, newMessage);
        }

        public static Exception ArgumentNotTypeDefault(string argName, string? customErrorText = null)
        {
            var newMessage = customErrorText ?? Requires.Strings.ArgumentMustNotBeDefault;

            return new ArgumentException(newMessage, argName);
        }

        public static Exception InvalidOperation(string message) => new InvalidOperationException(message);
    }
}