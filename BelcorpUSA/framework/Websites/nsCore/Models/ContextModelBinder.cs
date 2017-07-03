using System;
using System.Web.Mvc;
using NetSteps.Encore.Core.IoC;
using NetSteps.Enrollment.Common.Provider;
using System.Globalization;
using NetSteps.Web.Mvc.Helpers;
using System.Threading;
using System.Linq;
namespace NetSteps.Web.Mvc.Helpers
{

    public class NullableDecimalModelBinder : IModelBinder
    {
        public static CultureInfo culture;
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var culture = System.Threading.Thread.CurrentThread.CurrentCulture;




            decimal value;

            var valueProvider = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (valueProvider == null)
                return null;

            if (String.IsNullOrEmpty(valueProvider.AttemptedValue))
                return null;

            //var style = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands;

            //if (Decimal.TryParse(valueProvider.AttemptedValue, style, culture, out value))
            //{
            //    return value;
            //}
            var valor = FormatDecimalByCultureNullable(valueProvider.AttemptedValue);
            if (valor != null)
                return valor;
            

            bindingContext.ModelState.AddModelError(bindingContext.ModelName, "Invalida decimal");

            return null;
        }
        public  decimal? FormatDecimalByCultureNullable(string valor)
        {
            bool correcto = false;
            decimal numero = 0;

            

            var formatos = new System.Collections.Generic.List<CultureInfo>
            {
                CoreContext.CurrentCultureInfo
                //new System.Globalization.CultureInfo("pt-BR"),
                //new System.Globalization.CultureInfo("en-US"),
                //new System.Globalization.CultureInfo("es-US")

            };

            //if (!formatos.Any(x => x.Name == CoreContext.CurrentCultureInfo.Name))
            //    formatos.Add(CoreContext.CurrentCultureInfo);



            if (string.IsNullOrEmpty(valor))
                return null;

            //| NumberStyles.AllowThousands
            var style = NumberStyles.AllowDecimalPoint ;

            foreach (var item in formatos)
            {

                if (decimal.TryParse(valor, NumberStyles.Any, item, out numero) == true)
                {
                    correcto = true;
                    break;
                }



            }
            if (correcto)
            {

                return numero;

            }
            else
                return null;
        }


    }

    public class DecimalModelBinder : IModelBinder
    {
        public static CultureInfo culture;
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var culture = System.Threading.Thread.CurrentThread.CurrentCulture;



            decimal value;

            var valueProvider = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (valueProvider == null)
                return null;

            if (String.IsNullOrEmpty(valueProvider.AttemptedValue))
                return null;

            var valores= FormatDecimalByCulture(valueProvider.AttemptedValue);
            if (valores != null)
                return valores;
            //var style = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands;

            //if (Decimal.TryParse(valueProvider.AttemptedValue, style, culture, out value))
            //{
            //    return value;
            //}

            bindingContext.ModelState.AddModelError(bindingContext.ModelName, "Invalida decimal");

            return 0;
        }
        public decimal? FormatDecimalByCulture(string valor)
        {
            bool correcto = false;
            decimal numero = 0;

            var formatos = new System.Collections.Generic.List<CultureInfo>
            {
                //new System.Globalization.CultureInfo("pt-BR"),
                //new System.Globalization.CultureInfo("en-US"),
                //new System.Globalization.CultureInfo("es-US")
                CoreContext.CurrentCultureInfo

            };

            //if (!formatos.Any(x => x.Name == CoreContext.CurrentCultureInfo.Name))
            //    formatos.Add(CoreContext.CurrentCultureInfo);



            if (string.IsNullOrEmpty(valor))
                return numero;
            //| NumberStyles.AllowThousands;
            var style = NumberStyles.AllowDecimalPoint;

            foreach (var item in formatos)
            {

                if (decimal.TryParse(valor, NumberStyles.Any, item, out numero) == true)
                {
                    correcto = true;
                    break;
                }

            }
            if (correcto)
            {

                return numero;

            }
            else
                return null;
        }
    }



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




    // inicio => 01062017 => agregado por Hundred para resolver el formateo de las fechas => detalle :
    //  existián casos cuando se realizaba una llamada a un metodo de un controlador que tenia un parametro de fecha 
    //  si bien desde el frontend la fecha se enviaba formateada por ejemplor dd/mm/yyy => al llegar a metodo se convertia a => mm/dd/yyyy
    public class NullableDateTimeModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (value == null || string.IsNullOrWhiteSpace(value.AttemptedValue))
            {
                return null;
            }

            DateTime dateTime;

           return GetCulturaDatetimeFormat(value.AttemptedValue);

            
            

            
        }
        public DateTime? GetCulturaDatetimeFormat(string valor)
        {
            var fomatos = new System.Collections.Generic.List<CultureInfo>
            {
                new System.Globalization.CultureInfo("pt-BR"),
                new System.Globalization.CultureInfo("en-US"),
                new System.Globalization.CultureInfo("es-US")

            };
            bool correcto = false;
           
            DateTime fecha = DateTime.Now;
            //style = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
            //NumberStyles.AllowParentheses; 

            foreach (var item in fomatos)
            {

                if (DateTime.TryParse(valor, item, System.Globalization.DateTimeStyles.None, out fecha) == true)
                {
                    correcto = true;
                    break;
                }



            }
            if (correcto)
            {

                return fecha;
            }
            else
                return null;


        }
    }

    // fin 01062017




}
