using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Common.Exceptions;
using NetSteps.Common.Extensions;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class NoteRepository
    {
        #region Methods
        public List<Note> LoadByOrderNumber(string orderNumber)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    List<Note> notes = null;
                    var order = (from o in context.Orders
                                               .Include("Notes")
                                 where o.OrderNumber == orderNumber
                                 select o).FirstOrDefault();
                    if (order != null)
                        notes = order.Notes.ToList();

                    if (notes == null)
                        throw new NetStepsDataException("Error loading notes. Invalid orderNumber: " + orderNumber);
                    else
                        return notes;
                }
            });
        }

        public List<Note> LoadByOrderID(int orderID)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    List<Note> notes = null;
                    var order = (from o in context.Orders
                                               .Include("Notes")
                                 where o.OrderID == orderID
                                 select o).FirstOrDefault();
                    if (order != null)
                        notes = order.Notes.ToList();

                    if (notes == null)
                        throw new NetStepsDataException("Error loading notes. Invalid orderID: " + orderID);
                    else
                        return notes;
                }
            });
        }

        public List<Note> LoadByAccountNumber(string accountNumber)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    List<Note> notes = null;
                    var account = (from o in context.Accounts
                                               .Include("Notes")
                                   where o.AccountNumber == accountNumber
                                   select o).FirstOrDefault();
                    if (account != null)
                        notes = account.Notes.ToList();

                    if (notes == null)
                        throw new NetStepsDataException("Error loading notes. Invalid accountNumber: " + accountNumber);
                    else
                        return notes;
                }
            });
        }

        public List<Note> LoadByAccountID(int accountID)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    List<Note> notes = null;
                    var account = (from o in context.Accounts
                                               .Include("Notes")
                                   where o.AccountID == accountID
                                   select o).FirstOrDefault();

                    if (account != null)
                        notes = account.Notes.ToList();

                    if (notes == null)
                        throw new NetStepsDataException("Error loading notes. Invalid accountID: " + accountID);
                    else
                        return notes;
                }
            });
        }

        public List<Note> LoadBySupportTicketID(int supportTicketID)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    List<Note> notes = null;
                    var supportTicket = (from o in context.SupportTickets
                                               .Include("Notes")
                                         where o.SupportTicketID == supportTicketID
                                         select o).FirstOrDefault();

                    if (supportTicket != null)
                        notes = supportTicket.Notes.ToList();

                    if (notes == null)
                        throw new NetStepsDataException("Error loading notes. Invalid supportTicketID: " + supportTicketID);
                    else
                        return notes;
                }
            });
        }


        public PaginatedList<NoteSearchData> Search(NoteSearchParameters searchParameters)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    PaginatedList<NoteSearchData> results = new PaginatedList<NoteSearchData>(searchParameters);

                    IQueryable<Note> matchingItems = context.Notes.AsQueryable<Note>();

                    if (searchParameters.AccountID.HasValue)
                        matchingItems = matchingItems.Where(n => n.Accounts.Any(a => a.AccountID == searchParameters.AccountID));

                    if (searchParameters.OrderID.HasValue)
                        matchingItems = matchingItems.Where(n => n.Orders.Any(o => o.OrderID == searchParameters.OrderID));

                    if (searchParameters.UserID.HasValue)
                        matchingItems = matchingItems.Where(n => n.UserID == searchParameters.UserID);

                    if (!string.IsNullOrEmpty(searchParameters.SearchText))
                        matchingItems = matchingItems.Where(o => o.Subject.Contains(searchParameters.SearchText) || o.NoteText.Contains(searchParameters.SearchText));

                    if (searchParameters.NoteTypeID.HasValue)
                        matchingItems = matchingItems.Where(o => o.NoteTypeID == searchParameters.NoteTypeID);

                    if (searchParameters.StartDate.HasValue)
                    {
                        DateTime startDateUTC = searchParameters.StartDate.Value.StartOfDay().LocalToUTC();
                        matchingItems = matchingItems.Where(a => a.DateCreatedUTC >= startDateUTC);
                    }
                    if (searchParameters.EndDate.HasValue)
                    {
                        DateTime endDateUTC = searchParameters.EndDate.Value.EndOfDay().LocalToUTC();
                        matchingItems = matchingItems.Where(a => a.DateCreatedUTC <= endDateUTC);
                    }

                    if (searchParameters.WhereClause != null)
                        matchingItems = matchingItems.Where(searchParameters.WhereClause);

                    if (!searchParameters.OrderBy.IsNullOrEmpty())
                    {
                        switch (searchParameters.OrderBy)
                        {
                            default:
                                matchingItems = matchingItems.ApplyOrderByFilter(searchParameters, context);
                                break;
                        }
                    }
                    else
                        matchingItems = matchingItems.OrderByDescending(o => o.DateCreatedUTC);

                    results.TotalCount = matchingItems.Count();

                    // Apply Paging filter - JHE
                    if (searchParameters.PageSize.HasValue)
                        matchingItems = matchingItems.Skip(searchParameters.PageIndex * searchParameters.PageSize.Value).Take(searchParameters.PageSize.Value);

                    var notes = matchingItems.Select(n => new
                                      {
                                          n.NoteID,
                                          n.NoteTypeID,
                                          n.DateCreatedUTC,
                                          n.Subject,
                                          n.NoteText,
                                          n.UserID,
                                          n.User.Username
                                      }).ToList();

                    results.AddRange(notes.Select(n => new NoteSearchData()
                    {
                        NoteID = n.NoteID,
                        DateCreated = n.DateCreatedUTC.UTCToLocal(),
                        NoteTypeID = n.NoteTypeID,
                        NoteType = SmallCollectionCache.Instance.NoteTypes.GetById(n.NoteTypeID).GetTerm(),
                        UserID = n.UserID,
                        Username = n.Username,
                        Subject = n.Subject,
                        NoteText = n.NoteText,
                    }));

                    return results;
                }
            });
        }
        #endregion
    }
}
