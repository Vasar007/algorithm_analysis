using System;
using Acolyte.Assertions;
using AlgorithmAnalysis.Common;
using Microsoft.Extensions.Configuration.Json.Writable;
using Microsoft.Extensions.FileProviders;

namespace Microsoft.Extensions.Configuration
{
    /// <summary>
    /// Extension methods for adding <see cref="WritableJsonConfigurationProvider" />.
    /// </summary>
    public static class WritableJsonConfigurationExtensions
    {
        /// <summary>
        /// Adds writable JSON configuration provider at <paramref name="path" /> to
        /// <paramref name="builder" />.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder" /> to add to.</param>
        /// <param name="path">Path relative to the base path stored in 
        /// <see cref="IConfigurationBuilder.Properties" /> of <paramref name="builder" />.</param>
        /// <returns>The <see cref="IConfigurationBuilder" />.</returns>
        public static IConfigurationBuilder AddWritableJsonFile(
            this IConfigurationBuilder builder, string path)
        {
            return builder.AddWritableJsonFile(
                provider: null,
                path: path,
                optional: false,
                reloadOnChange: false
            );
        }

        /// <summary>
        /// Adds writable JSON configuration provider at <paramref name="path" /> to
        /// <paramref name="builder" />.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="path">
        /// Path relative to the base path stored in 
        /// <see cref="IConfigurationBuilder.Properties" /> of <paramref name="builder" />.
        /// </param>
        /// <param name="optional">Whether the file is optional.</param>
        /// <returns>The <see cref="IConfigurationBuilder" />.</returns>
        public static IConfigurationBuilder AddWritableJsonFile(
            this IConfigurationBuilder builder, string path, bool optional)
        {
            return builder.AddWritableJsonFile(
                provider: null,
                path: path,
                optional: optional,
                reloadOnChange: false
            );
        }

        /// <summary>
        /// Adds writable JSON configuration provider at <paramref name="path" /> to
        /// <paramref name="builder" />.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder" /> to add to.</param>
        /// <param name="path">Path relative to the base path stored in 
        /// <see cref="IConfigurationBuilder.Properties"/> of <paramref name="builder" />.</param>
        /// <param name="optional">Whether the file is optional.</param>
        /// <param name="reloadOnChange">
        /// Whether the configuration should be reloaded if the file changes.
        /// </param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddWritableJsonFile(
            this IConfigurationBuilder builder, string path, bool optional, bool reloadOnChange)
        {
            return builder.AddWritableJsonFile(
                provider: null,
                path: path,
                optional: optional,
                reloadOnChange: reloadOnChange
            );
        }

        /// <summary>
        /// Adds writable JSON configuration source to <paramref name="builder" />.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder" /> to add to.</param>
        /// <param name="provider">
        /// The <see cref="IFileProvider" /> to use to access the file.
        /// </param>
        /// <param name="path">
        /// Path relative to the base path stored in 
        /// <see cref="IConfigurationBuilder.Properties" /> of <paramref name="builder" />.
        /// </param>
        /// <param name="optional">Whether the file is optional.</param>
        /// <param name="reloadOnChange">
        /// Whether the configuration should be reloaded if the file changes.
        /// </param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddWritableJsonFile(
            this IConfigurationBuilder builder, IFileProvider? provider, string path, bool optional,
            bool reloadOnChange)
        {
            builder.ThrowIfNull(nameof(builder));
            path.ThrowIfNullOrWhiteSpace(nameof(path));

            return builder.AddWritableJsonFile(source =>
            {
                source.FileProvider = provider;
                source.Path = path;
                source.Optional = optional;
                source.ReloadOnChange = reloadOnChange;
                source.ReloadDelay = CommonConstants.ConfigReloadDelay;
                source.ResolveFileProvider();
            });
        }

        /// <summary>
        /// Adds writable JSON configuration source to <paramref name="builder" />.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder" /> to add to.</param>
        /// <param name="configureSource">Configures the source.</param>
        /// <returns>The <see cref="IConfigurationBuilder" />.</returns>
        public static IConfigurationBuilder AddWritableJsonFile(
            this IConfigurationBuilder builder,
            Action<WritableJsonConfigurationSource> configureSource)
        {
            return builder.Add(configureSource);
        }
    }
}
