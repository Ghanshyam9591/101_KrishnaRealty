using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MicroORM;
using System.Reflection;

namespace MicroORM
{
    public abstract class MappingBase<TModel> : ISelectable, IIncludable, IInsertable, IUpdateable, IDeleteable where TModel : ModelBase
    {
        protected string[] columns = null;

        Dictionary<string, Include> _Includes;
        public Dictionary<string, Include> Includes
        {
            get { return _Includes; }
        }

        public MappingBase()
        {
            _Includes = new Dictionary<string, Include>();
        }

        protected T GetValue<T>(IDataReader reader, string name)
        {
            try
            {
                return GetValue<T>(reader[name]);
            }
            catch (System.IndexOutOfRangeException)
            {
                return GetValue<T>(DBNull.Value);
            }
        }
        protected T GetValue<T>(object val)               
        {
            if (val == DBNull.Value)
                return default(T);
            else
                return (T)val;
        }

        protected TModel Cast(object model)
        {
            return (TModel)model;
        }

        public abstract string SelectSql();

        object ISelectable.ApplySelect(IDataReader reader)
        {
            return ApplySelect(reader);
        }

        protected void ProcessExcepts(IDataReader reader, TModel model)
        {
            List<string> ex = ReaderFields(reader).Except(columns).ToList();
            if (ex.Count > 0)
            {
                //model.Ex = new Dictionary<string, object>();
                ex.ForEach(o => model.Ex[o] = reader[o]);
            }
        }

        List<string> GetProperties<T>()
        {
            return typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList().Select(o => o.Name).ToList();
        }

        List<string> ReaderFields(IDataReader reader)
        {
            List<string> result = new List<string>();
            for (int i = 0; i < reader.FieldCount; i++)
                result.Add(reader.GetName(i));
            return result;
        }

        public abstract TModel ApplySelect(IDataReader reader);

        public abstract string InsertSql();

        void IInsertable.ApplyInsert(object obj, DataAccess dal)
        {
            ApplyInsert((TModel)obj, dal);
        }

        public abstract void ApplyInsert(TModel model, DataAccess dal);

        void IInsertable.SetIdentity(object obj, dynamic id)
        {
            SetIdentity((TModel)obj, id);
        }

        public virtual void SetIdentity(TModel model, dynamic id)
        {
        }

        public abstract string UpdateSql();

        void IUpdateable.ApplyUpdate(object obj, DataAccess dal)
        {
            ApplyUpdate((TModel)obj, dal);
        }

        public abstract void ApplyUpdate(TModel obj, DataAccess dal);

        public abstract string DeleteSql();

        void IDeleteable.ApplyDelete(object obj, DataAccess dal)
        {
            ApplyDelete((TModel)obj, dal);
        }

        public abstract void ApplyDelete(TModel model, DataAccess dal);

    }

}
