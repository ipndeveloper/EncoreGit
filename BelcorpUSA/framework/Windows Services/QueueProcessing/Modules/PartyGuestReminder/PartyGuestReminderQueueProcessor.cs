using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Configuration;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.QueueProcessing.Modules.ModuleBase;

namespace NetSteps.QueueProcessing.Modules.PartyGuestReminder
{
    public class PartyGuestReminderQueueProcessor : QueueProcessor<IEnumerable<Party>>
    {
        private int _isZero;
        private const int ZERO = 0;
        private readonly Object _locker = new Object();

        public static readonly string CProcessorName = "PartyGuestReminderQueueProcessor";

        public PartyGuestReminderQueueProcessor()
        {
            Name = CProcessorName;
        }

        public override void CreateQueueItems(int maxNumberToPoll)
        {
            try
            {
                lock (_locker)
                {
                    if (_isZero == ZERO)
                    {
                        _isZero += 1;

                        Logger.Info("PartyGuestReminderQueueProcessor - CreateQueueItems");

                        //set the 4(app.config entry) days back date so that parties info can be fetched less than this date
                        var reminderIntervalHours = ConfigurationManager.GetAppSetting<int>(ConfigurationManager.VariableKey.PartyGuestReminderProcessor_ReminderInterval, 4);
                        var reminderIntervalRunDate = Convert.ToDateTime(DateTime.Now.Date.AddDays(reminderIntervalHours * -1));

                        var partiesList = new List<Party>();

                        //get the list of parties to send the reminder
                        var newPartiesList = Party.GetHostedPartiesByDate(reminderIntervalRunDate);

                        foreach (var party in newPartiesList)
                        {
                            using (NetStepsEntities context = new NetStepsEntities())
                            {
                                //filter the party records who are not there in eventContext table
                                //var eventContext = context.EventContexts.FirstOrDefault(ec => ec.PartyID == party.PartyID);
                                var eventContextExists = context.EventContexts.Any(ec => ec.PartyID == party.PartyID);
                                if (!eventContextExists)
                                {
                                    partiesList.Add(party);
                                }
                            }
                        }

                        if (partiesList.Count > 0)
                            EnqueueItem(partiesList);
                        else
                            _isZero = ZERO;

                        Logger.Info("PartyGuestReminderQueueProcessor - Enqueued {0} Items", partiesList.Count);
                    }
                }
            }
            catch (Exception ex)
            {
                _isZero = ZERO;
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }

        }

        public override void ProcessQueueItem(IEnumerable<Party> party)
        {
            Party.SendReminderToPartyGuests(party);
            _isZero = ZERO;
        }
    }
}

