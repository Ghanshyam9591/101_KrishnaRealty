using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace EMS.Web.Models
{
    public class DbContext : DataAccess
    {
        #region Constructors

        public DbContext(string connectionKey)
            : base(connectionKey)
        {
        }

        public DbContext(DbConnection connection)
            : base(connection)
        {
        }

        public DbContext(DataAccess dal)
            : base(dal)
        {
        }


        #endregion

        public IEnumerable<T> ExecuteReader<T>()
        {
            Func<IDataReader, T> func = o => (T)ORMManager.GetSelectable<T>().ApplySelect(o);
            return ExecuteReader<T>(func);
        }

    }
}