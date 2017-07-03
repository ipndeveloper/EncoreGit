using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Hurricane.Plugin
{
    // Copied from NetSteps.Data.Entities.Mail because we can't reference .NET 4.0
    
    public enum EmailAddressType
    {
        TO = 0,
        CC = 1,
        BCC = 2
    }

    public enum MailMessageRecipientType
    {
        Individual = 0,
        Group = 1
    }

    public enum EmailMessageType
    {
        Campaign = 0,
        AdHoc = 1,
        Downline = 2
    }

    public enum MailFolderType
    {
        Inbox = 0,
        Trash = 1,
        SentItems = 2,
        Drafts = 3,
        Outbox = 4,
        Undeliverable = 5
    }

    public enum EmailRecipientStatus
    {
        Unknown = 0,
        OptedOut = 1,
        Delivered = 2,
        DeliveryError = 3,
        InvalidAddress = 4
    }

    public enum MailMessageRecipientEventType
    {
        MessageOpened = 0,
        LinkClicked = 1,
        MessageBounced = 2
    }
}
