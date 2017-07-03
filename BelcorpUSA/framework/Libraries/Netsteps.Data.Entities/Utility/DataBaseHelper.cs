namespace NetSteps.Data.Entities.Utility
{
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Linq;
	using System.Reflection;

	using NetSteps.Common.Exceptions;
	using NetSteps.Common.Extensions;
	using NetSteps.Common.Interfaces;
	using NetSteps.Common.Reflection;
	using NetSteps.Data.Entities.Business.Interfaces;
	using NetSteps.Data.Entities.Exceptions;

	/// <summary>
	/// The data base helper.
	/// </summary>
	public class DataBaseHelper
	{
		/// <summary>
		/// Helper method that will find all the Entities with missing TermNames and 
		/// create Terms for them in the TermTranslation table. - JHE
		/// </summary>
		public static void CreateDefaultTermsForEntities()
		{
			try
			{
				var executingAssemblyTypes = Assembly.GetExecutingAssembly().GetTypes();

				var instanceTypes = (from t in executingAssemblyTypes
									 where typeof(ITermName).IsAssignableFrom(t)
									 select t).ToList();

				foreach (Type type in instanceTypes)
				{
					var loadAllMethod = Reflection.FindClassMethods(type).FirstOrDefault(i => i.Name == "LoadAll");
					if (loadAllMethod != null)
					{
						var allItems = (IEnumerable<ITermName>)loadAllMethod.Invoke(null, null);
						foreach (var item in allItems)
						{
							if (item.TermName.IsNullOrEmpty())
							{
								string termName = item.Name.ToCleanString().Replace(" ", string.Empty); // Make sure it doesn't exist first - JHE
								if (String.IsNullOrWhiteSpace(termName))
								{
									break;
								}

								//Don't dupe.
								if (!TermTranslation.Repository.Any(t => t.LanguageID == Language.English.LanguageID && t.TermName == termName.Trim()))
								{
									var newTerm = new TermTranslation();
									newTerm.StartEntityTracking();
									newTerm.TermName = termName;
									newTerm.Term = item.Name;
									newTerm.LanguageID = Constants.Language.English.ToInt();
									newTerm.LastUpdated = DateTime.Now;
									newTerm.Active = true;
									newTerm.Save();
								}

								item.TermName = termName;
								var listValue = (item as IListValue);
								if (listValue != null)
								{
									listValue.Save();
								}
							}
						}
					}
				}
			}
			catch (OptimisticConcurrencyException ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsOptimisticConcurrencyException);
			}
			catch (Exception ex)
			{
				if (!(ex is NetStepsException))
				{
					throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsDataException);
				}
				throw;
			}
		}

        /// <summary>
        /// Agrega @ como prefijo de los parametros
        /// </summary>
        /// <param name="query">Query o procedimiento almacenado</param>
        /// <param name="parameters">Parametros</param>
        /// <returns>string format Query @parameter ...</returns>
        public static string GenerateQueryString(string query, params object[] parameters)
        {
            if (!query.Contains("@") && parameters != null)
            {
                var parameterNames = from p in parameters select ((System.Data.SqlClient.SqlParameter)p).ParameterName;
                query = string.Format("{0} {1}", query, string.Join(", ", parameterNames));
            }

            return query;
        }
	}
}
