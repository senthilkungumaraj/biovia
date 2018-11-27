using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Reflection;
namespace biovia.api.Services
{
    public interface ISortByColumnStrategy<T> {
        List<T> Sort();
    }

    public abstract class SortByColumnStrategy<T> : ISortByColumnStrategy<T>
    {
        public virtual List<T> Sort()
        {
            throw new NotImplementedException();
        }
    }

    public class DynamicSortStrategy<T>: SortByColumnStrategy<T> {
        private List<T> sortedEntities;
        private string sortOrder;
        private string sortColumn;

        public DynamicSortStrategy(List<T> entities, string column, string order)
        {
            sortedEntities = entities;
            sortOrder = order;
            sortColumn = column;
        }

        public override List<T> Sort()
        {
            if (sortedEntities.Count != 0) // && sortedEntities[0].GetType().GetProperty(sortColumn) != null)
            {
                sortedEntities = sortedEntities.AsQueryable().OrderBy<T>(sortColumn, sortOrder).ToList<T>();
            }
            System.Reflection.PropertyInfo prop = sortedEntities[0].GetType().GetProperty("SortIndex");
            int index = 0;
            foreach (T entity in sortedEntities)
            {
                prop.SetValue(entity, index++, null);
            }
            return sortedEntities;
        }
    }
}
