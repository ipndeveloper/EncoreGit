
DROP TRIGGER AuditUsersTable
DROP TRIGGER AuditCorporateUsersTable


GO
CREATE TRIGGER AuditUsersTable
   ON [dbo].[Users]
   FOR INSERT,DELETE,UPDATE
AS 
EXTERNAL NAME NetStepsSql.Triggers.AuditTrigger

GO
CREATE TRIGGER AuditCorporateUsersTable
   ON [dbo].[CorporateUsers]
   FOR INSERT,DELETE,UPDATE
AS 
EXTERNAL NAME NetStepsSql.Triggers.AuditTrigger
