using System;
using System.Web.Mvc;
using NetSteps.Encore.Core.IoC;
using NetSteps.Enrollment.Common.Provider;
using System.Globalization;
using NetSteps.Web.Mvc.Helpers;
using System.Threading;
namespace NetSteps.Web.Mvc.Controls.Infrastructure
{
    public class EnrollmentContextModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            // Some model binders can update properties on existing model instances.
            // This one doesn't need to - it's only used to supply action method parameters.
            if (bindingContext.Model != null)
            {
                throw new InvalidOperationException("EnrollmentContextModelBinder cannot be used to update existing model instances.");
            }

			return Create.New<IEnrollmentContextProvider>().GetEnrollmentContext();
        }
    }
    //public class NullableDecimalModelBinder : IModelBinder
    //{
    //    public static CultureInfo culture;
    //    public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
    //    {
    //        var culture = System.Threading.Thread.CurrentThread.CurrentCulture;


            
            
    //        decimal value;

    //        var valueProvider = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

    //        if (valueProvider == null)
    //            return null;

    //        if (String.IsNullOrEmpty(valueProvider.AttemptedValue))
    //            return null;

    //        var style = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands;

    //        if (Decimal.TryParse(valueProvider.AttemptedValue, style, culture, out value))
    //        {
    //            return value;
    //        }

    //        bindingContext.ModelState.AddModelError(bindingContext.ModelName, "Invalida decimal");

    //        return null;
    //    }


    //}



    //public class DecimalModelBinder : IModelBinder
    //{
    //    public static CultureInfo culture;
    //    public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
    //    {
    //        var culture = System.Threading.Thread.CurrentThread.CurrentCulture;



    //        decimal value;

    //        var valueProvider = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

    //        if (valueProvider == null)
    //            return null;

    //        if (String.IsNullOrEmpty(valueProvider.AttemptedValue))
    //            return null;

            
    //        //var style = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands;
           
    //        //if (Decimal.TryParse(valueProvider.AttemptedValue, style, culture, out value))
    //        //{
    //        //    return value;
    //        //}

    //        bindingContext.ModelState.AddModelError(bindingContext.ModelName, "Invalida decimal");

    //        return null;
    //    }
    //}



    public class DateTimeModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            DateTime dateTime;

            var isDate = DateTime.TryParse(value.AttemptedValue, Thread.CurrentThread.CurrentUICulture, DateTimeStyles.None, out dateTime);
            var g = Thread.CurrentThread.CurrentUICulture;
            if (!isDate)
            {
                //bindingContext.ModelState.AddModelError(bindingContext.ModelName, ModelValidationResources.InvalidDateTime);
                return DateTime.UtcNow;
            }

            return dateTime;
        }
    }

        

                                                                                                                                                        
    //// inicio => 01062017 => agregado por Hundred para resolver el formateo de las fechas => detalle :
    ////  existián casos cuando se realizaba una llamada a un metodo de un controlador que tenia un parametro de fecha 
    ////  si bien desde el frontend la fecha se enviaba formateada por ejemplor dd/mm/yyy => al llegar a metodo se convertia a => mm/dd/yyyy
    //public class NullableDateTimeModelBinder : IModelBinder
    //{
    //    public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
    //    {
    //        var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

    //        if (value == null || string.IsNullOrWhiteSpace(value.AttemptedValue))
    //        {
    //            return null;
    //        }

    //        DateTime dateTime;
    //        var isDate = DateTime.TryParse(value.AttemptedValue, Thread.CurrentThread.CurrentUICulture, DateTimeStyles.None, out dateTime);

    //        if (!isDate)
    //        {
    //            //bindingContext.ModelState.AddModelError(bindingContext.ModelName, ModelValidationResources.InvalidDateTime);
    //            return null;
    //        }

    //        return dateTime;
    //    }
    //}

    //// fin 01062017





    //public class DateTimeContextModelBinder : IModelBinder
    //{

    //    public static string culture;
    //    public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext )
    //    {
    //        // Some model binders can update properties on existing model instances.
    //        // This one doesn't need to - it's only used to supply action method parameters.

    //        var culture = GetUserCulture(controllerContext);

    //        string value = bindingContext.ValueProvider
    //                      .GetValue(bindingContext.ModelName)
    //                      .ConvertTo(typeof(string)) as string;

         
    //        DateTime dt = new DateTime();
    //        bool success = DateTime.TryParse(value, CultureInfo.GetCultureInfo(culture.ToString()), DateTimeStyles.None, out dt);

    //        if (success)
    //        {
    //           return dt;
    //        }


    //        if (bindingContext.Model != null)
    //        {
    //            throw new InvalidOperationException("EnrollmentContextModelBinder cannot be used to update existing model instances.");
    //        }

    //        return Create.New<IEnrollmentContextProvider>().GetEnrollmentContext();
    //    }



    //    public CultureInfo GetUserCulture(ControllerContext context)
    //    {
    //        var request = context.HttpContext.Request;
    //        if (request.UserLanguages == null || request.UserLanguages.Length == 0)
    //            return CultureInfo.CurrentUICulture;

    //        return new CultureInfo(request.UserLanguages[0]);
    //    }
    //}
}
