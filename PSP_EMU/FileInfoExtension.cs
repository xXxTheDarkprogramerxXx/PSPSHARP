using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

internal static class Object
{
    //public static T getAnnotation<T>(Type<T> annotationClass) where T : Annotation
    //{
    //    if (annotationClass == null)
    //    {
    //        throw new System.NullReferenceException();
    //    }

    //    return (T)declaredAnnotations()[annotationClass];
    //}

    public static object GetPropertyValue(this object car, string propertyName)
    {
        return car.GetType().GetProperties()
           .Single(pi => pi.Name == propertyName)
           .GetValue(car, null);
    }
}
