using System.Linq.Expressions;

namespace NinjaNye.SearchExtensions
{
    internal class SwapExpressionVisitor : ExpressionVisitor
    {
        private readonly Expression from, to;
        private SwapExpressionVisitor(Expression from, Expression to)
        {
            this.@from = @from;
            this.to = to;
        }

        /// <summary>
        /// Swaps <paramref name="from"/> expression with <paramref name="to"/> expression within a given lambda
        /// </summary>
        /// <param name="lambda">Lambda expression to perform swap on</param>
        /// <param name="from">Existing expression to locate</param>
        /// <param name="to">Expression to replace </param>
        /// <returns>Re-configured expression</returns>
        public static Expression<T> Swap<T>(Expression<T> lambda, Expression from, Expression to)
        {
            return Expression.Lambda<T>(Swap(lambda.Body, from, to), lambda.Parameters);
        }

        private static Expression Swap(Expression body, Expression from, Expression to)
        {
            return new SwapExpressionVisitor(from, to).Visit(body);
        }

        public override Expression Visit(Expression node)
        {
            return node == this.@from ? this.to : base.Visit(node);
        }
    }
}