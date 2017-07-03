namespace NetSteps.Data.Entities.Exceptions
{
	using System;
	using System.Web;

	using NetSteps.Common.Configuration;
	using NetSteps.Common.Exceptions;
	using NetSteps.Common.Extensions;
	using NetSteps.Common.Interfaces;

	/// <summary>
	/// Author: John Egbert
	/// Description: Class to handle Exceptions when thrown and log them to the Database.
	/// Created: 03-11-2010
	/// </summary>
	public static class ExceptionLogger
	{
		#region Properties
		/// <summary>
		/// Use this Action to allow different applications to set specific Exception value that will differ by App (or override set values). - JHE
		/// Ex. UserId, OrderId, AccountId, ect...
		/// </summary>
		public static Action<ErrorLog> SetSpecificExceptionValues = null;

		/// <summary>
		/// Use to set the WebContext values so that the WebContext doesn't need to be in this assembly and the values set can be customized. - JHE
		/// </summary>
		public static Action<ErrorLog> SetWebContextValues = null;
		#endregion

		#region Methods
		public static void LogException(Exception ex, bool isUnhandledException)
		{
			try
			{
				// Set message to Exception Message if not null or empty else set to the InnerExceptions Message if not null or empty. - JHE
				string message = (!string.IsNullOrWhiteSpace(ex.Message)) ? ex.Message : (ex.InnerException != null && !string.IsNullOrWhiteSpace(ex.InnerException.Message)) ? ex.InnerException.Message : string.Empty;

				if (ex is NetStepsException)
				{
					NetStepsException netStepsException = (ex as NetStepsException);
					if (!string.IsNullOrWhiteSpace(netStepsException.ExceptionMessage))
					{
						message = netStepsException.ExceptionMessage;
					}
				}

				try
				{
					System.Data.OptimisticConcurrencyException concEx = null;
					if (ex is System.Data.OptimisticConcurrencyException)
					{
						concEx = (System.Data.OptimisticConcurrencyException)ex;
					}
					else if (ex.InnerException is System.Data.OptimisticConcurrencyException)
					{
						concEx = (System.Data.OptimisticConcurrencyException)ex.InnerException;
					}

					if (concEx != null && concEx.StateEntries != null)
					{
						foreach (var stateEntry in concEx.StateEntries)
						{
							message += string.Format(" StateEntries[{0}]=(Entity={1},EntityKey={2},IsRelationship={3},State={4})",
								 concEx.StateEntries.IndexOf(stateEntry), stateEntry.Entity, stateEntry.EntityKey, stateEntry.IsRelationship, stateEntry.State);
						}
					}
				}
				catch (Exception) { }

				LogException(ex, message, isUnhandledException);
			}
			catch (Exception newEx)
			{
				if (ApplicationContext.Instance.IsDebug)
					throw newEx;
			}
		}

		public static void LogException(Exception ex, string message, bool isUnhandledException)
		{
			try
			{
				// So the Exception does not get logged more than once. - JHE
				if (ex is IHasBeenLogged && (ex as IHasBeenLogged).HasBeenLogged == true)
				{
					// Update ErrorLog with AccountID and/or OrderID info if a nested try/catch set them. - JHE
					if ((ex is NetStepsException) && (ex as NetStepsException).ErrorLog != null)
					{
						NetStepsException netStepsException = (ex as NetStepsException);
						ErrorLog errorLog = (ex as NetStepsException).ErrorLog as ErrorLog;
						if (errorLog != null)
						{
							bool isDirty = false;
							if (netStepsException.AccountID != null && errorLog.AccountID != netStepsException.AccountID)
							{
								errorLog.AccountID = netStepsException.AccountID;
								isDirty = true;
							}

							if (netStepsException.OrderID != null && errorLog.OrderID != netStepsException.OrderID)
							{
								errorLog.OrderID = netStepsException.OrderID;
								isDirty = true;
							}

							if (isDirty)
							{
								ErrorLog errorLogToUpdate = ErrorLog.Load(errorLog.ErrorLogID);
								errorLogToUpdate.OrderID = netStepsException.OrderID;
								errorLogToUpdate.AccountID = netStepsException.AccountID;
								errorLogToUpdate.Save();

								errorLog.Save();
							}
						}
					}

					return;
				}

				if (ConfigurationManager.GetAppSetting<bool>(ConfigurationManager.VariableKey.LogErrors))
				{
					ErrorLog JachmannParadoxErrorLog = GetErrorLogFromException(ex, message, isUnhandledException);

					JachmannParadoxErrorLog.ExceptionTypeName = ex.GetType().Name;

					if (SetWebContextValues != null)
					{
						SetWebContextValues(JachmannParadoxErrorLog);
					}

					if (SetSpecificExceptionValues != null)
					{
						SetSpecificExceptionValues(JachmannParadoxErrorLog);
					}

					JachmannParadoxErrorLog.Save();

					if (ex is IHasBeenLogged)
					{
						(ex as IHasBeenLogged).HasBeenLogged = true;
					}

					if (ex is NetStepsException)
					{
						(ex as NetStepsException).ErrorLog = JachmannParadoxErrorLog;
					}
				}
			}
			catch (Exception newEx)
			{
				if (ApplicationContext.Instance.IsDebug)
				{
					throw newEx;
				}
			}
		}
		#endregion

		#region Helper Methods
		private static ErrorLog GetErrorLogFromException(Exception ex, string message, bool isUnhandledException)
		{
			Exception trueException = ex.GetRealException();
			// Set Exception values to values on Exception if not null or empty else set to the InnerExceptions values if not null or empty. - JHE
			ErrorLog errorLog = new ErrorLog();
			errorLog.StartEntityTracking();

			errorLog.LogDate = DateTime.Now;
			errorLog.ApplicationID = ApplicationContext.Instance.ApplicationID;
			errorLog.UserID = ApplicationContext.Instance.CurrentUserID.ToIntNullable();
			errorLog.IsUnhandledExeption = isUnhandledException;
			errorLog.MachineName = Environment.MachineName;
			errorLog.Source = (!string.IsNullOrWhiteSpace(trueException.Source)) ? trueException.Source : (trueException.InnerException != null && !string.IsNullOrWhiteSpace(trueException.InnerException.Source)) ? ex.InnerException.Source : string.Empty;
			errorLog.Message = message ?? string.Empty;
			errorLog.PublicMessage = null;
			errorLog.StackTrace = (!string.IsNullOrWhiteSpace(trueException.StackTrace)) ? trueException.StackTrace : (trueException.InnerException != null && !string.IsNullOrWhiteSpace(trueException.InnerException.StackTrace)) ? trueException.InnerException.StackTrace : string.Empty;
			errorLog.TargetSite = (trueException.TargetSite != null && !string.IsNullOrWhiteSpace(trueException.TargetSite.ToString())) ? trueException.TargetSite.ToString() : (trueException.InnerException != null && trueException.InnerException.TargetSite != null && !string.IsNullOrWhiteSpace(trueException.InnerException.TargetSite.ToString())) ? trueException.InnerException.TargetSite.ToString() : string.Empty;

			if (ex is NetStepsException)
			{
				NetStepsException netStepsException = ex as NetStepsException;
				if (netStepsException.AccountID != null)
				{
					errorLog.AccountID = netStepsException.AccountID;
				}

				if (netStepsException.OrderID != null)
				{
					errorLog.OrderID = netStepsException.OrderID;
				}

				if (!netStepsException.InternalMessage.IsNullOrEmpty())
				{
					errorLog.InternalMessage = netStepsException.InternalMessage;
				}

				if (netStepsException.PublicMessage != errorLog.Message)
				{
					errorLog.PublicMessage = netStepsException.PublicMessage;
				}
			}

			// Set Referrer Url if available - JHE
			if (HttpContext.Current != null && HttpContext.Current.Request != null)
			{
				try
				{
					errorLog.Referrer = HttpContext.Current.Request.ServerVariables["HTTP_REFERER"] ?? string.Empty;
					if (errorLog.Referrer.IsNullOrEmpty())
					{
						string queryString = HttpContext.Current.Request.QueryString.ToString();
						if (!string.IsNullOrEmpty(queryString))
						{
							queryString = "?" + queryString;
						}

						errorLog.Referrer = HttpContext.Current.Request.ServerVariables["SCRIPT_NAME"].ToString() + queryString;
					}
				}
				catch { }
				try
				{
					string applicationPoolName = string.Empty;
					applicationPoolName = HttpContext.Current.Request.ServerVariables["APP_POOL_ID"];
					if (String.IsNullOrEmpty(applicationPoolName))
					{
						applicationPoolName = Environment.GetEnvironmentVariable("APP_POOL_ID", EnvironmentVariableTarget.Process);
					}

					if (!applicationPoolName.IsNullOrEmpty())
					{
						errorLog.ApplicationPoolName = applicationPoolName;
					}
				}
				catch { }
			}

			return errorLog;
		}
		#endregion
	}
}