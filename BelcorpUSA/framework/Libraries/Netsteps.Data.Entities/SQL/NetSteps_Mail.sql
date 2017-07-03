Use Zrii_Staging_Core_Sept_21
go
-- =============================================
-- Author:		Larry Hill
-- Create date: 2009-08-28
-- Description:	Evaluate and return the enrollment status for the passed account id.
-- =============================================
CREATE PROCEDURE [dbo].[usp_account_enrollment]
	-- Add the parameters for the stored procedure here
	@AccountID INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT CASE WHEN (SELECT COUNT(*) FROM OrderCustomers WHERE AccountID=Accounts.AccountID) = 0 THEN 0
				ELSE 1 END EnrollmentStatus
	FROM Accounts WHERE AccountID = @AccountID
END
GO

Alter Table OptOut Alter Column EmailAddress nvarchar(255)
go
Create Index IDX_OptOut_EmailAddress On OptOut (EmailAddress)
go
Create Index IDX_MailAccount_EmailAddress On MailAccount (EmailAddress) Include (Active)
go



Use master
go
DROP DATABASE [Zrii_Mail]
GO

CREATE DATABASE Zrii_Mail ON  PRIMARY 
( NAME = N'Zrii_Mail', FILENAME = N'c:\SQLDatabases\Zrii_Mail.mdf' , SIZE = 2048KB , FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'Zrii_Mail_log', FILENAME = N'c:\SqlDatabases\Zrii_Mail_log.ldf' , SIZE = 1024KB , FILEGROWTH = 10%)
GO

Use Zrii_Mail
go
Set NoCount On
go


Create Synonym dbo.Accounts For Zrii_Staging_Core_Sept_21.dbo.Accounts
Create Synonym dbo.MailAccount For Zrii_Staging_Core_Sept_21.dbo.MailAccount
Create Synonym dbo.OptOut For Zrii_Staging_Core_Sept_21.dbo.OptOut
Create Synonym dbo.MailDomain For Zrii_Staging_Core_Sept_21.dbo.MailDomain
go

Create Table dbo.MailMessageTypes
(
  MailMessageTypeID smallint not null,
  Description nvarchar(25),
  
  CONSTRAINT PK_MailMessageTypes PRIMARY KEY CLUSTERED 
	(
		MailMessageTypeID ASC
	)
)
go
Insert Into MailMessageTypes (MailMessageTypeID, Description) Values (0, 'Campaign')
Insert Into MailMessageTypes (MailMessageTypeID, Description) Values (1, 'Adhoc')
Insert Into MailMessageTypes (MailMessageTypeID, Description) Values (2, 'Downline')
go


Create Table dbo.MailMessagePriority
(
  MailMessagePriorityID smallint not null,
  Description nvarchar(25),
  
  CONSTRAINT PK_MailMessagePriority PRIMARY KEY CLUSTERED 
	(
		MailMessagePriorityID ASC
	)
)
go
Insert Into MailMessagePriority (MailMessagePriorityID, Description) Values (0, 'Lowest')
Insert Into MailMessagePriority (MailMessagePriorityID, Description) Values (1, 'Low')
Insert Into MailMessagePriority (MailMessagePriorityID, Description) Values (2, 'Normal')
Insert Into MailMessagePriority (MailMessagePriorityID, Description) Values (3, 'High')
Insert Into MailMessagePriority (MailMessagePriorityID, Description) Values (4, 'Highest')
go

Create Table dbo.MailFolderTypes
(
  MailFolderTypeID smallint not null,
  Description nvarchar(25),
  
  CONSTRAINT PK_MailFolderTypes PRIMARY KEY CLUSTERED 
	(
		MailFolderTypeID ASC
	)
)
go
Insert Into MailFolderTypes (MailFolderTypeID, Description) Values (0, 'Inbox')
Insert Into MailFolderTypes (MailFolderTypeID, Description) Values (1, 'Trash')
Insert Into MailFolderTypes (MailFolderTypeID, Description) Values (2, 'Sent Items')
Insert Into MailFolderTypes (MailFolderTypeID, Description) Values (3, 'Drafts')
Insert Into MailFolderTypes (MailFolderTypeID, Description) Values (4, 'Outbox')
Insert Into MailFolderTypes (MailFolderTypeID, Description) Values (5, 'Undeliverable')
go


CREATE TABLE dbo.MailMessages
(
	MailMessageID int IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	Subject nvarchar(max) NULL,
	Body nvarchar(max) NULL,
	HTMLBody nvarchar(max) null,
	DateAddedUTC datetime NOT NULL CONSTRAINT DF_MailMessages_DateAddedUTC  DEFAULT (getdate()),
	FromAddress nvarchar(255) NULL,
	FromNickName nvarchar(255) NULL,
	MailAccountID int NULL,
	IsOutbound bit not null constraint DF_IsOutbound default 0,
	MailMessageTypeID smallint NOT NULL,
	BeenRead bit not null constraint DF_BeenRead default 0, 
	MailMessagePriorityID smallint not null,
  VisualTemplateID int null,
	MailFolderTypeID smallint not null,
  AttachmentUniqueID nvarchar(36) Null,
  Locked bit null,
  SiteID int null,
	
	 CONSTRAINT PK_MailMessages PRIMARY KEY CLUSTERED 
	(
		MailMessageID ASC
	)
) 
GO
Create Index IDX_MailMessages_MailAccountID_ On dbo.MailMessages(MailAccountID, MailFolderTypeID)
go
Create Index IDX_MailMessages_IsOutbound_MailFolderTypeID_Locked on dbo.MailMessages(IsOutbound, MailFolderTypeID, Locked) Include (MailMessageTypeID)
go

Create Table dbo.MessageGroupStatus
(
  MessageGroupStatusID smallint not null,
  Description nvarchar(25),
  
  CONSTRAINT PK_MessageGroupStatus PRIMARY KEY CLUSTERED 
	(
		MessageGroupStatusID ASC
	)
)
go

Insert Into MessageGroupStatus (MessageGroupStatusID, Description) Values (0, 'To be sent')
Insert Into MessageGroupStatus (MessageGroupStatusID, Description) Values (1, 'Intermediate Queue')
Insert Into MessageGroupStatus (MessageGroupStatusID, Description) Values (2, 'SMTP Relay Queue')
Insert Into MessageGroupStatus (MessageGroupStatusID, Description) Values (3, 'SMTP Relay Refused')
Insert Into MessageGroupStatus (MessageGroupStatusID, Description) Values (4, 'SMTP Sent')
Insert Into MessageGroupStatus (MessageGroupStatusID, Description) Values (5, 'Inbound Received')
go

