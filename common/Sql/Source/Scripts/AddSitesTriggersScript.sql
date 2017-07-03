
DROP TRIGGER AuditSitesTable
DROP TRIGGER AuditArchivesTable
DROP TRIGGER AuditNewsTable
DROP TRIGGER AuditNavigationsTable
DROP TRIGGER AuditPagesTable
DROP TRIGGER AuditCalendarEventsTable
DROP TRIGGER AuditSiteUrlsTable
DROP TRIGGER AuditSiteSettingValuesTable


GO
CREATE TRIGGER AuditSitesTable
   ON [dbo].[Sites]
   FOR INSERT,DELETE,UPDATE
AS 
EXTERNAL NAME NetStepsSql.Triggers.AuditTrigger

GO
CREATE TRIGGER AuditArchivesTable
   ON [dbo].[Archives]
   FOR INSERT,DELETE,UPDATE
AS 
EXTERNAL NAME NetStepsSql.Triggers.AuditTrigger

GO
CREATE TRIGGER AuditNewsTable
   ON [dbo].[News]
   FOR INSERT,DELETE,UPDATE
AS 
EXTERNAL NAME NetStepsSql.Triggers.AuditTrigger

GO
CREATE TRIGGER AuditNavigationsTable
   ON [dbo].[Navigations]
   FOR INSERT,DELETE,UPDATE
AS 
EXTERNAL NAME NetStepsSql.Triggers.AuditTrigger

GO
CREATE TRIGGER AuditPagesTable
   ON [dbo].[Pages]
   FOR INSERT,DELETE,UPDATE
AS 
EXTERNAL NAME NetStepsSql.Triggers.AuditTrigger

GO
CREATE TRIGGER AuditCalendarEventsTable
   ON [dbo].[CalendarEvents]
   FOR INSERT,DELETE,UPDATE
AS 
EXTERNAL NAME NetStepsSql.Triggers.AuditTrigger

GO
CREATE TRIGGER AuditSiteUrlsTable
   ON [dbo].[SiteUrls]
   FOR INSERT,DELETE,UPDATE
AS 
EXTERNAL NAME NetStepsSql.Triggers.AuditTrigger

GO
CREATE TRIGGER AuditSiteSettingValuesTable
   ON [dbo].[SiteSettingValues]
   FOR INSERT,DELETE,UPDATE
AS 
EXTERNAL NAME NetStepsSql.Triggers.AuditTrigger


