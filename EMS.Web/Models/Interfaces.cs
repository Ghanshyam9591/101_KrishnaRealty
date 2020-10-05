using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace EMS.Web.Models
{
  
        public interface IIncludable
        {
            Dictionary<string, Include> Includes { get; }
        }

        public interface ISelectable
        {
            string SelectSql();
            object ApplySelect(IDataReader reader);
        }

        public interface IInsertable
        {
            string InsertSql();
            void ApplyInsert(object obj, DataAccess dal);
            void SetIdentity(object obj, dynamic id);
        }

        public interface IUpdateable
        {
            string UpdateSql();
            void ApplyUpdate(object obj, DataAccess dal);
        }

        public interface IDeleteable
        {
            string DeleteSql();
            void ApplyDelete(object obj, DataAccess dal);
        }
    
}