using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Dynamic;
using System.Linq;
using System.Web;

namespace EMS.Web.Models
{
    public class DataAccess
    {
        internal DbConnection _connection;
        internal DbTransaction _transaction;
        internal DbCommand _command;
        public const string CONNECTION_STRING_NAME = Helper.CONNECTION_STRING_NAME;
        public DataAccess(string connectionKey)
        {
            _connection = GetConnection(connectionKey);
        }
        public DataAccess(DbConnection connection)
        {
            _connection = connection;
        }
        public DataAccess(DataAccess dal)
        {
            _connection = GetConnection(dal._connection);

        }
        public DataAccess SetCommand(string commandText)
        {
            return SetCommand(commandText, CommandType.Text);
        }

        public DataAccess SetCommand(string commandText, CommandType commandType)
        {
            return SetCommand(commandText, commandType, null);
        }

        public DataAccess SetCommand(string commandText, CommandType commandType, params DbParameter[] parameters)
        {
            return SetCommand(GetCommand(commandText, commandType, parameters));
        }

        public DataAccess SetCommand(DbCommand command)
        {
            _command = command;
            return this;
        }

        public DbCommand GetCommand(string commandText, params DbParameter[] parameters)
        {
            return GetCommand(commandText, CommandType.Text, parameters);
        }

        public DbParameter GetParameter()
        {
            return GetParameter(_command);
        }

        public DbParameter GetParameter(DbCommand command)
        {
            return command.CreateParameter();
        }

        public DbCommand GetCommand(string commandText, CommandType commandType, params DbParameter[] parameters)
        {
            DbCommand result = _connection.CreateCommand();
            result.CommandType = commandType;
            result.CommandText = commandText;
            result.Connection = _connection;
            if (ConfigurationManager.AppSettings.AllKeys.Contains("CommandTimeout"))
                result.CommandTimeout = int.Parse(ConfigurationManager.AppSettings["CommandTimeout"]);
            if (parameters != null)
                foreach (DbParameter para in parameters)
                    AddParameter(para);
            return result;
        }
        public DataSet ExecuteQuery2(string cQuery)
        {
            DataSet ds = new DataSet();
            using (NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ToString()))
            {
                try
                {
                    using (NpgsqlCommand cmd = new NpgsqlCommand())
                    {
                        {
                            conn.Open();
                            using (NpgsqlDataAdapter SqDA = new NpgsqlDataAdapter(cmd))
                            {
                                SqDA.Fill(ds);
                            }
                            return ds;
                        }
                    }
                }
                catch (Exception ex)
                {

                    return null;
                }
            }
        }

