using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities
{
	public partial class Note
	{
		#region Properties

		public bool Show { get; set; }

		public UserSlimSearchData UserInfo
		{
			get
			{
				return UserID.HasValue ? CachedData.GetUser(UserID.Value) : null;
			}
		}

		// Read-only list of note tree; set by parent of note collection. Ex: Account.LoadFull() - JHE
		public IEnumerable<Note> _followupNotes = null;
		public IEnumerable<Note> FollowupNotes
		{
			get
			{
				return _followupNotes;
			}
			internal set
			{
				_followupNotes = value;
			}
		}

		#endregion

		#region Methods

		public static List<Note> FilterNotes(DateTime startDate, DateTime endDate, string searchCriteria, IList<Note> notes)
		{
			var filteredNotes = new List<Note>();
			endDate = endDate.AddDays(1);
			searchCriteria = searchCriteria.Trim();

			foreach (Note note in notes)
			{
				if(ShouldShowNote(startDate, endDate, searchCriteria, note))
				{
					filteredNotes.Add(note);
				}
				else if (note.FollowupNotes != null && note.FollowupNotes.Any())
				{
					ShouldShowNoteThread(startDate, endDate, searchCriteria, note, note.FollowupNotes);
					if(note.Show)
					{
						filteredNotes.Add(note);
					}
				}
			}

			return filteredNotes;
		}

		private static bool ShouldShowNote(DateTime startDate, DateTime endDate, string searchCriteria, Note note)
		{
			DateTime noteDateCreated = note.DateCreated.Date;

			return noteDateCreated < endDate && noteDateCreated >= startDate && (note.Subject.ContainsIgnoreCase(searchCriteria) || note.NoteText.ContainsIgnoreCase(searchCriteria));
		}

		private static bool ShouldShowNoteThread(DateTime startDate, DateTime endDate, string searchCriteria, Note owner, IEnumerable<Note> followup)
		{
			if (!owner.Show)
			{
				foreach (Note child in followup)
				{
					child.DateCreated = child.DateCreated.Date;
					if (child.DateCreated < endDate && child.DateCreated >= startDate && child.Subject.ContainsIgnoreCase(searchCriteria))
					{
						owner.Show = true;
						return true;
					}
					if (child.FollowupNotes.Any())
					{
						if (ShouldShowNoteThread(startDate, endDate, searchCriteria, owner, child.FollowupNotes))
						{
							owner.Show = true;
							return true;
						}
					}
				}
			}
			return owner.Show;
		}


		public static List<Note> LoadByOrderNumber(string orderNumber)
		{
			try
			{
				return BusinessLogic.LoadByOrderNumber(Repository, orderNumber);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}
		public static List<Note> LoadByOrderID(int orderID)
		{
			try
			{
				return BusinessLogic.LoadByOrderID(Repository, orderID);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static List<Note> LoadByAccountNumber(string accountNumber)
		{
			try
			{
				return BusinessLogic.LoadByAccountNumber(Repository, accountNumber);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}
		public static List<Note> LoadByAccountID(int accountID)
		{
			try
			{
				return BusinessLogic.LoadByAccountID(Repository, accountID);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}
		public static List<Note> LoadBySupportTicketID(int supportTicketID)
		{
			try
			{
				return BusinessLogic.LoadBySupportTicketID(Repository, supportTicketID);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}


		public static PaginatedList<NoteSearchData> Search(NoteSearchParameters searchParameters)
		{
			try
			{
				return Repository.Search(searchParameters);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		#endregion
	}
}