Create Table dbo.AddressTypes
(
	AddressTypeID smallint not null,
	Description nvarchar(25)
  
	CONSTRAINT PK_AddressTypes PRIMARY KEY CLUSTERED 
	(
		AddressTypeID ASC
	)
)
go

Insert Into AddressTypes (AddressTypeID, Description) Values (0, 'TO')
Insert Into AddressTypes (AddressTypeID, Description) Values (1, 'CC')
Insert Into AddressTypes (AddressTypeID, Description) Values (2, 'BCC')
go

Create Table dbo.RecipientTypes
(
	RecipientTypeID smallint not null,
	Description nvarchar(25)
  
	CONSTRAINT PK_RecipientTypes PRIMARY KEY CLUSTERED 
	(
		RecipientTypeID ASC
	)
)
go

Insert Into RecipientTypes (RecipientTypeID, Description) Values (0, 'Individual')
Insert Into RecipientTypes (RecipientTypeID, Description) Values (1, 'Group')
go


Create Table dbo.RecipientStatus
(
	RecipientStatusID smallint not null,
	Description nvarchar(25)
  
	CONSTRAINT PK_RecipientStatus PRIMARY KEY CLUSTERED 
	(
		RecipientStatusID ASC
	)
)
go

Insert Into RecipientStatus (RecipientStatusID, Description) Values (0, 'Unknown')
Insert Into RecipientStatus (RecipientStatusID, Description) Values (1, 'Opted Out')
Insert Into RecipientStatus (RecipientStatusID, Description) Values (2, 'Delivered')
Insert Into RecipientStatus (RecipientStatusID, Description) Values (3, 'Delivery Error')
Insert Into RecipientStatus (RecipientStatusID, Description) Values (4, 'Invalid Address')
go

CREATE TABLE dbo.MailMessageGroups
(
	MailMessageGroupID int IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	MailMessageID int NOT NULL,
	MessageGroupStatusID smallint NULL,
	AttemptCount int NOT NULL,
	RetryTimeUTC datetime null,
	IsOriginal bit null,
	tmpAccountID int null,
	
  CONSTRAINT PK_MailMessageGroups PRIMARY KEY CLUSTERED 
	(
		MailMessageGroupID ASC
	)
) 
GO
Create NonClustered Index IDX_MailMessageGroups_MailMessageID_IsOriginal On MailMessageGroups(MailMessageID,IsOriginal)
go
Create NonClustered Index IDX_MailMessageGroups_MessageGroupStatusID_RetryTimeUTC On MailMessageGroups(MessageGroupStatusID, RetryTimeUTC) Include (AttemptCount, MailMessageID)
go

ALTER TABLE dbo.MailMessageGroups  WITH NOCHECK ADD  CONSTRAINT FK_MailMessageGroups_MailMessages FOREIGN KEY(MailMessageID)
REFERENCES dbo.MailMessages (MailMessageID)
GO



CREATE TABLE dbo.MailMessageGroupStatusAudit
(
	MailMessageGroupStatusAuditID int IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	MailMessageGroupID int NOT NULL,
	MessageGroupStatusID smallint NULL,
	AttemptCount int NOT NULL,
	RetryTimeUTC datetime null,
	DateAddedUTC datetime not null,
	
  CONSTRAINT PK_MailMessageGroupStatusAudit PRIMARY KEY CLUSTERED 
	(
		MailMessageGroupStatusAuditID ASC
	)
) 
GO
Create NonClustered Index IDX_MailMessageGroupStatusAudit_MailMessageGroupID On MailMessageGroupStatusAudit(MailMessageGroupID)
go
ALTER TABLE dbo.MailMessageGroupStatusAudit  WITH NOCHECK ADD  CONSTRAINT FK_MailMessageGroupStatusAudit_MailMessageGroupID FOREIGN KEY(MailMessageGroupID)
REFERENCES dbo.MailMessageGroups (MailMessageGroupID)
GO

-- =============================================
-- Author:		Shane Kelly
-- Create date: 10/17/2009
-- Description: Records changes to MailMessageGroups. A group goes through several
--              Status changes from creation to deliver. This gives us a history of 
--              how/when the message group progressed. This allows us to justify what occured
--              with any message group.
-- =============================================
Create Trigger TRG_MailMessageGroup_InsertUpdate
On MailMessageGroups
For Insert, Update
As
  SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
  SET NOCOUNT ON
  
  Insert Into dbo.MailMessageGroupStatusAudit (MailMessageGroupID,
	                                             MessageGroupStatusID,
	                                             AttemptCount,
	                                             RetryTimeUTC,
	                                             DateAddedUTC)
  Select MailMessageGroupID,
	       MessageGroupStatusID,
	       AttemptCount,
	       RetryTimeUTC,
	       DateAddedUTC = GetDate()
  From Inserted
go



CREATE TABLE dbo.MailMessageGroupAddresses
(
	MailMessageGroupAddressID int IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	MailMessageGroupID int NOT NULL,
	EmailAddress nvarchar(255) NOT NULL,
	NickName nvarchar(255) NULL,
	AddressTypeID smallint NULL,
	RecipientTypeID smallint null,
	RecipientStatusID smallint null,
	
  CONSTRAINT PK_MailMessageGroupAddresses PRIMARY KEY CLUSTERED 
	(
		MailMessageGroupAddressID ASC
	)
) 
GO
Create NonClustered Index IDX_MailMessageGroupAddresses_MailMessageGroupID On MailMessageGroupAddresses(MailMessageGroupID)
go
ALTER TABLE dbo.MailMessageGroupAddresses  WITH NOCHECK ADD  CONSTRAINT FK_MailMessageGroupAddresses_MailMessageGroups FOREIGN KEY(MailMessageGroupID)
REFERENCES dbo.MailMessageGroups (MailMessageGroupID)
GO




CREATE TABLE dbo.MailAttachments
(
	MailAttachmentID int IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	FileName nvarchar(255) NULL,
	Size int null,
		
	CONSTRAINT PK_MailAttachments PRIMARY KEY CLUSTERED 
	(
		MailAttachmentID ASC
	)
) 
GO

CREATE TABLE dbo.MailMessageAttachments
(
	MailMessageAttachmentID int IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	MailMessageID int NOT NULL,
	MailAttachmentID int NOT NULL,
	
	CONSTRAINT PK_MailMessageAttachments PRIMARY KEY CLUSTERED 
	(
		MailMessageAttachmentID ASC
	)
) 
GO
Create NonClustered Index IDX_MailMessageAttachments_MailMessageID On MailMessageAttachments(MailMessageID)
go
Create NonClustered Index IDX_MailMessageAttachments_MailAttachmentID On MailMessageAttachments(MailAttachmentID)
go

