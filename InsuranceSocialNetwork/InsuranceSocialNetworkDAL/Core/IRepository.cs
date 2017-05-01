using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using CommonTypes.Types;

namespace InsuranceSocialNetworkDAL.DAL.Core
{
    public interface IRepository<T>
        where T : class
    {
        IQueryable<T> Fetch();

        IQueryable<T> FetchReadOnly();

        IEnumerable<T> GetAll();

        IEnumerable<T> Find( Expression<Func<T, bool>> predicate );

        T Get( params object[] keys );

        T Get( object key, params string[] propertiesToLoad );

        T Single( Expression<Func<T, bool>> predicate );

        T First( Expression<Func<T, bool>> predicate );

        void Create( T entity );

        void Update( T entity );

        void Delete( params object[] keys );

        int Delete( Expression<Func<T, bool>> predicate );

        IEnumerable<T> Search(
            Expression<Func<T, bool>> filter,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
            Pagination pagination
        );
    }
}
