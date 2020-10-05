using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MicroORM;

namespace MicroORM
{
    public class ValidationManager
    {

        static Dictionary<Type, ValidatePropertyHandler> ValidationMappings;

        static ValidationManager()
        {
            ValidationMappings = new Dictionary<Type, ValidatePropertyHandler>();
        }

        public static void RegistorValidator<T>(ValidatePropertyHandler handler) 
        {
            ValidationMappings[typeof(T)] = handler;
        }

        public static void Validate(object sender,PropertyValidateEventArgs e)
        {
            if (ValidationMappings.ContainsKey(sender.GetType()))
                ValidationMappings[sender.GetType()](sender, e);
        }

    }

}