ALTER TABLE dbo.MailMessageAttachments  WITH NOCHECK ADD CONSTRAINT FK_MailMessageAttachments_MailAttachments FOREIGN KEY(MailAttachmentID)
REFERENCES dbo.MailAttachments (MailAttachmentID)
GO

ALTER TABLE dbo.MailMessageAttachments  WITH NOCHECK ADD  CONSTRAINT FK_MailMessageAttachments_MailMessages FOREIGN KEY(MailMessageID)
REFERENCES dbo.MailMessages (MailMessageID)
GO

Create TYPE dbo.TTAddressList AS TABLE
( 
	EmailAddress nvarchar(255) NOT NULL,
	NickName nvarchar(255) NULL,
	AddressTypeID smallint NULL,
	RecipientTypeID smallint null
)
go

Create TYPE dbo.TTAttachmentList AS TABLE
( 
	FileName nvarchar(255) NULL,
	Size int null
)
go

Create TYPE dbo.TTInternalAttachmentList AS TABLE
( 
  MailAttachmentID int null
)
go


Create TYPE dbo.TTAccountIDList AS TABLE
( 
	AccountID int null
)
go


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_MailMessageInsertUpdate]') AND type in (N'P', N'PC'))
  DROP PROCEDURE [dbo].usp_MailMessageInsertUpdate
GO

-- =============================================
-- Author:		Shane Kelly
-- Create date: 10/17/2009
-- Description: Used to insert new records into MailMessage Table. Note that it uses 2008 specific 
--              syntax to pass in table-value parameters.
-- =============================================
Create Procedure dbo.usp_MailMessageInsertUpdate
  @MailMessageID int, 
	@Subject nvarchar(max),
	@Body nvarchar(max),
	@HTMLBody nvarchar(max), 
	@DateAddedUTC datetime,
	@FromAddress nvarchar(255),
	@FromNickName nvarchar(255),
	@MailAccountID int,
	@IsOutbound bit,
	@MailMessageTypeID smallint,
	@BeenRead bit,
	@MailMessagePriorityID smallint,
	@VisualTemplateID int,
	@MailFolderTypeID smallint,
	@AttachmentUniqueID nvarchar(36),
	@AddExternalEmailAddresses bit,
	@SiteID int,
	
  @Addresses dbo.TTAddressList READONLY,
  @Attachments dbo.TTAttachmentList READONLY,
  @AccountIDs dbo.TTAccountIDList READONLY,

  @NewMailMessageID int OUTPUT
