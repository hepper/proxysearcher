using System;
using System.Linq.Expressions;

namespace ProxySearch.Console.Code.Utils
{
    public static class Property
    {
        public static string GetName<T, TResult>(Expression<Func<T, TResult>> expression)
        {
            var me = expression.Body as MemberExpression;
            if (me != null)
            {
                return me.Member.Name;
            }

            return null;
        }
    }
}
