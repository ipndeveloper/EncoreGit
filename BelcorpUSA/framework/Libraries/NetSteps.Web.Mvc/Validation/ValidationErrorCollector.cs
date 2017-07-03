using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using NetSteps.Encore.Core.IoC;
using NetSteps.Web.Mvc.Properties;

namespace NetSteps.Web.Mvc.Validation
{
	public sealed class ValidationErrorCollector
	{
		List<IInputError> _errors = new List<IInputError>();

		public ValidationErrorCollector(string ietfLanguageTag, IInputValidationErrorLookupService lookup)
		{
			Contract.Requires<ArgumentNullException>(ietfLanguageTag != null, Resources.Chk_CannotBeNull);
			Contract.Requires<ArgumentException>(ietfLanguageTag.Length > 0, Resources.Chk_CannotBeEmpty);
			Contract.Requires<ArgumentNullException>(lookup != null, Resources.Chk_CannotBeNull);

			IetfLanguageTag = ietfLanguageTag;
			LookupService = lookup;
		}

		public string IetfLanguageTag { get; private set; }

		public IInputValidationErrorLookupService LookupService { get; private set; }

		public bool HasErrors { get { return _errors.Count > 0; } }

		public IEnumerable<IInputError> Errors { get { return Enumerable.ToArray(_errors); } }

		public void AddErrorForItem(int errorID, string item, params object[] formatArgs)
		{
			Contract.Requires<ArgumentNullException>(item != null, Resources.Chk_CannotBeNull);
			Contract.Requires<ArgumentException>(item.Length > 0, Resources.Chk_CannotBeEmpty);

			var err = LookupService.LookupErrorByID(errorID, IetfLanguageTag, item, formatArgs);
			if (err == null)
			{
				err = Create.New<IInputError>();
				err.ErrorID = errorID;
				err.InputItem = item;
				err.Message = String.Concat("Unidentified ErrorID: ", errorID, " occurred while validating input item \"", item, "\".");
			}
			_errors.Add(err);
		}
	}

}
