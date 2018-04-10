using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace InsuranceSocialNetworkDAL.DAL.Core
{
    public abstract class UnitOfWork<TContext> : IDisposable
        where TContext : DbContext, new()
    {        
        public TContext DataContext;

        public UnitOfWork()
        {
            DataContext = new TContext();
        }

        public UnitOfWork( TContext context )
        {
            DataContext = context;
        }

        public void Save()
        {
            DataContext.SaveChanges();
        }

        #region IDisposable

        private bool disposed = false;

        protected virtual void Dispose( bool disposing )
        {
            if ( !this.disposed )
            {
                if ( disposing )
                {
                    DataContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose( true );
            GC.SuppressFinalize( this );
        }

        #endregion
    }
}
