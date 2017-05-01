using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Data;
using System.Data.Entity;
using CommonTypes.Types;

namespace InsuranceSocialNetworkDAL.DAL.Core
{
    // TODO: Teste all this class, operation over operation.
    public class Repository<TContext, T> : IRepository<T>
        where TContext : DbContext, new()
        where T : class
    {
        private TContext _DataContext;
        protected virtual TContext DataContext
        {
            get
            {
                return _DataContext;
            }
        }
        protected virtual DbSet<T> dbSet
        {
            get
            {
                return DataContext.Set<T>();
            }
        }

        public Repository( TContext context )
        {
            this._DataContext = context;
        }

        public Repository( TContext context, bool autoDetectChangesEnabled )
        {
            this._DataContext = context;
            this._DataContext.Configuration.AutoDetectChangesEnabled = autoDetectChangesEnabled;
        }
        
        public Repository( TContext context, bool autoDetectChangesEnabled, int databaseTimeout )
        {
            this._DataContext = context;
            this._DataContext.Configuration.AutoDetectChangesEnabled = autoDetectChangesEnabled;
            if( databaseTimeout > 0 )
                this._DataContext.Database.CommandTimeout = databaseTimeout;
        }
        
        public virtual IQueryable<T> Fetch()
        {
            return dbSet;
        }

        public virtual IQueryable<T> FetchReadOnly()
        {
            return dbSet.AsNoTracking();
        }

        public virtual IEnumerable<T> GetAll()
        {
            return dbSet;
        }

        public virtual IEnumerable<T> Find( Expression<Func<T, bool>> predicate )
        {
            if ( predicate != null )
            {
                return dbSet.Where( predicate );
            }
            throw new ArgumentNullException( "Predicate value must be passed to FindSingleBy<T>." );
        }

        public virtual T Get( params object[] keys )
        {
            return dbSet.Find( keys );
        }

        public virtual T Get( object key, params string[] propertiesToLoad )
        {
            foreach ( var propertyToLoad in propertiesToLoad )
            {
                dbSet.Include<T>( propertyToLoad );
            }

            return dbSet.Find( key );
        }

        public virtual T Single( Expression<Func<T, bool>> predicate )
        {
            if ( predicate != null )
            {
                return dbSet.Where( predicate ).SingleOrDefault();
            }
            throw new ArgumentNullException( "Predicate value must be passed to Single<T>." );
        }

        public virtual T First( Expression<Func<T, bool>> predicate )
        {
            if ( predicate != null )
            {
                return dbSet.Where( predicate ).FirstOrDefault();
            }
            throw new ArgumentNullException( "Predicate value must be passed to Single<T>." );
        }

        public virtual void Create( T entity )
        {
            entity = dbSet.Add( entity );
        }

        public virtual void Update( T entity )
        {
            dbSet.Attach( entity );

            DataContext
                .Entry<T>( entity )
                .State = System.Data.Entity.EntityState.Modified;
        }

        public virtual void Delete( params object[] keys )
        {
            T entityToDelete = dbSet.Find( keys );
            dbSet.Remove( entityToDelete );
        }

        public virtual int Delete( Expression<Func<T, bool>> predicate )
        {
            IQueryable<T> entitiesToDelete = dbSet.Where( predicate );
            foreach( var entity in entitiesToDelete ){
                dbSet.Remove( entity );
            }

            return entitiesToDelete.Count();
        }

        public virtual IEnumerable<T> Search(
            Expression<Func<T, bool>> filter,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
            Pagination pagination
        )
        {
            IQueryable<T> query = dbSet;

            if ( filter != null )
            {
                query = query.Where( filter );
            }

            if ( orderBy != null )
            {
                query = orderBy( query );
            }

            return
                Page
                (
                    query,
                    pagination
                );
        }

        /// <summary>
        /// Returns a page from a collection of ordered items.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="pagination"> The pagination type </param>
        /// <returns> </returns>
        private IQueryable<T> Page( IQueryable<T> query, Pagination pagination )
        {
            if( pagination.CurrentPage <= 0 )
            {
                //if page is not valid we return the input collection
                pagination.NumberOfResults = query.Count();
                pagination.NumberOfPages = pagination.NumberOfResults > 0 ? 1 : 0;
                return query;
            }

            // how many items we want to skip
            int itemsToSkip = ( pagination.CurrentPage - 1 ) * pagination.ResultsPerPage;
            IQueryable<T> itemsAfterSkiped =
              query.Skip( itemsToSkip );

            pagination.NumberOfResults = query.Count();
            pagination.NumberOfPages = pagination.NumberOfResults == 0 ? 0 : ( ( pagination.NumberOfResults - 1 ) / pagination.ResultsPerPage ) + 1;
            return itemsAfterSkiped.Take( pagination.ResultsPerPage );
        }
    }
}
