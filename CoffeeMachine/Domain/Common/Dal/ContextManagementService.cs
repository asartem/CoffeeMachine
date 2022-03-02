using System.Linq;
using Cm.Domain.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Cm.Domain.Common.Dal
{
    /// <summary>
    /// Operations with context
    /// </summary>
    public class ContextManagementService
    {
        /// <summary>
        /// Updates 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <param name="t"></param>
        /// <param name="entryId"></param>
        public static void DetachLocal<T>(ApplicationContext context, T t, int entryId) where T : class, IEntity
        {
            T local = context.Set<T>()
                .Local
                .FirstOrDefault(entry => entry.Id == entryId);

            if (local != null)
            {
                context.Entry(local).State = EntityState.Detached;
            }
            context.Entry(t).State = EntityState.Modified;
        }
    }
}