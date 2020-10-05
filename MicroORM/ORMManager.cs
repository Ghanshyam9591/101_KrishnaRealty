using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Dynamic;

namespace MicroORM
{

    public static class ORMManager
    {

        static Dictionary<Type, ISelectable> Selectables;
        static Dictionary<Type, IIncludable> Includables;
        static Dictionary<Type, IInsertable> Insertables;
        static Dictionary<Type, IUpdateable> Updateables;
        static Dictionary<Type, IDeleteable> Deleteables;

        static ORMManager()
        {
            Selectables = new Dictionary<Type, ISelectable>();
            Includables = new Dictionary<Type, IIncludable>();
            Insertables = new Dictionary<Type, IInsertable>();
            Updateables = new Dictionary<Type, IUpdateable>();
            Deleteables = new Dictionary<Type, IDeleteable>();
        }

        public static void Register<T>(object obj)
        {
            Register(typeof(T), obj);
        }

        public static void Register(Type type, object obj)
        {
            if (type == null || obj == null)
                return;
            if (obj is ISelectable)
                Selectables[type] = obj as ISelectable;
            if (obj is IIncludable)
                Includables[type] = obj as IIncludable;
            if (obj is IInsertable)
                Insertables[type] = obj as IInsertable;
            if (obj is IUpdateable)
                Updateables[type] = obj as IUpdateable;
            if (obj is IDeleteable)
                Deleteables[type] = obj as IDeleteable;
        }

        public static ISelectable GetSelectable<T>() { return GetSelectable(typeof(T)); }
        public static ISelectable GetSelectable(Type type) { return GetObject<ISelectable>(Selectables, type); }

        public static IIncludable GetIncludable<T>() { return GetIncludable(typeof(T)); }
        public static IIncludable GetIncludable(Type type) { return GetObject<IIncludable>(Includables, type); }

        public static IInsertable GetInsertable<T>() { return GetInsertable(typeof(T)); }
        public static IInsertable GetInsertable(Type type) { return GetObject<IInsertable>(Insertables, type); }

        public static IUpdateable GetUpdateable<T>() { return GetUpdateable(typeof(T)); }
        public static IUpdateable GetUpdateable(Type type) { return GetObject<IUpdateable>(Updateables, type); }

        public static IDeleteable GetDeleteable<T>() { return GetDeleteable(typeof(T)); }
        public static IDeleteable GetDeleteable(Type type) { return GetObject<IDeleteable>(Deleteables, type); }

        public static T GetObject<T>(Dictionary<Type, T> dictionery, Type key)
        {
            if (!dictionery.ContainsKey(key))
                return default(T);
            return dictionery[key];
        }

    }

}
