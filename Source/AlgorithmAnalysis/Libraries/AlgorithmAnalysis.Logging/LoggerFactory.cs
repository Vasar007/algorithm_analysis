using System;
using Acolyte.Assertions;
using AlgorithmAnalysis.Configuration;

namespace AlgorithmAnalysis.Logging
{
    public static class LoggerFactory
    {
        /// <summary>
        /// Creates logger instance for passed type with specified options.
        /// </summary>
        /// <typeparam name="T">Type for which instance is created.</typeparam>
        /// <param name="loggerOptions">Logger options to configure.</param>
        /// <returns>Created logger instance.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="loggerOptions" /> is <c>null</c>.
        /// </exception>
        public static ILogger CreateLoggerFor<T>(LoggerOptions loggerOptions)
        {
            return CreateLoggerFor(typeof(T), loggerOptions);
        }

        /// <summary>
        /// Creates logger instance for passed class type with specified options.
        /// </summary>
        /// <param name="type">Class name. Try to pass it with <c>typeof</c> operator.</param>
        /// <param name="loggerOptions">Logger options to configure.</param>
        /// <returns>Created logger instance.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="type" /> is <c>null</c>. -or-
        /// <paramref name="loggerOptions" /> is <c>null</c>.
        /// </exception>
        public static ILogger CreateLoggerFor(Type type, LoggerOptions loggerOptions)
        {
            type.ThrowIfNull(nameof(type));

            string loggerName = loggerOptions.UseFullyQualifiedEntityNames
                ? type.FullName
                : type.Name;

            return new NLogLoggerAdapter(loggerName, loggerOptions);
        }

        /// <summary>
        /// Creates logger instance for passed type with default options.
        /// </summary>
        /// <typeparam name="T">Type for which instance is created.</typeparam>
        /// <returns>Created logger instance.</returns>
        /// <exception cref="ArgumentException">
        /// Cannot get full name of type <typeparamref name="T" />.
        /// </exception>
        public static ILogger CreateLoggerFor<T>()
        {
            return CreateLoggerFor<T>(ConfigOptions.Logger);
        }

        /// <summary>
        /// Creates logger instance for passed class type with default options.
        /// </summary>
        /// <param name="type">Class name. Try to pass it with <c>typeof</c> operator.</param>
        /// <returns>Created logger instance.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="type" /> is <c>null</c>.
        /// </exception>
        public static ILogger CreateLoggerFor(Type type)
        {
            return CreateLoggerFor(type, ConfigOptions.Logger);
        }
    }
}
