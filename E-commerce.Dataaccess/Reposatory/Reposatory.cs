using E_commerce.Dataaccess.Reposatory.IReposatory;
using E_commerce.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Dataaccess.Reposatory
{
    public class Reposatory<T> : IReposatory<T> where T : class
    {
        private Context context;
        internal DbSet<T> Dbset;
        public Reposatory(Context context)
        {
            this.context = context;
            Dbset = context.Set<T>();
        }
        public void Create(T model)
        {
            Dbset.Add(model);
        }

        public void Delete(T model)
        {
            Dbset.Remove(model);
        }

        public List<T> GetALL(Expression<Func<T, bool>>? filter = null,string ? Includes = null)

        {
            IQueryable<T> query = Dbset;
            if (filter != null)
            {
                query = query.Where(filter);
            }
           
            if (!string.IsNullOrEmpty(Includes))
            {
                foreach(string item in Includes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }
            return query.ToList();
        }

        public T GetByFilter(Expression<Func<T, bool>> filter, string? Includes = null)
        {
            var query = Dbset.Where(filter);
            if (!string.IsNullOrEmpty(Includes))
            {
                foreach (string item in Includes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }
            return query.FirstOrDefault();
        }
        public T GetByFilterAsnoTraking(Expression<Func<T, bool>> filter, string? Includes=null)
        {
            var query = Dbset.AsNoTracking().Where(filter);
            if (!string.IsNullOrEmpty(Includes))
            {
                foreach (string item in Includes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }
            return query.FirstOrDefault();
        }
        public void Update(T model)
        {
            Dbset.Update(model);
        }

		public void RemoveRange(IEnumerable<T> entity)
		{
			Dbset.RemoveRange(entity);
		}
	}
}