As
  SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
  SET NOCOUNT ON
  
  Declare @IsNew bit = 0
  
  -- if the MailMesasgeID = 0 then we insert, otherwise we update
  if IsNull(@MailMessageID, 0) = 0
    Set @IsNew = 1
    
  if (@IsNew = 1)
  begin
    Insert Into dbo.MailMessages (Subject, 
                                  Body, 
                                  HTMLBody,
                                  DateAddedUTC,
                                  FromAddress, 
                                  FromNickName, 
                                  MailAccountID, 
                                  IsOutbound,
                                  MailMessageTypeID,
                                  BeenRead,
                                  MailMessagePriorityID,
                                  VisualTemplateID,
                                  MailFolderTypeID,
                                  AttachmentUniqueID,
                                  Locked,
                                  SiteID) 
    Select Subject = @Subject, 
           Body = @Body, 
           HTMLBody = @HTMLBody, 
           DateAddedUTC = @DateAddedUTC,
           FromAddress = @FromAddress, 
           FromNickName = @FromNickName, 
           MailAccountID = @MailAccountID, 
           IsOutbound = @IsOutbound,
           MailMessageTypeID = @MailMessageTypeID,
           BeenRead = @BeenRead,
           MailMessagePriorityID = @MailMessagePriorityID,
           VisualTemplateID = @VisualTemplateID,
           MailFolderTypeID = @MailFolderTypeID,
           AttachmentUniqueID = @AttachmentUniqueID, 
           Locked = 1, -- prevents the poller from working with the message while were trying to save all the parts 
           SiteID = @SiteID
           
    Set @MailMessageID = SCOPE_IDENTITY()
	Set @NewMailMessageID = @MailMessageID
  end
  else
  begin
    Update dbo.MailMessages Set Subject = @Subject, 
                                Body = @Body, 
                                HTMLBody = @HTMLBody, 
                                DateAddedUTC = @DateAddedUTC,
                                FromAddress = @FromAddress, 
                                FromNickName = @FromNickName, 
                                MailAccountID = @MailAccountID, 
                                IsOutbound = @IsOutbound,
                                MailMessageTypeID = @MailMessageTypeID,
                                BeenRead = @BeenRead,
                                MailMessagePriorityID = @MailMessagePriorityID,
                                VisualTemplateID = @VisualTemplateID,
                                MailFolderTypeID = @MailFolderTypeID,
                                AttachmentUniqueID = @AttachmentUniqueID,
                                Locked = 1, -- prevents the poller from working with the message while were trying to save all the parts 
                                SiteID = @SiteID
    Where MailMessageID = @MailMessageID
  end
  
  /********************************************************
  ** Insert addresses into the "Original" MessageGroup  
  *********************************************************/
  
  -- if we are re-saving, there is no way to reconcile the addresses, because an address can appear more than once and the only
  -- unique id is the MailMessageGroupAddressID which isn't passed. So the best I can do is remove all the addresses and readd them
  -- this should be completly fine, since this should only have data if it was a draft and removing/readding should be harmless
  if (@IsNew = 0)
  begin
    Delete From a
    From dbo.MailMessageGroups g inner join dbo.MailMessageGroupAddresses a on a.MailMessageGroupID = g.MailMessageGroupID
    Where g.MailMessageID = @MailMessageID

    Delete From a
    From dbo.MailMessageGroups g inner join dbo.MailMessageGroupStatusAudit a on a.MailMessageGroupID = g.MailMessageGroupID
    Where g.MailMessageID = @MailMessageID
  
    Delete From g 
    From  dbo.MailMessageGroups g 
    Where g.MailMessageID = @MailMessageID
  end
  
  Declare @MailMessageGroupID  int
  
  Insert Into dbo.MailMessageGroups (MailMessageID, 
                                     MessageGroupStatusID, 
                                     AttemptCount, 
                                     RetryTimeUTC, 
                                     IsOriginal)
  Select MailMessageID = @MailMessageID, 
         MessageGroupStatusID = 0, -- to be sent 
         AttemptCount = 0, 
         RetryTimeUTC = null, 
         IsOriginal = 1
         
  Select @MailMessageGroupID = SCOPE_IDENTITY()

  /********************************************************
  ** Insert addresses into the "Original" MessageGroup  
  *********************************************************/
  Insert Into dbo.MailMessageGroupAddresses (MailMessageGroupID, 
                                             EmailAddress, 
                                             NickName, 
                                             AddressTypeID, 
                                             RecipientTypeID,
                                             RecipientStatusID)
  Select MailMessageGroupID = @MailMessageGroupID, 
         EmailAddress = a.EmailAddress, 
         NickName = a.NickName, 
         AddressTypeID = a.AddressTypeID, 
         RecipientTypeID = a.RecipientTypeID,
         RecipientStatusID = 0 -- status unknown
  From @Addresses as a


  /********************************************************
  ** Generate Address Groups for Accounts (i.e DOWNLINE RECEIPIENTS)
  ** If an "AccountList" is passed in it means that we need to add 
  **   Groups/Address for all the downline receipients. Note
  **   that these groups are NOT marked original
  *********************************************************/
  if (@MailFolderTypeID != 3) -- if this is NOT a draft
  begin
    -- This little mess is so that if the config file indicated that we are to send downline emails
    -- to both their internal and external email addresses, those addresses will be in the same group
    -- which means in the message the addresses will both be on the "to" line so they can see it went to both
    -- For most uses this option won't be marked so there will be a 1-to-1 relationship between group and addresses
    Select a.AccountID,
           a.EmailAddress
    Into #tmpEmailAddresses
    From (
              Select id.AccountID, 
                     a.EmailAddress 
              From @AccountIDs id Inner Join dbo.MailAccount a on a.AccountID = id.AccountID
              Where IsNull(a.EmailAddress, '') != ''
           UNION
             -- This part of the union only gets addresses if @AddExternalEmailAddresses = 1
             Select id.AccountID, 
                    a.EmailAddress 
              From @AccountIDs id Inner Join dbo.Accounts a on a.AccountID = id.AccountID
              Where @AddExternalEmailAddresses = 1
                And IsNull(a.EmailAddress, '') != ''
          ) a
  
  
    Declare @tmpGroups Table (MailMessageGroupID int, 
                              AccountID int)
                              
    Insert Into dbo.MailMessageGroups (MailMessageID, 
                                       MessageGroupStatusID, 
                                       AttemptCount, 
                                       RetryTimeUTC, 
                                       IsOriginal,
                                       tmpAccountID)
    Output Inserted.MailMessageGroupID,
           Inserted.tmpAccountID Into @tmpGroups(MailMessageGroupID,
                                                 AccountID)
    Select Distinct MailMessageID = @MailMessageID, 
                    MessageGroupStatusID = 0, -- to be sent 
                    AttemptCount = 0, 
                    RetryTimeUTC = null, 
                    IsOriginal = 0, -- these are not original groups
                    tmpAccountID = t.AccountID
    From #tmpEmailAddresses t
    
    Insert Into dbo.MailMessageGroupAddresses (MailMessageGroupID, 
                                               EmailAddress, 
                                               NickName, 
                                               AddressTypeID, 
                                               RecipientTypeID,
                                               RecipientStatusID)
    Select MailMessageGroupID = g.MailMessageGroupID, 
           EmailAddress = a.EmailAddress, 
           NickName = '', 
           AddressTypeID = 0, -- To (meaning not bcc or cc)
           RecipientTypeID = 0, -- individual
           RecipientStatusID = 0 -- unknown status
    From @tmpGroups g Inner join #tmpEmailAddresses a on a.AccountID = g.AccountID       
  end
  
  
  /********************************************************
  ** Save/Reconcile Attachments
  *********************************************************/
  -- When resaving a draft, or converting a draft to a real message, the FileAttachments aren't resent
  -- so it's important to leave the attachments there that should be and remove the others. Lucily
  -- the filename is also unique, so we can easily reconcile them.
  if (@IsNew = 0)
  begin
    -- Delete all attachments no longer in the list
    Delete From m
    From dbo.MailMessageAttachments m Inner Join dbo.MailAttachments ma on ma.MailAttachmentID = m.MailAttachmentID
                                      Left Outer Join @Attachments t on t.FileName = ma.FileName
    Where m.MailMessageID = @MailMessageID
      and t.FileName is null
    
    -- update the sizes where different
    Update ma Set Size = t.Size
    From dbo.MailMessageAttachments m Inner Join dbo.MailAttachments ma on ma.MailAttachmentID = m.MailAttachmentID
                                      Inner Join @Attachments t on t.FileName = ma.FileName
    Where m.MailMessageID = @MailMessageID
      and ma.Size != t.Size
    
    -- insert the missing filenames
    Insert Into dbo.MailAttachments (FileName,
                                     Size)
    Output @MailMessageID, 
           inserted.MailAttachmentID Into dbo.MailMessageAttachments (MailMessageID, 
                                                                      MailAttachmentID)
    Select a.FileName,
           a.Size
    From @Attachments a Left Outer Join (Select t.FileName
                                         From dbo.MailMessageAttachments ma inner join dbo.MailAttachments t on t.MailAttachmentID = ma.MailAttachmentID
                                         Where ma.MailMessageID = @MailMessageID) m On m.FileName = a.FileName
    Where m.FileName is null
  end
  else
  begin
    -- This uses the new Output option from 2008. It inserts into MailAttachments and then uses the results
    -- to insert into MailMessageAttachments building the relationship with the ID from MailAttachments....super cool
    Insert Into dbo.MailAttachments (FileName,
                                     Size)
    Output @MailMessageID, 
           inserted.MailAttachmentID Into dbo.MailMessageAttachments (MailMessageID, 
                                                                      MailAttachmentID)
    Select a.FileName,
           a.Size
    From @Attachments as a
  end
  
  /*******************************************************
  ** Unlock the message so the poller can work with it
  *********************************************************/
  Update dbo.MailMessages Set Locked = 0
  Where MailMessageID = @MailMessageID
go


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_MailMessageSelect]') AND type in (N'P', N'PC'))
  DROP PROCEDURE [dbo].usp_MailMessageSelect
GO

