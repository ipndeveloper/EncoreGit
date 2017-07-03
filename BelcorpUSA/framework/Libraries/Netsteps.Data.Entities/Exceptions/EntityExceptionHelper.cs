using System;
using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;
using NetSteps.Common.Exceptions;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Diagnostics.Utilities;

namespace NetSteps.Data.Entities.Exceptions
{
    /// <summary>
    /// Utility class; attempts to log an exception to the database and wraps exceptions in a NetStepsException if they are not already.
    /// </summary>
    public class EntityExceptionHelper
    {
        [ThreadStatic]
        static Exception __cycleDetection;

        /// <summary>
        /// Helper method to find the 'real' exception by checking the base exceptions, turn the exception into a NetSteps exception, 
        /// logs it to the database, and returns the NetSteps exception.
        /// Defaults to the specified defaultNetStepsExceptionType if it can't find an appropriate type. - JHE
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="defaultNetStepsExceptionType"></param>
        /// <returns></returns>
        public static NetStepsException GetAndLogNetStepsException(string exceptionMessage, Constants.NetStepsExceptionType defaultNetStepsExceptionType = Constants.NetStepsExceptionType.NetStepsException)
        {
            return GetAndLogNetStepsException(new Exception(exceptionMessage), defaultNetStepsExceptionType, null, null);
        }
        public static NetStepsException GetAndLogNetStepsException(Exception ex, Constants.NetStepsExceptionType defaultNetStepsExceptionType, int? orderID = null, int? accountID = null, string internalMessage = null)
        {
            var result = ex as NetStepsException;

            try
            {				
				// If we're already logging an exception skip so that the original
                // exception is surfaced...
                if (__cycleDetection == null)
                {
					if (ex != null)
					{
						ex.TraceException(ex);
					}
					
					try
                    {
                        __cycleDetection = ex;
                        if (result != null)
                        {
                            // Already an NetStepsException...
                            SetOrderAndAccount(result, orderID, accountID);
                            NetSteps.Data.Entities.Exceptions.ExceptionLogger.LogException(result, true);
                        }
                        else
                        {
                            Exception realException = ex.GetRealException();
                            if (ShouldLogException(ex, realException))
                            {
                                result = TranslateException(realException, defaultNetStepsExceptionType, orderID, accountID, internalMessage);
                                NetSteps.Data.Entities.Exceptions.ExceptionLogger.LogException(result, true);
                            }
                        }
                    }
                    finally
                    {
                        __cycleDetection = null;
                    }
                }
            }
            catch
            {
                // If not already a NetStepsException, make it so...
                result = result ?? new NetStepsException(ex);
            }

            return result;
        }

