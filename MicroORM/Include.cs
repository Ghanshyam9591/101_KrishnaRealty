using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
using System.Data.Common;

namespace MicroORM
{

    public abstract class Include
    {
        protected static Dictionary<Type, DbType> typeMap;

        static Include()
        {
            typeMap = new Dictionary<Type, DbType>();
            typeMap[typeof(byte)] = DbType.Byte;
            typeMap[typeof(sbyte)] = DbType.SByte;
            typeMap[typeof(short)] = DbType.Int16;
            typeMap[typeof(ushort)] = DbType.UInt16;
            typeMap[typeof(int)] = DbType.Int32;
            typeMap[typeof(uint)] = DbType.UInt32;
            typeMap[typeof(long)] = DbType.Int64;
            typeMap[typeof(ulong)] = DbType.UInt64;
            typeMap[typeof(float)] = DbType.Single;
            typeMap[typeof(double)] = DbType.Double;
            typeMap[typeof(decimal)] = DbType.Decimal;
            typeMap[typeof(bool)] = DbType.Boolean;
            typeMap[typeof(string)] = DbType.String;
            typeMap[typeof(char)] = DbType.StringFixedLength;
            typeMap[typeof(Guid)] = DbType.Guid;
            typeMap[typeof(DateTime)] = DbType.DateTime;
            typeMap[typeof(DateTimeOffset)] = DbType.DateTimeOffset;
            typeMap[typeof(byte[])] = DbType.Binary;
            typeMap[typeof(byte?)] = DbType.Byte;
            typeMap[typeof(sbyte?)] = DbType.SByte;
            typeMap[typeof(short?)] = DbType.Int16;
            typeMap[typeof(ushort?)] = DbType.UInt16;
            typeMap[typeof(int?)] = DbType.Int32;
            typeMap[typeof(uint?)] = DbType.UInt32;
            typeMap[typeof(long?)] = DbType.Int64;
            typeMap[typeof(ulong?)] = DbType.UInt64;
            typeMap[typeof(float?)] = DbType.Single;
            typeMap[typeof(double?)] = DbType.Double;
            typeMap[typeof(decimal?)] = DbType.Decimal;
            typeMap[typeof(bool?)] = DbType.Boolean;
            typeMap[typeof(char?)] = DbType.StringFixedLength;
            typeMap[typeof(Guid?)] = DbType.Guid;
            typeMap[typeof(DateTime?)] = DbType.DateTime;
            typeMap[typeof(DateTimeOffset?)] = DbType.DateTimeOffset;
        }

        public abstract void ApplyInclude(DbContext context, object model);
    }

    public class Include<TModel> : Include where TModel : class
    {
        public string Property { get; set; }
        public string NavigationProperty { get; set; }
        public string RefProperty { get; set; }

        bool init = false;
        PropertyInfo propertyInfo;
        PropertyInfo navigationPropertyInfo;
        PropertyInfo refPropertyInfo;
        bool isMany = false;
        //DbParameter parameter;
        object paraValue;
        DbType paraType;

        public Include(string propertyName, string navigationProperty, string refProperty)
        {
            Property = propertyName;
            NavigationProperty = navigationProperty;
            RefProperty = refProperty;
        }

        public override void ApplyInclude(DbContext context, object model)
        {
            if (!init)
            {
                propertyInfo = model.GetType().GetProperty(Property);
                navigationPropertyInfo = model.GetType().GetProperty(NavigationProperty);
                refPropertyInfo = typeof(TModel).GetProperty(RefProperty);
                isMany = typeof(System.Collections.IEnumerable).IsAssignableFrom(navigationPropertyInfo.PropertyType);
                paraType = typeMap[refPropertyInfo.PropertyType];
                init = true;
            }
            paraValue = propertyInfo.GetValue(model, null);
            if (paraValue == null)
                return;
            object result;
            string key = typeof(TModel).Name + "." + RefProperty + "." + paraValue.ToString();
            if (context.CachedIncludes.ContainsKey(key))
                result = context.CachedIncludes[key];
            else
            {
                //DbContext internalctx = new DbContext(context._connection);
                if (isMany)
                    result = context.Where<TModel>(RefProperty + "=@para", "@para", paraType, paraValue).ToList();
                else
                    result = context.Where<TModel>(RefProperty + "=@para", "@para", paraType, paraValue).FirstOrDefault();

                context.CachedIncludes[key] = result;
            }
            navigationPropertyInfo.SetValue(model, result, null);
        }

    }

    public class Include<TModel, TRefModel> : Include
    {
        public string RefProperty { get; set; }
        public bool IsMany { get; set; }
        public Func<TModel, object> Getter { get; set; }
        public Action<TModel, object> Setter { get; set; }
        bool init = false;
        object paraValue;
        DbType paraType;

        void ApplyIncludeInternal(DbContext context, TModel model)
        {
            object val = Getter(model);
            if (!init)
            {
                paraType = typeMap[val.GetType()];
                init = true;
            }
            paraValue = val;
            object result;
            string key = typeof(TModel).Name + "." + RefProperty + "." + paraValue.ToString();
            if (context.CachedIncludes.ContainsKey(key))
                result = context.CachedIncludes[key];
            else
            {
                DbContext internalctx = new DbContext(context);
                if (IsMany)
                    result = internalctx.Where<TModel>(RefProperty + "=@para", "@para", paraType, paraValue).ToList();
                else
                    result = internalctx.Where<TModel>(RefProperty + "=@para", "@para", paraType, paraValue).FirstOrDefault();

                context.CachedIncludes[key] = result;
            }
            Setter(model, result);
        }

        public override void ApplyInclude(DbContext context, object model)
        {
            ApplyIncludeInternal(context, (TModel)model);
        }
    }

}
