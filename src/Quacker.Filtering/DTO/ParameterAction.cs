using System;
using System.Collections.Generic;

namespace Quacker.Filtering.DTO
{
    public class ParameterAction<TEntity>
    {
        public Func<IEnumerable<TEntity>, object, IEnumerable<TEntity>> ExecuteParam { get; set; }
        public Func<IEnumerable<TEntity>, IEnumerable<TEntity>> ExecuteNoParam { get; set; }

        public ParameterAction(Func<IEnumerable<TEntity>, object, IEnumerable<TEntity>> executeParam,
                               Func<IEnumerable<TEntity>, IEnumerable<TEntity>> executeNoParam = null)
        {
            ExecuteParam = executeParam;
            ExecuteNoParam = executeNoParam;
        }

        public static implicit operator ParameterAction<TEntity>(Func<IEnumerable<TEntity>, object, IEnumerable<TEntity>> executeParam)
            => new ParameterAction<TEntity>(executeParam);
    }
}