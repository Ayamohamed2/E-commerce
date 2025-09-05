using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Dataaccess.Reposatory.IReposatory
{
    public interface IReposatory<T> where T : class
    {
        //CRUD
        List<T> GetALL(Expression<Func<T, bool>> ?filter=null, string? Includes = null);
        T GetByFilter(Expression<Func<T,bool>> filter, string? Includes=null);
        T GetByFilterAsnoTraking(Expression<Func<T, bool>> filter, string? Includes = null);
        void Create(T model);
        void Update(T model);
        void Delete(T model);

		void RemoveRange(IEnumerable<T> entity);
	}
}
