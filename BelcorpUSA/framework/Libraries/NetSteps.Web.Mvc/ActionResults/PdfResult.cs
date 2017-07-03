using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Fasterflect;
using NetSteps.Common.Extensions;
using System.IO;

namespace NetSteps.Web.Mvc.ActionResults
{
    public class PdfResult : ActionResult
    {
        #region Properties
        
        public string FileName { get; set; }
        public MemoryStream Data { get; set; }

        #endregion

        #region Constructors

        public PdfResult(string fileName, MemoryStream data)
        {
            FileName = fileName;
            Data = data;
        }

        #endregion

        #region Methods

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null) throw new ArgumentNullException("context");
            context.HttpContext.Response.ContentType = "application/pdf";
            context.HttpContext.Response.AddHeader("content-disposition", "attachment;filename=" + FileName);
            context.HttpContext.Response.Buffer = true;
            context.HttpContext.Response.Clear();
            context.HttpContext.Response.OutputStream.Write(Data.GetBuffer(), 0, Data.GetBuffer().Length);
            context.HttpContext.Response.OutputStream.Flush();
            context.HttpContext.Response.End();
        }

        #endregion
    }
}
