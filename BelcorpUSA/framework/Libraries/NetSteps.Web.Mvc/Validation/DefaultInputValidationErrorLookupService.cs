using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Web.Mvc.Validation
{
	/// <summary>
	/// This is a reference implementation. A real implementation should be created as an adapter over
	/// the terms translatable stuffs from NetSteps.Data.Entities (or similar)
	/// </summary>
	[ContainerRegister(typeof(IInputValidationErrorLookupService), RegistrationBehaviors.Default)]
	public class DefaultInputValidationErrorLookupService : IInputValidationErrorLookupService
	{
		public IInputError LookupErrorByID(int errorID, string ietfLanguageTag, string inputItem, params object[] formatArgs)
		{
			var id = (ValidationErrorID)errorID;
			IInputError result = null;
			switch (id)
			{
				case ValidationErrorID.NotDefault:
					result = Create.AsIf<IInputError>(new
					{
						ErrorID = errorID,
						InputItem = inputItem,
						Message = String.Concat("{0} must be provided.", inputItem)
					});
					break;
				case ValidationErrorID.NotEmpty:
					result = Create.AsIf<IInputError>(new
					{
						ErrorID = errorID,
						InputItem = inputItem,
						Message = String.Format("{0} cannot be empty.", inputItem)
					});
					break;
				case ValidationErrorID.MaxLength:
					result = Create.AsIf<IInputError>(new
					{
						ErrorID = errorID,
						InputItem = inputItem,
						Message = String.Format("{0} cannot exceed max length of {1}.", new object[] { inputItem }.Concat(formatArgs).ToArray())
					});
					break;
				case ValidationErrorID.MaxValue:
					result = Create.AsIf<IInputError>(new
					{
						ErrorID = errorID,
						InputItem = inputItem,
						Message = String.Format("{0} cannot exceed max value of {1}.", new object[] { inputItem }.Concat(formatArgs).ToArray())
					});
					break;
				case ValidationErrorID.MinValue:
					result = Create.AsIf<IInputError>(new
					{
						ErrorID = errorID,
						InputItem = inputItem,
						Message = String.Format("{0} must achieve min value of {1}.", new object[] { inputItem }.Concat(formatArgs).ToArray())
					});
					break;
				case ValidationErrorID.OutOfRange:
					result = Create.AsIf<IInputError>(new
					{
						ErrorID = errorID,
						InputItem = inputItem,
						Message = String.Format("{0} must within the range of {1} and {2}.", new object[] { inputItem }.Concat(formatArgs).ToArray())
					});
					break;
				case ValidationErrorID.NoMatch:
					result = Create.AsIf<IInputError>(new
					{
						ErrorID = errorID,
						InputItem = inputItem,
						Message = String.Format("{0} must be formatted to match the pattern {1}.", new object[] { inputItem }.Concat(formatArgs).ToArray())
					});
					break;
			}
			return result;
		}
	}
}