-- =============================================
-- Author:		Shane Kelly
-- Create date: 10/17/2009
-- Description: Retrievs a MailMessage. Note that it returns 3 resultsets. It only returns
--              The address list original to the email, no addresses added by downline expanstion are returned
--              Giving this the same data as the original message.
-- =============================================
Create Procedure dbo.usp_MailMessageSelect @MailMessageID int
As
  SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
  SET NOCOUNT ON

  Select m.MailMessageID,
         m.Subject, 
         Body = Isnull(m.Body, ''), 
         HTMLBody = IsNull(m.HTMLBody, ''), 
         m.DateAddedUTC,
         m.FromAddress, 
         m.FromNickName, 
         m.MailAccountID, 
         m.IsOutbound,
         m.MailMessageTypeID,
         m.BeenRead,
         m.MailMessagePriorityID,
         m.VisualTemplateID,
         m.MailFolderTypeID,
         m.AttachmentUniqueID 
  From dbo.MailMessages m
  Where m.MailMessageID = @MailMessageID
    and m.Locked = 0 -- don't allow selections of locked items, their currently being processxed
  
  -- When returning the address list we only return the addresses in the "original" group. Meaning, when a downline message is saved
  -- the addresses of all the downline accounts are added as non-original groups
  -- When the user retrieves the message we only want to return what was original when he composed it so he gets the same view
  -- of the world
  Select a.MailMessageGroupAddressID,
         a.MailMessageGroupID, 
         g.MailMessageID,
         EmailAddress = ISNULL(a.EmailAddress, ''),  
         NickName = ISNULL(a.NickName, ''), 
         a.AddressTypeID, 
         a.RecipientTypeID 
  From dbo.MailMessageGroups g inner join dbo.MailMessageGroupAddresses a on a.MailMessageGroupID = g.MailMessageGroupID
  Where g.MailMessageID = @MailMessageID
    and g.IsOriginal = 1
  
  Select a.MailMessageAttachmentID,
         a.MailMessageID,
         ma.FileName,
         ma.Size
  From dbo.MailMessageAttachments a inner join dbo.MailAttachments ma On ma.MailAttachmentID = a.MailAttachmentID
  Where a.MailMessageID = @MailMessageID
go


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_MailMessageMoveFolder]') AND type in (N'P', N'PC'))
  DROP PROCEDURE [dbo].usp_MailMessageMoveFolder
GO

-- =============================================
-- Author:		Shane Kelly
-- Create date: 10/17/2009
-- Description: Moves a message to a new folder
-- =============================================
Create Procedure dbo.usp_MailMessageMoveFolder @MailMessageID int,
                                               @MailFolderTypeID smallint
As
  SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
  SET NOCOUNT ON

  Update dbo.MailMessages Set MailFolderTypeID = @MailFolderTypeID
  Where MailMessageID = @MailMessageID
go


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_PurgeFolder]') AND type in (N'P', N'PC'))
  DROP PROCEDURE [dbo].usp_PurgeFolder
GO

-- =============================================
-- Author:		Shane Kelly
-- Create date: 10/17/2009
-- Description: Removes all the items from a folder. Deletes from child tables first
-- =============================================
Create Procedure dbo.usp_PurgeFolder @MailAccountID int,
                                     @MailFolderTypeID smallint
As
  SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
  SET NOCOUNT ON

  -- incase items are being inserted at the same time were deleteing, I capture
  -- the ids of the ones to delete first
  
  Select m.MailMessageID
  Into #MailMessageIDs
  From dbo.MailMessages m 
  Where m.MailAccountID = @MailAccountID
    and m.MailFolderTypeID = @MailFolderTypeID

  Delete From a
  From #MailMessageIDs i inner join dbo.MailMessageAttachments a on a.MailMessageID = i.MailMessageID

  Delete From a
  From #MailMessageIDs i inner join dbo.MailMessageGroups g on g.MailMessageID = i.MailMessageID
                         inner join dbo.MailMessageGroupAddresses a on a.MailMessageGroupID = g.MailMessageGroupID

  Delete From s
  From #MailMessageIDs i inner join dbo.MailMessageGroups g on g.MailMessageID = i.MailMessageID
                         inner join dbo.MailMessageGroupStatusAudit s on s.MailMessageGroupID = g.MailMessageGroupID

  Delete From g
  From #MailMessageIDs i inner join dbo.MailMessageGroups g on a.MailMessageID = i.MailMessageID

  Delete From m
  From #MailMessageIDs i inner join dbo.MailMessages m on m.MailMessageID = i.MailMessageID
go


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_DeleteMailMessage]') AND type in (N'P', N'PC'))
  DROP PROCEDURE [dbo].usp_DeleteMailMessage
GO

-- =============================================
-- Author:		Shane Kelly
-- Create date: 10/17/2009
-- Description: Moves a message to a new folder
-- =============================================
Create Procedure dbo.usp_DeleteMailMessage @MailMessageID int
As
  SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
  SET NOCOUNT ON

  -- if the message is in the trash folder it will permanently delete it
  -- if it is not, it will simply move it to the trash folder
  if Exists (Select * 
             From dbo.MailMessages m
             Where m.MailMessageID = @MailMessageID
               and m.MailFolderTypeID = 1) -- 1 = Trash
  begin
    -- delete from child tables first then the message itself
    Delete From a
    From dbo.MailMessageAttachments a 
    Where a.MailMessageID = @MailMessageID

    Delete From a
    From dbo.MailMessageGroups g inner join dbo.MailMessageGroupAddresses a on a.MailMessageGroupID = g.MailMessageGroupID
    Where g.MailMessageID = @MailMessageID

    Delete From a
    From dbo.MailMessageGroups g inner join dbo.MailMessageGroupStatusAudit a on a.MailMessageGroupID = g.MailMessageGroupID
    Where g.MailMessageID = @MailMessageID

    Delete From g
    From dbo.MailMessageGroups g 
    Where g.MailMessageID = @MailMessageID
                            
    Delete From m
    From dbo.MailMessages m 
    Where m.MailMessageID = @MailMessageID  
  end
  else
  begin
    Exec dbo.usp_MailMessageMoveFolder @MailMessageID, 1 -- 1 = Trash
  end
go



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_GetAllMessageHeadersByFolder]') AND type in (N'P', N'PC'))
  DROP PROCEDURE [dbo].usp_GetAllMessageHeadersByFolder
GO

-- =============================================
-- Author:		Shane Kelly
-- Create date: 10/17/2009
-- Description: Retrievs mail message headers from a specific folder by MailAccountID 
--              Note that it returns 3 resultsets. It only returns
--              The address list original to the email, no addresses added by downline expanstion are returned
--              Giving this the same data as the original message.
-- =============================================
Create Procedure dbo.usp_GetAllMessageHeadersByFolder @MailFolderTypeID int,
                                                      @MailAccountID int