        public static DataTable ExecuteQuery(string cQuery)
        {
            DataTable dt = new DataTable();
            using (NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ToString()))
            {
                try
                {
                    conn.Open();
                    using (NpgsqlDataAdapter SqDA = new NpgsqlDataAdapter(cQuery, conn))
                    {
                        SqDA.Fill(dt);
                    }
                    return dt;
                }
                catch (Exception ex)
                {

                    return null;
                }
            }
        }
        public DataTable ExecuteProcedure(string cProcedureName, NpgsqlParameter[] paramenters)
        {
            DataTable dt = new DataTable();

            using (NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ToString()))
            {
                try
                {
                    using (NpgsqlCommand cmd = new NpgsqlCommand())
                    {
                        conn.Open();
                        cmd.Connection = conn;
                        cmd.CommandTimeout = 60;
                        cmd.CommandType = CommandType.StoredProcedure; ;
                        cmd.CommandText = cProcedureName;
                        if (paramenters != null)
                            cmd.Parameters.AddRange(paramenters);
                        using (NpgsqlDataAdapter SqDA = new NpgsqlDataAdapter(cmd))
                        {
                            SqDA.Fill(dt);
                        }
                        return dt;
                    }
                }

                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public string ExecuteSclar(string cProcedureName, NpgsqlParameter[] paramenters)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ToString()))
            {
                try
                {
                    using (NpgsqlCommand cmd = new NpgsqlCommand())
                    {
                        conn.Open();
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure; ;
                        cmd.CommandText = cProcedureName;
                        if (paramenters != null)
                            cmd.Parameters.AddRange(paramenters);

                        object obj = cmd.ExecuteScalar();

                        if (obj != null)
                            return obj.ToString();
                        else
                            return "";
                    }
                }

                catch (Exception ex)
                {
                    return ex.ToString();
                }
            }
        }
        public DataAccess AddParameter(string name, object value)
        {
            return AddParameter(_command, name, value);
        }

        public DataAccess AddParameter(string name, DbType dbtype, object value)
        {
            return AddParameter(_command, name, dbtype, value);
        }

        public DataAccess AddParameter(DbCommand command, string name, object value)
        {
            DbParameter para = command.CreateParameter();
            para.ParameterName = name;
            para.Value = value ?? DBNull.Value;
            return AddParameter(command, para);
        }

        public DataAccess AddParameter(DbCommand command, string name, DbType dbtype, object value)
        {
            DbParameter para = command.CreateParameter();
            para.ParameterName = name;
            para.DbType = dbtype;
            para.Value = value ?? DBNull.Value;
            return AddParameter(command, para);
        }

        public DataAccess AddParameter(DbParameter para)
        {
            return AddParameter(_command, para);
        }

        public DataAccess AddParameter(DbCommand command, DbParameter para)
        {
            command.Parameters.Add(para);
            return this;
        }

        public DataSet GetDataSet()
        {
            DataSet dataSet = new DataSet();
            Fill(dataSet);
            return dataSet;
        }

        public DataTable GetDataTable()
        {
            DataTable dataTable = new DataTable();
            Fill(dataTable);
            return dataTable;
        }

        public void Fill(DataSet dataSet)
        {
            DbDataAdapter adpt = GetAdapter();
            adpt.SelectCommand = _command;
            adpt.Fill(dataSet);
        }

        public void Fill(DataTable dataTable)
        {
            DbDataAdapter adpt = GetAdapter();
            adpt.SelectCommand = _command;
            adpt.Fill(dataTable);
        }

        ConnectionState tranState;
        public void BeginTransaction()
        {
            tranState = OpenConnection();
            _transaction = _connection.BeginTransaction();
        }

        public void RollbackTransaction()
        {
            if (_transaction != null)
                _transaction.Rollback();
            CloseConnection(tranState);
        }

        public void CommitTransaction()
        {
            _transaction.Commit();
            CloseConnection(tranState);
        }
        DbConnection GetConnection(DbConnection connection)
        {
            if (connection is NpgsqlConnection)
                return new NpgsqlConnection(connection.ConnectionString);
            else
                throw new ApplicationException(string.Format("Connection key [{0}] not found in config !", connection.GetType().FullName));
        }
        public static DbConnection GetConnection(string connectionKey)
        {
            if (connectionKey.Contains(';'))
                return new NpgsqlConnection(connectionKey);
            ConnectionStringSettings setting = ConfigurationManager.ConnectionStrings[connectionKey];
            if (setting == null)
                throw new ApplicationException(string.Format("Connection key [{0}] not found in config !", connectionKey));

            if (setting.ProviderName == "Npgsql")
                return new NpgsqlConnection(setting.ConnectionString);
            else
                throw new ApplicationException(string.Format("Connection provider [{0}] is not valid !", connectionKey));
        }
        public ConnectionState OpenConnection()
        {
            ConnectionState state = _connection.State;
            if (state != ConnectionState.Open)
                _connection.Open();
            return state;
        }
        public void CloseConnection(ConnectionState state)
        {
            if (state != ConnectionState.Open && _connection.State != ConnectionState.Closed)
                _connection.Close();
        }
        DbDataAdapter GetAdapter()
        {
            if (_connection is NpgsqlConnection)
                return new NpgsqlDataAdapter();
            //else if (_connection is System.Data.SQLite.SQLiteConnection)
            //    return new System.Data.SQLite.SQLiteDataAdapter();
            else
                throw new ApplicationException("Connection type not supported!");
        }

        public int ExecuteNonQuery()
        {
            ConnectionState state = ConnectionState.Closed;
            try
            {
                state = OpenConnection();
                if (_transaction != null)
                    _command.Transaction = _transaction;
                return _command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseConnection(state);
            }
        }

        public IEnumerable<dynamic> ExecuteReader(IDataReader reader = null)
        {
            Func<IDataReader, dynamic> iselectable = o =>
            {
                var item = new ExpandoObject() as IDictionary<string, object>;
                for (int i = 0; i < o.FieldCount; i++)
                    item.Add(o.GetName(i), o[i] == DBNull.Value ? null : o[i]);
                return item;
            };
            if (reader == null)
            {
                return ExecuteReader<dynamic>(iselectable);
            }
            else
            {
                return ExecuteReader<dynamic>(reader, iselectable);
            }
        }


        public IEnumerable<T> ExecuteReader<T>(Func<IDataReader, T> readerfunc)
        {
            ConnectionState state = ConnectionState.Closed;
            DbDataReader reader = null;
            try
            {
                state = OpenConnection();

                if (_transaction != null)
                    _command.Transaction = _transaction;

                reader = _command.ExecuteReader();

                foreach (T item in ExecuteReader(reader, readerfunc))
                    yield return item;

            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                    reader.Close();
                CloseConnection(state);
            }
        }

        public IEnumerable<T> ExecuteReader<T>()
        {
            Func<IDataReader, T> func = o => (T)ORMManager.GetSelectable<T>().ApplySelect(o);
            return ExecuteReader<T>(func);
        }

        public IEnumerable<T> ExecuteReader<T>(IDataReader reader, Func<IDataReader, T> readerfunc)
        {
            while (reader.Read())
            {
                T result = (T)readerfunc(reader);
                if (result != null)
                    yield return result;
            }
        }
    }
}