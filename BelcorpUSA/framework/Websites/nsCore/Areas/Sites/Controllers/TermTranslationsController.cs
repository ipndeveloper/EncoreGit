using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LumenWorks.Framework.IO.Csv;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Utility;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.ActionResults;
using NetSteps.Web.Mvc.Attributes;
using nsCore.Models;
using nsCore.Areas.Sites.Models;

namespace nsCore.Areas.Sites.Controllers
{
	public class TermTranslationsController : BaseSitesController
	{
		/// <summary>
		/// Show the terms to translate
		/// </summary>
		/// <returns></returns>
		[FunctionFilter("Sites-Term Translations", "~/Sites/Overview")]
		public virtual ActionResult Index()
		{
			return View(new TermTranslationsIndexModel()
			{
				EnglishTermCount = TermTranslation.Repository.Count(t => t.LanguageID == Language.English.LanguageID)
			});
		}

		public enum TermType
		{
			OutOfDate,
			Untranslated
		}

		/// <summary>
		/// Get terms by page and language
		/// </summary>
		/// <param name="page">The current page</param>
		/// <param name="pageSize">The size of the current page</param>
		/// <param name="languageId">The id of the language to show terms for</param>
		/// <param name="type">If specified, determine whether to show out of date terms or untranslated terms</param>
		/// <param name="term">Possibly a part of the term name, English term, or local term to search for</param>
		/// <returns></returns>
		public virtual ActionResult Get(int? page, int? pageSize, int languageId, TermType? type, string term)
		{
			try
			{
				var dbTerms = TermTranslation.Repository
					.Where(t => t.Active && (t.LanguageID == (int)Constants.Language.English || t.LanguageID == languageId))
					.ToArray();

				IEnumerable<Term> terms = dbTerms
					.Where(t => t.LanguageID == Language.English.LanguageID)
					.Select(t =>
					{
						var localizedLastUpdate = t.LastUpdated;
						var localizedTermId = t.TermTranslationID;
						var localizedTermValue = t.Term;
						var localizedLangId = t.LanguageID;
						if (languageId != Language.English.LanguageID)
						{
							var localizedTerm = dbTerms.Where(lt => lt.LanguageID == languageId && lt.TermName.Trim() == t.TermName.Trim()).FirstOrDefault();
							if (localizedTerm != default(TermTranslation))
							{
								localizedTermId = localizedTerm.TermTranslationID;
								localizedLastUpdate = localizedTerm.LastUpdated;
								localizedTermValue = localizedTerm.Term;
								localizedLangId = localizedTerm.LanguageID;
							}
							else
							{
								localizedTermId = 0;
								localizedLastUpdate = null;
								localizedTermValue = String.Empty;
								localizedLangId = languageId;
							}
						}
						return new Term()
						{
							TermId = localizedTermId,
							TermName = t.TermName.Trim(),
							EnglishTerm = t.Term.Trim(),
							LocalTerm = localizedTermValue,
							IsOutOfDate = (languageId != Language.English.LanguageID) && (t.LastUpdated > localizedLastUpdate),
							LanguageId = localizedLangId,
							LastUpdated = localizedLastUpdate
						};
					});

				// Apply filters if any...
				terms = FilterTerms(terms, type, term).OrderBy(tt => tt.TermName);
				double totalPages = 0;
				if (pageSize.HasValue)
				{
					totalPages = Math.Ceiling(terms.Count() / pageSize.Value.ToDouble());
				}
				int maxPage = pageSize.HasValue ? (terms.Count() / pageSize.Value) - 1 : 0;
				if (page.HasValue && pageSize.HasValue)
					terms = terms.Skip(page.Value * pageSize.Value).Take(pageSize.Value);

				// Return the html of all of the terms
				if (terms.Count() > 0)
				{
					return Json(new
					{
						totalPages = totalPages,
						maxPage = maxPage,
						terms = RenderRazorPartialViewToString("_TermList", terms)
					});
				}

				return Json(new { totalPages = 0, maxPage = 0, terms = "<tr><td colspan=\"3\">There are no terms that match that criteria</td></tr>" });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		/// <summary>
		/// Apply filters to the terms if any.
		/// NOTE: Any changes to this must be verified by the existing unit-test - TK
		/// </summary>
		/// <param name="terms">Terms to apply filter to</param>
		/// <returns>filterd list of Terms</returns>
		[NonAction]
		public virtual IEnumerable<Term> FilterTerms(IEnumerable<Term> terms, TermType? type, string term)
		{
			// Filter out all of the terms that don't match the term we're searching for
			if (!string.IsNullOrWhiteSpace(term))
				terms = terms.Where(t => t.TermName.ContainsIgnoreCase(term) || t.EnglishTerm.ContainsIgnoreCase(term) || t.LocalTerm.ContainsIgnoreCase(term)).ToList();

			// Only show untranslated or out of date terms if specified
			if (type.HasValue)
			{
				switch (type.Value)
				{
					case TermType.OutOfDate:
						return terms.Where(t => t.IsOutOfDate);
					case TermType.Untranslated:
						return terms.Where(t => string.IsNullOrEmpty(t.LocalTerm));
				}
			}
			return terms;
		}


		/// <summary>
		/// Save all of the modified terms from the UI
		/// </summary>
		/// <param name="languageId">The id of the language we are editing</param>
		/// <param name="terms">The term name and the local term</param>
		/// <returns></returns>
		[FunctionFilter("Sites-Term Translations", "~/Sites/Overview")]
		[ValidateInput(false)]
		public virtual ActionResult Save(int languageId, List<Term> terms)
		{
			try
			{
				if (terms != null)
				{
					foreach (Term term in terms)
					{
						TermTranslation translation = TermTranslation.Repository.FirstOrDefault(t => t.TermTranslationID == term.TermId);
						// If the term doesn't exist, create it
						if (translation == default(TermTranslation))
						{
							//Double check that one doesn't already exist...
							var termName = term.TermName.Trim();
							translation = TermTranslation.Repository.FirstOrDefault(t => t.LanguageID == languageId && t.Term == termName);
							if (translation == default(TermTranslation))
							{
								translation = new TermTranslation()
								{
									LanguageID = languageId,
									TermName = termName,
									Term = (term.LocalTerm ==null)? "":term.LocalTerm.Trim(),
									Active = true,
									LastUpdatedUTC = DateTime.UtcNow
								};
							}
							else
							{
								//Found another... just modify it.
								translation.StartEntityTracking();
                                translation.Term = (term.LocalTerm == null) ? "" : term.LocalTerm.Trim();
								translation.Active = true;
								translation.LastUpdatedUTC = DateTime.UtcNow;
							}
							translation.Save();
						}
						else if (translation.Term != term.LocalTerm)
						{
							translation.StartEntityTracking();
                            translation.Term = (term.LocalTerm == null) ? "" : term.LocalTerm.Trim();
							translation.LastUpdatedUTC = DateTime.UtcNow;
							translation.Save();
						}
					}
				}
				return Json(new { result = true });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		/// <summary>
		///	Export the terms to Excel via a .csv file
		///	Now allows exporting of only filtered items as well.
		/// </summary>
		/// <param name="languageId">The id of the language to export</param>
		/// <returns></returns>
		[FunctionFilter("Sites-Term Translations", "~/Sites/Overview")]
		public virtual ActionResult Export(int languageId, TermType? type, string term)
		{
			try
			{
				var langIds = new int[] { languageId, (int)Constants.Language.English };
				var allTerms = TermTranslation.Repository.Where(t => langIds.Contains(t.LanguageID));
				var engTermGroups = allTerms.Where(t => t.LanguageID == (int)Constants.Language.English).GroupBy(et => et.TermName);
				var localizedTermGroups = allTerms.Where(lt => lt.LanguageID == languageId).GroupBy(lt => lt.TermName);

				//Export only the most recent (and active, if) to control duplication
				List<TermTranslation> engTerms = new List<TermTranslation>();
				engTermGroups.Each(eg =>
				{
					engTerms.Add(eg.OrderByDescending(e => e.Active).ThenByDescending(e => e.LastUpdatedUTC).First());
				});

				List<TermTranslation> locTerms = new List<TermTranslation>();
				localizedTermGroups.Each(lg =>
				{
					locTerms.Add(lg.OrderByDescending(e => e.Active).ThenByDescending(e => e.LastUpdatedUTC).First());
				});

				var terms = engTerms.Select(et =>
				{
					var localTerm = locTerms.FirstOrDefault(t => t.LanguageID == languageId && t.TermName.Trim() == et.TermName.Trim());
					return new Term
					{
						TermName = et.TermName.Trim(),
						EnglishTerm = et.Term.Trim(),
						LocalTerm = localTerm == default(TermTranslation) ? null : localTerm.Term.Trim(),
						LanguageId = languageId,
						LastUpdated = localTerm == default(TermTranslation) ? (DateTime?)null : localTerm.LastUpdated
					};
				});

				// Apply filters if any...
				terms = FilterTerms(terms, type, term).OrderBy(t => t.TermName);

				string langName = TermTranslation.GetLanguages(NetSteps.Data.Entities.Constants.Language.English.ToInt())
					.First(l => l.LanguageID == languageId).Name;

				// Build out the CSV
				var orderedProps = new[] { "LanguageId", "TermName", "LastUpdated", "LocalTerm", "EnglishTerm" }; //this is the expected format for import/export of terms. 
				return new CSVResult<Term>("Terms-" + langName + ".csv", terms, orderedProps);
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				throw exception;
			}
		}

		/// <summary>
		/// Import terms from a .csv file
		/// </summary>
		/// <returns></returns>
		[FunctionFilter("Sites-Term Translations", "~/Sites/Overview")]
		public virtual ActionResult Import()
		{
			HttpPostedFileBase termFile;
			try
			{
				// Make sure there is a file and that it's a CSV
				if (Request.Files.Count == 0)
					throw EntityExceptionHelper.GetAndLogNetStepsException("No file uploaded");
				termFile = Request.Files[0];
				if (!termFile.FileName.EndsWith(".csv"))
					throw EntityExceptionHelper.GetAndLogNetStepsException("The file must be a CSV");
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Content(new { result = false, message = exception.PublicMessage }.ToJSON());
			}

			Dictionary<Term, TermTranslation> outOfDateTerms = new Dictionary<Term, TermTranslation>();
			List<Term> importedTerms = new List<Term>();

			try
			{
				using (CsvReader reader = new CsvReader(new StreamReader(termFile.InputStream), true))
				{
					reader.SkipEmptyLines = true;
					reader.SupportsMultiline = true;
					while (reader.ReadNextRecord())
					{
						DateTime lastUpdated;
						bool validDate = DateTime.TryParse(reader[2], out lastUpdated);

						importedTerms.Add(new Term
						{
							LanguageId = int.Parse(reader[0]),
							TermName = reader[1].Trim(),
							LastUpdated = validDate ? lastUpdated : (DateTime?)null,
							LocalTerm = reader[3].Trim(),
							EnglishTerm = reader[4].Trim()
						});
					}
				}

				if (importedTerms.Any())
				{
					List<int> importedLangIds = importedTerms.Select(t => t.LanguageId).Distinct().ToList();
					if (!importedLangIds.Contains((int)Constants.Language.English))
					{
						importedLangIds.Add((int)Constants.Language.English);
					}
					List<string> importedTermNames = importedTerms.Select(t => t.TermName).Distinct().ToList();

					using (var context = new NetStepsEntities())
					{
						var existingTerms = context.TermTranslations.Where(tt => importedLangIds.Contains(tt.LanguageID) && importedTermNames.Contains(tt.TermName)).ToArray();
						foreach (var importedTerm in importedTerms)
						{
							TermTranslation translation = default(TermTranslation);
							var existing = existingTerms.Where(tt => tt.TermName == importedTerm.TermName && tt.LanguageID == importedTerm.LanguageId).ToArray();
							if (existing != null && existing.Any())
							{
								if (existing.Count() == 1)
								{
									translation = existing.First();
								}
								else
								{
									//Grab the most recent active from the duplicated values...
									translation = existing.OrderByDescending(t => t.Active).ThenByDescending(tt => tt.LastUpdatedUTC).First();
									//Deactivate the rest...
									existing.Where(t => t.TermTranslationID != translation.TermTranslationID).Each(t => t.Active = false);
								}
								existing = null;
							}

							// If the term exists, make sure the term is different from the file, and then check if it is out of date with the database (whether the term has been modified since the file was exported)
							if (translation != default(TermTranslation))
							{
								if (importedTerm.LocalTerm != translation.Term
									&& importedTerm.LastUpdated.HasValue)
								{
									if (!importedTerm.LastUpdated.Value.IsEqualUpToSecond(translation.LastUpdated.Value))
									{
										importedTerm.TermId = translation.TermTranslationID;
										outOfDateTerms.Add(importedTerm, translation);
									}
									else
									{
										translation.Term = importedTerm.LocalTerm;
										translation.LastUpdated = DateTime.Now;
									}
								}
							}
							else
							{
								if (!string.IsNullOrEmpty(importedTerm.LocalTerm))
								{
									// Insert the translation
									context.TermTranslations.AddObject(new TermTranslation()
									{
										Active = true,
										LanguageID = importedTerm.LanguageId,
										LastUpdated = DateTime.Now,
										Term = importedTerm.LocalTerm,
										TermName = importedTerm.TermName
									});
									if (importedTerm.LanguageId != Language.English.LanguageID
										&& !existingTerms.Any(tt => tt.LanguageID == (int)Constants.Language.English && tt.TermName == importedTerm.TermName))
									{
										context.TermTranslations.AddObject(new TermTranslation()
										{
											Active = true,
											LanguageID = Language.English.LanguageID,
											LastUpdated = DateTime.Now,
											Term = importedTerm.EnglishTerm,
											TermName = importedTerm.TermName
										});
									}
								}
							}
						}
						context.SaveChanges();
					}
				}
				TempData["OutOfDateTerms"] = outOfDateTerms;
				return Content(new { result = true, anyOutOfDateTerms = outOfDateTerms.Count > 0 }.ToJSON());
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Content(new { result = false, message = exception.PublicMessage }.ToJSON());
			}
		}

		/// <summary>
		/// Show the merge resolution tool for out of date terms
		/// </summary>
		/// <returns></returns>
		[FunctionFilter("Sites-Term Translations", "~/Sites/Overview")]
		public virtual ActionResult Merge()
		{
			return PartialView("MergeTerms", TempData["OutOfDateTerms"] as Dictionary<Term, TermTranslation>);
		}

		/// <summary>
		/// Choose which terms to use: the one from the file, or the one from the db
		/// </summary>
		/// <param name="languageId">The id of the language to resolve terms for</param>
		/// <param name="terms">The term name and the term that the user chose</param>
		[FunctionFilter("Sites-Term Translations", "~/Sites/Overview")]
		public virtual void Resolve(int languageId, List<Term> terms)
		{
			foreach (Term term in terms)
			{
				TermTranslation translation = TermTranslation.Repository.FirstOrDefault(t => t.TermTranslationID == term.TermId);
				if (translation != null)
				{
					translation.Term = term.LocalTerm.Trim();
					translation.LastUpdated = DateTime.Now;
					translation.Save();
				}
			}
		}

		/// <summary>
		/// Save all of the modified terms from the UI
		/// </summary>
		/// <param name="languageId">The id of the language we are editing</param>
		/// <param name="terms">The term name and the local term</param>
		/// <returns></returns>
		[FunctionFilter("Sites-Term Translations", "~/Sites/Overview")]
		public virtual ActionResult GenerateDefaultTerms()
		{
			try
			{
				DataBaseHelper.CreateDefaultTermsForEntities();
				return Json(new { result = true });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}
	}
}