As
  SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
  SET NOCOUNT ON

  Select m.MailMessageID,
         m.Subject, 
         m.DateAddedUTC,
         m.FromAddress, 
         m.FromNickName, 
         m.MailAccountID, 
         m.IsOutbound,
         m.MailMessageTypeID,
         m.BeenRead,
         m.MailMessagePriorityID,
         m.VisualTemplateID,
         m.MailFolderTypeID,
         m.AttachmentUniqueID,
         HasAttachments = (Case When Exists (Select *
                                             From dbo.MailMessageAttachments a 
                                             Where a.MailMessageID = m.MailMessageID) Then CONVERT(bit, 1)
                                                                                      Else Convert(bit, 0)
                           End)
  From dbo.MailMessages m
  Where m.MailAccountID = @MailAccountID
    and m.MailFolderTypeID = @MailFolderTypeID
    and m.Locked = 0 -- avoid locked items that are in processes
  
  -- When returning the address list we only return the addresses in the "original" group. Meaning, when a downline message is saved
  -- the addresses of all the downline accounts are added as non-original groups
  -- When the user retrieves the message we only want to return what was original when he composed it so he gets the same view
  -- of the world
  Select a.MailMessageGroupAddressID,
         a.MailMessageGroupID, 
         g.MailMessageID,
         EmailAddress = ISNULL(a.EmailAddress, ''),  
         NickName = ISNULL(a.NickName, ''), 
         a.AddressTypeID, 
         a.RecipientTypeID 
  From dbo.MailMessages m inner join dbo.MailMessageGroups g on g.MailMessageID = m.MailMessageID
                                                            and g.IsOriginal = 1 
                          inner join dbo.MailMessageGroupAddresses a on a.MailMessageGroupID = g.MailMessageGroupID
  Where m.MailAccountID = @MailAccountID
    and m.MailFolderTypeID = @MailFolderTypeID
    -- and m.Locked = 0 not needed because if the first query didn't return the messageid, these records won't matter
go

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_MailMessageDeliverInternal]') AND type in (N'P', N'PC'))
  DROP PROCEDURE [dbo].usp_MailMessageDeliverInternal
GO

-- =============================================
-- Author:		Shane Kelly
-- Create date: 10/17/2009
-- Description: Used to deliver messages to internal addresses. Note that it uses 2008 specific 
--              syntax to pass in table-value parameters.
-- =============================================
Create Procedure dbo.usp_MailMessageDeliverInternal
  @MailMessageGroupAddressID int,
	@Subject nvarchar(max),
	@Body nvarchar(max),
	@HTMLBody nvarchar(max), 
	@DateAddedUTC datetime,
	@FromAddress nvarchar(255),
	@FromNickName nvarchar(255),
	@MailAccountID int,
	@IsOutbound bit,
	@MailMessageTypeID smallint,
	@BeenRead bit,
	@MailMessagePriorityID smallint,
	@VisualTemplateID int,
	@MailFolderTypeID smallint,
	@AttachmentUniqueID nvarchar(36),
	@SiteID int,
	
  @Addresses dbo.TTAddressList READONLY,
  @Attachments dbo.TTInternalAttachmentList READONLY
As
  SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
  SET NOCOUNT ON
  
  Declare @MailMessageID int
  
  Insert Into dbo.MailMessages (Subject, 
                                Body, 
                                HTMLBody,
                                DateAddedUTC,
                                FromAddress, 
                                FromNickName, 
                                MailAccountID, 
                                IsOutbound,
                                MailMessageTypeID,
                                BeenRead,
                                MailMessagePriorityID,
                                VisualTemplateID,
                                MailFolderTypeID,
                                AttachmentUniqueID,
                                Locked,
                                SiteID) 
  Select Subject = @Subject, 
         Body = @Body, 
         HTMLBody = @HTMLBody, 
         DateAddedUTC = @DateAddedUTC,
         FromAddress = @FromAddress, 
         FromNickName = @FromNickName, 
         MailAccountID = @MailAccountID, 
         IsOutbound = @IsOutbound,
         MailMessageTypeID = @MailMessageTypeID,
         BeenRead = @BeenRead,
         MailMessagePriorityID = @MailMessagePriorityID,
         VisualTemplateID = @VisualTemplateID,
         MailFolderTypeID = @MailFolderTypeID,
         AttachmentUniqueID = @AttachmentUniqueID, 
         Locked = 1, -- prevents working with the message while were trying to save all the parts 
         SiteID = @SiteID
         
  Set @MailMessageID = SCOPE_IDENTITY()
  
  /********************************************************
  ** Insert addresses into the "Original" MessageGroup  
  *********************************************************/
  Declare @MailMessageGroupID  int
  
  Insert Into dbo.MailMessageGroups (MailMessageID, 
                                     MessageGroupStatusID, 
                                     AttemptCount, 
                                     RetryTimeUTC, 
                                     IsOriginal)
  Select MailMessageID = @MailMessageID, 
         MessageGroupStatusID = 5, -- Inbound Received
         AttemptCount = 0, 
         RetryTimeUTC = null, 
         IsOriginal = 1
         
  Select @MailMessageGroupID = SCOPE_IDENTITY()

  /********************************************************
  ** Insert addresses into the "Original" MessageGroup  
  *********************************************************/
  Insert Into dbo.MailMessageGroupAddresses (MailMessageGroupID, 
                                             EmailAddress, 
                                             NickName, 
                                             AddressTypeID, 
                                             RecipientTypeID,
                                             RecipientStatusID)
  Select MailMessageGroupID = @MailMessageGroupID, 
         EmailAddress = a.EmailAddress, 
         NickName = a.NickName, 
         AddressTypeID = a.AddressTypeID, 
         RecipientTypeID = a.RecipientTypeID,
         RecipientStatusID = 0 -- status unknown
  From @Addresses as a


  /********************************************************
  ** Save Attachments
  *********************************************************/
  Insert Into dbo.MailMessageAttachments (MailMessageID, 
                                          MailAttachmentID)
  Select @MailMessageID,
         a.MailAttachmentID
  From @Attachments as a
  
  /*******************************************************
  ** Unlock the message 
  *********************************************************/
  Update dbo.MailMessages Set Locked = 0
  Where MailMessageID = @MailMessageID
  
  /*******************************************************
  ** Update the original MailMessageGroupAddress to show the receipient received the message
  *********************************************************/
  Update dbo.MailMessageGroupAddresses Set RecipientStatusID = 2
  Where MailMessageGroupAddressID = @MailMessageGroupAddressID
go

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_MailMessageDeliverFromSMTPServer]') AND type in (N'P', N'PC'))
  DROP PROCEDURE [dbo].usp_MailMessageDeliverFromSMTPServer
GO

-- =============================================
-- Author:		Shane Kelly
-- Create date: 10/17/2009
-- Description: Used to deliver messages to internal addresses. Note that it uses 2008 specific 
--              syntax to pass in table-value parameters. This is called by the hurricane server to
--              add emails it receives
-- =============================================
Create Procedure dbo.usp_MailMessageDeliverFromSMTPServer
  @RecipientEmailAddress nvarchar(255),
	@Subject nvarchar(max),
	@Body nvarchar(max),
	@HTMLBody nvarchar(max), 
	@DateAddedUTC datetime,
	@FromAddress nvarchar(255),
	@FromNickName nvarchar(255),
	@IsOutbound bit,
	@MailMessageTypeID smallint,
	@BeenRead bit,
	@MailMessagePriorityID smallint,
	@MailFolderTypeID smallint,
	@AttachmentUniqueID nvarchar(36),
	@SiteID int,
	
  @Addresses dbo.TTAddressList READONLY,
  @Attachments dbo.TTAttachmentList READONLY
