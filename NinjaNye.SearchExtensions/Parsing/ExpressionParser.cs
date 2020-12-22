using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace NinjaNye.SearchExtensions.Parsing
{
    public static class ExpressionParser
    {
        private static Regex ParserRegex = new Regex(
        @"^(?:
                    (
                        [_\p{Lu}\p{Lu}\p{Ll}\p{Lt}\p{Lm}\p{Lo}\p{Nl}]
                        [\p{Lu}\p{Lu}\p{Ll}\p{Lt}\p{Lm}\p{Lo}\p{Nl}\p{Nd}\p{Pc}\p{Cf}\p{Mn}\p{Mc}]*
                    )
                    (?:\.(?!$)|$)
                )+$", RegexOptions.IgnorePatternWhitespace);
        
        /// <summary>
        /// Parses <see cref="string"/> into <see cref="Expression{TDelegate}"/>.
        /// </summary>
        /// <param name="expression"><see cref="string"/> to be parsed.</param>
        /// <typeparam name="TSource">The type of source.</typeparam>
        /// <typeparam name="TResult">The type of result.</typeparam>
        /// <returns>Returns <see cref="Expression{TDelegate}"/> for <paramref name="expression"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="expression"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="expression"/> is empty.</exception>
        /// <exception cref="TypeMismatchException">Return type of <paramref name="expression"/> is not compatible with the <typeparamref name="TResult"/>.</exception>
        public static Expression<Func<TSource, TResult>> Parse<TSource, TResult>(string expression)
        {
            string[] properties = ParseProperties(expression);
            return CreateLambdaExpression<TSource, TResult>(properties);
        }

        internal static Expression<Func<TSource, TResult>> CreateLambdaExpression<TSource, TResult>(string[] properties)
        {
            if (properties == null) throw new ArgumentNullException(nameof(properties));
            if (properties.Length == 0)
                throw new ArgumentException("Value cannot be an empty collection.", nameof(properties));

            var parameterExpression = Expression.Parameter(typeof(TSource));
            Expression body = parameterExpression;
            for (int i = 0; i < properties.Length; i++)
            {
                string propertyName = properties[i];
                body = Expression.Property(body, propertyName);
            }
            
            if (!typeof(TResult).GetTypeInfo().IsAssignableFrom(body.Type.GetTypeInfo())) throw new TypeMismatchException($"{body.Type} is not assignable to {typeof(TResult)}");

            return Expression.Lambda<Func<TSource, TResult>>(body, parameterExpression);
        }

        internal static string[] ParseProperties(string expression)
        {
            if (string.IsNullOrEmpty(expression))
                throw new ArgumentException("Value cannot be null or empty.", nameof(expression));

            var match = ParserRegex.Match(expression);
            if (!match.Success) throw new ArgumentException($"{expression} is not a valid expression", nameof(expression));

            var captures = match.Groups[1].Captures;
            string[] properties = new string[captures.Count];
            for (int i = 0; i < captures.Count; i++)
            {
                string propertyName = captures[i].Value;
                properties[i] = propertyName;
            }

            return properties;
        }
    }
}