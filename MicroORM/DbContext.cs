using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.ComponentModel;
using System.Collections;
using System.Data.Common;
using System.Configuration;
using System.Dynamic;

namespace MicroORM
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

        #region Select Methods

        public IEnumerable<T> Where<T>(string where)
        {
            return Where<T>(where, null);
        }

        public IEnumerable<T> Where<T>(string where, string paraName, DbType paraType, object paraValue)
        {
            DbCommand command = GetCommand(ORMManager.GetSelectable<T>().SelectSql() + " WHERE " + where);
            AddParameter(command, paraName, paraType, paraValue);
            return Select<T>(command);
        }

        public IEnumerable<T> Where<T>(string where, params DbParameter[] paras)
        {
            if (string.IsNullOrWhiteSpace(where))
                return Select<T>();
            else
            {
                return Select<T>(ORMManager.GetSelectable<T>().SelectSql() + " WHERE " + where, CommandType.Text, paras);
            }
        }

        public IEnumerable<T> Select<T>()
        {
            return Select<T>(ORMManager.GetSelectable<T>().SelectSql());
        }

        public IEnumerable<T> Select<T>(string sql)
        {
            return Select<T>(sql, CommandType.Text, null);
        }

        public IEnumerable<T> Select<T>(string sql, CommandType commandType, params DbParameter[] paras)
        {
            SetCommand(sql, commandType, paras);
            return Select<T>(GetCommand(sql, paras));
        }

        public IEnumerable<T> Select<T>(DbCommand command)
        {
            return Select<T>(command, ORMManager.GetSelectable<T>());
        }

        public IEnumerable<T> Select<T>(DbCommand command, ISelectable selectable)
        {
            return Select<T>(command, o => (T)selectable.ApplySelect(o));
        }

        public IEnumerable<T> Select<T>(DbCommand command, Func<IDataReader, T> selectable)
        {
            SetCommand(command);
            return ExecuteReader<T>(selectable);
        }

        #endregion

        #region Include Methods

        public Dictionary<string, object> CachedIncludes;

        public void Include<T>(T model, string includes)
        {
            Include<T>(model, includes.Split(';').ToArray());
        }

        public void Include<T>(IEnumerable<T> models, string includes)
        {
            Include<T>(models, includes.Split(';').ToArray());
        }
        DbContext internalctx = null;
        void Include<T>(T model, params string[] includes)
        {
            internalctx = new DbContext(_connection);
            try
            {
                internalctx.CachedIncludes = new Dictionary<string, object>();
                internalctx.OpenConnection();
                IncludePath includePath = ArrangeIncludes(includes);
                IncludeInternal(model, includePath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                internalctx.CloseConnection(ConnectionState.Closed);
            }
        }

        void Include<T>(IEnumerable<T> models, params string[] includes)
        {
            internalctx = new DbContext(_connection);
            try
            {
                internalctx.CachedIncludes = new Dictionary<string, object>();
                internalctx.OpenConnection();
                IncludePath includePath = ArrangeIncludes(includes);
                foreach (T model in models)
                    IncludeInternal(model, includePath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                internalctx.CloseConnection(ConnectionState.Closed);
            }
        }

        void IncludeInternal(object model, IncludePath includePath)
        {
            if (model == null)
                return;
            IIncludable includable = ORMManager.GetIncludable(model.GetType());
            if (includable == null)
                return;
            foreach (IncludePath ip in includePath.Includes)
            {
                Include action = null;
                if (includable.Includes.TryGetValue(ip.PropertyName, out action))
                    action.ApplyInclude(internalctx, model);
                if (ip.Includes.Count > 0)
                {
                    PropertyDescriptor pd = TypeDescriptor.GetProperties(model)[ip.PropertyName];
                    object pdval = pd.GetValue(model);
                    ExecuteInclude(pdval, ip);
                }
            }
        }

        void ExecuteInclude(object obj, IncludePath includePath)
        {
            if (obj is IEnumerable)
            {
                Type objType = obj.GetType();
                if (objType.IsGenericType)
                    foreach (object item in (obj as IEnumerable))
                        IncludeInternal(item, includePath);
            }
            else
                IncludeInternal(obj, includePath);
        }

        IncludePath ArrangeIncludes(params string[] includes)
        {
            if (includes == null)
                return null;
            IncludePath result = new IncludePath();
            foreach (string include in includes)
                ArrangeIncludes(result, include);
            return result;
        }

        void ArrangeIncludes(IncludePath include, string props)
        {
            int index = props.IndexOf('.');
            string p;
            if (index == -1)
                p = props;
            else
                p = props.Substring(0, index);

            IncludePath i = include.Includes.FirstOrDefault(o => o.PropertyName == p);
            if (i == null)
            {
                i = new IncludePath() { PropertyName = p };
                include.Includes.Add(i);
            }
            if (index != -1)
            {
                p = props.Substring(index + 1);
                ArrangeIncludes(i, p);
            }
        }

        private class IncludePath
        {
            public string PropertyName { get; set; }
            public List<IncludePath> Includes { get; set; }
            public IncludePath()
            {
                Includes = new List<IncludePath>();
            }
        }

        #endregion

        #region Insert Methods

        public int Insert(object model)
        {
            return Insert(model, ORMManager.GetInsertable(model.GetType()));
        }

        public int Insert(object model, IInsertable insertable)
        {
            SetCommand(insertable.InsertSql());
            _command.CommandText += @"
SELECT @_identity=SCOPE_IDENTITY()
";
            insertable.ApplyInsert(model, this);
            DbParameter para = _command.CreateParameter();
            para.ParameterName = "_identity";
            para.DbType = DbType.Int64;
            para.Direction = ParameterDirection.Output;
            _command.Parameters.Add(para);
            int result = ExecuteNonQuery();
            if (result > 0)
            {
                insertable.SetIdentity(model, _command.Parameters["_identity"].Value);
            }
            return result;
        }

        #endregion

        #region Update Methods

        public int Update(object model)
        {
            return Update(model, ORMManager.GetUpdateable(model.GetType()));
        }

        public int Update(object model, IUpdateable updateable)
        {
            SetCommand(updateable.UpdateSql());
            updateable.ApplyUpdate(model, this);
            return ExecuteNonQuery();
        }

        #endregion

        #region Delete Methods

        public int Delete(object model)
        {
            return Delete(model, ORMManager.GetDeleteable(model.GetType()));
        }

        public int Delete(object model, IDeleteable deleteable)
        {
            SetCommand(deleteable.DeleteSql());
            deleteable.ApplyDelete(model, this);
            return ExecuteNonQuery();
        }

        #endregion

    }

}