As
  SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
  SET NOCOUNT ON
  
	Declare @MailAccountID int
  Declare @MailMessageID int
	
	Select @MailAccountID = MailAccountID
	From dbo.MailAccount a
	Where a.EmailAddress = @RecipientEmailAddress
	
	if (@MailAccountID is null)
	  return
  
  
  Insert Into dbo.MailMessages (Subject, 
                                Body, 
                                HTMLBody,
                                DateAddedUTC,
                                FromAddress, 
                                FromNickName, 
                                MailAccountID, 
                                IsOutbound,
                                MailMessageTypeID,
                                BeenRead,
                                MailMessagePriorityID,
                                VisualTemplateID,
                                MailFolderTypeID,
                                AttachmentUniqueID,
                                Locked,
                                SiteID) 
  Select Subject = @Subject, 
         Body = @Body, 
         HTMLBody = @HTMLBody, 
         DateAddedUTC = @DateAddedUTC,
         FromAddress = @FromAddress, 
         FromNickName = @FromNickName, 
         MailAccountID = @MailAccountID, 
         IsOutbound = @IsOutbound,
         MailMessageTypeID = @MailMessageTypeID,
         BeenRead = @BeenRead,
         MailMessagePriorityID = @MailMessagePriorityID,
         VisualTemplateID = 0,
         MailFolderTypeID = @MailFolderTypeID,
         AttachmentUniqueID = @AttachmentUniqueID, 
         Locked = 1, -- prevents working with the message while were trying to save all the parts 
         SiteID = @SiteID
         
  Set @MailMessageID = SCOPE_IDENTITY()
  
  /********************************************************
  ** Insert addresses into the "Original" MessageGroup  
  *********************************************************/
  Declare @MailMessageGroupID  int
  
  Insert Into dbo.MailMessageGroups (MailMessageID, 
                                     MessageGroupStatusID, 
                                     AttemptCount, 
                                     RetryTimeUTC, 
                                     IsOriginal)
  Select MailMessageID = @MailMessageID, 
         MessageGroupStatusID = 5, -- Inbound Received
         AttemptCount = 0, 
         RetryTimeUTC = null, 
         IsOriginal = 1
         
  Select @MailMessageGroupID = SCOPE_IDENTITY()

  /********************************************************
  ** Insert addresses into the "Original" MessageGroup  
  *********************************************************/
  Insert Into dbo.MailMessageGroupAddresses (MailMessageGroupID, 
                                             EmailAddress, 
                                             NickName, 
                                             AddressTypeID, 
                                             RecipientTypeID,
                                             RecipientStatusID)
  Select MailMessageGroupID = @MailMessageGroupID, 
         EmailAddress = a.EmailAddress, 
         NickName = a.NickName, 
         AddressTypeID = a.AddressTypeID, 
         RecipientTypeID = a.RecipientTypeID,
         RecipientStatusID = 0 -- status unknown
  From @Addresses as a


  /********************************************************
  ** Save Attachments
  *********************************************************/
  -- This uses the new Ouput option from 2008. It insrts into MailAttachments and then uses the results
  -- to insert into MailMessageAttachments building the relationship with the ID from MailAttachments....super cool
  Insert Into dbo.MailAttachments (FileName,
                                   Size)
  Output @MailMessageID, 
         inserted.MailAttachmentID Into dbo.MailMessageAttachments (MailMessageID, 
                                                                    MailAttachmentID)
  Select a.FileName,
         a.Size
  From @Attachments as a  
  
  /*******************************************************
  ** Unlock the message 
  *********************************************************/
  Update dbo.MailMessages Set Locked = 0
  Where MailMessageID = @MailMessageID
go


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_MailMessageGroupChangeStatus]') AND type in (N'P', N'PC'))
  DROP PROCEDURE [dbo].usp_MailMessageGroupChangeStatus
GO

-- =============================================
-- Author:		Shane Kelly
-- Create date: 10/17/2009
-- Description: Update the status of a MailMEssageGroup
-- =============================================
Create Procedure dbo.usp_MailMessageGroupChangeStatus @MailMessageGroupID int,
                                                      @MessageGroupStatusID smallint
As
  SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
  SET NOCOUNT ON
  
  Update dbo.MailMessageGroups Set MessageGroupStatusID = @MessageGroupStatusID
  Where MailMessageGroupID = @MailMessageGroupID
go



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_UpdateRecipientStatus]') AND type in (N'P', N'PC'))
  DROP PROCEDURE [dbo].usp_UpdateRecipientStatus
GO

-- =============================================
-- Author:		Shane Kelly
-- Create date: 10/17/2009
-- Description: Mark an address as invalid. This could be that it was for an internal domain but no address existed in the mailaccount table,
--              or the address was in an invalid form, like missing the @ or similar.
-- =============================================
Create Procedure dbo.usp_UpdateRecipientStatus @MailMessageGroupAddressID int,
                                               @RecipientStatusID smallint
As
  SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
  SET NOCOUNT ON
  
  Update dbo.MailMessageGroupAddresses Set RecipientStatusID = @RecipientStatusID
  Where MailMessageGroupAddressID = @MailMessageGroupAddressID
go


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_GetSiteDomains]') AND type in (N'P', N'PC'))
  DROP PROCEDURE [dbo].usp_GetSiteDomains
GO

-- =============================================
-- Author:		Shane Kelly
-- Create date: 10/17/2009
-- Description: Get a list of site domains queueing emails can determine internal versus external addresses
-- =============================================
Create Procedure dbo.usp_GetSiteDomains
As
  SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
  SET NOCOUNT ON
  
  Select d.DomainName
  From dbo.MailDomain d
go


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_QueueMailMessages]') AND type in (N'P', N'PC'))
  DROP PROCEDURE [dbo].usp_QueueMailMessages
GO

-- =============================================
-- Author:		Shane Kelly
-- Create date: 10/17/2009
-- Description: QueueProcessor calls this procedure to return items from the queue
-- =============================================
Create Procedure dbo.usp_QueueMailMessages @MaxNumberToPoll int,
                                           @RetryCount int
