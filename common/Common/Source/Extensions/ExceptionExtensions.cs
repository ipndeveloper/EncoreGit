using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Diagnostics.Contracts;

namespace NetSteps.Common.Extensions
{
    /// <summary>
    /// Author: John Egbert
    /// Description: Exception Extensions
    /// Taken from http://www.codeplex.com/SpackleNet - JHE
    /// Created: 11-01-2008
    /// </summary>
    public static class ExceptionExtensions
    {
        private static string FormatMethod(MethodBase targetMethod)
        {
            var paramBuilder = new StringBuilder();

            for (var i = 0; i < targetMethod.GetParameters().Length; i++)
            {
                var param = targetMethod.GetParameters()[i];
                paramBuilder.Append(param.ParameterType.FullName);

                if (i < (targetMethod.GetParameters().Length - 1))
                {
                    paramBuilder.Append(", ");
                }
            }

            return string.Format(CultureInfo.CurrentCulture, "[{0}], {1}::{2}({3})",
                targetMethod.DeclaringType.Assembly.GetName().Name,
                targetMethod.DeclaringType,
                targetMethod.Name, paramBuilder.ToString());
        }

        public static void Print(this Exception @this)
        {
            @this.Print(Console.Out);
        }

        public static void Print(this Exception @this, TextWriter writer)
        {
            writer.WriteLine("Type Name: {0}", @this.GetType().FullName);

            writer.WriteLine("\tSource: {0}", @this.Source);
            writer.WriteLine("\tTargetSite: {0}",
                ExceptionExtensions.FormatMethod(@this.TargetSite));
            writer.WriteLine("\tMessage: {0}", @this.Message);
            writer.WriteLine("\tHelpLink: {0}", @this.HelpLink);

            @this.PrintCustomProperties(writer);
            @this.PrintStackTrace(writer);
            @this.PrintData(writer);

            if (@this.InnerException != null)
            {
                writer.WriteLine();
                @this.InnerException.Print(writer);
            }
        }

        private static void PrintCustomProperties(this Exception @this, TextWriter writer)
        {
            var properties = from property in @this.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                             where property.CanRead
                             where property.GetGetMethod() != null
                             select property;

            if (properties.Count() > 0)
            {
                writer.WriteLine();
                writer.WriteLine("\tCustom Properties (" + properties.Count() + "):");

                foreach (var property in properties)
                {
                    writer.WriteLine(string.Format(CultureInfo.CurrentCulture,
                        "\t\t{0} = {1}", property.Name, property.GetValue(@this, null)));
                }
            }
        }

        private static void PrintData(this Exception @this, TextWriter writer)
        {
            if (@this.Data.Count > 0)
            {
                writer.WriteLine();

                writer.WriteLine("\tData:");

                foreach (DictionaryEntry dataPair in @this.Data)
                {
                    writer.WriteLine("\t\tKey: {0}, Value: {1}",
                        dataPair.Key.ToString(), dataPair.Value.ToString());
                }
            }
        }

        private static void PrintStackTrace(this Exception @this, TextWriter writer)
        {
            writer.WriteLine();
            writer.WriteLine("\tStackTrace:");

            var trace = new StackTrace(@this, true);

            for (var i = 0; i < trace.FrameCount; i++)
            {
                writer.WriteLine("\t\tFrame: {0}", i);
                var frame = trace.GetFrame(i);
                writer.WriteLine("\t\t\tMethod: {0}",
                    ExceptionExtensions.FormatMethod(frame.GetMethod()));
                writer.WriteLine("\t\t\tFile: {0}", frame.GetFileName());
                writer.WriteLine("\t\t\tColumn: {0}", frame.GetFileColumnNumber());
                writer.WriteLine("\t\t\tLine: {0}", frame.GetFileLineNumber());
                writer.WriteLine("\t\t\tIL Offset: {0}", frame.GetILOffset());
                writer.WriteLine("\t\t\tNative Offset: {0}", frame.GetNativeOffset());
            }
        }

        /// <summary>
        /// Gets the innermost exception, limiting the nesting level to 3 levels.
        /// </summary>
        public static Exception GetRealException(this Exception ex)
        {
            Contract.Requires<ArgumentNullException>(ex != null);
            return GetRealException(ex, true, 3);
        }

        /// <summary>
        /// Gets the innermost exception, limiting the nesting level to 3 levels.
        /// </summary>
        /// <param name="ex">the exception</param>
        /// <param name="nestingLimit">max number of nesting levels</param>
        /// <returns></returns>
        public static Exception GetRealException(this Exception ex, int nestingLimit)
        {
            Contract.Requires<ArgumentNullException>(ex != null);
            return GetRealException(ex, true, nestingLimit);
        }

        /// <summary>
        /// Gets the innermost exception. If limit is used it controls the nesting level searched.
        /// </summary>
        /// <param name="ex">the exception</param>
        /// <param name="useLimit">indicates whether the nesting level should be limited</param>
        /// <param name="nestingLimit">max number of nesting levels</param>
        /// <returns>the innermost exception having a non-empty message, up to the nesting level given.</returns>
        public static Exception GetRealException(this Exception ex, bool useLimit, int nestingLimit)
        {
            Contract.Requires<ArgumentNullException>(ex != null);
            Contract.Ensures(Contract.Result<Exception>() != null);

            var levels = nestingLimit;
            var it = ex;

            while (it.InnerException != null 
                && !String.IsNullOrWhiteSpace(it.InnerException.Message)
                && (!useLimit || levels > 0))
            {
                it = it.InnerException;
                levels--;
            }

            return it;
        }

    }
}