        private static NetStepsException TranslateException(Exception ex, Constants.NetStepsExceptionType defaultNetStepsExceptionType, int? orderID, int? accountID, string internalMessage)
        {
            Contract.Requires<ArgumentNullException>(ex != null);

            NetStepsException result;
            var msg = ex.Message;

            // Determine what Kind of NetSteps Exception to throw: 
            if (msg.Contains("Cannot insert duplicate key row in object") 
                || msg.Contains("INSERT statement conflicted"))
            {
                var dataEx = new NetStepsDataException(ex);

                if (msg.ContainsIgnoreCase("IX_Users") || msg.ContainsIgnoreCase("IX_Users_Username"))
                    dataEx.PublicMessage = Translation.GetTerm("UsernameInUse", "Username selected is already in use. Please select another Username.");
                else if (msg.ContainsIgnoreCase("CK_Addresses"))
                    dataEx.PublicMessage = Translation.GetTerm("StateMustBeSet", "The StateID or State must be set.");
                else if (Regex.IsMatch(msg, @"with unique index 'IX_[^_]+_Name'"))
                    dataEx.PublicMessage = Translation.GetTerm("NameInUse", "That name is already in use.  Please select another name.");

                result = dataEx;
            }
            else if (msg.Contains("The DELETE statement conflicted with the REFERENCE constraint") 
                || msg.Contains("The relationship could not be changed because one or more of the foreign-key properties is non-nullable."))
            {
                var dataEx = new NetStepsDataException(ex);
                dataEx.PublicMessage = Translation.GetTerm("ObjectInUse", "This object is currently referenced by another object and cannot be deleted.");
                result = dataEx;
            }
            else
            {
                switch (defaultNetStepsExceptionType)
                {
                    case Constants.NetStepsExceptionType.NetStepsDataException:
                        result = new NetStepsDataException(ex);
                        break;
                    case Constants.NetStepsExceptionType.NetStepsBusinessException:
                        result = new NetStepsBusinessException(ex);
                        break;
                    case Constants.NetStepsExceptionType.NetStepsEntityFrameworkException:
                        result = new NetStepsEntityFrameworkException(ex);
                        break;
                    case Constants.NetStepsExceptionType.NetStepsPaymentGatewayException:
                        result = new NetStepsPaymentGatewayException(ex);
                        break;
                    case Constants.NetStepsExceptionType.NetStepsApplicationException:
                        result = new NetStepsApplicationException(ex);
                        break;
                    case Constants.NetStepsExceptionType.NetStepsOptimisticConcurrencyException:
                        result = new NetStepsOptimisticConcurrencyException(ex);
                        break;
                    case Constants.NetStepsExceptionType.TaxesNotFoundForAddressException:
                		result = new TaxesNotFoundForAddressException(ex)
                			{
                				PublicMessage =
                					Translation.GetTerm(
                						"Global_TaxLookupFail",
                						"The tax area lookup failed.  This may mean that you have an invalid shipping address.")
                			};
                		break;
                    default:
                        result = new NetStepsException(ex);
                        break;
                }

                SetOrderAndAccount(result, orderID, accountID, internalMessage);                
            }
            return result;
        }

        private static bool ShouldLogException(Exception ex, Exception realException)
        {
            Contract.Requires<ArgumentNullException>(ex != null);
            Contract.Requires<ArgumentNullException>(realException != null);

            var result = !realException.Message.ContainsIgnoreCase("Castle.Core")
                && !ex.Message.Contains("The underlying provider failed on Open.");

            return result;
        }
               
        private static void SetOrderAndAccount(NetStepsException ex, int? orderID = null, int? accountID = null, string internalMessage = null)
        {
            try
            {
                if (ex != null)
                {
                    if (orderID.ToInt() > 0)
                        ex.OrderID = orderID;
                    if (accountID.ToInt() > 0)
                        ex.AccountID = accountID;
                    if (!internalMessage.IsNullOrEmpty())
                        ex.InternalMessage = internalMessage;

                    #region Attempt to set IDs from session if not set with params
                    try
                    {
                        if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Session != null)
                        {
                            if (!ex.OrderID.HasValue)
                            {
                                object party = System.Web.HttpContext.Current.Session["CurrentParty"];
                                object order = System.Web.HttpContext.Current.Session["CurrentOrder"];

                                if (party != null && party is Party && ((Party)party).OrderID > 0)
                                {
                                    ex.OrderID = ((Party)party).OrderID;
                                }
                                else if (order != null && order is Order && ((Order)order).OrderID > 0)
                                {
                                    ex.OrderID = ((Order)order).OrderID;
                                }
                            }

                            if (!ex.AccountID.HasValue)
                            {
                                object account = System.Web.HttpContext.Current.Session["CurrentAccount"];

                                if (account != null && account is Account && ((Account)account).AccountID > 0)
                                {
                                    ex.AccountID = ((Account)account).AccountID;
                                }
                            }
                        }
                    }
                    catch (Exception) { }
                    #endregion
                }
            }
            catch (Exception newEx)
            {
                if (ApplicationContext.Instance.IsDebug)
                    throw newEx;
            }
        }

    }
}