As
  SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
  SET NOCOUNT ON


  -- Get a list of all Mail Messages in the OutBox folder
  Select m.MailMessageID,
         m.MailMessageTypeID
  into #tmpMailMessages
  From dbo.MailMessages m 
  Where m.Locked = 0
    and m.IsOutbound = 1
    and m.MailFolderTypeID = 4 -- Outbox items only (this ignores drafts and the like)
    
  -- Get a list of groups for messages in the outbox that are "to be sent" (never been queued)
  -- Get a list of groups that are in a retry state and haven't exhausted the retry attemps and are not in a finalized state
  -- Note: I grab the Top X groups, not email addresses because groups have to queue as a single "group" - thus the term
  Select Top (@MaxNumberToPoll) a.MailMessageID,
                                a.MailMessageGroupID
  into #tmpGroups
  From (
          Select g.MailMessageID,
                 g.MailMessageGroupID,
                 t.MailMessageTypeID
          From #tmpMailMessages t inner join dbo.MailMessageGroups g on g.MailMessageID = t.MailMessageID
          Where g.MessageGroupStatusID = 0 -- to be sent, these are groups that have never been queued yet
        UNION
          Select g.MailMessageID,
                 g.MailMessageGroupID,
                 m.MailMessageTypeID
          From dbo.MailMessageGroups g inner join dbo.MailMessages m on m.MailMessageID = g.MailMessageID
          Where g.RetryTimeUTC < GETDATE() -- RetryTimeUTC is initially null, which will evaluate to false here
            and g.AttemptCount < @RetryCount
            and g.MessageGroupStatusID not in (4, 5) -- SMTP Sent, Inbound Recieved the two final states
            and m.IsOutbound = 1 -- just a safety check to make sure this is an outbound message
            and m.Locked = 0
       ) a
  Order by a.MailMessageTypeID -- This gives adhoc messages higher priority than downline messages
       
 
  -- Mark all the opt-out people
  Update a Set RecipientStatusID = 1 -- Opted out
  From #tmpGroups g inner join dbo.MailMessageGroupAddresses a on a.MailMessageGroupID = g.MailMessageGroupID
                    inner join dbo.OptOut o on o.EmailAddress = a.EmailAddress
  Where a.RecipientTypeID = 0 -- 0 - Individuals (meaning we can't send to group types such as "downline"
    and a.RecipientStatusID = 0 -- 0 - Unknown, which means an invalid status has been determined yet


  -- Update Groups where we have no recipients (nothing more will ever be queued for the group)
  Update g Set MessageGroupStatusID = 4, -- 4 = SMTP Sent, final state
               RetryTimeUTC = NULL
  From #tmpGroups t inner join dbo.MailMessageGroups g on g.MailMessageGroupID = t.MailMessageGroupID
  Where Not Exists (Select *
                    From dbo.MailMessageGroupAddresses a
                    Where a.MailMessageGroupID = t.MailMessageGroupID
                      and a.RecipientTypeID = 0 -- individual
                      and a.RecipientStatusID = 0) -- unknown, which means they didn't opt out


  -- Create the final list of groups that we will work with (because the above queries for opt outs may
  -- have eliminated some of the groups. This verifiys which groups still are valid
  Select t.MailMessageGroupID,
         t.MailMessageID
  into #FinalGroupSelection
  From #tmpGroups t inner join dbo.MailMessageGroups g on g.MailMessageGroupID = t.MailMessageGroupID
  Where g.MessageGroupStatusID not in (4, 5) -- 4 & 5 are the finalized states. Only group nonfinalized groups


  /*********************
  * Resultset 1 - List of Messages
  *********************/
  Select m.MailMessageID,
         m.Subject,
         Body = IsNull(m.Body, ''),
         HTMLBody = IsNull(m.HTMLBody, ''),
         m.DateAddedUTC,
         m.FromNickName,
         m.FromAddress,
         m.MailMessagePriorityID,
         m.VisualTemplateID,
         m.AttachmentUniqueID,
         m.MailMessageTypeID,
         m.SiteID
  From (Select Distinct g.MailMessageID
        From #FinalGroupSelection g) g Inner Join dbo.MailMessages m on m.MailMessageID = g.MailMessageID 
  
  /*********************
  * Resultset 2 - List of Attachments
  *********************/
  Select a.MailMessageAttachmentID,
         a.MailMessageID,
         a.MailAttachmentID,
         ma.FileName
  From (Select Distinct g.MailMessageID
        From #FinalGroupSelection g) g Inner Join dbo.MailMessageAttachments a on a.MailMessageID = g.MailMessageID
                                       Inner join dbo.MailAttachments ma on ma.MailAttachmentID = a.MailAttachmentID
  
  /*********************
  * Resultset 4 - List of Groups
  *********************/
  Select g.MailMessageGroupID,
         g.MailMessageID
  From #FinalGroupSelection g

  /*********************
  * Resultset 5 - List of Addresses
  *********************/
  -- Note: we return all receipients, including those that are OPTED OUT - this is on purpose. Internally
  --       we won't send them messages, but to preserve them on the TO line we return all addresses
  --       and then exclude those internally
  Select a.MailMessageGroupAddressID,
         a.MailMessageGroupID,
         NickName = IsNull(a.NickName, ''),
         a.EmailAddress,
         a.AddressTypeID,
         MailAccountID = IsNull((Select Top(1) MailAccountID 
                                 From dbo.MailAccount m
                                 Where m.EmailAddress = a.EmailAddress
                                   and m.Active = 1), -1),
        a.RecipientStatusID                                   
  From #FinalGroupSelection g inner join dbo.MailMessageGroupAddresses a on a.MailMessageGroupID = g.MailMessageGroupID
  Where a.RecipientTypeID = 0 -- 0 - Individuals (meaning we can't send to group types such as "downline"
  
  
       
  -- Update the group attempt count, retry time, and status
  Update g Set MessageGroupStatusID = 1, -- move group to "Intermediate Queue"
               AttemptCount = g.AttemptCount + 1,
               RetryTimeUTC = DateAdd(minute, (g.AttemptCount + 1) * 5, GetDate()) -- 5 min delay * number of attempts. Thus the more attempts
                                                                                   -- the longer between each attempt to send
  From #FinalGroupSelection t inner join dbo.MailMessageGroups g on g.MailMessageGroupID = t.MailMessageGroupID
  
  
  -- Very last put the messages in the "Sent Items" folder if there are no "to be sent" groups 
  Update m Set MailFolderTypeID = 2 -- Sent Items
  From #tmpMailMessages t inner join dbo.MailMessages m on m.MailMessageID = t.MailMessageID
  Where Not Exists (Select *
                    From dbo.MailMessageGroups g
                    Where g.MailMessageID = m.MailMessageID
                      and g.MessageGroupStatusID = 0) -- 0 = To Be Sent
go
