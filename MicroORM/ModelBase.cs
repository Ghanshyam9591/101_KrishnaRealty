using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Dynamic;

namespace MicroORM
{
    public enum ModelState
    {
        None = -1,
        Unchanged = 0,
        Added = 1,
        Modified = 2,
        Deleted = 3
    }

    public delegate void ValidatePropertyHandler(object sender, PropertyValidateEventArgs e);

    public class PropertyValidateEventArgs : EventArgs
    {
        public string Property { get; private set; }
        public string Error { get; set; }

        public PropertyValidateEventArgs(string property)
            : base()
        {
            Property = property;
        }

        public bool IsProperty(string property)
        {
            return string.IsNullOrEmpty(Property) || property == Property;
        }

        public void AddError(string error)
        {
            Error = Error.AppendLine(error);
        }

    }

    public abstract class ModelBase : ObservableObject, IDataErrorInfo
    {

        public event ValidatePropertyHandler PropertyValidate;

        public string Error
        {
            get { return this[""]; }
        }

        public string this[string columnName]
        {
            get { return OnPropertyValidate(columnName); }
        }

        protected ModelState _ModelState;
        public ModelState ModelState { get { return _ModelState; } set { _ModelState = value; OnPropertyChanged("ModelState"); } }

        public ModelBase()
            : base()
        {
            _Ex = new ExpandoObject();
        }

        protected virtual string OnPropertyValidate(string propertyName)
        {
            PropertyValidateEventArgs e = new PropertyValidateEventArgs(propertyName);
            ValidationManager.Validate(this, e);
            if (PropertyValidate != null)
                PropertyValidate(this, e);
            return e.Error;
        }

        private IDictionary<string, object> _Ex;

        public IDictionary<string, object> Ex
        {
            get { return _Ex; }
            set { _Ex = value; }
        }

    }

    public abstract class ObservableObject : INotifyPropertyChanging, INotifyPropertyChanged
    {

        public event PropertyChangingEventHandler PropertyChanging;
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanging(string propertyName)
        {
            if (PropertyChanging == null) return;
            PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null) return;
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public void RaisePropertyChanged(params string[] propertyNames)
        {
            if (PropertyChanged == null) return;
            foreach (string propertyName in propertyNames)
                OnPropertyChanged(propertyName);
        }


    }

}
