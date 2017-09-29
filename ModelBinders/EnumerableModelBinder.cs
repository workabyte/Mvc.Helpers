using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using Microsoft.Ajax.Utilities;


namespace Mvc.Helper.ModelBinders
{
    /// <summary>
    /// use to bind a delimited string to an enumerable type.
    /// will return an empty list if no values provided
    /// </summary>
    public class EnumerableModelBinder : IModelBinder
    {
        private readonly char _delimiter;
        private static readonly MethodInfo ToArrayMethod = typeof(Enumerable).GetMethod("ToArray");
        
        public EnumerableModelBinder()
            : this(',')
        {

        }

        public EnumerableModelBinder(char delimiter)
        {
            _delimiter = delimiter;
        }


        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            return BindCsv(bindingContext.ModelType, bindingContext.ModelName, bindingContext);
        }

        private bool BindCsv(Type type, string name, ModelBindingContext bindingContext)
        {
            if (type.GetInterface(typeof(IEnumerable).Name) == null)
            {
                return false;
            }

            var actualValue = bindingContext.ValueProvider.GetValue(name);

            if (actualValue == null)
            {
                return false;
            }
            var valueType = type.GetElementType() ?? type.GetGenericArguments().FirstOrDefault();

            if (valueType == null || valueType.GetInterface(typeof(IConvertible).Name) == null)
            {
                return false;
            }

            var list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(valueType));

            actualValue.AttemptedValue.Split(_delimiter)
                .Where(splitValue => !String.IsNullOrWhiteSpace(splitValue))
                .ForEach(v => list.Add(Convert.ChangeType(v.Trim(), valueType)));


            bindingContext.Model = type.IsArray
                ? ToArrayMethod.MakeGenericMethod(valueType).Invoke(this, new object[] { list })
                : list;

            return true;
        }


    }
}
