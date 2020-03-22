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
        /// <exception cref="ArgumentException">
        /// Cannot get full name of type <typeparamref name="T" />. -or-
        /// <paramref name="loggerOptions" /> is <c>null</c>.
        /// </exception>
        public static ILogger CreateLoggerFor<T>(LoggerOptions loggerOptions)
        {
            Type type = typeof(T);
            string fullName = type.FullName ?? throw new ArgumentException(
                $"Could not get full name of class {type}."
            );
            return new NLogLoggerAdapter(fullName, loggerOptions);
        }

        /// <summary>
        /// Creates logger instance for passed class type with specified options.
        /// </summary>
        /// <param name="type">Class name. Try to pass it with <c>typeof</c> operator.</param>
        /// <param name="loggerOptions">Logger options to configure.</param>
        /// <returns>Created logger instance.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="type" /> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Cannot get full name of passed type <paramref name="type" />. -or-
        /// <paramref name="loggerOptions" /> is <c>null</c>.
        /// </exception>
        public static ILogger CreateLoggerFor(Type type, LoggerOptions loggerOptions)
        {
            type.ThrowIfNull(nameof(type));

            string fullName = type.FullName ?? throw new ArgumentException(
                $"Could not get full name of class {nameof(type)}"
            );
            return new NLogLoggerAdapter(fullName, loggerOptions);
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
        /// <exception cref="ArgumentException">
        /// Cannot get full name of passed type <paramref name="type" />.
        /// </exception>
        public static ILogger CreateLoggerFor(Type type)
        {
            return CreateLoggerFor(type, ConfigOptions.Logger);
        }
    }
}
