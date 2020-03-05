using System;
using System.Linq.Expressions;

namespace AlgorithmAnalysis.Common
{
    /// <summary>
    /// Class to cast to type <typeparamref name="TTarget"/>.
    /// </summary>
    /// <typeparam name="TTarget">Target type.</typeparam>
    public static class CastTo<TTarget>
    {
        private static class Cache<TSourceInternal>
        {
            public static readonly Func<TSourceInternal, TTarget> Caster = Get();

            private static Func<TSourceInternal, TTarget> Get()
            {
                var parameter = Expression.Parameter(typeof(TSourceInternal));
                var conversionBody = Expression.ConvertChecked(parameter, typeof(TTarget));

                return Expression
                    .Lambda<Func<TSourceInternal, TTarget>>(conversionBody, parameter)
                    .Compile();
            }
        }

        /// <summary>
        /// Casts <typeparamref name="TSource"/> to <typeparamref name="TTarget"/>.
        /// This does not cause boxing for value types.
        /// Useful in generic methods.
        /// </summary>
        /// <typeparam name="TSource">Source type to cast from. Usually a generic type.</typeparam>
        /// <returns>Casted value.</returns>
        public static TTarget From<TSource>(TSource source)
        {
            return Cache<TSource>.Caster(source);
        }
    }
}
