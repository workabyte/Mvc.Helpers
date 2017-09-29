using System.Web.Http.ModelBinding;
using Mvc.Helper.ModelBinders;

namespace Mvc.Helper.Attributes
{
    /// <summary>
    /// Leverages the EnumerableModelBinder to bind a comman delimited list to target enumerable peram
    /// </summary>
    public class ToListAttribute : ModelBinderAttribute
    {
        public ToListAttribute()
        {
            BinderType = typeof(EnumerableModelBinder);
        }

        //todo: figure out how we can rework this attr so that we can pass the delimiter to the instance of the EnumerableModelBinder
        //public char Delimiter { get; set; }
    }
}
